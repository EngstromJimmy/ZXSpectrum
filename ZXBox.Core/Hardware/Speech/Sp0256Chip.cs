// Derived from the BSD-3-Clause SP0256 implementation in MAME.
// Copyright (c) Joseph Zbiciak, Tim Lindner

#nullable enable

using System;
using System.Linq;
using System.Security.Cryptography;

namespace ZXBox.Hardware.Speech;

public sealed class Sp0256Chip
{
    public const int Sp0256RomSize = 0x800;
    public const int NormalClockHz = 3_050_000;
    public const int HighClockHz = 3_260_000;

    private const int ClockDivider = 7 * 6 * 8;
    private const int PerPause = 64;
    private const int PerNoise = 64;
    private const int RomPageOffset = 0x1000;
    private const int ZxTStatesPerSecond = 69_888 * 50;
    private const string Al2RomSha1 = "e60fcb5fa16ff3f3b69d36c7a6e955744d3feafc";

    private const int Amplitude = 0;
    private const int Period = 1;
    private const int B0 = 2;
    private const int F0 = 3;
    private const int B1 = 4;
    private const int F1 = 5;
    private const int B2 = 6;
    private const int F2 = 7;
    private const int B3 = 8;
    private const int F3 = 9;
    private const int B4 = 10;
    private const int F4 = 11;
    private const int B5 = 12;
    private const int F5 = 13;
    private const int InterpolateAmplitude = 14;
    private const int InterpolatePeriod = 15;

    private const ushort DeltaMask = 1 << 12;
    private const ushort FieldMask = 1 << 13;
    private const ushort ClearFiveMask = 1 << 14;
    private const ushort ClearAllMask = 1 << 15;

    private static readonly short[] QuantizationTable =
    {
        0, 9, 17, 25, 33, 41, 49, 57,
        65, 73, 81, 89, 97, 105, 113, 121,
        129, 137, 145, 153, 161, 169, 177, 185,
        193, 201, 209, 217, 225, 233, 241, 249,
        257, 265, 273, 281, 289, 297, 301, 305,
        309, 313, 317, 321, 325, 329, 333, 337,
        341, 345, 349, 353, 357, 361, 365, 369,
        373, 377, 381, 385, 389, 393, 397, 401,
        405, 409, 413, 417, 421, 425, 427, 429,
        431, 433, 435, 437, 439, 441, 443, 445,
        447, 449, 451, 453, 455, 457, 459, 461,
        463, 465, 467, 469, 471, 473, 475, 477,
        479, 481, 482, 483, 484, 485, 486, 487,
        488, 489, 490, 491, 492, 493, 494, 495,
        496, 497, 498, 499, 500, 501, 502, 503,
        504, 505, 506, 507, 508, 509, 510, 511
    };

