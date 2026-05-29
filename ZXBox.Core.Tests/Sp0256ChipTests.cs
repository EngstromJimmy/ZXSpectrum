using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using ZXBox.Hardware.Speech;

namespace ZXBox.Core.Tests;

[TestClass]
public class Sp0256ChipTests
{
    [TestMethod]
    public void Sp0256RejectsSmallRoms()
    {
        var chip = new Sp0256Chip();

        try
        {
            chip.LoadRom(new byte[32]);
            Assert.Fail("Expected LoadRom to reject undersized SP0256 ROMs.");
        }
        catch (ArgumentException)
        {
        }
    }

    [TestMethod]
    public void Sp0256StaysBusyUntilQueuedAllophoneCompletes()
    {
        var chip = new Sp0256Chip();
        chip.LoadRom(CreateSyntheticSpeechRom());

        chip.WriteAllophone(0x00, 0);
        Assert.IsTrue(chip.ReadBusy(0));

        var frame = chip.RenderFrame(48000 / 50, 69888);
        Assert.IsTrue(frame.Any(sample => Math.Abs(sample) > 0.001f));

        var busyCleared = false;
        for (var frameIndex = 0; frameIndex < 8; frameIndex++)
        {
            if (!chip.ReadBusy(0))
            {
                busyCleared = true;
                break;
            }

            chip.RenderFrame(48000 / 50, 69888);
        }

        Assert.IsTrue(busyCleared);
    }

    [TestMethod]
    public void Sp0256WithoutRomStaysReadyAndSilent()
    {
        var chip = new Sp0256Chip();
        chip.WriteAllophone(0x00, 0);
        Assert.IsFalse(chip.ReadBusy(0));
        CollectionAssert.AreEqual(new float[16], chip.RenderFrame(16, 69888));
    }

    private static byte[] CreateSyntheticSpeechRom()
    {
        var rom = new byte[Sp0256Chip.Sp0256RomSize];
        rom[0] = 0x87;
        rom[1] = 0x1f;
        rom[2] = 0x20;
        rom[3] = 0x00;
        return rom;
    }
}
