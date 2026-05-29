using System;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace ZXBox.Blazor.Interop;

[SupportedOSPlatform("browser")]
internal static partial class BrowserRuntimeInterop
{
    private const string AudioModuleName = "ZXBoxAudioInterop";
    private static Task<JSObject>? _audioModuleTask;

    internal static Task EnsureAudioModuleAsync()
    {
        return _audioModuleTask ??= JSHost.ImportAsync(AudioModuleName, "/audioInterop.js");
    }

    [JSImport("globalThis.getKeyMask")]
    internal static partial double GetKeyMask();

    [JSImport("globalThis.getKempstonState")]
    internal static partial int GetKempstonState();

    [JSImport("createAudioController", AudioModuleName)]
    internal static partial Task<JSObject> CreateAudioControllerAsync(
        int requestedSampleRate,
        float gain,
        double targetQueuedSeconds,
        double lowWatermarkSeconds);

    [JSImport("disposeAudioController", AudioModuleName)]
    internal static partial Task DisposeAudioControllerAsync(JSObject controller);

    [JSImport("resumeAudioController", AudioModuleName)]
    internal static partial Task<bool> ResumeAudioControllerAsync(JSObject controller);

    [JSImport("startAudioPump", AudioModuleName)]
    internal static partial void StartAudioPump(JSObject controller, int instanceId);

    [JSImport("stopAudioPump", AudioModuleName)]
    internal static partial void StopAudioPump(JSObject controller);

    [JSImport("pushPendingAudioFrame", AudioModuleName)]
    internal static partial void PushPendingAudioFrame(JSObject controller, int sampleCount);

    [JSImport("getAudioSampleRate", AudioModuleName)]
    internal static partial int GetAudioSampleRate(JSObject controller);

    [JSImport("getQueuedSeconds", AudioModuleName)]
    internal static partial double GetQueuedSeconds(JSObject controller);

    [JSImport("isAudioDriven", AudioModuleName)]
    internal static partial bool IsAudioDriven(JSObject controller);

    [JSImport("resetPerformanceMetrics", AudioModuleName)]
    internal static partial void ResetPerformanceMetrics(JSObject controller);

    [JSImport("resetControllerForMeasurement", AudioModuleName)]
    internal static partial void ResetControllerForMeasurement(JSObject controller);

    [JSImport("getPerformanceMetricsJson", AudioModuleName)]
    internal static partial string GetPerformanceMetricsJson(JSObject controller);
}