    private static readonly ushort[] DataFormat =
    {
        Encode(0, 0, 0, false, false, false, true),

        Encode(8, 0, Amplitude, false, false, false, true),
        Encode(8, 0, Period, false, false, false, false),
        Encode(8, 0, B0, false, false, false, false),
        Encode(8, 0, F0, false, false, false, false),
        Encode(8, 0, B1, false, false, false, false),
        Encode(8, 0, F1, false, false, false, false),
        Encode(8, 0, B2, false, false, false, false),
        Encode(8, 0, F2, false, false, false, false),
        Encode(8, 0, B3, false, false, false, false),
        Encode(8, 0, F3, false, false, false, false),
        Encode(8, 0, B4, false, false, false, false),
        Encode(8, 0, F4, false, false, false, false),
        Encode(8, 0, B5, false, false, false, false),
        Encode(8, 0, F5, false, false, false, false),
        Encode(8, 0, InterpolateAmplitude, false, false, false, false),
        Encode(8, 0, InterpolatePeriod, false, false, false, false),

        Encode(6, 2, Amplitude, false, false, false, true),
        Encode(8, 0, Period, false, false, false, false),
        Encode(4, 3, B3, false, false, false, false),
        Encode(6, 2, F3, false, false, false, false),
        Encode(7, 1, B4, false, false, false, false),
        Encode(6, 2, F4, false, false, false, false),
        Encode(8, 0, B5, false, false, false, false),
        Encode(8, 0, F5, false, false, false, false),

        Encode(6, 2, Amplitude, false, false, false, true),
        Encode(8, 0, Period, false, false, false, false),
        Encode(6, 1, B3, false, false, false, false),
        Encode(7, 1, F3, false, false, false, false),
        Encode(8, 0, B4, false, false, false, false),
        Encode(8, 0, F4, false, false, false, false),
        Encode(8, 0, B5, false, false, false, false),
        Encode(8, 0, F5, false, false, false, false),

        Encode(0, 0, 0, false, false, true, false),
        Encode(6, 2, Amplitude, false, false, false, false),
        Encode(6, 2, F3, false, true, false, false),
        Encode(6, 2, F4, false, true, false, false),
        Encode(8, 0, F5, false, true, false, false),

        Encode(0, 0, 0, false, false, true, false),
        Encode(6, 2, Amplitude, false, false, false, false),
        Encode(7, 1, F3, false, true, false, false),
        Encode(8, 0, F4, false, true, false, false),
        Encode(8, 0, F5, false, true, false, false),

        0,
        0,

        Encode(4, 2, Amplitude, true, false, false, false),
        Encode(5, 0, Period, true, false, false, false),
        Encode(3, 4, B0, true, false, false, false),
        Encode(3, 3, F0, true, false, false, false),
        Encode(3, 4, B1, true, false, false, false),
        Encode(3, 3, F1, true, false, false, false),
        Encode(3, 4, B2, true, false, false, false),
        Encode(3, 3, F2, true, false, false, false),
        Encode(3, 3, B3, true, false, false, false),
        Encode(4, 2, F3, true, false, false, false),
        Encode(4, 1, B4, true, false, false, false),
        Encode(4, 2, F4, true, false, false, false),
        Encode(5, 0, B5, true, false, false, false),
        Encode(5, 0, F5, true, false, false, false),

        Encode(4, 2, Amplitude, true, false, false, false),
        Encode(5, 0, Period, true, false, false, false),
        Encode(4, 1, B0, true, false, false, false),
        Encode(4, 2, F0, true, false, false, false),
        Encode(4, 1, B1, true, false, false, false),
        Encode(4, 2, F1, true, false, false, false),
        Encode(4, 1, B2, true, false, false, false),
        Encode(4, 2, F2, true, false, false, false),
        Encode(4, 1, B3, true, false, false, false),
        Encode(5, 1, F3, true, false, false, false),
        Encode(5, 0, B4, true, false, false, false),
        Encode(5, 0, F4, true, false, false, false),
        Encode(5, 0, B5, true, false, false, false),
        Encode(5, 0, F5, true, false, false, false),

        Encode(0, 0, 0, false, false, true, false),
        Encode(6, 2, Amplitude, false, false, false, false),
        Encode(5, 3, F0, false, true, false, false),
        Encode(5, 3, F1, false, true, false, false),
        Encode(5, 3, F2, false, true, false, false),

        Encode(0, 0, 0, false, false, true, false),
        Encode(6, 2, Amplitude, false, false, false, false),
        Encode(6, 2, F0, false, true, false, false),
        Encode(6, 2, F1, false, true, false, false),
        Encode(6, 2, F2, false, true, false, false),

        Encode(6, 2, Amplitude, false, false, false, true),
        Encode(8, 0, Period, false, false, false, false),
        Encode(3, 4, B0, false, false, false, false),
        Encode(5, 3, F0, false, false, false, false),
        Encode(3, 4, B1, false, false, false, false),
        Encode(5, 3, F1, false, false, false, false),
        Encode(3, 4, B2, false, false, false, false),
        Encode(5, 3, F2, false, false, false, false),
        Encode(4, 3, B3, false, false, false, false),
        Encode(6, 2, F3, false, false, false, false),
        Encode(7, 1, B4, false, false, false, false),
        Encode(6, 2, F4, false, false, false, false),
        Encode(5, 0, InterpolateAmplitude, false, false, false, false),
        Encode(5, 0, InterpolatePeriod, false, false, false, false),

        Encode(6, 2, Amplitude, false, false, false, true),
        Encode(8, 0, Period, false, false, false, false),
        Encode(6, 1, B0, false, false, false, false),
        Encode(6, 2, F0, false, false, false, false),
        Encode(6, 1, B1, false, false, false, false),
        Encode(6, 2, F1, false, false, false, false),
        Encode(6, 1, B2, false, false, false, false),
        Encode(6, 2, F2, false, false, false, false),
        Encode(6, 1, B3, false, false, false, false),
        Encode(7, 1, F3, false, false, false, false),
        Encode(8, 0, B4, false, false, false, false),
        Encode(8, 0, F4, false, false, false, false),
        Encode(5, 0, InterpolateAmplitude, false, false, false, false),
        Encode(5, 0, InterpolatePeriod, false, false, false, false),

        Encode(4, 2, Amplitude, true, false, false, false),
        Encode(5, 0, Period, true, false, false, false),
        Encode(3, 3, B3, true, false, false, false),
        Encode(4, 2, F3, true, false, false, false),
        Encode(4, 1, B4, true, false, false, false),
        Encode(4, 2, F4, true, false, false, false),
        Encode(5, 0, B5, true, false, false, false),
        Encode(5, 0, F5, true, false, false, false),

        Encode(4, 2, Amplitude, true, false, false, false),
        Encode(5, 0, Period, true, false, false, false),
        Encode(4, 1, B3, true, false, false, false),
        Encode(5, 1, F3, true, false, false, false),
        Encode(5, 0, B4, true, false, false, false),
        Encode(5, 0, F4, true, false, false, false),
        Encode(5, 0, B5, true, false, false, false),
        Encode(5, 0, F5, true, false, false, false),

        Encode(6, 2, Amplitude, false, false, false, false),
        Encode(8, 0, Period, false, false, false, false),

        Encode(6, 2, Amplitude, false, false, false, true),
        Encode(8, 0, Period, false, false, false, false),
        Encode(3, 4, B0, false, false, false, false),
        Encode(5, 3, F0, false, false, false, false),
        Encode(3, 4, B1, false, false, false, false),
        Encode(5, 3, F1, false, false, false, false),
        Encode(3, 4, B2, false, false, false, false),
        Encode(5, 3, F2, false, false, false, false),
        Encode(4, 3, B3, false, false, false, false),
        Encode(6, 2, F3, false, false, false, false),
        Encode(7, 1, B4, false, false, false, false),
        Encode(6, 2, F4, false, false, false, false),
        Encode(8, 0, B5, false, false, false, false),
        Encode(8, 0, F5, false, false, false, false),
        Encode(5, 0, InterpolateAmplitude, false, false, false, false),
        Encode(5, 0, InterpolatePeriod, false, false, false, false),

        Encode(6, 2, Amplitude, false, false, false, true),
        Encode(8, 0, Period, false, false, false, false),
        Encode(6, 1, B0, false, false, false, false),
        Encode(6, 2, F0, false, false, false, false),
        Encode(6, 1, B1, false, false, false, false),
        Encode(6, 2, F1, false, false, false, false),
        Encode(6, 1, B2, false, false, false, false),
        Encode(6, 2, F2, false, false, false, false),
        Encode(6, 1, B3, false, false, false, false),
        Encode(7, 1, F3, false, false, false, false),
        Encode(8, 0, B4, false, false, false, false),
        Encode(8, 0, F4, false, false, false, false),
        Encode(8, 0, B5, false, false, false, false),
        Encode(8, 0, F5, false, false, false, false),
        Encode(5, 0, InterpolateAmplitude, false, false, false, false),
        Encode(5, 0, InterpolatePeriod, false, false, false, false),

        Encode(0, 0, 0, false, false, true, false),
        Encode(6, 2, Amplitude, false, false, false, false),
        Encode(8, 0, Period, false, false, false, false),
        Encode(5, 3, F0, false, true, false, false),
        Encode(5, 3, F1, false, true, false, false),
        Encode(5, 3, F2, false, true, false, false),
        Encode(5, 0, InterpolateAmplitude, false, false, false, false),
        Encode(5, 0, InterpolatePeriod, false, false, false, false),

        Encode(0, 0, 0, false, false, true, false),
        Encode(6, 2, Amplitude, false, false, false, false),
        Encode(8, 0, Period, false, false, false, false),
        Encode(6, 2, F0, false, true, false, false),
        Encode(6, 2, F1, false, true, false, false),
        Encode(6, 2, F2, false, true, false, false),
        Encode(5, 0, InterpolateAmplitude, false, false, false, false),
        Encode(5, 0, InterpolatePeriod, false, false, false, false)
    };

