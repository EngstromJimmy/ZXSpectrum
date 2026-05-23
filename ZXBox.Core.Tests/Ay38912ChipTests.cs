using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ZXBox.Hardware.Sound;

namespace ZXBox.Core.Tests;

[TestClass]
public class Ay38912ChipTests
{
    [TestMethod]
    public void AyPortWritesRoundTripRegisters()
    {
        var chip = new Ay38912Chip();

        WriteRegister(chip, 0xC000, 0x8000, 0, 0x34);
        WriteRegister(chip, 0xFFFD, 0xBFFD, 1, 0x1F);
        WriteRegister(chip, 0xFFFD, 0xBFFD, 6, 0xFF);
        WriteRegister(chip, 0xFFFD, 0xBFFD, 8, 0x3F);

        Assert.AreEqual(0x34, ReadRegister(chip, 0));
        Assert.AreEqual(0x0F, ReadRegister(chip, 1));
        Assert.AreEqual(0x1F, ReadRegister(chip, 6));
        Assert.AreEqual(0x1F, ReadRegister(chip, 8));
    }

    [TestMethod]
    public void AyToneGenerationProducesExpectedFrequency()
    {
        var chip = new Ay38912Chip();
        ProgramChannelATone(chip, 0x0100, 0x0F, 0);

        var frame = chip.RenderAudioFrame(960, Ay38912Chip.FrameTStates128k);
        var estimatedFrequency = EstimateFrequency(frame, 48000);

        Assert.IsTrue(frame.Any(sample => Math.Abs(sample) > 0.01f));
        Assert.IsTrue(estimatedFrequency > 380d && estimatedFrequency < 490d, $"Expected ~433 Hz, got {estimatedFrequency:F2} Hz.");
    }

    [TestMethod]
    public void AyMidFrameVolumeChangeSilencesSecondHalf()
    {
        var chip = new Ay38912Chip();
        ProgramChannelATone(chip, 0x0080, 0x0F, 0);

        WriteRegister(chip, 0xFFFD, 0xBFFD, 8, 0x00, Ay38912Chip.FrameTStates128k / 2);

        var frame = chip.RenderAudioFrame(960, Ay38912Chip.FrameTStates128k);
        var firstQuarter = AverageAbsolute(frame.Take(240));
        var lastQuarter = AverageAbsolute(frame.Skip(720));

        Assert.IsTrue(firstQuarter > 0.02f, $"Expected audible first quarter, got {firstQuarter:F4}.");
        Assert.IsTrue(lastQuarter < firstQuarter / 4f, $"Expected quiet last quarter, first={firstQuarter:F4}, last={lastQuarter:F4}.");
    }

    private static void ProgramChannelATone(Ay38912Chip chip, int period, int volume, int tState)
    {
        WriteRegister(chip, 0xFFFD, 0xBFFD, 0, period & 0xFF, tState);
        WriteRegister(chip, 0xFFFD, 0xBFFD, 1, (period >> 8) & 0x0F, tState);
        WriteRegister(chip, 0xFFFD, 0xBFFD, 7, 0x3E, tState);
        WriteRegister(chip, 0xFFFD, 0xBFFD, 8, volume & 0x0F, tState);
    }

    private static void WriteRegister(Ay38912Chip chip, int selectPort, int dataPort, int register, int value, int tState = 0)
    {
        chip.HandlePortWrite(selectPort, register, tState);
        chip.HandlePortWrite(dataPort, value, tState);
    }

    private static int ReadRegister(Ay38912Chip chip, int register)
    {
        chip.HandlePortWrite(0xFFFD, register, 0);
        chip.TryReadPort(0xFFFD, 0, out var value);
        return value;
    }

    private static double EstimateFrequency(float[] frame, int sampleRate)
    {
        var zeroCrossings = 0;

        for (var i = 1; i < frame.Length; i++)
        {
            if ((frame[i - 1] <= 0f && frame[i] > 0f) || (frame[i - 1] >= 0f && frame[i] < 0f))
            {
                zeroCrossings++;
            }
        }

        return zeroCrossings / (2d * frame.Length / sampleRate);
    }

    private static float AverageAbsolute(IEnumerable<float> samples)
    {
        return samples.Select(sample => Math.Abs(sample)).DefaultIfEmpty().Average();
    }
}
