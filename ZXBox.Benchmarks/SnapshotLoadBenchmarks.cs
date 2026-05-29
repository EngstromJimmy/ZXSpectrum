using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using ZXBox.Snapshot;

namespace ZXBox.Benchmarks;

public class SnapshotLoadBenchmarks : BenchmarkBase
{
    private const int LoadsPerBatch = 10;

    private byte[] _snapshotBytes = Array.Empty<byte>();
    private readonly ISnapshot _snapshotHandler = FileFormatFactory.GetSnapShotHandler("ManicMiner.z80")!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _snapshotBytes = File.ReadAllBytes(BenchmarkAssetPaths.ManicMinerSnapshot);
    }

    [Benchmark(Description = "Load Manic Miner .z80 snapshot", OperationsPerInvoke = LoadsPerBatch)]
    public void LoadSnapshot()
    {
        for (var i = 0; i < LoadsPerBatch; i++)
        {
            var spectrum = new ZXSpectrum();
            _snapshotHandler.LoadSnapshot(_snapshotBytes, spectrum);
            GC.KeepAlive(spectrum);
        }
    }
}
