using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using ZXBox.Hardware.Speech;

namespace ZXBox.Core.Tests;

[TestClass]
public class CurrahMicroSpeechTests
{
    [TestMethod]
    public void CurrahReadAt0038TogglesOverlayAndOpcodeFetchStillWorks()
    {
        var baseline = new ZXSpectrum();
        var baselineValue = baseline.ReadByteFromMemory(0x0001);

        var speccy = new ZXSpectrum();
        speccy.ConnectCurrahMicroSpeech();

        var currahRom = CreateCurrahRom();
        speccy.LoadCurrahMicroSpeechRom(currahRom);

        speccy.ReadByteFromMemory(0x0038);

        Assert.IsTrue(speccy.CurrahMicroSpeech.Active);
        Assert.AreEqual(currahRom[0x0001], speccy.ReadByteFromMemory(0x0801));
        Assert.AreEqual(0xff, speccy.ReadByteFromMemory(0x2000));

        speccy.PC = 0x0038;
        speccy.NextOpcode();

        Assert.IsFalse(speccy.CurrahMicroSpeech.Active);
        Assert.AreEqual(baselineValue, speccy.ReadByteFromMemory(0x0001));
    }

    [TestMethod]
    public void CurrahPortReadAt0038StillTogglesOverlay()
    {
        var speccy = new ZXSpectrum();
        speccy.ConnectCurrahMicroSpeech();
        speccy.LoadCurrahMicroSpeechRom(CreateCurrahRom());

        Assert.IsFalse(speccy.CurrahMicroSpeech.Active);

        speccy.In(0x0038);
        Assert.IsTrue(speccy.CurrahMicroSpeech.Active);

        speccy.In(0x0038);
        Assert.IsFalse(speccy.CurrahMicroSpeech.Active);
    }

    [TestMethod]
    public void CurrahQueuedAllophoneSetsBusyAndProducesSpeechSamples()
    {
        var speccy = new ZXSpectrum();
        speccy.ConnectCurrahMicroSpeech();
        speccy.CurrahMicroSpeech.LoadSpeechRom(CreateSyntheticSpeechRom());

        speccy.ReadByteFromMemory(0x0038);
        speccy.WriteByteToMemory(0x1000, 0x00);

        Assert.AreEqual(1, speccy.ReadByteFromMemory(0x1000) & 0x01);

        var frame = speccy.CurrahMicroSpeech.RenderAudioFrame(48000 / 50, 69888);

        Assert.IsTrue(frame.Any(sample => Math.Abs(sample) > 0.001f));
        Assert.AreEqual(0, speccy.ReadByteFromMemory(0x1000) & 0x01);
    }

    [TestMethod]
    public void CurrahMasksAllophoneWritesToSixBits()
    {
        var speccy = new ZXSpectrum();
        speccy.ConnectCurrahMicroSpeech();
        speccy.CurrahMicroSpeech.LoadSpeechRom(CreateSyntheticSpeechRom());

        speccy.ReadByteFromMemory(0x0038);
        speccy.WriteByteToMemory(0x1000, 0x40);

        Assert.AreEqual(1, speccy.ReadByteFromMemory(0x1000) & 0x01);

        var frame = speccy.CurrahMicroSpeech.RenderAudioFrame(48000 / 50, 69888);

        Assert.IsTrue(frame.Any(sample => Math.Abs(sample) > 0.001f));
    }

    [TestMethod]
    public void CurrahDisconnectStopsOverlayAndAudio()
    {
        var speccy = new ZXSpectrum();
        speccy.ConnectCurrahMicroSpeech();
        speccy.CurrahMicroSpeech.LoadSpeechRom(CreateSyntheticSpeechRom());
        speccy.ReadByteFromMemory(0x0038);
        speccy.WriteByteToMemory(0x1000, 0x00);

        speccy.DisconnectCurrahMicroSpeech();

        Assert.IsFalse(speccy.CurrahMicroSpeech.Connected);
        Assert.IsFalse(speccy.CurrahMicroSpeech.Active);
        Assert.AreEqual(0xff, speccy.In(0x1000));
        CollectionAssert.AreEqual(new float[16], speccy.CurrahMicroSpeech.RenderAudioFrame(16, 69888));
    }

    private static byte[] CreateCurrahRom()
    {
        var rom = new byte[CurrahMicroSpeech.CurrahRomSize];
        for (var i = 0; i < rom.Length; i++)
        {
            rom[i] = (byte)((i + 0x40) & 0xff);
        }

        return rom;
    }

    private static byte[] CreateSyntheticSpeechRom()
    {
        var rom = new byte[Sp0256Chip.Sp0256RomSize];
        rom[0] = 0x87; // immed4=7, opcode=8? actual LOAD_E uses fetched opcode 0x7, repeat 7
        rom[1] = 0x1f; // amplitude
        rom[2] = 0x20; // period
        rom[3] = 0x00; // halt
        return rom;
    }
}
