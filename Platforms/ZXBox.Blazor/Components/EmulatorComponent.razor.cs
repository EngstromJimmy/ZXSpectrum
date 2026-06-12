using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using SkiaSharp.Views.Blazor;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using ZXBox.Blazor.Interop;
using ZXBox.Core.Hardware.Input;
using ZXBox.Hardware.Input;
using ZXBox.Hardware.Input.Joystick;
using ZXBox.Hardware.Output;
using ZXBox.Snapshot;
using ZXBox.Core.Tape;

namespace ZXBox.Blazor.Pages
{
    [SupportedOSPlatform("browser")]
    public partial class EmulatorComponentModel : ComponentBase, IAsyncDisposable
    {
        private const float BeeperMixGain = 0.45f;
        private const float SpeechMixGain = 6.0f;
        private const float AyMixGain = 0.35f;
        private const float AudioOutputGain = 0.35f;
        private const float ConsumerTvCurvature = 0.07f;
        private const float ConsumerTvScanlineStrength = 0.24f;
        private const float ConsumerTvVignetteStrength = 0.16f;
        private const float ConsumerTvSaturation = 0.92f;
        private const float ConsumerTvBrightness = 1.04f;
        private const int DefaultAudioSampleRate = 48000;
        private const int DefaultAudioFramesPerBatch = 2;
        private const double AudioTargetQueuedSeconds = 0.04d;
        private const double AudioLowWatermarkSeconds = 0.02d;
        private const int PrinterDisplayScale = 1;
        private const int PrinterPreviewScale = 3;
        private const uint PrinterPaperColor = 0xFFC0BCB8;
        private const uint PrinterInkColor = 0xFF1F1F1F;
        private const string CurrahRomAssetPath = "Roms/CURRAH.ROM";
        private const string Sp0256RomAssetPath = "Roms/SP0256-AL2.BIN";
        private const string ConsumerTvShaderSource = @"
uniform shader content;
uniform float2 outputSize;
uniform float2 inputSize;
uniform float curvature;
uniform float scanlineStrength;
uniform float vignetteStrength;
uniform float saturation;
uniform float brightness;

half4 main(float2 fragCoord)
{
    float2 uv = fragCoord / outputSize;
    float2 centered = uv * 2.0 - 1.0;
    float radius2 = dot(centered, centered);
    centered *= 1.0 + curvature * radius2;
    float2 warpedUv = centered * 0.5 + 0.5;

    if (warpedUv.x < 0.0 || warpedUv.y < 0.0 || warpedUv.x > 1.0 || warpedUv.y > 1.0)
    {
        return half4(0.0, 0.0, 0.0, 1.0);
    }

    float2 sampleCoord = warpedUv * inputSize;
    half3 color = content.eval(sampleCoord).rgb;

    float sourceY = warpedUv.y * inputSize.y;
    float beamPosition = abs(fract(sourceY) - 0.5) * 2.0;
    float scanline = 1.0 - scanlineStrength * beamPosition * beamPosition;
    color *= clamp(scanline, 0.0, 1.0);

    float vignette = 1.0 - vignetteStrength * radius2;
    color *= vignette * brightness;

    float greyscale = dot(color, half3(0.299, 0.587, 0.114));
    color = mix(half3(greyscale, greyscale, greyscale), color, saturation);

    return half4(color, 1.0);
}";
        private static readonly SKSamplingOptions ConsumerTvSampling = new(SKFilterMode.Linear);
        private enum ExecutionDriver
        {
            TimerFallback,
            AudioPump
        }

        private static readonly Dictionary<int, EmulatorComponentModel> AudioPumpTargets = new();
        private static EmulatorComponentModel? _activeDebugTarget;
        private static int _nextAudioPumpInstanceId;

