using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using ZXBox.Snapshot;

namespace ZXBox.Core.Tests.GameBoy;

[TestClass]
public class GBFileFormatTests
{
    [TestMethod]
    public void TestLoadGBFile()
    {
        var bytes = File.ReadAllBytes(@"C:\Code\Roms\Test.gb");
        var ff = new GBFileFormat();
        ff.LoadSnapshot(bytes, new ZXSpectrum());
    }

    [TestMethod]
    public void LoadTileTest()
    {
        var bytes = File.ReadAllBytes(@"C:\Code\Roms\Test.gb");
        var ff = new GBFileFormat();
        var gb = new Gameboy();
        ff.LoadSnapshot(bytes, gb);
        List<int> tiles = new List<int>();
        //gb.PC = 0x100; //Entry point
        gb.DoIntructions(1000000000);
        for (int b = 0x8000; b <= 0x97FF; b++)
        {
            tiles.Add(gb.ReadByteFromMemory(b));
        }

    }
}