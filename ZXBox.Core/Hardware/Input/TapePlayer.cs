using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using ZXBox.Core.Tape;
using ZXBox.Hardware.Interfaces;

namespace ZXBox.Core.Hardware.Input
{
    public class TapePlayer : IInput
    {
        private const int TStatesPerMillisecond = 3500;
        private readonly IOutput? _beeper;
        private readonly List<TapePlaybackPosition> _blockPlaybackPositions = new();

        public TapePlayer(IOutput? beeper)
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
                _blockPlaybackPositions.Add(new TapePlaybackPosition(EarValues.Count, tstate));
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
                    case TapeDirectRecordingBlock directRecordingBlock:
                        AppendDirectRecordingBlock(directRecordingBlock, ref ear, ref tstate);
                        break;
                    case TapeCswRecordingBlock cswRecordingBlock:
                        AppendCswRecordingBlock(cswRecordingBlock, ref ear, ref tstate);
                        break;
                    case TapeGeneralizedDataBlock generalizedDataBlock:
                        AppendGeneralizedDataBlock(generalizedDataBlock, ref ear, ref tstate);
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

            TotalTstates = EarValues.Count > 0 ? EarValues[^1].TState : 0;
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
            if (EarValues.Count == 0 || _currentBlockIndex >= Tape.Blocks.Count)
            {
                return;
            }

            MoveToBlock(_currentBlockIndex);
            IsPlaying = true;
        }

        public bool TryConsumeNextQuickLoadBlock(out TapeDataBlock block)
        {
            block = null;
            if (!IsPlaying || _currentBlockIndex >= Tape.Blocks.Count)
            {
                return false;
            }

            if (Tape.Blocks[_currentBlockIndex] is not TapeDataBlock dataBlock || !dataBlock.IsQuickLoadCandidate)
            {
                return false;
            }

            block = dataBlock;
            MoveToBlock(_currentBlockIndex + 1);
            IsPlaying = _currentBlockIndex < Tape.Blocks.Count;
            return true;
        }

        public bool IsPlaying { get; set; }

        public long CurrentTstate;
        public long TotalTstates;

        private byte _returnValue = 0xFF;
        private EarValue _ear;
        private bool _firstRead = true;
        private int _currentBlockIndex;
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
                        if (_currentBlockIndex < _blockPlaybackPositions.Count)
                        {
                            CurrentTstate = _blockPlaybackPositions[_currentBlockIndex].TState;
                        }
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

                        _returnValue &= unchecked((byte)~(1 << 6));
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

        private void AppendDirectRecordingBlock(TapeDirectRecordingBlock block, ref bool ear, ref long tstate)
        {
            var lastByteBits = block.UsedBitsInLastByte is >= 1 and <= 8 ? block.UsedBitsInLastByte : 8;
            for (var byteIndex = 0; byteIndex < block.Data.Length; byteIndex++)
            {
                var bitsInByte = byteIndex == block.Data.Length - 1 ? lastByteBits : 8;
                for (var bitIndex = 0; bitIndex < bitsInByte; bitIndex++)
                {
                    var bitMask = 0x80 >> bitIndex;
                    var signal = (block.Data[byteIndex] & bitMask) == bitMask;
                    if (signal != ear)
                    {
                        ear = signal;
                        EarValues.Add(new EarValue { Ear = ear, TState = tstate, Pulse = PulseTypeEnum.Data });
                    }

                    tstate += block.TStatesPerSample;
                }
            }

            AppendPauseOrStop(block.PauseAfterMilliseconds, ref ear, ref tstate);
        }

        private void AppendCswRecordingBlock(TapeCswRecordingBlock block, ref bool ear, ref long tstate)
        {
            AppendPulseSequence(block.PulseLengths, PulseTypeEnum.Data, ref ear, ref tstate);
            AppendPauseOrStop(block.PauseAfterMilliseconds, ref ear, ref tstate);
        }

