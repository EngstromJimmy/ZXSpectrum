using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ZXBox.Core.Hardware.Input;
using ZXBox.Core.Tape;

namespace ZXBox.Core.Tests.FileFormats;

[TestClass]
public class TzxFormatTests
{
    [TestMethod]
    public void StandardSpeedTzxBlockMatchesTapPulseStream()
    {
        var tapBytes = new byte[]
        {
            0x02, 0x00, 0x00, 0xA5
        };
        var tzxBytes = CreateTzx(
            0x10,
            0xE8, 0x03,
            0x02, 0x00,
            0x00, 0xA5);

        var tapPlayer = new TapePlayer(null);
        var tzxPlayer = new TapePlayer(null);

        tapPlayer.LoadTape(tapBytes, "demo.tap");
        tzxPlayer.LoadTape(tzxBytes, "demo.tzx");

        Assert.AreEqual(tapPlayer.EarValues.Count, tzxPlayer.EarValues.Count);
        for (var index = 0; index < tapPlayer.EarValues.Count; index++)
        {
            Assert.AreEqual(tapPlayer.EarValues[index].Pulse, tzxPlayer.EarValues[index].Pulse, $"Pulse mismatch at {index}.");
            Assert.AreEqual(tapPlayer.EarValues[index].Ear, tzxPlayer.EarValues[index].Ear, $"Signal mismatch at {index}.");
            Assert.AreEqual(tapPlayer.EarValues[index].TState, tzxPlayer.EarValues[index].TState, $"Timing mismatch at {index}.");
        }
    }

    [TestMethod]
    public void StandardSpeedTzxBlockIsMarkedAsQuickLoadCompatible()
    {
        var tzxBytes = CreateTzx(
            0x10,
            0xE8, 0x03,
            0x02, 0x00,
            0x00, 0xA5);

        var tapeImage = new TzxFormat().ReadFile(tzxBytes);
        var block = (TapeDataBlock)tapeImage.Blocks[0];

        Assert.IsTrue(block.IsQuickLoadCandidate);
    }

    [TestMethod]
    public void TzxFormatSkipsMetadataAndResolvesLoops()
    {
        var tzxBytes = CreateTzx(
            0x30, 0x04, (byte)'T', (byte)'E', (byte)'S', (byte)'T',
            0x24, 0x02, 0x00,
            0x12, 0x64, 0x00, 0x01, 0x00,
            0x25);

        var tapeImage = new TzxFormat().ReadFile(tzxBytes);

        Assert.AreEqual(2, tapeImage.Blocks.Count);
        Assert.IsTrue(tapeImage.Blocks.TrueForAll(block => block is TapePureToneBlock));
    }

    [TestMethod]
    public void TurboDataBlockHonorsUsedBitsInLastByte()
    {
        var tzxBytes = CreateTzx(
            0x11,
            0x10, 0x00,
            0x20, 0x00,
            0x30, 0x00,
            0x04, 0x00,
            0x08, 0x00,
            0x00, 0x00,
            0x03,
            0x00, 0x00,
            0x01, 0x00, 0x00,
            0xA0);

        var tapeImage = new TzxFormat().ReadFile(tzxBytes);
        var block = (TapeDataBlock)tapeImage.Blocks[0];
        var tapePlayer = new TapePlayer(null);
        tapePlayer.LoadTape(tapeImage);

        Assert.AreEqual(3, block.UsedBitsInLastByte);
        Assert.AreEqual(6, tapePlayer.EarValues.Count(earValue => earValue.Pulse == PulseTypeEnum.Data));
    }

    [TestMethod]
    public void ZeroLengthPauseCreatesExplicitStopBlock()
    {
        var tzxBytes = CreateTzx(
            0x20, 0x00, 0x00);

        var tapeImage = new TzxFormat().ReadFile(tzxBytes);

        Assert.AreEqual(1, tapeImage.Blocks.Count);
        Assert.IsInstanceOfType<TapeStopBlock>(tapeImage.Blocks[0]);
    }

    private static byte[] CreateTzx(params byte[] body)
    {
        return
        [
            (byte)'Z',
            (byte)'X',
            (byte)'T',
            (byte)'a',
            (byte)'p',
            (byte)'e',
            (byte)'!',
            0x1A,
            0x01,
            0x20,
            .. body
        ];
    }
}