    private static readonly short[] DataFormatIndex =
    {
        -1, -1, -1, -1, -1, -1, -1, -1,
        -1, -1, -1, -1, -1, -1, -1, -1,
        17, 22, 17, 24, 25, 30, 25, 32,
        83, 94, 129, 142, 97, 108, 145, 158,
        83, 96, 129, 144, 97, 110, 145, 160,
        73, 77, 74, 77, 78, 82, 79, 82,
        33, 36, 34, 37, 38, 41, 39, 42,
        127, 128, 127, 128, 127, 128, 127, 128,
        1, 14, 1, 16, 1, 14, 1, 16,
        45, 56, 45, 58, 59, 70, 59, 72,
        161, 166, 162, 166, 169, 174, 170, 174,
        111, 116, 111, 118, 119, 124, 119, 126,
        161, 168, 162, 168, 169, 176, 170, 176,
        -1, -1, -1, -1, -1, -1, -1, -1,
        -1, -1, -1, -1, -1, -1, -1, -1,
        0, 0, 0, 0, 0, 0, 0, 0
    };

    private readonly byte[] _rom = new byte[0x10000];
    private short[] _frameSamples = new short[4096];
    private int _frameSampleCount;
    private readonly FilterState _filter = new();

    private bool _silent = true;
    private bool _lrq = true;
    private int _ald;
    private int _pc;
    private int _stack;
    private bool _halted = true;
    private uint _mode;
    private uint _page = RomPageOffset << 3;
    private int _clockHz = NormalClockHz;
    private long _sampleAccumulator;
    private int _frameTStateCursor;