        private void AppendGeneralizedDataBlock(TapeGeneralizedDataBlock block, ref bool ear, ref long tstate)
        {
            foreach (var pilotRun in block.PilotRuns)
            {
                if (pilotRun.SymbolIndex < 0 || pilotRun.SymbolIndex >= block.PilotSymbols.Count)
                {
                    throw new InvalidDataException("Generalized data block references an invalid pilot symbol.");
                }

                for (var repetition = 0; repetition < pilotRun.RepeatCount; repetition++)
                {
                    AppendGeneralizedSymbol(block.PilotSymbols[pilotRun.SymbolIndex], ref ear, ref tstate);
                }
            }

            var bitsPerSymbol = GetPackedSymbolBitCount(block.DataSymbols.Count);
            for (var symbolIndex = 0; symbolIndex < block.DataSymbolCount; symbolIndex++)
            {
                var dataSymbolIndex = ReadPackedSymbolIndex(block.EncodedDataSymbols, bitsPerSymbol, symbolIndex);
                if (dataSymbolIndex < 0 || dataSymbolIndex >= block.DataSymbols.Count)
                {
                    throw new InvalidDataException("Generalized data block references an invalid data symbol.");
                }

                AppendGeneralizedSymbol(block.DataSymbols[dataSymbolIndex], ref ear, ref tstate);
            }

            AppendPauseOrStop(block.PauseAfterMilliseconds, ref ear, ref tstate);
        }

        private void AppendGeneralizedSymbol(TapeGeneralizedSymbol symbol, ref bool ear, ref long tstate)
        {
            if (symbol.PulseLengths.Count == 0)
            {
                return;
            }

            ear = (symbol.Flags & 0x03) switch
            {
                0 => !ear,
                1 => ear,
                2 => false,
                3 => true,
                _ => ear
            };

            for (var pulseIndex = 0; pulseIndex < symbol.PulseLengths.Count; pulseIndex++)
            {
                if (pulseIndex > 0)
                {
                    ear = !ear;
                }

                tstate += symbol.PulseLengths[pulseIndex];
                EarValues.Add(new EarValue { Ear = ear, TState = tstate, Pulse = PulseTypeEnum.Data });
            }
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
            _blockPlaybackPositions.Clear();
            IsPlaying = false;
            CurrentTstate = 0;
            TotalTstates = 0;
            _returnValue = 0xFF;
            _ear = null;
            _firstRead = true;
            _currentBlockIndex = 0;
            _tapePosition = 0;
        }

        private void MoveToBlock(int blockIndex)
        {
            _currentBlockIndex = Math.Clamp(blockIndex, 0, Tape.Blocks.Count);
            _firstRead = true;

            if (_currentBlockIndex >= Tape.Blocks.Count)
            {
                CurrentTstate = TotalTstates;
                _tapePosition = EarValues.Count > 0 ? EarValues.Count - 1 : 0;
                return;
            }

            var playbackPosition = _blockPlaybackPositions[_currentBlockIndex];
            CurrentTstate = playbackPosition.TState;
            _tapePosition = playbackPosition.EarValueIndex;
        }

        private static int GetPackedSymbolBitCount(int alphabetSize)
        {
            var bitsPerSymbol = 0;
            while ((1 << bitsPerSymbol) < alphabetSize)
            {
                bitsPerSymbol++;
            }

            return bitsPerSymbol;
        }

        private static int ReadPackedSymbolIndex(byte[] data, int bitsPerSymbol, int symbolIndex)
        {
            if (bitsPerSymbol == 0)
            {
                return 0;
            }

            var bitOffset = symbolIndex * bitsPerSymbol;
            var value = 0;
            for (var bitIndex = 0; bitIndex < bitsPerSymbol; bitIndex++)
            {
                var absoluteBitIndex = bitOffset + bitIndex;
                var sourceByte = data[absoluteBitIndex / 8];
                var bit = (sourceByte >> (7 - (absoluteBitIndex % 8))) & 0x01;
                value = (value << 1) | bit;
            }

            return value;
        }

        private sealed record TapePlaybackPosition(int EarValueIndex, long TState);
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
