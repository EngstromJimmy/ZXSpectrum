using System;
using System.Collections.Generic;
using System.Linq;
using ZXBox.Core.Tape;
using ZXBox.Hardware.Interfaces;
using ZXBox.Hardware.Output;

namespace ZXBox.Core.Hardware.Input
{
    public class TapePlayer : IInput
    {
        private const int TStatesPerMillisecond = 3500;
        private readonly Beeper<byte> _beeper;

        public TapePlayer(Beeper<byte> beeper)
        {
            _beeper = beeper;
        }

        public TapeImage Tape { get; private set; } = new();

        public void LoadTape(byte[] data, string fileName = null)
        {
            LoadTape(TapeFormatFactory.ReadTape(data, fileName));
        }

        public void LoadTape(TapeImage tape)
        {
            ResetTapeState();
            Tape = tape;

            var ear = false;
            long tstate = 0;

            foreach (var block in tape.Blocks)
            {
                switch (block)
                {
                    case TapeDataBlock dataBlock:
                        AppendDataBlock(dataBlock, ref ear, ref tstate);
                        break;
                    case TapePureToneBlock pureToneBlock:
                        AppendPulseRepeats(pureToneBlock.PulseCount, pureToneBlock.PulseLength, PulseTypeEnum.Data, ref ear, ref tstate);
                        break;
                    case TapePulseSequenceBlock pulseSequenceBlock:
                        AppendPulseSequence(pulseSequenceBlock.PulseLengths, PulseTypeEnum.Data, ref ear, ref tstate);
                        break;
                    case TapePauseBlock pauseBlock:
                        AppendPause(pauseBlock.DurationMilliseconds, ref ear, ref tstate);
                        break;
                    case TapeSetSignalLevelBlock signalLevelBlock:
                        ear = signalLevelBlock.High;
                        break;
                    case TapeStopBlock:
                        EarValues.Add(new EarValue { Ear = false, TState = tstate, Pulse = PulseTypeEnum.Stop });
                        break;
                }
            }

            if (EarValues.Count == 0 || EarValues[^1].Pulse != PulseTypeEnum.Stop)
            {
                ear = !ear;
                tstate += 947;
                EarValues.Add(new EarValue { Ear = ear, TState = tstate, Pulse = PulseTypeEnum.Termination });
                EarValues.Add(new EarValue { Ear = false, TState = tstate, Pulse = PulseTypeEnum.Stop });
            }
        }

        public void AddTStates(int tstates)
        {
            if (IsPlaying)
            {
                CurrentTstate += tstates;
            }
        }

        public List<EarValue> EarValues { get; } = new();

        public void Play()
        {
            if (EarValues.Count == 0)
            {
                return;
            }

            TotalTstates = EarValues.Last().TState;
            IsPlaying = true;
        }

        public bool IsPlaying { get; set; }

        public long CurrentTstate;
        public long TotalTstates;

        private byte _returnValue = 0xFF;
        private EarValue _ear;
        private bool _firstRead = true;
        private int _tapePosition;

