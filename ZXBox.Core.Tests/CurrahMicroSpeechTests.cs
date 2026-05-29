using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using ZXBox.Hardware.Input;
using ZXBox.Hardware.Interfaces;
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

        var busyCleared = false;
        for (var frameIndex = 0; frameIndex < 8; frameIndex++)
        {
            if ((speccy.ReadByteFromMemory(0x1000) & 0x01) == 0)
            {
                busyCleared = true;
                break;
            }

            speccy.CurrahMicroSpeech.RenderAudioFrame(48000 / 50, 69888);
        }

        Assert.IsTrue(busyCleared);
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

    [TestMethod]
    public void CurrahBundledSpeechRomProducesAudibleSamples()
    {
        var speccy = new ZXSpectrum();
        speccy.ConnectCurrahMicroSpeech();
        speccy.LoadCurrahMicroSpeechRom(File.ReadAllBytes(Path.Combine(FindRepositoryRoot(), "Platforms", "ZXBox.Blazor", "wwwroot", "Roms", "CURRAH.ROM")));
        speccy.CurrahMicroSpeech.LoadSpeechRom(File.ReadAllBytes(Path.Combine(FindRepositoryRoot(), "Platforms", "ZXBox.Blazor", "wwwroot", "Roms", "SP0256-AL2.BIN")));

        QueueAllophone(speccy, 0x1B);

        var frame = speccy.CurrahMicroSpeech.RenderAudioFrame(48000 / 50, 69888);

        Assert.IsTrue(frame.Any(sample => Math.Abs(sample) > 0.0001f));
    }

    [TestMethod]
    public void CurrahBundledRomSpeaksBasicJKeypress()
    {
        var speccy = new ZXSpectrum();
        var keyboard = new TestKeyboard();
        speccy.InputHardware.Add(keyboard);
        speccy.ConnectCurrahMicroSpeech();
        speccy.LoadCurrahMicroSpeechRom(File.ReadAllBytes(Path.Combine(FindRepositoryRoot(), "Platforms", "ZXBox.Blazor", "wwwroot", "Roms", "CURRAH.ROM")));
        speccy.CurrahMicroSpeech.LoadSpeechRom(File.ReadAllBytes(Path.Combine(FindRepositoryRoot(), "Platforms", "ZXBox.Blazor", "wwwroot", "Roms", "SP0256-AL2.BIN")));
        speccy.Reset();

        RunFrames(speccy, 300);

        keyboard.JPressed = true;
        var spokeDuringKeypress = RunFrames(speccy, 50);
        keyboard.JPressed = false;
        var spokeAfterRelease = RunFrames(speccy, 50);
        var lastKey = speccy.ReadByteFromMemory(0x5C08);

        Assert.AreNotEqual(0, lastKey, $"Expected BASIC to recognize the J keypress. PC={speccy.PC:X4}.");
        Assert.IsTrue(spokeDuringKeypress || spokeAfterRelease);
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

    private static void QueueAllophone(ZXSpectrum speccy, byte allophone)
    {
        if (!speccy.CurrahMicroSpeech.Active)
        {
            speccy.ReadByteFromMemory(0x0038);
        }

        speccy.WriteByteToMemory(0x1000, allophone);
    }

    private static bool RunFrames(ZXSpectrum speccy, int frameCount)
    {
        for (var frameIndex = 0; frameIndex < frameCount; frameIndex++)
        {
            speccy.DoInstructions(69888);
            var frame = speccy.CurrahMicroSpeech.RenderAudioFrame(48000 / 50, 69888);
            if (frame.Any(sample => Math.Abs(sample) > 0.0001f))
            {
                return true;
            }
        }

        return false;
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "ZXBox.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate the repository root.");
    }

    private sealed class TestKeyboard : IInput
    {
        public bool JPressed { get; set; }

        public JoystickTypeEnum JoystickType { get; set; }

        public void AddTStates(int tstates)
        {
        }

        public byte Input(ushort port, int tstates)
        {
            if (!JPressed || (port & 0x00ff) != 0x00fe)
            {
                return 0xff;
            }

            var highByte = (byte)(port >> 8);
            var value = 0xff;

            if (highByte is 0x00 or 0x01 or 0x02)
            {
                value &= unchecked((byte)~1);
            }

            if ((highByte & (1 << 6)) == 0)
            {
                value &= unchecked((byte)~8);
            }

            return (byte)value;
        }
    }
}
