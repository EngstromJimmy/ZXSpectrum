using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using ZXBox.Snapshot;

namespace ZXBox.Benchmarks;

public class SpectrumFrameBenchmarks : BenchmarkBase
{
    private const int FramesPerBatch = 10;
    private const int ScreenRendersPerBatch = 10;

    private byte[] _snapshotBytes = Array.Empty<byte>();
    private readonly ISnapshot _snapshotHandler = FileFormatFactory.GetSnapShotHandler("ManicMiner.z80")!;
    private ZXSpectrum _spectrum = null!;
    private ZXSpectrum[] _renderSpectra = Array.Empty<ZXSpectrum>();

    [GlobalSetup]
    public void GlobalSetup()
    {
        _snapshotBytes = File.ReadAllBytes(BenchmarkAssetPaths.ManicMinerSnapshot);
    }

    [IterationSetup(Target = nameof(ExecuteFrames))]
    public void SetupFrameExecution()
    {
        _spectrum = CreateLoadedSpectrum();
    }

    [IterationSetup(Target = nameof(RenderScreenAfterFrame))]
    public void SetupScreenRender()
    {
        _renderSpectra = new ZXSpectrum[ScreenRendersPerBatch];
        for (var i = 0; i < _renderSpectra.Length; i++)
        {
            var spectrum = CreateLoadedSpectrum();
            spectrum.DoInstructions(spectrum.FrameTStates);
            _renderSpectra[i] = spectrum;
        }
    }

    [Benchmark(Description = "Execute 10 Manic Miner frames", OperationsPerInvoke = FramesPerBatch)]
    public void ExecuteFrames()
    {
        for (var i = 0; i < FramesPerBatch; i++)
        {
            _spectrum.DoInstructions(_spectrum.FrameTStates);
        }
    }

    [Benchmark(Description = "Render screen after frame execution", OperationsPerInvoke = ScreenRendersPerBatch)]
    public void RenderScreenAfterFrame()
    {
        for (var i = 0; i < _renderSpectra.Length; i++)
        {
            var screen = _renderSpectra[i].GetScreenInUint(false);
            GC.KeepAlive(screen);
        }
    }

    private ZXSpectrum CreateLoadedSpectrum()
    {
        var spectrum = new ZXSpectrum();
        _snapshotHandler.LoadSnapshot(_snapshotBytes, spectrum);
        return spectrum;
    }
}
