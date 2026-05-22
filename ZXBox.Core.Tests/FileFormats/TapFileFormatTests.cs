using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ZXBox.Core.Hardware.Input;
using ZXBox.Core.Tape;

namespace ZXBox.Core.Tests.FileFormats;

[TestClass]
public class TapFileFormatTests
{
    [TestMethod]
    public void ReadFileParsesLengthPrefixedTapBlocks()
    {
        var tapFormat = new TapFormat();
        var bytes = new byte[]
        {
            0x03, 0x00, 0x00, 0xAA, 0x55,
            0x02, 0x00, 0xFF, 0x10
        };

        var tapeImage = tapFormat.ReadFile(bytes);

        Assert.AreEqual(2, tapeImage.Blocks.Count);
        Assert.IsInstanceOfType<TapeDataBlock>(tapeImage.Blocks[0]);
        Assert.IsInstanceOfType<TapeDataBlock>(tapeImage.Blocks[1]);
        CollectionAssert.AreEqual(new byte[] { 0x00, 0xAA, 0x55 }, ((TapeDataBlock)tapeImage.Blocks[0]).Data);
        CollectionAssert.AreEqual(new byte[] { 0xFF, 0x10 }, ((TapeDataBlock)tapeImage.Blocks[1]).Data);
    }

    [TestMethod]
    public void ReadFileKeepsShortFinalBlockWhenTapDataIsTruncated()
    {
        var tapFormat = new TapFormat();
        var bytes = new byte[]
        {
            0x04, 0x00, 0x01, 0x02
        };

        var tapeImage = tapFormat.ReadFile(bytes);

        Assert.AreEqual(1, tapeImage.Blocks.Count);
        Assert.IsInstanceOfType<TapeDataBlock>(tapeImage.Blocks[0]);
        CollectionAssert.AreEqual(new byte[] { 0x01, 0x02 }, ((TapeDataBlock)tapeImage.Blocks[0]).Data);
    }

    [TestMethod]
    public void TapePlayerLoadTapeBuildsExpectedPulseSequence()
    {
        var tapePlayer = new TapePlayer(null!);
        var bytes = new byte[]
        {
            0x02, 0x00, 0x00, 0x80
        };

        tapePlayer.LoadTape(bytes);

        Assert.AreEqual(8063, tapePlayer.EarValues.Count(earValue => earValue.Pulse == PulseTypeEnum.Pilot));
        Assert.AreEqual(PulseTypeEnum.Sync1, tapePlayer.EarValues[8063].Pulse);
        Assert.AreEqual(PulseTypeEnum.Sync2, tapePlayer.EarValues[8064].Pulse);
        Assert.AreEqual(32, tapePlayer.EarValues.Count(earValue => earValue.Pulse == PulseTypeEnum.Data));
        Assert.AreEqual(PulseTypeEnum.Pause, tapePlayer.EarValues[^3].Pulse);
        Assert.AreEqual(PulseTypeEnum.Termination, tapePlayer.EarValues[^2].Pulse);
        Assert.AreEqual(PulseTypeEnum.Stop, tapePlayer.EarValues[^1].Pulse);
    }
}