    public bool HasRom { get; private set; }

    public int ClockHz => _clockHz;

    public void LoadRom(byte[] romBytes)
    {
        ArgumentNullException.ThrowIfNull(romBytes);

        if (romBytes.Length < Sp0256RomSize)
        {
            throw new ArgumentException($"SP0256 ROM must be at least {Sp0256RomSize} bytes.", nameof(romBytes));
        }

        var normalizedRom = NormalizeAl2Rom(romBytes);

        Array.Clear(_rom);
        Array.Copy(normalizedRom, 0, _rom, RomPageOffset, Sp0256RomSize);
        HasRom = true;
        ResetRuntime();
    }

    public void ClearRom()
    {
        Array.Clear(_rom);
        HasRom = false;
        ResetRuntime();
    }

    public void ResetRuntime()
    {
        _frameSampleCount = 0;
        _sampleAccumulator = 0;
        _frameTStateCursor = 0;
        _silent = true;
        _lrq = true;
        _ald = 0;
        _pc = 0;
        _stack = 0;
        _halted = true;
        _mode = 0;
        _page = RomPageOffset << 3;
        _filter.Reset();
    }

    public void SetClock(int clockHz, int frameTState)
    {
        if (clockHz <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(clockHz));
        }

        AdvanceToFrameTState(frameTState);
        _clockHz = clockHz;
    }

    public void WriteAllophone(byte allophone, int frameTState)
    {
        if (!HasRom)
        {
            return;
        }

        AdvanceToFrameTState(frameTState);

        if (!_lrq)
        {
            return;
        }

        _lrq = false;
        _ald = allophone << 4;
    }

    public bool ReadBusy(int frameTState)
    {
        if (!HasRom)
        {
            return false;
        }

        AdvanceToFrameTState(frameTState);
        return !_lrq;
    }

    public float[] RenderFrame(int outputSampleCount, int tStatesPerFrame)
    {
        var output = new float[outputSampleCount];
        RenderFrame(output, tStatesPerFrame);
        return output;
    }

    public void RenderFrame(Span<float> destination, int tStatesPerFrame)
    {
        AdvanceToFrameTState(tStatesPerFrame);
        ResampleFrame(destination);
        _frameSampleCount = 0;
        _frameTStateCursor = 0;
    }

    private void AdvanceToFrameTState(int frameTState)
    {
        if (frameTState <= _frameTStateCursor)
        {
            return;
        }

        if (!HasRom)
        {
            _frameTStateCursor = frameTState;
            return;
        }

        var deltaTStates = frameTState - _frameTStateCursor;
        _sampleAccumulator += (long)deltaTStates * _clockHz;

        var threshold = (long)ZxTStatesPerSecond * ClockDivider;
        while (_sampleAccumulator >= threshold)
        {
            _sampleAccumulator -= threshold;
            EnsureFrameSampleCapacity(_frameSampleCount + 1);
            _frameSamples[_frameSampleCount++] = GenerateNativeSample();
        }

        _frameTStateCursor = frameTState;
    }

    private short GenerateNativeSample()
    {
        if (_filter.Repeat <= 0)
        {
            Micro();
        }

        if (_silent && _filter.Repeat <= 0)
        {
            return 0;
        }

        return _filter.UpdateOneSample();
    }

    private void ResampleFrame(Span<float> output)
    {
        if (output.Length == 0)
        {
            return;
        }

        if (_frameSampleCount == 0)
        {
            output.Clear();
            return;
        }

        if (_frameSampleCount == 1)
        {
            var single = NormalizeSample(_frameSamples[0]);
            output.Fill(single);
            return;
        }

        var lastSourceIndex = _frameSampleCount - 1;
        for (var i = 0; i < output.Length; i++)
        {
            var sourcePosition = output.Length == 1
                ? 0d
                : (double)i * lastSourceIndex / (output.Length - 1);
            var sourceIndex = (int)Math.Floor(sourcePosition);
            var fraction = sourcePosition - sourceIndex;
            var nextIndex = Math.Min(sourceIndex + 1, lastSourceIndex);

            var sample = _frameSamples[sourceIndex] +
                         ((_frameSamples[nextIndex] - _frameSamples[sourceIndex]) * fraction);
            output[i] = NormalizeSample(sample);
        }
    }

    private void EnsureFrameSampleCapacity(int requiredCapacity)
    {
        if (_frameSamples.Length >= requiredCapacity)
        {
            return;
        }

        Array.Resize(ref _frameSamples, Math.Max(requiredCapacity, _frameSamples.Length * 2));
    }

    private void Micro()
    {
        while (_filter.Repeat <= 0)
        {
            if (_halted && !_lrq)
            {
                _pc = _ald | (RomPageOffset << 3);
                _halted = false;
                _lrq = true;
                _ald = 0;
                Array.Clear(_filter.Registers);
            }

            if (_halted)
            {
                _filter.Repeat = 1;
                _lrq = true;
                _ald = 0;
                Array.Clear(_filter.Registers);
                return;
            }

            var immediate4 = (byte)GetBits(4);
            var opcode = (byte)GetBits(4);
            var repeat = 0;
            var controlTransfer = false;

            switch (opcode)
            {
                case 0x0:
                    if (immediate4 != 0)
                    {
                        _page = BitReverse32(immediate4) >> 13;
                    }
                    else
                    {
                        var branchTarget = _stack;
                        _stack = 0;

                        if (branchTarget == 0)
                        {
                            _halted = true;
                            _pc = 0;
                        }
                        else
                        {
                            _pc = branchTarget;
                        }

                        controlTransfer = true;
                    }
                    break;

                case 0xE:
                case 0xD:
                    var target =
                        _page |
                        (BitReverse32(immediate4) >> 17) |
                        (BitReverse32(GetBits(8)) >> 21);

                    if (opcode == 0xD)
                    {
                        _stack = (_pc + 7) & ~7;
                    }

                    _pc = (int)target;
                    controlTransfer = true;
                    break;

                case 0x1:
                    _mode = (uint)(((immediate4 & 8) >> 2) | (immediate4 & 4) | ((immediate4 & 3) << 4));
                    break;

                default:
                    repeat = immediate4 | (int)(_mode & 0x30);
                    break;
            }

            if (opcode != 0x1)
            {
                _mode &= 0x0f;
            }

            if (controlTransfer)
            {
                continue;
            }

            if (repeat == 0)
            {
                continue;
            }

            _filter.Repeat = repeat + 1;

            var formatIndex = (opcode << 3) | (int)(_mode & 6);
            var dataStart = DataFormatIndex[formatIndex];
            var dataEnd = DataFormatIndex[formatIndex + 1];

            for (var i = dataStart; i <= dataEnd; i++)
            {
                var control = DataFormat[i];

                var bitLength = control & 0x0f;
                var shift = (control >> 4) & 0x0f;
                var parameter = (control >> 8) & 0x0f;
                var clearAll = (control & ClearAllMask) != 0;
                var clearFive = (control & ClearFiveMask) != 0;
                var isDelta = (control & DeltaMask) != 0;
                var isFieldReplace = (control & FieldMask) != 0;

                if (clearAll)
                {
                    Array.Clear(_filter.Registers);
                    _silent = true;
                }

                if (clearFive)
                {
                    _filter.Registers[B5] = 0;
                    _filter.Registers[F5] = 0;
                }

                if (bitLength == 0)
                {
                    continue;
                }

                var value = (int)GetBits(bitLength);

                if (isDelta && (value & (1 << (bitLength - 1))) != 0)
                {
                    value |= -1 << bitLength;
                }

                if (shift != 0)
                {
                    value <<= shift;
                }

                _silent = false;

                if (isFieldReplace)
                {
                    var preservedMask = shift == 0 ? 0 : (1 << shift) - 1;
                    _filter.Registers[parameter] = unchecked((byte)((_filter.Registers[parameter] & preservedMask) | value));
                    continue;
                }

                if (isDelta)
                {
                    _filter.Registers[parameter] = unchecked((byte)(_filter.Registers[parameter] + value));
                    continue;
                }

                _filter.Registers[parameter] = unchecked((byte)value);
            }

            if (opcode == 0xF)
            {
                _silent = true;
                _filter.Registers[Period] = PerPause;
            }

            _filter.DecodeRegisters();
            break;
        }
    }

    private uint GetBits(int length)
    {
        var index0 = _pc >> 3;
        var index1 = (_pc + 8) >> 3;
        var data0 = _rom[index0 & 0xffff];
        var data1 = _rom[index1 & 0xffff];
        var data = ((uint)data1 << 8 | data0) >> (_pc & 7);
        _pc += length;

        return data & ((1u << length) - 1);
    }

    private static float NormalizeSample(double sample)
    {
        return Math.Clamp((float)(sample / 32768d), -1f, 1f);
    }

    private static uint BitReverse32(uint value)
    {
        value = ((value & 0xffff0000) >> 16) | ((value & 0x0000ffff) << 16);
        value = ((value & 0xff00ff00) >> 8) | ((value & 0x00ff00ff) << 8);
        value = ((value & 0xf0f0f0f0) >> 4) | ((value & 0x0f0f0f0f) << 4);
        value = ((value & 0xcccccccc) >> 2) | ((value & 0x33333333) << 2);
        value = ((value & 0xaaaaaaaa) >> 1) | ((value & 0x55555555) << 1);
        return value;
    }

    private static ushort Encode(int length, int shift, int parameter, bool delta, bool field, bool clearFive, bool clearAll)
    {
        return (ushort)(
            ((length & 0x0f) << 0) |
            ((shift & 0x0f) << 4) |
            ((parameter & 0x0f) << 8) |
            ((delta ? 1 : 0) << 12) |
            ((field ? 1 : 0) << 13) |
            ((clearFive ? 1 : 0) << 14) |
            ((clearAll ? 1 : 0) << 15));
    }

    private static byte[] NormalizeAl2Rom(byte[] romBytes)
    {
        var rom = romBytes.Take(Sp0256RomSize).ToArray();
        if (ComputeSha1Hex(rom) == Al2RomSha1)
        {
            return rom;
        }

        var reversed = rom.Select(ReverseBits).ToArray();
        return ComputeSha1Hex(reversed) == Al2RomSha1
            ? reversed
            : rom;
    }

    private static string ComputeSha1Hex(byte[] bytes)
    {
        return Convert.ToHexString(SHA1.HashData(bytes)).ToLowerInvariant();
    }

    private static byte ReverseBits(byte value)
    {
        value = (byte)(((value & 0xF0) >> 4) | ((value & 0x0F) << 4));
        value = (byte)(((value & 0xCC) >> 2) | ((value & 0x33) << 2));
        value = (byte)(((value & 0xAA) >> 1) | ((value & 0x55) << 1));
        return value;
    }

    private sealed class FilterState
    {
        public int Repeat = -1;
        public int Count;
        public uint Period;
        public uint Random = 1;
        public int Amplitude;
        public readonly short[] ForwardCoefficients = new short[6];
        public readonly short[] BackwardCoefficients = new short[6];
        public readonly short[,] Delay = new short[6, 2];
        public readonly byte[] Registers = new byte[16];
        public bool Interpolate;

        public void Reset()
        {
            Repeat = -1;
            Count = 0;
            Period = 0;
            Random = 1;
            Amplitude = 0;
            Interpolate = false;
            Array.Clear(ForwardCoefficients);
            Array.Clear(BackwardCoefficients);
            Array.Clear(Delay);
            Array.Clear(Registers);
        }

        public void DecodeRegisters()
        {
            Amplitude = DecodeAmplitude(Registers[Sp0256Chip.Amplitude]);
            Count = 0;
            Period = Registers[Sp0256Chip.Period];

            for (var i = 0; i < 6; i++)
            {
                BackwardCoefficients[i] = DecodeCoefficient(Registers[B0 + (i * 2)]);
                ForwardCoefficients[i] = DecodeCoefficient(Registers[F0 + (i * 2)]);
            }

            Interpolate = Registers[InterpolateAmplitude] != 0 || Registers[InterpolatePeriod] != 0;
        }

        public short UpdateOneSample()
        {
            var interpolateNow = false;
            ushort sampleBits = 0;

            if (Period != 0)
            {
                if (Count <= 0)
                {
                    Count += (int)Period;
                    sampleBits = unchecked((ushort)Amplitude);
                    Repeat--;
                    interpolateNow = Interpolate;

                    for (var i = 0; i < 6; i++)
                    {
                        Delay[i, 0] = 0;
                        Delay[i, 1] = 0;
                    }
                }
                else
                {
                    Count--;
                }
            }
            else
            {
                if (--Count <= 0)
                {
                    interpolateNow = Interpolate;
                    Count = PerNoise;
                    Repeat--;

                    for (var i = 0; i < 6; i++)
                    {
                        Delay[i, 0] = 0;
                        Delay[i, 1] = 0;
                    }
                }

                var bit = (Random & 1) != 0;
                Random = (Random >> 1) ^ (bit ? 0x4001u : 0u);
                sampleBits = unchecked((ushort)(bit ? Amplitude : -Amplitude));
            }

            if (interpolateNow)
            {
                Registers[Sp0256Chip.Amplitude] = unchecked((byte)(Registers[Sp0256Chip.Amplitude] + Registers[InterpolateAmplitude]));
                Registers[Sp0256Chip.Period] = unchecked((byte)(Registers[Sp0256Chip.Period] + Registers[InterpolatePeriod]));
                Amplitude = DecodeAmplitude(Registers[Sp0256Chip.Amplitude]);
                Period = Registers[Sp0256Chip.Period];
            }

            if (Repeat <= 0)
            {
                return 0;
            }

            for (var i = 0; i < 6; i++)
            {
                var accumulatedSample = (int)sampleBits;
                accumulatedSample += (BackwardCoefficients[i] * Delay[i, 1]) >> 9;
                accumulatedSample += (ForwardCoefficients[i] * Delay[i, 0]) >> 8;
                sampleBits = unchecked((ushort)accumulatedSample);
                var sample = unchecked((short)sampleBits);

                Delay[i, 1] = Delay[i, 0];
                Delay[i, 0] = sample;
            }

            return (short)(Limit(unchecked((short)sampleBits)) << 2);
        }

        private static int DecodeAmplitude(byte amplitude)
        {
            return (amplitude & 0x1f) << ((amplitude & 0xe0) >> 5);
        }

        private static short DecodeCoefficient(byte value)
        {
            return (value & 0x80) != 0
                ? QuantizationTable[0x7f & -value]
                : (short)-QuantizationTable[value];
        }

        private static short Limit(short value)
        {
            if (value > 8191)
            {
                return 8191;
            }

            if (value < -8192)
            {
                return -8192;
            }

            return value;
        }
    }
}
