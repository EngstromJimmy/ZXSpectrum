using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ZXBox.Core.Tests;

[TestClass]
public class ZxSpectrumAyIntegrationTests
{
    [TestMethod]
    public void Spectrum128kRoutesAyPortsWhile48kIgnoresThem()
    {
        var plus = new ZXSpectrum(rom: RomEnum.ZXSpectrumPlus);
        plus.Reset();
        ProgramChannelATone(plus);

        var plusFrame = plus.AyChip.RenderAudioFrame(960, plus.FrameTStates);

        var classic48 = new ZXSpectrum(rom: RomEnum.ZXSpectrum48k);
        classic48.Reset();
        ProgramChannelATone(classic48);

        var classic48Frame = classic48.AyChip.RenderAudioFrame(960, classic48.FrameTStates);

        Assert.IsTrue(plusFrame.Any(sample => Math.Abs(sample) > 0.01f));
        Assert.IsFalse(classic48Frame.Any(sample => Math.Abs(sample) > 0.001f));
    }

    [TestMethod]
    public void Spectrum128kPagingUsesAllThreeBankBitsAndHonorsPagingLock()
    {
        var speccy = new ZXSpectrum(rom: RomEnum.ZXSpectrumPlus);
        speccy.Reset();

        speccy.Out(0x3FFD, 0x27, 0);
        speccy.WriteByteToMemory(0xC000, 0xA5);

        Assert.AreEqual(0xA5, speccy.Banks[7][0]);

        speccy.Out(0x7FFD, 0x00, 0);
        speccy.WriteByteToMemory(0xC001, 0x5A);

        Assert.AreEqual(0x00, speccy.Banks[0][1]);
        Assert.AreEqual(0x5A, speccy.Banks[7][1]);
    }

    private static void ProgramChannelATone(ZXSpectrum speccy)
    {
        speccy.Out(0xFFFD, 0x00, 0);
        speccy.Out(0xBFFD, 0x40, 0);
        speccy.Out(0xFFFD, 0x01, 0);
        speccy.Out(0xBFFD, 0x00, 0);
        speccy.Out(0xFFFD, 0x07, 0);
        speccy.Out(0xBFFD, 0x3E, 0);
        speccy.Out(0xFFFD, 0x08, 0);
        speccy.Out(0xBFFD, 0x0F, 0);
    }
}
