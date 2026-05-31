using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZXBox.Core.Hardware.Input;
using ZXBox.Core.Tape;

namespace ZXBox.Core.Tests;

[TestClass]
public class TapeQuickLoadTests
{
    [TestMethod]
    public void DoInstructionsQuickLoadsStandardBlockIntoMemory()
    {
        var spectrum = CreateSpectrum(out var tapePlayer);
        var tapeImage = new TapeImage();
        tapeImage.Blocks.Add(CreateStandardDataBlock(0xFF, 0x12, 0x34, 0x56));

        tapePlayer.LoadTape(tapeImage);
        tapePlayer.Play();
        PrepareQuickLoadState(spectrum, 0xFF, true, 0x8000, 3);

        spectrum.DoInstructions(4);

        CollectionAssert.AreEqual(new byte[] { 0x12, 0x34, 0x56 }, new byte[]
        {
            spectrum.ReadByteFromMemory(0x8000),
            spectrum.ReadByteFromMemory(0x8001),
            spectrum.ReadByteFromMemory(0x8002)
        });
        Assert.AreEqual((ushort)0x053F, spectrum.PC);
        Assert.AreEqual((ushort)0x8003, spectrum.IX);
        Assert.AreEqual((ushort)0x0000, spectrum.DE);
        Assert.AreEqual((ushort)0x9002, spectrum.SP);
        Assert.AreEqual(1, spectrum.F & 0x01);
        Assert.IsFalse(tapePlayer.IsPlaying);
    }

    [TestMethod]
    public void DoInstructionsQuickLoadVerifyFailureLeavesMemoryUnchanged()
    {
        var spectrum = CreateSpectrum(out var tapePlayer);
        var tapeImage = new TapeImage();
        tapeImage.Blocks.Add(CreateStandardDataBlock(0xFF, 0x12, 0x34, 0x56));

        tapePlayer.LoadTape(tapeImage);
        tapePlayer.Play();
        PrepareQuickLoadState(spectrum, 0xFF, false, 0x8000, 3);
        spectrum.WriteByteToMemory(0x8000, 0x00);
        spectrum.WriteByteToMemory(0x8001, 0x34);
        spectrum.WriteByteToMemory(0x8002, 0x56);

        spectrum.DoInstructions(4);

        CollectionAssert.AreEqual(new byte[] { 0x00, 0x34, 0x56 }, new byte[]
        {
            spectrum.ReadByteFromMemory(0x8000),
            spectrum.ReadByteFromMemory(0x8001),
            spectrum.ReadByteFromMemory(0x8002)
        });
        Assert.AreEqual((ushort)0x053F, spectrum.PC);
        Assert.AreEqual((ushort)0x8000, spectrum.IX);
        Assert.AreEqual((ushort)0x0003, spectrum.DE);
        Assert.AreEqual((ushort)0x9002, spectrum.SP);
        Assert.AreEqual(0, spectrum.F & 0x01);
        Assert.IsFalse(tapePlayer.IsPlaying);
    }

    [TestMethod]
    public void TryConsumeNextQuickLoadBlockAdvancesAcrossBlocks()
    {
        var tapePlayer = new TapePlayer(null);
        var tapeImage = new TapeImage();
        var headerBlock = CreateStandardDataBlock(0x00, 0x01, 0x02);
        var dataBlock = CreateStandardDataBlock(0xFF, 0x10, 0x20, 0x30);
        tapeImage.Blocks.Add(headerBlock);
        tapeImage.Blocks.Add(dataBlock);

        tapePlayer.LoadTape(tapeImage);
        tapePlayer.Play();

        Assert.IsTrue(tapePlayer.TryConsumeNextQuickLoadBlock(out var firstBlock));
        Assert.AreSame(headerBlock, firstBlock);
        Assert.IsTrue(tapePlayer.IsPlaying);
        Assert.IsTrue(tapePlayer.TryConsumeNextQuickLoadBlock(out var secondBlock));
        Assert.AreSame(dataBlock, secondBlock);
        Assert.IsFalse(tapePlayer.IsPlaying);
    }

    private static ZXSpectrum CreateSpectrum(out TapePlayer tapePlayer)
    {
        var spectrum = new ZXSpectrum();
        tapePlayer = new TapePlayer(null);
        spectrum.InputHardware.Add(tapePlayer);
        return spectrum;
    }

    private static void PrepareQuickLoadState(ZXSpectrum spectrum, byte expectedFlag, bool loadBytes, ushort destination, ushort length)
    {
        spectrum.PC = 0x056C;
        spectrum.SP = 0x9000;
        spectrum.WriteWordToMemory(spectrum.SP, 0x053F);
        spectrum.APrim = expectedFlag;
        spectrum.FPrim = loadBytes ? (byte)0x01 : (byte)0x00;
        spectrum.IX = destination;
        spectrum.DE = length;
    }

    private static TapeDataBlock CreateStandardDataBlock(byte flag, params byte[] payload)
    {
        var data = new byte[payload.Length + 2];
        data[0] = flag;
        data[^1] = flag;

        for (var index = 0; index < payload.Length; index++)
        {
            data[index + 1] = payload[index];
            data[^1] ^= payload[index];
        }

        return new TapeDataBlock
        {
            Data = data,
            PilotPulseLength = 2168,
            PilotPulseCount = flag < 0x80 ? 8063 : 3223,
            SyncFirstPulseLength = 667,
            SyncSecondPulseLength = 735,
            ZeroBitPulseLength = 855,
            OneBitPulseLength = 1710,
            UsedBitsInLastByte = 8,
            PauseAfterMilliseconds = 1000
        };
    }
}
