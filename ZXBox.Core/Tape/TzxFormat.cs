using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace ZXBox.Core.Tape;

public class TzxFormat
{
    private static readonly byte[] Signature = Encoding.ASCII.GetBytes("ZXTape!");

    public static bool IsTzx(byte[] data, string fileName = null)
    {
        if (data.Length >= 8 &&
            data[0] == (byte)'Z' &&
            data[1] == (byte)'X' &&
            data[2] == (byte)'T' &&
            data[3] == (byte)'a' &&
            data[4] == (byte)'p' &&
            data[5] == (byte)'e' &&
            data[6] == (byte)'!' &&
            data[7] == 0x1A)
        {
            return true;
        }

        return !string.IsNullOrWhiteSpace(fileName) &&
               Path.GetExtension(fileName).Equals(".tzx", StringComparison.OrdinalIgnoreCase);
    }

    public TapeImage ReadFile(byte[] data)
    {
        using MemoryStream stream = new(data);
        using BinaryReader reader = new(stream);

        ValidateHeader(reader);

        var blocks = new List<object>();
        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            blocks.Add(ReadBlock(reader));
        }

        return ResolveBlocks(blocks);
    }

    private static void ValidateHeader(BinaryReader reader)
    {
        var header = reader.ReadBytes(10);
        if (header.Length < 10)
        {
            throw new InvalidDataException("Invalid TZX file header.");
        }

        for (var i = 0; i < Signature.Length; i++)
        {
            if (header[i] != Signature[i])
            {
                throw new InvalidDataException("Missing TZX signature.");
            }
        }

        if (header[7] != 0x1A)
        {
            throw new InvalidDataException("Invalid TZX end-of-text marker.");
        }
    }

    private static object ReadBlock(BinaryReader reader)
    {
        return reader.ReadByte() switch
        {
            0x10 => ReadStandardSpeedDataBlock(reader),
            0x11 => ReadTurboSpeedDataBlock(reader),
            0x12 => new TapePureToneBlock
            {
                PulseLength = reader.ReadUInt16(),
                PulseCount = reader.ReadUInt16()
            },
            0x13 => ReadPulseSequenceBlock(reader),
            0x14 => ReadPureDataBlock(reader),
            0x15 => ReadDirectRecordingBlock(reader),
            0x18 => ReadCswRecordingBlock(reader),
            0x19 => ReadGeneralizedDataBlock(reader),
            0x20 => ReadPauseOrStopBlock(reader),
            0x21 => SkipGroupStart(reader),
            0x22 => NoOpBlock.Instance,
            0x23 => new JumpBlock(reader.ReadInt16()),
            0x24 => new LoopStartBlock(reader.ReadUInt16()),
            0x25 => LoopEndBlock.Instance,
            0x26 => ReadCallSequenceBlock(reader),
            0x27 => ReturnFromSequenceBlock.Instance,
            0x28 => ReadSelectBlock(reader),
            0x2A => SkipLengthPrefixedDwordBlock(reader),
            0x2B => ReadSignalLevelBlock(reader),
            0x30 => SkipByteLengthTextBlock(reader),
            0x31 => SkipMessageBlock(reader),
            0x32 => SkipLengthPrefixedWordBlock(reader),
            0x33 => SkipHardwareTypeBlock(reader),
            0x35 => SkipCustomInfoBlock(reader),
            0x5A => SkipGlueBlock(reader),
            var blockId => throw new NotSupportedException($"Unsupported TZX block 0x{blockId:X2}.")
        };
    }

    private static TapeDataBlock ReadStandardSpeedDataBlock(BinaryReader reader)
    {
        var pauseAfterMilliseconds = reader.ReadUInt16();
        var dataLength = reader.ReadUInt16();
        var blockData = reader.ReadBytes(dataLength);

        return new TapeDataBlock
        {
            Data = blockData,
            PilotPulseLength = 2168,
            PilotPulseCount = blockData.Length > 0 && blockData[0] < 0x80 ? 8063 : 3223,
            SyncFirstPulseLength = 667,
            SyncSecondPulseLength = 735,
            ZeroBitPulseLength = 855,
            OneBitPulseLength = 1710,
            UsedBitsInLastByte = 8,
            PauseAfterMilliseconds = pauseAfterMilliseconds
        };
    }

    private static TapeDataBlock ReadTurboSpeedDataBlock(BinaryReader reader)
    {
        var block = new TapeDataBlock
        {
            PilotPulseLength = reader.ReadUInt16(),
            SyncFirstPulseLength = reader.ReadUInt16(),
            SyncSecondPulseLength = reader.ReadUInt16(),
            ZeroBitPulseLength = reader.ReadUInt16(),
            OneBitPulseLength = reader.ReadUInt16(),
            PilotPulseCount = reader.ReadUInt16(),
            UsedBitsInLastByte = reader.ReadByte(),
            PauseAfterMilliseconds = reader.ReadUInt16(),
            Data = reader.ReadBytes(ReadUInt24(reader))
        };

        return block;
    }

    private static TapePulseSequenceBlock ReadPulseSequenceBlock(BinaryReader reader)
    {
        var pulseCount = reader.ReadByte();
        var pulses = new int[pulseCount];

        for (var pulseIndex = 0; pulseIndex < pulseCount; pulseIndex++)
        {
            pulses[pulseIndex] = reader.ReadUInt16();
        }

        return new TapePulseSequenceBlock
        {
            PulseLengths = pulses
        };
    }

    private static TapeDataBlock ReadPureDataBlock(BinaryReader reader)
    {
        var block = new TapeDataBlock
        {
            PilotPulseLength = 0,
            PilotPulseCount = 0,
            SyncFirstPulseLength = 0,
            SyncSecondPulseLength = 0,
            ZeroBitPulseLength = reader.ReadUInt16(),
            OneBitPulseLength = reader.ReadUInt16(),
            UsedBitsInLastByte = reader.ReadByte(),
            PauseAfterMilliseconds = reader.ReadUInt16(),
            Data = reader.ReadBytes(ReadUInt24(reader))
        };

        return block;
    }

    private static TapeDirectRecordingBlock ReadDirectRecordingBlock(BinaryReader reader)
    {
        return new TapeDirectRecordingBlock
        {
            TStatesPerSample = reader.ReadUInt16(),
            PauseAfterMilliseconds = reader.ReadUInt16(),
            UsedBitsInLastByte = reader.ReadByte(),
            Data = reader.ReadBytes(ReadUInt24(reader))
        };
    }

    private static TapeCswRecordingBlock ReadCswRecordingBlock(BinaryReader reader)
    {
        var blockLength = reader.ReadUInt32();
        var pauseAfterMilliseconds = reader.ReadUInt16();
        var samplingRate = ReadUInt24(reader);
        var compressionType = reader.ReadByte();
        reader.ReadUInt32();

        var encodedData = reader.ReadBytes((int)blockLength - 10);
        var pulseData = compressionType switch
        {
            1 => encodedData,
            2 => InflateCswPulseData(encodedData),
            var type => throw new NotSupportedException($"Unsupported CSW compression type {type}.")
        };

        return new TapeCswRecordingBlock
        {
            PauseAfterMilliseconds = pauseAfterMilliseconds,
            PulseLengths = DecodeCswPulseLengths(pulseData, samplingRate)
        };
    }

    private static TapeGeneralizedDataBlock ReadGeneralizedDataBlock(BinaryReader reader)
    {
        var blockLength = reader.ReadUInt32();
        var blockStart = reader.BaseStream.Position;
        var pauseAfterMilliseconds = reader.ReadUInt16();
        var totalPilotSymbols = reader.ReadUInt32();
        var maxPilotPulses = reader.ReadByte();
        var pilotSymbolCount = ReadAlphabetSize(reader.ReadByte());
        var totalDataSymbols = reader.ReadUInt32();
        var maxDataPulses = reader.ReadByte();
        var dataSymbolCount = ReadAlphabetSize(reader.ReadByte());

        var pilotSymbols = totalPilotSymbols > 0 ? ReadGeneralizedSymbols(reader, maxPilotPulses, pilotSymbolCount) : Array.Empty<TapeGeneralizedSymbol>();
        var pilotRuns = totalPilotSymbols > 0 ? ReadPilotRuns(reader, totalPilotSymbols) : Array.Empty<TapeGeneralizedSymbolRun>();
        var dataSymbols = totalDataSymbols > 0 ? ReadGeneralizedSymbols(reader, maxDataPulses, dataSymbolCount) : Array.Empty<TapeGeneralizedSymbol>();
        var bitsPerDataSymbol = GetPackedSymbolBitCount(dataSymbolCount);
        var encodedDataSymbols = totalDataSymbols > 0
            ? reader.ReadBytes((int)((bitsPerDataSymbol * totalDataSymbols + 7) / 8))
            : Array.Empty<byte>();

        if (reader.BaseStream.Position - blockStart != blockLength)
        {
            throw new InvalidDataException("TZX generalized data block length mismatch.");
        }

        return new TapeGeneralizedDataBlock
        {
            PilotSymbols = pilotSymbols,
            PilotRuns = pilotRuns,
            DataSymbols = dataSymbols,
            DataSymbolCount = (int)totalDataSymbols,
            EncodedDataSymbols = encodedDataSymbols,
            PauseAfterMilliseconds = pauseAfterMilliseconds
        };
    }

    private static CallSequenceBlock ReadCallSequenceBlock(BinaryReader reader)
    {
        var callCount = reader.ReadUInt16();
        var offsets = new short[callCount];
        for (var callIndex = 0; callIndex < callCount; callIndex++)
        {
            offsets[callIndex] = reader.ReadInt16();
        }

        return new CallSequenceBlock(offsets);
    }

    private static SelectBlock ReadSelectBlock(BinaryReader reader)
    {
        var blockLength = reader.ReadUInt16();
        var count = reader.ReadByte();
        var offsets = new short[count];
        var consumed = 1;

        for (var optionIndex = 0; optionIndex < count; optionIndex++)
        {
            offsets[optionIndex] = reader.ReadInt16();
            var descriptionLength = reader.ReadByte();
            reader.ReadBytes(descriptionLength);
            consumed += 3 + descriptionLength;
        }

        if (consumed != blockLength)
        {
            throw new InvalidDataException("TZX select block length mismatch.");
        }

        return new SelectBlock(offsets);
    }

    private static IReadOnlyList<TapeGeneralizedSymbol> ReadGeneralizedSymbols(BinaryReader reader, int maxPulseCount, int symbolCount)
    {
        var symbols = new List<TapeGeneralizedSymbol>(symbolCount);
        for (var symbolIndex = 0; symbolIndex < symbolCount; symbolIndex++)
        {
            var flags = reader.ReadByte();
            var pulseLengths = new List<int>(maxPulseCount);
            for (var pulseIndex = 0; pulseIndex < maxPulseCount; pulseIndex++)
            {
                var pulseLength = reader.ReadUInt16();
                if (pulseLength != 0)
                {
                    pulseLengths.Add(pulseLength);
                }
            }

            symbols.Add(new TapeGeneralizedSymbol
            {
                Flags = flags,
                PulseLengths = pulseLengths
            });
        }

        return symbols;
    }

    private static IReadOnlyList<TapeGeneralizedSymbolRun> ReadPilotRuns(BinaryReader reader, uint totalPilotSymbols)
    {
        var runs = new List<TapeGeneralizedSymbolRun>();
        uint decodedSymbols = 0;
        while (decodedSymbols < totalPilotSymbols)
        {
            var symbolIndex = reader.ReadByte();
            var repeatCount = reader.ReadUInt16();
            decodedSymbols += repeatCount;
            if (decodedSymbols > totalPilotSymbols)
            {
                throw new InvalidDataException("TZX generalized pilot run exceeds the declared symbol count.");
            }

            runs.Add(new TapeGeneralizedSymbolRun
            {
                SymbolIndex = symbolIndex,
                RepeatCount = repeatCount
            });
        }

        return runs;
    }

    private static TapeBlock ReadPauseOrStopBlock(BinaryReader reader)
    {
        var durationMilliseconds = reader.ReadUInt16();
        if (durationMilliseconds == 0)
        {
            return new TapeStopBlock();
        }

        return new TapePauseBlock
        {
            DurationMilliseconds = durationMilliseconds
        };
    }

    private static object SkipGroupStart(BinaryReader reader)
    {
        var length = reader.ReadByte();
        reader.ReadBytes(length);
        return NoOpBlock.Instance;
    }

    private static object SkipByteLengthTextBlock(BinaryReader reader)
    {
        var length = reader.ReadByte();
        reader.ReadBytes(length);
        return NoOpBlock.Instance;
    }

    private static object SkipMessageBlock(BinaryReader reader)
    {
        reader.ReadByte();
        var length = reader.ReadByte();
        reader.ReadBytes(length);
        return NoOpBlock.Instance;
    }

    private static object SkipLengthPrefixedWordBlock(BinaryReader reader)
    {
        var length = reader.ReadUInt16();
        reader.ReadBytes(length);
        return NoOpBlock.Instance;
    }

    private static object SkipLengthPrefixedDwordBlock(BinaryReader reader)
    {
        var length = reader.ReadUInt32();
        reader.ReadBytes((int)length);
        return NoOpBlock.Instance;
    }

    private static object SkipHardwareTypeBlock(BinaryReader reader)
    {
        var count = reader.ReadByte();
        reader.ReadBytes(count * 3);
        return NoOpBlock.Instance;
    }

    private static object SkipCustomInfoBlock(BinaryReader reader)
    {
        reader.ReadBytes(10);
        var length = reader.ReadUInt32();
        reader.ReadBytes((int)length);
        return NoOpBlock.Instance;
    }

    private static object SkipGlueBlock(BinaryReader reader)
    {
        reader.ReadBytes(9);
        return NoOpBlock.Instance;
    }

    private static TapeSetSignalLevelBlock ReadSignalLevelBlock(BinaryReader reader)
    {
        var length = reader.ReadUInt32();
        var level = reader.ReadByte();
        if (length > 1)
        {
            reader.ReadBytes((int)length - 1);
        }

        return new TapeSetSignalLevelBlock
        {
            High = level != 0
        };
    }

    private static int ReadUInt24(BinaryReader reader)
    {
        var b0 = reader.ReadByte();
        var b1 = reader.ReadByte();
        var b2 = reader.ReadByte();
        return b0 | (b1 << 8) | (b2 << 16);
    }

    private static TapeImage ResolveBlocks(List<object> blocks)
    {
        var tapeImage = new TapeImage();
        var loopStack = new Stack<LoopState>();
        var callStack = new Stack<CallState>();
        var blockIndex = 0;
        var guard = 0;

        while (blockIndex < blocks.Count)
        {
            guard++;
            if (guard > blocks.Count * 1024)
            {
                throw new InvalidDataException("TZX control flow exceeded safety limit.");
            }

            switch (blocks[blockIndex])
            {
                case TapeBlock tapeBlock:
                    tapeImage.Blocks.Add(tapeBlock);
                    blockIndex++;
                    break;
                case JumpBlock jumpBlock:
                    if (jumpBlock.RelativeOffset == 0)
                    {
                        throw new InvalidDataException("TZX jump block cannot jump to itself.");
                    }

                    blockIndex += jumpBlock.RelativeOffset;
                    if (blockIndex < 0 || blockIndex > blocks.Count)
                    {
                        throw new InvalidDataException("TZX jump moved outside the tape.");
                    }

                    break;
                case LoopStartBlock loopStartBlock:
                    loopStack.Push(new LoopState(blockIndex + 1, loopStartBlock.Repetitions));
                    blockIndex++;
                    break;
                case LoopEndBlock:
                    if (loopStack.Count == 0)
                    {
                        throw new InvalidDataException("TZX loop end encountered without a loop start.");
                    }

                    var loopState = loopStack.Pop();
                    if (loopState.RemainingRepetitions > 1)
                    {
                        loopState.RemainingRepetitions--;
                        loopStack.Push(loopState);
                        blockIndex = loopState.BlockIndex;
                    }
                    else
                    {
                        blockIndex++;
                    }

                    break;
                case CallSequenceBlock callSequenceBlock:
                    if (callSequenceBlock.RelativeOffsets.Count == 0)
                    {
                        blockIndex++;
                        break;
                    }

                    callStack.Push(new CallState(blockIndex + 1, callSequenceBlock.RelativeOffsets));
                    blockIndex += callSequenceBlock.RelativeOffsets[0];
                    if (blockIndex < 0 || blockIndex > blocks.Count)
                    {
                        throw new InvalidDataException("TZX call sequence moved outside the tape.");
                    }

                    break;
                case ReturnFromSequenceBlock:
                    if (callStack.Count == 0)
                    {
                        throw new InvalidDataException("TZX return encountered without a matching call sequence.");
                    }

                    var callState = callStack.Pop();
                    if (callState.NextCallIndex < callState.RelativeOffsets.Count)
                    {
                        var nextCallIndex = callState.NextCallIndex;
                        callState.NextCallIndex++;
                        callStack.Push(callState);
                        blockIndex = callState.CallBlockIndex + callState.RelativeOffsets[nextCallIndex];
                    }
                    else
                    {
                        blockIndex = callState.ReturnBlockIndex;
                    }

                    if (blockIndex < 0 || blockIndex > blocks.Count)
                    {
                        throw new InvalidDataException("TZX return moved outside the tape.");
                    }

                    break;
                case SelectBlock selectBlock:
                    if (selectBlock.RelativeOffsets.Count == 0)
                    {
                        blockIndex++;
                        break;
                    }

                    blockIndex += selectBlock.RelativeOffsets[0];
                    if (blockIndex < 0 || blockIndex > blocks.Count)
                    {
                        throw new InvalidDataException("TZX select block moved outside the tape.");
                    }

                    break;
                case NoOpBlock:
                    blockIndex++;
                    break;
                default:
                    throw new InvalidDataException($"Unsupported TZX control block type '{blocks[blockIndex].GetType().Name}'.");
            }
        }

        return tapeImage;
    }

    private sealed class JumpBlock(short relativeOffset)
    {
        public short RelativeOffset { get; } = relativeOffset;
    }

    private sealed class LoopStartBlock(ushort repetitions)
    {
        public ushort Repetitions { get; } = repetitions;
    }

    private sealed class LoopEndBlock
    {
        public static LoopEndBlock Instance { get; } = new();
    }

    private sealed class CallSequenceBlock(IReadOnlyList<short> relativeOffsets)
    {
        public IReadOnlyList<short> RelativeOffsets { get; } = relativeOffsets;
    }

    private sealed class ReturnFromSequenceBlock
    {
        public static ReturnFromSequenceBlock Instance { get; } = new();
    }

    private sealed class SelectBlock(IReadOnlyList<short> relativeOffsets)
    {
        public IReadOnlyList<short> RelativeOffsets { get; } = relativeOffsets;
    }

    private sealed class NoOpBlock
    {
        public static NoOpBlock Instance { get; } = new();
    }

    private sealed class LoopState(int blockIndex, int remainingRepetitions)
    {
        public int BlockIndex { get; } = blockIndex;

        public int RemainingRepetitions { get; set; } = remainingRepetitions;
    }

    private sealed class CallState(int returnBlockIndex, IReadOnlyList<short> relativeOffsets)
    {
        public int ReturnBlockIndex { get; } = returnBlockIndex;

        public int CallBlockIndex { get; } = returnBlockIndex - 1;

        public IReadOnlyList<short> RelativeOffsets { get; } = relativeOffsets;

        public int NextCallIndex { get; set; } = 1;
    }

    private static byte[] InflateCswPulseData(byte[] encodedData)
    {
        using MemoryStream input = new(encodedData);
        using ZLibStream zlibStream = new(input, CompressionMode.Decompress);
        using MemoryStream output = new();
        zlibStream.CopyTo(output);
        return output.ToArray();
    }

    private static IReadOnlyList<int> DecodeCswPulseLengths(byte[] encodedData, int samplingRate)
    {
        var pulseLengths = new List<int>();
        var position = 0;
        while (position < encodedData.Length)
        {
            uint sampleCount = encodedData[position++];
            if (sampleCount == 0)
            {
                if (position + 4 > encodedData.Length)
                {
                    throw new InvalidDataException("Invalid CSW pulse data.");
                }

                sampleCount = BitConverter.ToUInt32(encodedData, position);
                position += 4;
            }

            pulseLengths.Add(SamplesToTStates(sampleCount, samplingRate));
        }

        return pulseLengths;
    }

    private static int SamplesToTStates(uint sampleCount, int samplingRate)
    {
        if (samplingRate <= 0)
        {
            throw new InvalidDataException("CSW sampling rate must be positive.");
        }

        return (int)Math.Max(1, Math.Round(sampleCount * 3500000d / samplingRate));
    }

    private static int ReadAlphabetSize(byte encodedAlphabetSize) => encodedAlphabetSize == 0 ? 256 : encodedAlphabetSize;

    private static int GetPackedSymbolBitCount(int alphabetSize)
    {
        var bitsPerSymbol = 0;
        while ((1 << bitsPerSymbol) < alphabetSize)
        {
            bitsPerSymbol++;
        }

        return bitsPerSymbol;
    }
}
