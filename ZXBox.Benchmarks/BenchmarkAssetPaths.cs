using System;
using System.IO;

namespace ZXBox.Benchmarks;

internal static class BenchmarkAssetPaths
{
    public static string ManicMinerSnapshot => Path.Combine(AppContext.BaseDirectory, "Assets", "ManicMiner.z80");
}