        public ZXSpectrum speccy;
        public System.Timers.Timer gameLoop;
        int flashcounter = 16;
        bool flash = false;
        JavaScriptKeyboard Keyboard = new();
        Kempston kempston;
        BrowserBeeper beeper;
        public TapePlayer tapePlayer;
        public SKGLView _canvasView;
        public SKCanvasView _printerCanvasView;
        public SKCanvasView _printerPreviewCanvasView;
        private SKBitmap _printerBitmap = new(1, 1);
        private uint[] _printerPixels = new uint[1];
        private SKRuntimeEffect _consumerTvEffect;
        private SKRuntimeShaderBuilder _consumerTvShaderBuilder;
        private string _consumerTvShaderError = string.Empty;
        private int _lastPrinterVersion = -1;
        private int _lastPrinterHeight;
        private float[] _audioFrameBuffer = new float[DefaultAudioSampleRate / 50];
        private byte[] _audioFrameBytes = new byte[(DefaultAudioSampleRate / 50) * sizeof(float)];
        private float[] _speechAudioBuffer = new float[DefaultAudioSampleRate / 50];
        private float[] _ayAudioBuffer = new float[DefaultAudioSampleRate / 50];
        private JSObject _audioController;
        private bool _audioInteropReady;
        private bool _audioDrivenLoopActive;
        private int _configuredAudioFramesPerBatch = DefaultAudioFramesPerBatch;
        private int _configuredDisplayFrameDivisor = 1;
        private int _audioSampleRate = DefaultAudioSampleRate;
        private int _audioSamplesPerFrame = DefaultAudioSampleRate / 50;
        private int _displayFrameCounter;
        private bool _screenDirty;
        private bool _screenFlashState;
        private readonly object _perfSync = new();
        private readonly BlazorPerfTimingStats _perfLoopStats = new();
        private readonly BlazorPerfTimingStats _perfCpuStats = new();
        private readonly BlazorPerfTimingStats _perfAudioStats = new();
        private readonly BlazorPerfTimingStats _perfPaintRequestStats = new();
        private readonly BlazorPerfTimingStats _perfOnPaintSurfaceStats = new();
        private bool _perfScenarioStarted;
        private bool _perfSamplingActive;
        private string _perfStatusText = "Idle";
        private string _perfResultJson = string.Empty;
        private BlazorPerfResult _perfResult;
        private long _perfMeasurementStartTimestamp;
        private int _perfTimerTicksObserved;
        private int _perfSchedulerCallbacksObserved;
        private int _perfFramesExecuted;
        private int _perfFramesSkipped;
        private int _perfPaintInvalidations;
        private int _perfPaintSurfaceCalls;
        private bool _isDisposed;
        private readonly int _audioPumpInstanceId = Interlocked.Increment(ref _nextAudioPumpInstanceId);

        [Inject]
        protected HttpClient Http { get; set; }
        [Inject]
        protected IJSInProcessRuntime JSRuntime { get; set; }

        [Parameter]
        public bool PerfMode { get; set; }

        [Parameter]
        public int PerfWarmupSeconds { get; set; } = 2;

        [Parameter]
        public int PerfMeasureSeconds { get; set; } = 8;

        [Parameter]
        public string PerfGame { get; set; } = "ManicMiner.z80";

        [Parameter]
        public string PerfModel { get; set; } = "48k";

        [Parameter]
        public int DisplayFrameDivisor { get; set; } = 1;

        [Parameter]
        public int AudioFramesPerBatch { get; set; } = DefaultAudioFramesPerBatch;

        public string PerfStatusText => _perfStatusText;

        public string PerfResultJson => _perfResultJson;

        public EmulatorComponentModel()
        {
            gameLoop = new System.Timers.Timer(20);
            gameLoop.Elapsed += GameLoop_Elapsed;
            lock (AudioPumpTargets)
            {
                AudioPumpTargets[_audioPumpInstanceId] = this;
            }
            _activeDebugTarget = this;
        }

        public ZXSpectrum GetZXSpectrum(RomEnum rom)
        {
            return new ZXSpectrum(true, true, 20, 20, 20, rom);
        }

        public async Task StartZXSpectrumAsync(RomEnum rom)
        {
            await EnsureAudioRunningAsync();
            StartZXSpectrum(rom);
        }

        public void StartZXSpectrum(RomEnum rom)
        {
            speccy = GetZXSpectrum(rom);
            _activeDebugTarget = this;
            speccy.InputHardware.Add(Keyboard);
            IsSettingsOpen = false;
            _lastPrinterVersion = -1;
            _lastPrinterHeight = 0;

            kempston = new Kempston();
            speccy.InputHardware.Add(kempston);
            //48000 samples per second, 50 frames per second (20ms per frame) Mono
            beeper = new BrowserBeeper(_audioSamplesPerFrame, speccy.FrameTStates, BeeperMixGain);
            speccy.OutputHardware.Add(beeper);
            tapePlayer = new(beeper);
            speccy.InputHardware.Add(tapePlayer);
            _configuredDisplayFrameDivisor = Math.Max(1, DisplayFrameDivisor);
            _configuredAudioFramesPerBatch = Math.Max(1, AudioFramesPerBatch);
            _audioDrivenLoopActive = false;
            _displayFrameCounter = 0;
            _screenDirty = true;
            flashcounter = 16;
            flash = false;
            speccy.Reset();
            StartExecutionLoop();

            if (ConsumerTvShaderEnabled)
            {
                EnsureConsumerTvShader();
            }
        }

        public bool IsSettingsOpen { get; set; } = true;
        public bool ConsumerTvShaderEnabled { get; set; }
        public string ConsumerTvShaderError => _consumerTvShaderError;

        public string TapeName { get; set; }
        public string PeripheralStatusMessage { get; set; } = string.Empty;
        public bool IsPrinterPreviewOpen { get; set; }

        public void OpenSettingsPanel()
        {
            IsSettingsOpen = true;
        }

        public void CloseSettingsPanel()
        {
            IsSettingsOpen = false;
        }

        public async Task HandleFileSelected(InputFileChangeEventArgs args)
        {
            await EnsureAudioRunningAsync();

            var file = args.File;
            var ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms);
            var bytes = ms.ToArray();

            if (TapeFormatFactory.IsSupportedTapeFile(args.File.Name))
            {
                tapePlayer.LoadTape(bytes, args.File.Name);
                TapeName = Path.GetFileNameWithoutExtension(args.File.Name);
                return;
            }

