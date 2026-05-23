using System;
using System.Collections.Generic;

namespace ZXBox.Hardware.Sound;

public sealed class Ay38912Chip
{
    public const int RegisterCount = 16;
    public const int FrameTStates128k = 70908;

    private const int InternalSampleRate = 192000;
    private const int Zx128TStatesPerSecond = 3546900;
    private const int SelectPortMask = 0xC002;
    private const int SelectPortValue = 0xC000;
    private const int DataPortMask = 0xC002;
    private const int DataPortValue = 0x8000;

    private static readonly float[] VolumeTable = BuildVolumeTable();

    private readonly byte[] _registers = new byte[RegisterCount];
    private readonly List<float> _frameSamples = new();
    private readonly double[] _tonePhase = new double[3];

    private long _sampleAccumulator;
    private int _frameTStateCursor;
    private int _selectedRegister;
    private double _noisePhase;
    private int _noiseShiftRegister = 0x1FFFF;
    private bool _noiseOutput = true;
    private double _envelopePhase;
    private int _envelopeLevel;
    private int _envelopeDirection = -1;
    private bool _envelopeContinue;
    private bool _envelopeAttack;
    private bool _envelopeAlternate;
    private bool _envelopeHold;
    private bool _envelopeHolding = true;
    private float _highPassInput;
    private float _highPassOutput;

    public void Reset()
    {
        Array.Clear(_registers);
        Array.Clear(_tonePhase);
        _frameSamples.Clear();
        _sampleAccumulator = 0;
        _frameTStateCursor = 0;
        _selectedRegister = 0;
        _noisePhase = 0d;
        _noiseShiftRegister = 0x1FFFF;
        _noiseOutput = true;
        _envelopePhase = 0d;
        _envelopeLevel = 0;
        _envelopeDirection = -1;
        _envelopeContinue = false;
        _envelopeAttack = false;
        _envelopeAlternate = false;
        _envelopeHold = false;
        _envelopeHolding = true;
        _highPassInput = 0f;
        _highPassOutput = 0f;
    }

    public bool HandlePortWrite(int port, int value, int frameTState)
    {
        port &= 0xFFFF;
        value &= 0xFF;

        if ((port & SelectPortMask) == SelectPortValue)
        {
            AdvanceToFrameTState(frameTState);

            if (value < RegisterCount)
            {
                _selectedRegister = value;
            }

            return true;
        }

        if ((port & DataPortMask) == DataPortValue)
        {
            AdvanceToFrameTState(frameTState);
            WriteSelectedRegister((byte)value);
            return true;
        }

        return false;
    }

    public bool TryReadPort(int port, int frameTState, out int value)
    {
        value = 0xFF;
        port &= 0xFFFF;

        if ((port & SelectPortMask) != SelectPortValue)
        {
            return false;
        }

        AdvanceToFrameTState(frameTState);
        value = ReadSelectedRegister();
        return true;
    }

    public float[] RenderAudioFrame(int outputSampleCount, int frameTStates)
    {
        AdvanceToFrameTState(frameTStates);

        var rendered = ResampleFrame(outputSampleCount);
        _frameSamples.Clear();
        _frameTStateCursor = 0;

        return rendered;
    }

    private void AdvanceToFrameTState(int frameTState)
    {
        if (frameTState <= _frameTStateCursor)
        {
            return;
        }

        var deltaTStates = frameTState - _frameTStateCursor;
        _sampleAccumulator += (long)deltaTStates * InternalSampleRate;

        while (_sampleAccumulator >= Zx128TStatesPerSecond)
        {
            _sampleAccumulator -= Zx128TStatesPerSecond;
            _frameSamples.Add(GenerateInternalSample());
        }

        _frameTStateCursor = frameTState;
    }

    private float GenerateInternalSample()
    {
        AdvanceTonePhases();
        AdvanceNoise();
        AdvanceEnvelope();

        var mixer = _registers[7];
        var envelopeVolume = VolumeTable[_envelopeLevel];
        var sample = 0f;

        for (var channel = 0; channel < 3; channel++)
        {
            var toneDisabled = (mixer & (1 << channel)) != 0;
            var noiseDisabled = (mixer & (1 << (channel + 3))) != 0;
            var toneActive = toneDisabled || _tonePhase[channel] < 0.5d;
            var noiseActive = noiseDisabled || _noiseOutput;

            if (toneActive && noiseActive)
            {
                sample += GetChannelVolume(channel, envelopeVolume);
            }
        }

        return sample;
    }

    private void AdvanceTonePhases()
    {
        for (var channel = 0; channel < 3; channel++)
        {
            var period = GetTonePeriod(channel);
            var frequency = 1773450d / (16d * period);
            _tonePhase[channel] += frequency / InternalSampleRate;
            _tonePhase[channel] -= Math.Floor(_tonePhase[channel]);
        }
    }

    private void AdvanceNoise()
    {
        var period = Math.Max(_registers[6] & 0x1F, 1);
        var frequency = 1773450d / (16d * period);
        _noisePhase += frequency / InternalSampleRate;

        while (_noisePhase >= 1d)
        {
            _noisePhase -= 1d;
            var feedback = ((_noiseShiftRegister ^ (_noiseShiftRegister >> 3)) & 0x01) != 0 ? 1 : 0;
            _noiseShiftRegister = (_noiseShiftRegister >> 1) | (feedback << 16);
            _noiseOutput = (_noiseShiftRegister & 0x01) != 0;
        }
    }