        public byte Input(ushort Port, int tact)
        {
            if (IsPlaying)
            {
                _returnValue = 0xFF;
                if ((Port & 0xff) == 0xfe)
                {
                    if (_firstRead)
                    {
                        CurrentTstate = 0;
                        _firstRead = false;
                    }

                    for (; _tapePosition < EarValues.Count - 1;)
                    {
                        if (EarValues[_tapePosition + 1].TState < CurrentTstate)
                        {
                            _tapePosition++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    _ear = EarValues[_tapePosition];
                    _beeper?.Output((ushort)0xfe, (byte)((_ear.Ear ? 1 : 0) << 4), tact);
                    if (_ear != null)
                    {
                        if (_ear.Pulse == PulseTypeEnum.Stop)
                        {
                            IsPlaying = false;
                        }

                        if (_ear.Ear)
                        {
                            _returnValue |= 1 << 6;
                            return _returnValue;
                        }

                        _returnValue &= (byte)~(1 << 6);
                        return _returnValue;
                    }
                }

                if (CurrentTstate > TotalTstates)
                {
                    IsPlaying = false;
                }
            }

            return _returnValue;
        }

        private void AppendDataBlock(TapeDataBlock block, ref bool ear, ref long tstate)
        {
            if (block.PilotPulseCount > 0 && block.PilotPulseLength > 0)
            {
                AppendPulseRepeats(block.PilotPulseCount, block.PilotPulseLength, PulseTypeEnum.Pilot, ref ear, ref tstate);
            }

            if (block.SyncFirstPulseLength > 0)
            {
                ear = !ear;
                tstate += block.SyncFirstPulseLength;
                EarValues.Add(new EarValue { Ear = ear, TState = tstate, Pulse = PulseTypeEnum.Sync1 });
            }

            if (block.SyncSecondPulseLength > 0)
            {
                ear = !ear;
                tstate += block.SyncSecondPulseLength;
                EarValues.Add(new EarValue { Ear = ear, TState = tstate, Pulse = PulseTypeEnum.Sync2 });
            }

            var lastByteBits = block.UsedBitsInLastByte is >= 1 and <= 8 ? block.UsedBitsInLastByte : 8;
            for (var byteIndex = 0; byteIndex < block.Data.Length; byteIndex++)
            {
                var bitsInByte = byteIndex == block.Data.Length - 1 ? lastByteBits : 8;
                for (var bitIndex = 0; bitIndex < bitsInByte; bitIndex++)
                {
                    var bitMask = 0x80 >> bitIndex;
                    var signal = (block.Data[byteIndex] & bitMask) == bitMask;
                    var pulseLength = signal ? block.OneBitPulseLength : block.ZeroBitPulseLength;

                    ear = !ear;
                    tstate += pulseLength;
                    EarValues.Add(new EarValue { Ear = ear, TState = tstate, Pulse = PulseTypeEnum.Data });

                    ear = !ear;
                    tstate += pulseLength;
                    EarValues.Add(new EarValue { Ear = ear, TState = tstate, Pulse = PulseTypeEnum.Data });
                }
            }

            AppendPauseOrStop(block.PauseAfterMilliseconds, ref ear, ref tstate);
        }

        private void AppendPulseRepeats(int count, int pulseLength, PulseTypeEnum pulseType, ref bool ear, ref long tstate)
        {
            for (var pulseIndex = 0; pulseIndex < count; pulseIndex++)
            {
                ear = !ear;
                tstate += pulseLength;
                EarValues.Add(new EarValue { Ear = ear, TState = tstate, Pulse = pulseType });
            }
        }

        private void AppendPulseSequence(IReadOnlyList<int> pulseLengths, PulseTypeEnum pulseType, ref bool ear, ref long tstate)
        {
            for (var pulseIndex = 0; pulseIndex < pulseLengths.Count; pulseIndex++)
            {
                ear = !ear;
                tstate += pulseLengths[pulseIndex];
                EarValues.Add(new EarValue { Ear = ear, TState = tstate, Pulse = pulseType });
            }
        }

        private void AppendPause(int durationMilliseconds, ref bool ear, ref long tstate)
        {
            AppendPauseOrStop(durationMilliseconds, ref ear, ref tstate);
        }

        private void AppendPauseOrStop(int durationMilliseconds, ref bool ear, ref long tstate)
        {
            if (durationMilliseconds <= 0)
            {
                EarValues.Add(new EarValue { Ear = false, TState = tstate, Pulse = PulseTypeEnum.Stop });
                return;
            }

            ear = !ear;
            tstate += TStatesPerMillisecond;
            EarValues.Add(new EarValue { Ear = ear, TState = tstate, Pulse = PulseTypeEnum.Pause });

            ear = false;
            tstate += Math.Max(durationMilliseconds - 1, 0) * TStatesPerMillisecond;
            EarValues.Add(new EarValue { Ear = ear, TState = tstate, Pulse = PulseTypeEnum.Pause });
        }

        private void ResetTapeState()
        {
            Tape = new TapeImage();
            EarValues.Clear();
            IsPlaying = false;
            CurrentTstate = 0;
            TotalTstates = 0;
            _returnValue = 0xFF;
            _ear = null;
            _firstRead = true;
            _tapePosition = 0;
        }
    }

    public class EarValue
    {
        public long TState { get; set; }
        public bool Ear { get; set; }
        public PulseTypeEnum Pulse { get; set; }
    }

    public enum PulseTypeEnum
    {
        Data,
        Termination,
        Pilot,
        Sync1,
        Sync2,
        Stop,
        Pause
    }
}