            var handler = FileFormatFactory.GetSnapShotHandler(file.Name);
            handler.LoadSnapshot(bytes, speccy);
        }

        public async Task ConnectCurrahMicroSpeech()
        {
            if (speccy is null)
            {
                return;
            }

            await EnsureAudioRunningAsync();
            speccy.ConnectCurrahMicroSpeech();
            await TryLoadCurrahAssetsAsync();
            ResetMachineStateAfterPeripheralChange();
            var currahReady = speccy.CurrahMicroSpeech.HasRom && speccy.CurrahMicroSpeech.HasSpeechRom;
            PeripheralStatusMessage = currahReady
                ? "Currah connected. The emulator was reset so Currah-aware software can detect the peripheral from startup."
                : "Currah connected, but one or both Currah ROM assets could not be loaded.";
        }

        public void DisconnectCurrahMicroSpeech()
        {
            if (speccy is null)
            {
                return;
            }
            speccy.DisconnectCurrahMicroSpeech();
            speccy.DisconnectCurrahMicroSpeech();
            ResetMachineStateAfterPeripheralChange();
            PeripheralStatusMessage = "Currah disconnected. The emulator was reset to apply the hardware change.";
        }

        public void ConnectZxPrinter()
        {
            if (speccy is null)
            {
                return;
            }

            speccy.ConnectZxPrinter();
            _lastPrinterVersion = -1;
            _lastPrinterHeight = speccy.ZxPrinter.PaperHeight;
            PeripheralStatusMessage = string.Empty;
        }

        public void DisconnectZxPrinter()
        {
            if (speccy is null)
            {
                return;
            }

            speccy.DisconnectZxPrinter();
            _lastPrinterVersion = -1;
            _lastPrinterHeight = 0;
            IsPrinterPreviewOpen = false;
            PeripheralStatusMessage = string.Empty;
        }
        [Inject]
        HttpClient httpClient { get; set; }
        public string Instructions = "";
        public async Task LoadGame(string filename, string instructions)
        {
            await EnsureAudioRunningAsync();

            var ms = new MemoryStream();
            var handler = FileFormatFactory.GetSnapShotHandler(filename);
            var stream = await httpClient.GetStreamAsync("Roms/" + filename + ".json");
            await stream.CopyToAsync(ms);
            var bytes = ms.ToArray();
            handler.LoadSnapshot(bytes, speccy);
            Instructions = instructions;
        }

        public async Task StartTapePlaybackAsync()
        {
            await EnsureAudioRunningAsync();
            tapePlayer.Play();
        }

        private async Task TryLoadCurrahAssetsAsync()
        {
            var currahRom = await TryLoadAssetAsync(CurrahRomAssetPath);
            var speechRom = await TryLoadAssetAsync(Sp0256RomAssetPath);

            if (currahRom is { Length: >= 0x800 })
            {
                speccy.LoadCurrahMicroSpeechRom(currahRom);
            }

            if (speechRom is { Length: >= 0x800 })
            {
                speccy.CurrahMicroSpeech.LoadSpeechRom(speechRom);
            }

        }

        private async Task<byte[]?> TryLoadAssetAsync(string assetPath)
        {
            using var response = await httpClient.GetAsync(assetPath);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        private async Task EnsureAudioRunningAsync()
        {
            if (!OperatingSystem.IsBrowser())
            {
                return;
            }

            if (_audioController is null)
            {
                await BrowserRuntimeInterop.EnsureAudioModuleAsync();
                _audioController = await BrowserRuntimeInterop.CreateAudioControllerAsync(
                    DefaultAudioSampleRate,
                    AudioOutputGain,
                    AudioTargetQueuedSeconds,
                    AudioLowWatermarkSeconds);
                _audioSampleRate = Math.Max(1, BrowserRuntimeInterop.GetAudioSampleRate(_audioController));
                ConfigureAudioFormat(_audioSampleRate);
                _audioInteropReady = true;
            }

            await BrowserRuntimeInterop.ResumeAudioControllerAsync(_audioController);
        }

        private void ResetMachineStateAfterPeripheralChange()
        {
            if (speccy is null)
            {
                return;
            }

            speccy.Reset();
            _displayFrameCounter = 0;
            _screenDirty = true;
            _screenFlashState = false;
            flashcounter = 16;
            flash = false;
            _lastPrinterVersion = speccy.ZxPrinter.Connected ? -1 : _lastPrinterVersion;
            _lastPrinterHeight = speccy.ZxPrinter.Connected ? speccy.ZxPrinter.PaperHeight : 0;
            _ = InvokeAsync(StateHasChanged);
        }

        private void GameLoop_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_perfSamplingActive)
            {
                lock (_perfSync)
                {
                    _perfTimerTicksObserved++;
                }
            }