    private void AdvanceEnvelope()
    {
        if (_envelopeHolding)
        {
            return;
        }

        var period = GetEnvelopePeriod();
        var frequency = 1773450d / (256d * period);
        _envelopePhase += frequency / InternalSampleRate;

        while (_envelopePhase >= 1d)
        {
            _envelopePhase -= 1d;
            StepEnvelope();
        }
    }

    private void StepEnvelope()
    {
        _envelopeLevel += _envelopeDirection;

        if (_envelopeLevel >= 0 && _envelopeLevel <= 15)
        {
            return;
        }

        if (!_envelopeContinue)
        {
            _envelopeHolding = true;
            _envelopeLevel = 0;
            return;
        }

        if (_envelopeHold)
        {
            _envelopeHolding = true;
            _envelopeLevel = _envelopeAlternate
                ? (_envelopeAttack ? 0 : 15)
                : (_envelopeAttack ? 15 : 0);
            return;
        }

        if (_envelopeAlternate)
        {
            _envelopeDirection = -_envelopeDirection;
        }

        _envelopeLevel = _envelopeDirection > 0 ? 0 : 15;
    }

    private float[] ResampleFrame(int outputSampleCount)
    {
        if (outputSampleCount <= 0)
        {
            return Array.Empty<float>();
        }

        var output = new float[outputSampleCount];

        if (_frameSamples.Count == 0)
        {
            return output;
        }

        if (_frameSamples.Count == 1)
        {
            Array.Fill(output, _frameSamples[0]);
            return NormalizeFrame(output);
        }

        var lastSourceIndex = _frameSamples.Count - 1;
        for (var i = 0; i < output.Length; i++)
        {
            var sourcePosition = output.Length == 1
                ? 0d
                : (double)i * lastSourceIndex / (output.Length - 1);
            var sourceIndex = (int)Math.Floor(sourcePosition);
            var fraction = sourcePosition - sourceIndex;
            var nextIndex = Math.Min(sourceIndex + 1, lastSourceIndex);

            output[i] = _frameSamples[sourceIndex] +
                        ((_frameSamples[nextIndex] - _frameSamples[sourceIndex]) * (float)fraction);
        }

        return NormalizeFrame(output);
    }

    private float[] NormalizeFrame(float[] buffer)
    {
        if (buffer.Length == 0)
        {
            return buffer;
        }

        for (var i = 0; i < buffer.Length; i++)
        {
            var sample = buffer[i] / 1.5f;
            var filtered = sample - _highPassInput + (0.995f * _highPassOutput);
            _highPassInput = sample;
            _highPassOutput = filtered;
            buffer[i] = Math.Clamp(filtered, -1f, 1f);
        }

        return buffer;
    }

    private void WriteSelectedRegister(byte value)
    {
        switch (_selectedRegister)
        {
            case 0:
            case 2:
            case 4:
            case 11:
            case 12:
            case 14:
                _registers[_selectedRegister] = value;
                break;
            case 1:
            case 3:
            case 5:
                _registers[_selectedRegister] = (byte)(value & 0x0F);
                break;
            case 6:
                _registers[_selectedRegister] = (byte)(value & 0x1F);
                break;
            case 7:
                _registers[_selectedRegister] = value;
                break;
            case 8:
            case 9:
            case 10:
                _registers[_selectedRegister] = (byte)(value & 0x1F);
                break;
            case 13:
                _registers[_selectedRegister] = (byte)(value & 0x0F);
                TriggerEnvelope();
                break;
        }
    }

    private int ReadSelectedRegister()
    {
        if (_selectedRegister < 0 || _selectedRegister >= RegisterCount)
        {
            return 0xFF;
        }

        return _registers[_selectedRegister];
    }

    private void TriggerEnvelope()
    {
        _envelopeContinue = (_registers[13] & 0x08) != 0;
        _envelopeAttack = (_registers[13] & 0x04) != 0;
        _envelopeAlternate = (_registers[13] & 0x02) != 0;
        _envelopeHold = (_registers[13] & 0x01) != 0;
        _envelopeHolding = false;
        _envelopePhase = 0d;
        _envelopeDirection = _envelopeAttack ? 1 : -1;
        _envelopeLevel = _envelopeAttack ? 0 : 15;
    }

    private int GetTonePeriod(int channel)
    {
        var fineRegister = channel * 2;
        var coarseRegister = fineRegister + 1;
        var period = _registers[fineRegister] | ((_registers[coarseRegister] & 0x0F) << 8);
        return Math.Max(period, 1);
    }

    private int GetEnvelopePeriod()
    {
        var period = _registers[11] | (_registers[12] << 8);
        return Math.Max(period, 1);
    }

    private float GetChannelVolume(int channel, float envelopeVolume)
    {
        var register = _registers[8 + channel];
        return (register & 0x10) != 0
            ? envelopeVolume
            : VolumeTable[register & 0x0F];
    }

    private static float[] BuildVolumeTable()
    {
        var table = new float[16];
        table[0] = 0f;

        for (var i = 1; i < table.Length; i++)
        {
            table[i] = (float)Math.Pow(10d, ((i - 15) * 3d) / 20d);
        }

        table[15] = 1f;
        return table;
    }
}
