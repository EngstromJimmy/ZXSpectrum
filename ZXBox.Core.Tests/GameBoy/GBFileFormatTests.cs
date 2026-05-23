using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZXBox.Snapshot;

namespace ZXBox.Core.Tests.GameBoy;

[TestClass]
public class GBFileFormatTests
{
    [TestMethod]
    public void LoadSnapshotCopiesRomBytesIntoSpectrumMemory()
    {
        var bytes = CreateGameBoyRom();
        var fileFormat = new GBFileFormat();
        var spectrum = new ZXSpectrum();

        fileFormat.LoadSnapshot(bytes, spectrum);

        Assert.AreEqual(0xC3, spectrum.ReadByteFromMemory(0x0100));
        Assert.AreEqual(0x50, spectrum.ReadByteFromMemory(0x0101));
        Assert.AreEqual(0x01, spectrum.ReadByteFromMemory(0x0102));
        Assert.AreEqual(0x42, spectrum.ReadByteFromMemory(0x0147));
    }

    [TestMethod]
    public void LoadSnapshotCopiesRomBytesIntoGameBoyMemoryMap()
    {
        var bytes = CreateGameBoyRom();
        var fileFormat = new GBFileFormat();
        var gameboy = new Gameboy();

        fileFormat.LoadSnapshot(bytes, gameboy);

        Assert.AreEqual(0xC3, gameboy.ReadByteFromMemory(0x0100));
        Assert.AreEqual(0x50, gameboy.ReadByteFromMemory(0x0101));
        Assert.AreEqual(0x01, gameboy.ReadByteFromMemory(0x0102));
        Assert.AreEqual((byte)'Z', gameboy.ReadByteFromMemory(0x0134));
        Assert.AreEqual(0x42, gameboy.ReadByteFromMemory(0x0147));
    }

    private static byte[] CreateGameBoyRom()
    {
        var rom = new byte[0x200];
        rom[0x0100] = 0xC3;
        rom[0x0101] = 0x50;
        rom[0x0102] = 0x01;

        rom[0x0134] = (byte)'Z';
        rom[0x0135] = (byte)'X';
        rom[0x0136] = (byte)'B';
        rom[0x0137] = (byte)'O';
        rom[0x0138] = (byte)'X';
        rom[0x0147] = 0x42;
        rom[0x0148] = 0x03;
        rom[0x0149] = 0x02;
        rom[0x014A] = 0x01;
        rom[0x014B] = 0x33;
        rom[0x014C] = 0x07;
        rom[0x014D] = 0xA5;
        rom[0x014E] = 0x12;
        rom[0x014F] = 0x34;
        return rom;
    }
}
