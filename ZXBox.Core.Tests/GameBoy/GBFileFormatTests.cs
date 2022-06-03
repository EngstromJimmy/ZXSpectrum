using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using ZXBox.Snapshot;

namespace ZXBox.Core.Tests.GameBoy;

[TestClass]
public class UnitTests
{
    [TestMethod]
    public void TestLoadGBFile()
    {
        var bytes = File.ReadAllBytes(@"C:\Code\Roms\Test.gb");
        var ff = new GBFileFormat();
        ff.LoadSnapshot(bytes, new ZXSpectrum());
    }
}