            TryRunFrame(ExecutionDriver.TimerFallback);
        }
        bool TapeStopped = false;
        private int _frameInProgress;

        private void OnAudioDataRequested()
        {
            if (_isDisposed || speccy is null || _audioController is null)
            {
                return;
            }

            if (_perfSamplingActive)
            {
                lock (_perfSync)
                {
                    _perfSchedulerCallbacksObserved++;
                }
            }

            if (!_audioDrivenLoopActive)
            {
                _audioDrivenLoopActive = true;
                gameLoop.Stop();
            }

            for (var generatedFrames = 0; generatedFrames < _configuredAudioFramesPerBatch; generatedFrames++)
            {
                if (BrowserRuntimeInterop.GetQueuedSeconds(_audioController) >= AudioTargetQueuedSeconds)
                {
                    break;
                }

                if (!TryRunFrame(ExecutionDriver.AudioPump))
                {
                    break;
                }
            }
        }

        [JSInvokable]
        public static void RequestAudioPump(int instanceId)
        {
            lock (AudioPumpTargets)
            {
                if (!AudioPumpTargets.TryGetValue(instanceId, out var target))
                {
                    return;
                }

                target.OnAudioDataRequested();
            }
        }

        [JSInvokable]
        public static async Task<bool> DebugRunImmediateLprint(string text)
        {
            var target = _activeDebugTarget;
            if (target is null || target.speccy is null)
            {
                return false;
            }

            await target.InvokeAsync(() => target.RunImmediateLprint(text));
            return true;
        }

        private bool TryRunFrame(ExecutionDriver driver)
        {
            if (speccy is null || kempston is null || beeper is null)
            {
                return false;
            }

            if (Interlocked.Exchange(ref _frameInProgress, 1) != 0)
            {
                if (_perfSamplingActive)
                {
                    lock (_perfSync)
                    {
                        _perfFramesSkipped++;
                    }
                }

                return false;
            }

            var perfLoopStart = _perfSamplingActive ? Stopwatch.GetTimestamp() : 0L;

            try
            {
                kempston.DirectState = (byte)BrowserRuntimeInterop.GetKempstonState();
                Keyboard.SetKeyMask((ulong)BrowserRuntimeInterop.GetKeyMask());
                var frameTStates = speccy.FrameTStates;

                var perfCpuStart = _perfSamplingActive ? Stopwatch.GetTimestamp() : 0L;
                speccy.DoInstructions(frameTStates);
                if (_perfSamplingActive)
                {
                    RecordPerfTiming(_perfCpuStats, perfCpuStart);
                }

                var perfAudioStart = _perfSamplingActive ? Stopwatch.GetTimestamp() : 0L;
                BufferSound(frameTStates);
                if (_perfSamplingActive)
                {
                    RecordPerfTiming(_perfAudioStats, perfAudioStart);
                }

                var perfPaintRequestStart = _perfSamplingActive ? Stopwatch.GetTimestamp() : 0L;
                Paint();
                if (_perfSamplingActive)
                {
                    lock (_perfSync)
                    {
                        _perfFramesExecuted++;
                    }

                    RecordPerfTiming(_perfPaintRequestStats, perfPaintRequestStart);
                }

                if (tapePlayer != null && tapePlayer.IsPlaying)
                {
                    TapeStopped = false;
                    PercentLoaded = ((Convert.ToDouble(tapePlayer.CurrentTstate) / Convert.ToDouble(tapePlayer.TotalTstates)) * 100);
                    _ = InvokeAsync(StateHasChanged);
                }
                if (!TapeStopped && !tapePlayer.IsPlaying)
                {
                    TapeStopped = true;
                    _ = InvokeAsync(StateHasChanged);
                }

                return true;
            }
            finally
            {
                if (_perfSamplingActive)
                {
                    RecordPerfTiming(_perfLoopStats, perfLoopStart);
                }

                Interlocked.Exchange(ref _frameInProgress, 0);
            }
        }

        protected void BufferSound(int frameTStates)
        {
            var frameBuffer = _audioFrameBuffer.AsSpan();
            beeper.CompleteFrame(frameBuffer);

            var hasCurrahAudio = speccy.CurrahMicroSpeech.Connected;
            var hasAyAudio = speccy.Is128KModel;

            if (hasCurrahAudio)
            {
                speccy.CurrahMicroSpeech.RenderAudioFrame(_speechAudioBuffer.AsSpan(), frameTStates);
            }

            if (hasAyAudio)
            {
                speccy.AyChip.RenderAudioFrame(_ayAudioBuffer.AsSpan(), frameTStates);
            }

            if (hasCurrahAudio || hasAyAudio)
            {
                MixAudioBuffers(
                    frameBuffer,
                    hasCurrahAudio ? _speechAudioBuffer : null,
                    hasAyAudio ? _ayAudioBuffer : null,
                    SpeechMixGain,
                    AyMixGain);
            }

            if (_audioController is not null)
            {
                Buffer.BlockCopy(_audioFrameBuffer, 0, _audioFrameBytes, 0, _audioFrameBytes.Length);
                _audioController.SetProperty("pendingFrameBytes", _audioFrameBytes);
                BrowserRuntimeInterop.PushPendingAudioFrame(_audioController, _audioSamplesPerFrame);
            }
        }

        private static void MixAudioBuffers(Span<float> destination, float[]? secondary, float[]? tertiary, float secondaryGain, float tertiaryGain)
        {
            for (var sample = 0; sample < destination.Length; sample++)
            {
                var secondarySample = secondary is null ? 0f : secondary[sample] * secondaryGain;
                var tertiarySample = tertiary is null ? 0f : tertiary[sample] * tertiaryGain;
                destination[sample] = Math.Clamp(
                    destination[sample] + secondarySample + tertiarySample,
                    -1f,
                    1f);
            }
        }

        public double PercentLoaded = 0;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //if (firstRender)
            //{
            //    await JSRuntime.InvokeAsync<bool>("InitCanvas");
            //}
            await base.OnAfterRenderAsync(firstRender);

            if (!firstRender || _audioInteropReady || !OperatingSystem.IsBrowser())
            {
                return;
            }

            if (PerfMode)
            {
                await EnsureAudioRunningAsync();
            }

            if (PerfMode && !_perfScenarioStarted)
            {
                _perfScenarioStarted = true;
                _ = RunPerfScenarioAsync();
            }
        }

        GCHandle gchscreen;
        IntPtr pinnedscreen;
      
        uint[] screen = new uint[68672]; //Height * width (256+20+20)*(192+20+20)
        public void Paint()
        {
            if (flashcounter == 0)
            {
                flashcounter = 16;
                flash = !flash;
            }
            else
            {
                flashcounter--;
            }

            if (_displayFrameCounter == 0)
            {
                _screenFlashState = flash;
                _screenDirty = true;

                _canvasView?.Invalidate();
                if (_perfSamplingActive)
                {
                    lock (_perfSync)
                    {
                        _perfPaintInvalidations++;
                    }
                }
            }

            _displayFrameCounter++;
            if (_displayFrameCounter >= _configuredDisplayFrameDivisor)
            {
                _displayFrameCounter = 0;
            }

            if (speccy?.ZxPrinter.Connected == true)
            {
                var printerVersion = speccy.ZxPrinter.PaperVersion;
                if (printerVersion != _lastPrinterVersion)
                {
                    _lastPrinterVersion = printerVersion;
                    _printerCanvasView?.Invalidate();
                    _printerPreviewCanvasView?.Invalidate();

                    var printerHeight = speccy.ZxPrinter.PaperHeight;
                    if (printerHeight != _lastPrinterHeight)
                    {
                        _lastPrinterHeight = printerHeight;
                        _ = InvokeAsync(() => StateHasChanged());
                    }
                }
            }
        }

        SKBitmap bitmap = new SKBitmap(296, 232);

        //SKPaint paint = new SKPaint
        //{
        //    FilterQuality = SKFilterQuality.High, // High-quality filter for smoother rendering
        //    IsAntialias = true // Additional anti-aliasing if necessary
        //};
        public void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            DrawDisplaySurface(e.Surface.Canvas, e.Info);
        }

        public void OnPaintGLSurface(SKPaintGLSurfaceEventArgs e)
        {
            DrawDisplaySurface(e.Surface.Canvas, e.Info);
        }

        private void DrawDisplaySurface(SKCanvas canvas, SKImageInfo info)
        {
            var perfPaintStart = _perfSamplingActive ? Stopwatch.GetTimestamp() : 0L;

            if (_screenDirty)
            {
                screen = speccy.GetScreenInUint(_screenFlashState);
                _screenDirty = false;
            }

            unsafe
            {
                var ptr = (uint*)bitmap.GetPixels().ToPointer();

                fixed (uint* srcPtr = screen)
                {
                    // Use Buffer.MemoryCopy for fast memory copying
                    Buffer.MemoryCopy(srcPtr, ptr, screen.Length * sizeof(uint), screen.Length * sizeof(uint));
                }
            }
            
            if (ConsumerTvShaderEnabled && EnsureConsumerTvShader())
            {
                DrawConsumerTvShader(canvas, info);
            }
            else
            {
                canvas.DrawBitmap(bitmap, new SKRect(0, 0, info.Width, info.Height));
            }

            if (_perfSamplingActive)
            {
                lock (_perfSync)
                {
                    _perfPaintSurfaceCalls++;
                }

                RecordPerfTiming(_perfOnPaintSurfaceStats, perfPaintStart);
            }
        }

        private bool EnsureConsumerTvShader()
        {
            if (_consumerTvShaderBuilder != null)
            {
                return true;
            }

            if (!string.IsNullOrEmpty(_consumerTvShaderError))
            {
                return false;
            }

            _consumerTvEffect = SKRuntimeEffect.CreateShader(ConsumerTvShaderSource, out var shaderErrors);
            if (_consumerTvEffect == null)
            {
                _consumerTvShaderError = string.IsNullOrWhiteSpace(shaderErrors)
                    ? "Failed to compile the CRT shader."
                    : shaderErrors;
                _ = InvokeAsync(StateHasChanged);
                return false;
            }

            _consumerTvShaderBuilder = new SKRuntimeShaderBuilder(_consumerTvEffect);
            return true;
        }

        private void DrawConsumerTvShader(SKCanvas canvas, SKImageInfo info)
        {
            using var sourceShader = bitmap.ToShader(SKShaderTileMode.Clamp, SKShaderTileMode.Clamp, ConsumerTvSampling);
            _consumerTvShaderBuilder.Children["content"] = sourceShader;
            _consumerTvShaderBuilder.Uniforms["outputSize"] = new SKSize(info.Width, info.Height);
            _consumerTvShaderBuilder.Uniforms["inputSize"] = new SKSize(bitmap.Width, bitmap.Height);
            _consumerTvShaderBuilder.Uniforms["curvature"] = ConsumerTvCurvature;
            _consumerTvShaderBuilder.Uniforms["scanlineStrength"] = ConsumerTvScanlineStrength;
            _consumerTvShaderBuilder.Uniforms["vignetteStrength"] = ConsumerTvVignetteStrength;
            _consumerTvShaderBuilder.Uniforms["saturation"] = ConsumerTvSaturation;
            _consumerTvShaderBuilder.Uniforms["brightness"] = ConsumerTvBrightness;
            using var crtShader = _consumerTvShaderBuilder.Build();
            using var paint = new SKPaint
            {
                Shader = crtShader,
                IsAntialias = false
            };

            canvas.DrawRect(SKRect.Create(info.Width, info.Height), paint);
        }

        public int PrinterCanvasDisplayHeight
        {
            get
            {
                var printerHeight = speccy?.ZxPrinter.PaperHeight ?? 0;
                return Math.Max(printerHeight * PrinterDisplayScale, 96);
            }
        }

        public int PrinterCanvasDisplayWidth => ZxPrinter.PaperWidth * PrinterDisplayScale;

        public string PrinterCanvasStyle => $"display:block; width:{PrinterCanvasDisplayWidth}px; height:{PrinterCanvasDisplayHeight}px; max-width:none;";

        public int PrinterPreviewCanvasDisplayHeight
        {
            get
            {
                var printerHeight = speccy?.ZxPrinter.PaperHeight ?? 0;
                return Math.Max(printerHeight * PrinterPreviewScale, 288);
            }
        }

        public int PrinterPreviewCanvasDisplayWidth => ZxPrinter.PaperWidth * PrinterPreviewScale;

        public string PrinterPreviewCanvasStyle => $"display:block; width:{PrinterPreviewCanvasDisplayWidth}px; height:{PrinterPreviewCanvasDisplayHeight}px; max-width:none;";

        public void OnPaintPrinterSurface(SKPaintSurfaceEventArgs e)
        {
            DrawPrinterSurface(e, PrinterDisplayScale);
        }

        public void OnPaintPrinterPreviewSurface(SKPaintSurfaceEventArgs e)
        {
            DrawPrinterSurface(e, PrinterPreviewScale);
        }

        private void DrawPrinterSurface(SKPaintSurfaceEventArgs e, int displayScale)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(new SKColor(184, 188, 192));

            if (speccy?.ZxPrinter.Connected != true)
            {
                return;
            }

            var snapshot = speccy.ZxPrinter.GetPaperSnapshot();
            var bitmapHeight = Math.Max(snapshot.Height, 1);
            EnsurePrinterBitmap(snapshot.Width, bitmapHeight);
            Array.Fill(_printerPixels, PrinterPaperColor);

            for (var y = 0; y < snapshot.Height; y++)
            {
                var lineOffset = y * snapshot.Width;
                for (var x = 0; x < snapshot.Width; x++)
                {
                    if (snapshot.Pixels[lineOffset + x] != 0)
                    {
                        _printerPixels[lineOffset + x] = PrinterInkColor;
                    }
                }
            }

            unsafe
            {
                var ptr = (uint*)_printerBitmap.GetPixels().ToPointer();
                fixed (uint* srcPtr = _printerPixels)
                {
                    Buffer.MemoryCopy(srcPtr, ptr, _printerPixels.Length * sizeof(uint), _printerPixels.Length * sizeof(uint));
                }
            }

            using var paint = new SKPaint
            {
                FilterQuality = SKFilterQuality.None,
                IsAntialias = false
            };

            var renderedWidth = Math.Min(e.Info.Width, snapshot.Width * displayScale);
            var renderedHeight = Math.Min(e.Info.Height, bitmapHeight * displayScale);
            if (renderedWidth <= 0 || renderedHeight <= 0)
            {
                return;
            }

            canvas.DrawBitmap(_printerBitmap, new SKRect(0, 0, renderedWidth, renderedHeight), paint);
        }

        public void OpenPrinterPreview()
        {
            if (speccy?.ZxPrinter.Connected != true)
            {
                return;
            }

            IsPrinterPreviewOpen = true;
            _ = InvokeAsync(() =>
            {
                StateHasChanged();
                _printerPreviewCanvasView?.Invalidate();
            });
        }

        public void ClosePrinterPreview()
        {
            IsPrinterPreviewOpen = false;
        }

        private void EnsurePrinterBitmap(int width, int height)
        {
            if (_printerBitmap.Width == width && _printerBitmap.Height == height && _printerPixels.Length == width * height)
            {
                return;
            }

            _printerBitmap.Dispose();
            _printerBitmap = new SKBitmap(width, height);
            _printerPixels = new uint[width * height];
        }

        private void RunImmediateLprint(string text)
        {
            if (speccy is null)
            {
                return;
            }

            StopExecutionLoop();

            if (!speccy.ZxPrinter.Connected)
            {
                ConnectZxPrinter();
            }

            RunSpectrumFrames(300);

            var eLine = ReadWordFromSpectrumMemory(0x5C59);
            var commandBytes = BuildImmediateLprintLine(text);
            WriteBytesToSpectrumMemory(eLine, commandBytes);
            WriteWordToSpectrumMemory(0x5C61, eLine + commandBytes.Length);
            speccy.WriteByteToMemory(0x5C44, 0x01);
            speccy.WriteByteToMemory(0x5C3B, (byte)(speccy.ReadByteFromMemory(0x5C3B) | 0x80));
            speccy.PC = 0x1B8A;

            RunSpectrumFrames(300);
            Paint();
            _printerCanvasView?.Invalidate();
            _printerPreviewCanvasView?.Invalidate();
            StartExecutionLoop();
        }

        private byte[] BuildImmediateLprintLine(string text)
        {
            var safeText = text ?? string.Empty;
            var bytes = new byte[safeText.Length + 4];
            bytes[0] = 0xE0;
            bytes[1] = 0x22;

            for (var index = 0; index < safeText.Length; index++)
            {
                bytes[index + 2] = (byte)safeText[index];
            }

            bytes[^2] = 0x22;
            bytes[^1] = 0x0D;
            return bytes;
        }

        private int ReadWordFromSpectrumMemory(int address)
        {
            return speccy.ReadByteFromMemory((ushort)address) | (speccy.ReadByteFromMemory((ushort)(address + 1)) << 8);
        }

        private void WriteWordToSpectrumMemory(int address, int value)
        {
            speccy.WriteByteToMemory((ushort)address, (byte)(value & 0xFF));
            speccy.WriteByteToMemory((ushort)(address + 1), (byte)((value >> 8) & 0xFF));
        }

        private void WriteBytesToSpectrumMemory(int address, byte[] bytes)
        {
            for (var index = 0; index < bytes.Length; index++)
            {
                speccy.WriteByteToMemory((ushort)(address + index), bytes[index]);
            }
        }

        private void RunSpectrumFrames(int frameCount)
        {
            for (var frameIndex = 0; frameIndex < frameCount; frameIndex++)
            {
                speccy.DoInstructions(speccy.FrameTStates);
            }
        }

        public async ValueTask DisposeAsync()
        {
            _isDisposed = true;
            StopExecutionLoop();
            gameLoop.Dispose();
            _consumerTvShaderBuilder?.Dispose();
            _consumerTvEffect?.Dispose();
            if (_audioController is not null)
            {
                await BrowserRuntimeInterop.DisposeAudioControllerAsync(_audioController);
                _audioController.Dispose();
            }
            lock (AudioPumpTargets)
            {
                AudioPumpTargets.Remove(_audioPumpInstanceId);
            }
            if (ReferenceEquals(_activeDebugTarget, this))
            {
                _activeDebugTarget = null;
            }
            _printerBitmap.Dispose();
            bitmap.Dispose();
        }

        private async Task RunPerfScenarioAsync()
        {
            _perfStatusText = "Starting perf scenario...";
            await InvokeAsync(StateHasChanged);

            StartZXSpectrum(ParsePerfModel());
            await LoadGame(PerfGame, string.Empty);

            _perfStatusText = $"Warming up for {PerfWarmupSeconds} seconds...";
            await InvokeAsync(StateHasChanged);
            await Task.Delay(TimeSpan.FromSeconds(Math.Max(0, PerfWarmupSeconds)));

            ResetPerfCounters();
            JSRuntime.InvokeVoid("startPerfRafSampling");
            _perfMeasurementStartTimestamp = Stopwatch.GetTimestamp();
            _perfSamplingActive = true;
            if (_audioController is not null)
            {
                BrowserRuntimeInterop.ResetControllerForMeasurement(_audioController);
                if (BrowserRuntimeInterop.IsAudioDriven(_audioController))
                {
                    OnAudioDataRequested();
                }

            }

            _perfStatusText = $"Measuring for {PerfMeasureSeconds} seconds...";
            await InvokeAsync(StateHasChanged);
            await Task.Delay(TimeSpan.FromSeconds(Math.Max(1, PerfMeasureSeconds)));

            _perfSamplingActive = false;
            StopExecutionLoop();

            while (Interlocked.CompareExchange(ref _frameInProgress, 0, 0) != 0)
            {
                await Task.Delay(20);
            }

            await Task.Delay(100);

            var measureSeconds = Stopwatch.GetElapsedTime(_perfMeasurementStartTimestamp).TotalSeconds;
            var audioMetrics = _audioController is null
                ? new BlazorPerfAudioMetrics()
                : JsonSerializer.Deserialize<BlazorPerfAudioMetrics>(
                    BrowserRuntimeInterop.GetPerformanceMetricsJson(_audioController),
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? new BlazorPerfAudioMetrics();
            var rafMetrics = JSRuntime.Invoke<BlazorPerfRafMetrics>("stopPerfRafSampling");

            lock (_perfSync)
            {
                _perfResult = new BlazorPerfResult
                {
                    Game = PerfGame,
                    Model = PerfModel,
                    WarmupSeconds = PerfWarmupSeconds,
                    MeasureSeconds = measureSeconds,
                    DisplayFrameDivisor = _configuredDisplayFrameDivisor,
                    AudioFramesPerBatch = _configuredAudioFramesPerBatch,
                    SchedulerCallbacksObserved = _perfSchedulerCallbacksObserved,
                    TimerTicksObserved = _perfTimerTicksObserved,
                    FramesExecuted = _perfFramesExecuted,
                    FramesSkipped = _perfFramesSkipped,
                    PaintInvalidations = _perfPaintInvalidations,
                    PaintSurfaceCalls = _perfPaintSurfaceCalls,
                    LoopFps = measureSeconds == 0 ? 0d : _perfFramesExecuted / measureSeconds,
                    RequestedPaintFps = measureSeconds == 0 ? 0d : _perfPaintInvalidations / measureSeconds,
                    PresentedPaintFps = measureSeconds == 0 ? 0d : _perfPaintSurfaceCalls / measureSeconds,
                    AverageGameLoopMs = _perfLoopStats.AverageMilliseconds,
                    MaxGameLoopMs = _perfLoopStats.MaxMilliseconds,
                    AverageCpuMs = _perfCpuStats.AverageMilliseconds,
                    MaxCpuMs = _perfCpuStats.MaxMilliseconds,
                    AverageAudioMs = _perfAudioStats.AverageMilliseconds,
                    MaxAudioMs = _perfAudioStats.MaxMilliseconds,
                    AveragePaintRequestMs = _perfPaintRequestStats.AverageMilliseconds,
                    MaxPaintRequestMs = _perfPaintRequestStats.MaxMilliseconds,
                    AverageOnPaintSurfaceMs = _perfOnPaintSurfaceStats.AverageMilliseconds,
                    MaxOnPaintSurfaceMs = _perfOnPaintSurfaceStats.MaxMilliseconds,
                    Audio = audioMetrics,
                    Raf = rafMetrics
                };
            }

            _perfResultJson = JsonSerializer.Serialize(_perfResult, new JsonSerializerOptions { WriteIndented = true });
            JSRuntime.InvokeVoid("setPerfResult", _perfResultJson);
            _perfStatusText = "Perf scenario complete";
            await InvokeAsync(StateHasChanged);
        }

        private RomEnum ParsePerfModel()
        {
            return PerfModel.Equals("128k", StringComparison.OrdinalIgnoreCase)
                || PerfModel.Equals("plus", StringComparison.OrdinalIgnoreCase)
                ? RomEnum.ZXSpectrumPlus
                : RomEnum.ZXSpectrum48k;
        }

        private void ResetPerfCounters()
        {
            lock (_perfSync)
            {
                _perfTimerTicksObserved = 0;
                _perfSchedulerCallbacksObserved = 0;
                _perfFramesExecuted = 0;
                _perfFramesSkipped = 0;
                _perfPaintInvalidations = 0;
                _perfPaintSurfaceCalls = 0;
                _perfLoopStats.Reset();
                _perfCpuStats.Reset();
                _perfAudioStats.Reset();
                _perfPaintRequestStats.Reset();
                _perfOnPaintSurfaceStats.Reset();
            }
        }

        private void RecordPerfTiming(BlazorPerfTimingStats stats, long startTimestamp)
        {
            if (!_perfSamplingActive)
            {
                return;
            }

            var elapsedMilliseconds = Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds;
            lock (_perfSync)
            {
                stats.Add(elapsedMilliseconds);
            }
        }

        private void ConfigureAudioFormat(int sampleRate)
        {
            _audioSampleRate = Math.Max(1, sampleRate);
            _audioSamplesPerFrame = Math.Max(1, (int)Math.Round(_audioSampleRate / 50d));
            _audioFrameBuffer = new float[_audioSamplesPerFrame];
            _audioFrameBytes = new byte[_audioSamplesPerFrame * sizeof(float)];
            _speechAudioBuffer = new float[_audioSamplesPerFrame];
            _ayAudioBuffer = new float[_audioSamplesPerFrame];
        }

        private void StartExecutionLoop()
        {
            if (_isDisposed || speccy is null)
            {
                return;
            }

            _audioDrivenLoopActive = false;
            gameLoop.Stop();
            if (_audioController is not null)
            {
                BrowserRuntimeInterop.StartAudioPump(_audioController, _audioPumpInstanceId);
                if (_audioDrivenLoopActive)
                {
                    return;
                }
            }

            gameLoop.Start();
        }

        private void StopExecutionLoop()
        {
            gameLoop.Stop();
            if (_audioController is not null)
            {
                BrowserRuntimeInterop.StopAudioPump(_audioController);
            }

            _audioDrivenLoopActive = false;
        }

    }
}
