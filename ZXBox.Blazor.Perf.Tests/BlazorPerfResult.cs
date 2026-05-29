namespace ZXBox.Blazor.Perf.Tests;

public sealed class BlazorPerfResult
{
    public string Game { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public double WarmupSeconds { get; set; }

    public double MeasureSeconds { get; set; }

    public int DisplayFrameDivisor { get; set; }

    public int AudioFramesPerBatch { get; set; }

    public int SchedulerCallbacksObserved { get; set; }

    public int TimerTicksObserved { get; set; }

    public int FramesExecuted { get; set; }

    public int FramesSkipped { get; set; }

    public int PaintInvalidations { get; set; }

    public int PaintSurfaceCalls { get; set; }

    public double LoopFps { get; set; }

    public double RequestedPaintFps { get; set; }

    public double PresentedPaintFps { get; set; }

    public double AverageGameLoopMs { get; set; }

    public double MaxGameLoopMs { get; set; }

    public double AverageCpuMs { get; set; }

    public double MaxCpuMs { get; set; }

    public double AverageAudioMs { get; set; }

    public double MaxAudioMs { get; set; }

    public double AveragePaintRequestMs { get; set; }

    public double MaxPaintRequestMs { get; set; }

    public double AverageOnPaintSurfaceMs { get; set; }

    public double MaxOnPaintSurfaceMs { get; set; }

    public BlazorPerfAudioMetrics Audio { get; set; } = new();

    public BlazorPerfRafMetrics Raf { get; set; } = new();
}

public sealed class BlazorPerfAudioMetrics
{
    public int EnqueueCount { get; set; }

    public double AverageEnqueueMs { get; set; }

    public double MaxEnqueueMs { get; set; }

    public int UnderrunCount { get; set; }

    public double MaxQueuedAheadSeconds { get; set; }

    public string ContextState { get; set; } = string.Empty;

    public double CurrentTimeSeconds { get; set; }

    public int SampleRate { get; set; }

    public long SamplesPushed { get; set; }

    public double GeneratedSeconds { get; set; }

    public bool AudioDriven { get; set; }
}

public sealed class BlazorPerfRafMetrics
{
    public int FrameCount { get; set; }

    public double ElapsedSeconds { get; set; }

    public double Fps { get; set; }

    public int HiddenTransitions { get; set; }

    public string VisibilityState { get; set; } = string.Empty;
}
