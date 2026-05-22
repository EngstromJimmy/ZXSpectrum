using System;
using System.Collections.Generic;
using System.IO;
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
            0x20 => ReadPauseOrStopBlock(reader),
            0x21 => SkipGroupStart(reader),
            0x22 => NoOpBlock.Instance,
            0x23 => new JumpBlock(reader.ReadInt16()),
            0x24 => new LoopStartBlock(reader.ReadUInt16()),
            0x25 => LoopEndBlock.Instance,
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

    private sealed class NoOpBlock
    {
        public static NoOpBlock Instance { get; } = new();
    }

    private sealed class LoopState(int blockIndex, int remainingRepetitions)
    {
        public int BlockIndex { get; } = blockIndex;

        public int RemainingRepetitions { get; set; } = remainingRepetitions;
    }
}
