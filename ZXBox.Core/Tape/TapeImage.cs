using System;
using System.Collections.Generic;

namespace ZXBox.Core.Tape;

public sealed class TapeImage
{
    public List<TapeBlock> Blocks { get; } = new();
}

public abstract class TapeBlock
{
}

public sealed class TapeDataBlock : TapeBlock
{
    public required byte[] Data { get; init; }

    public required int PilotPulseLength { get; init; }

    public required int PilotPulseCount { get; init; }

    public required int SyncFirstPulseLength { get; init; }

    public required int SyncSecondPulseLength { get; init; }

    public required int ZeroBitPulseLength { get; init; }

    public required int OneBitPulseLength { get; init; }

    public required int UsedBitsInLastByte { get; init; }

    public required int PauseAfterMilliseconds { get; init; }

    public bool IsQuickLoadCandidate => UsedBitsInLastByte == 8 && Data.Length >= 2;
}

public sealed class TapePureToneBlock : TapeBlock
{
    public required int PulseLength { get; init; }

    public required int PulseCount { get; init; }
}

public sealed class TapePulseSequenceBlock : TapeBlock
{
    public required IReadOnlyList<int> PulseLengths { get; init; }
}

public sealed class TapePauseBlock : TapeBlock
{
    public required int DurationMilliseconds { get; init; }
}

public sealed class TapeDirectRecordingBlock : TapeBlock
{
    public required int TStatesPerSample { get; init; }

    public required int PauseAfterMilliseconds { get; init; }

    public required int UsedBitsInLastByte { get; init; }

    public required byte[] Data { get; init; }
}

public sealed class TapeCswRecordingBlock : TapeBlock
{
    public required IReadOnlyList<int> PulseLengths { get; init; }

    public required int PauseAfterMilliseconds { get; init; }
}

public sealed class TapeGeneralizedDataBlock : TapeBlock
{
    public required IReadOnlyList<TapeGeneralizedSymbol> PilotSymbols { get; init; }

    public required IReadOnlyList<TapeGeneralizedSymbolRun> PilotRuns { get; init; }

    public required IReadOnlyList<TapeGeneralizedSymbol> DataSymbols { get; init; }

    public required int DataSymbolCount { get; init; }

    public required byte[] EncodedDataSymbols { get; init; }

    public required int PauseAfterMilliseconds { get; init; }
}

public sealed class TapeGeneralizedSymbol
{
    public required byte Flags { get; init; }

    public required IReadOnlyList<int> PulseLengths { get; init; }
}

public sealed class TapeGeneralizedSymbolRun
{
    public required int SymbolIndex { get; init; }

    public required int RepeatCount { get; init; }
}

public sealed class TapeStopBlock : TapeBlock
{
}

public sealed class TapeSetSignalLevelBlock : TapeBlock
{
    public required bool High { get; init; }
}
