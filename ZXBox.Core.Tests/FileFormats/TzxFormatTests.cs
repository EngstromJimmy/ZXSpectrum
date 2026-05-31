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

    [TestMethod]
    public void DirectRecordingBlockPreservesSampleLevelsAndUsedBits()
    {
        var tzxBytes = CreateTzx(
            0x15,
            0x0A, 0x00,
            0x64, 0x00,
            0x03,
            0x01, 0x00, 0x00,
            0xA0);

        var tapeImage = new TzxFormat().ReadFile(tzxBytes);
        var block = (TapeDirectRecordingBlock)tapeImage.Blocks[0];
        var tapePlayer = new TapePlayer(null);
        tapePlayer.LoadTape(tapeImage);

        Assert.AreEqual(10, block.TStatesPerSample);
        Assert.AreEqual(100, block.PauseAfterMilliseconds);
        Assert.AreEqual(3, block.UsedBitsInLastByte);
        CollectionAssert.AreEqual(new byte[] { 0xA0 }, block.Data);

        Assert.AreEqual(7, tapePlayer.EarValues.Count);
        Assert.AreEqual((PulseTypeEnum.Data, true, 0L), (tapePlayer.EarValues[0].Pulse, tapePlayer.EarValues[0].Ear, tapePlayer.EarValues[0].TState));
        Assert.AreEqual((PulseTypeEnum.Data, false, 10L), (tapePlayer.EarValues[1].Pulse, tapePlayer.EarValues[1].Ear, tapePlayer.EarValues[1].TState));
        Assert.AreEqual((PulseTypeEnum.Data, true, 20L), (tapePlayer.EarValues[2].Pulse, tapePlayer.EarValues[2].Ear, tapePlayer.EarValues[2].TState));
        Assert.AreEqual((PulseTypeEnum.Pause, false, 3530L), (tapePlayer.EarValues[3].Pulse, tapePlayer.EarValues[3].Ear, tapePlayer.EarValues[3].TState));
        Assert.AreEqual((PulseTypeEnum.Pause, false, 350030L), (tapePlayer.EarValues[4].Pulse, tapePlayer.EarValues[4].Ear, tapePlayer.EarValues[4].TState));
        Assert.AreEqual(PulseTypeEnum.Termination, tapePlayer.EarValues[5].Pulse);
        Assert.AreEqual(PulseTypeEnum.Stop, tapePlayer.EarValues[6].Pulse);
    }

    [TestMethod]
    public void CallSequenceResolvesCalledBlocksInOrder()
    {
        var tzxBytes = CreateTzx(
            0x26, 0x02, 0x00, 0x03, 0x00, 0x05, 0x00,
            0x10, 0xE8, 0x03, 0x03, 0x00, 0xFF, 0x40, 0xBF,
            0x23, 0x05, 0x00,
            0x10, 0xE8, 0x03, 0x03, 0x00, 0xFF, 0x10, 0xEF,
            0x27,
            0x10, 0xE8, 0x03, 0x03, 0x00, 0xFF, 0x30, 0xCF,
            0x27,
            0x10, 0xE8, 0x03, 0x03, 0x00, 0xFF, 0x50, 0xAF);

        var tapeImage = new TzxFormat().ReadFile(tzxBytes);

        Assert.AreEqual(4, tapeImage.Blocks.Count);
        CollectionAssert.AreEqual(new byte[] { 0xFF, 0x10, 0xEF }, ((TapeDataBlock)tapeImage.Blocks[0]).Data);
        CollectionAssert.AreEqual(new byte[] { 0xFF, 0x30, 0xCF }, ((TapeDataBlock)tapeImage.Blocks[1]).Data);
        CollectionAssert.AreEqual(new byte[] { 0xFF, 0x40, 0xBF }, ((TapeDataBlock)tapeImage.Blocks[2]).Data);
        CollectionAssert.AreEqual(new byte[] { 0xFF, 0x50, 0xAF }, ((TapeDataBlock)tapeImage.Blocks[3]).Data);
    }

    [TestMethod]
    public void SelectBlockUsesFirstOption()
    {
        var tzxBytes = CreateTzx(
            0x28, 0x09, 0x00, 0x02, 0x01, 0x00, 0x01, (byte)'A', 0x04, 0x00, 0x01, (byte)'B',
            0x10, 0xE8, 0x03, 0x03, 0x00, 0xFF, 0x10, 0xEF,
            0x23, 0x02, 0x00,
            0x10, 0xE8, 0x03, 0x03, 0x00, 0xFF, 0x20, 0xDF,
            0x10, 0xE8, 0x03, 0x03, 0x00, 0xFF, 0x30, 0xCF);

        var tapeImage = new TzxFormat().ReadFile(tzxBytes);

        Assert.AreEqual(2, tapeImage.Blocks.Count);
        CollectionAssert.AreEqual(new byte[] { 0xFF, 0x10, 0xEF }, ((TapeDataBlock)tapeImage.Blocks[0]).Data);
        CollectionAssert.AreEqual(new byte[] { 0xFF, 0x30, 0xCF }, ((TapeDataBlock)tapeImage.Blocks[1]).Data);
    }

    [TestMethod]
    public void CswRecordingBlockDecodesPulseStream()
    {
        var tzxBytes = CreateTzx(
            0x18,
            0x11, 0x00, 0x00, 0x00,
            0x64, 0x00,
            0x44, 0xAC, 0x00,
            0x01,
            0x02, 0x00, 0x00, 0x00,
            0x01, 0x02, 0x00, 0x10, 0x27, 0x00, 0x00);

        var tapeImage = new TzxFormat().ReadFile(tzxBytes);
        var block = (TapeCswRecordingBlock)tapeImage.Blocks[0];
        var tapePlayer = new TapePlayer(null);
        tapePlayer.LoadTape(tapeImage);

        CollectionAssert.AreEqual(new[] { 79, 159, 793651 }, block.PulseLengths.ToArray());
        Assert.AreEqual(100, block.PauseAfterMilliseconds);
        Assert.AreEqual((PulseTypeEnum.Data, true, 79L), (tapePlayer.EarValues[0].Pulse, tapePlayer.EarValues[0].Ear, tapePlayer.EarValues[0].TState));
        Assert.AreEqual((PulseTypeEnum.Data, false, 238L), (tapePlayer.EarValues[1].Pulse, tapePlayer.EarValues[1].Ear, tapePlayer.EarValues[1].TState));
        Assert.AreEqual((PulseTypeEnum.Data, true, 793889L), (tapePlayer.EarValues[2].Pulse, tapePlayer.EarValues[2].Ear, tapePlayer.EarValues[2].TState));
    }

    [TestMethod]
    public void GeneralizedDataBlockDecodesSymbolStream()
    {
        var tzxBytes = CreateTzx(
            0x19,
            0x15, 0x00, 0x00, 0x00,
            0x64, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00,
            0x00,
            0x03, 0x00, 0x00, 0x00,
            0x01,
            0x02,
            0x00, 0x64, 0x00,
            0x00, 0xC8, 0x00,
            0xA0);

        var tapeImage = new TzxFormat().ReadFile(tzxBytes);
        var block = (TapeGeneralizedDataBlock)tapeImage.Blocks[0];
        var tapePlayer = new TapePlayer(null);
        tapePlayer.LoadTape(tapeImage);

        Assert.AreEqual(2, block.DataSymbols.Count);
        Assert.AreEqual(3, block.DataSymbolCount);
        Assert.AreEqual(100, block.PauseAfterMilliseconds);
        Assert.AreEqual((PulseTypeEnum.Data, true, 200L), (tapePlayer.EarValues[0].Pulse, tapePlayer.EarValues[0].Ear, tapePlayer.EarValues[0].TState));
        Assert.AreEqual((PulseTypeEnum.Data, false, 300L), (tapePlayer.EarValues[1].Pulse, tapePlayer.EarValues[1].Ear, tapePlayer.EarValues[1].TState));
        Assert.AreEqual((PulseTypeEnum.Data, true, 500L), (tapePlayer.EarValues[2].Pulse, tapePlayer.EarValues[2].Ear, tapePlayer.EarValues[2].TState));
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
