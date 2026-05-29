using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZXBox.Hardware.Output;

namespace ZXBox.Core.Tests;

[TestClass]
public class ZxPrinterTests
{
    [TestMethod]
    public void PrinterOnlyRespondsWhenConnected()
    {
        var printer = new ZxPrinter();

        Assert.IsFalse(printer.TryReadPort(0x00FB, out _));
        Assert.IsFalse(printer.HandlePortWrite(0x00FB, 0x00));

        printer.Connect();

        Assert.IsTrue(printer.TryReadPort(0x00FB, out var status));
        Assert.AreEqual(0, status & 0x40);
        Assert.AreEqual(0, status & 0x80);
    }

    [TestMethod]
    public void PrinterRespondsOnAnyPortWithA2Low()
    {
        var printer = new ZxPrinter();
        printer.Connect();

        Assert.IsTrue(printer.TryReadPort(0x00FB, out _));
        Assert.IsTrue(printer.TryReadPort(0x00F3, out _));
        Assert.IsTrue(printer.TryReadPort(0x00EB, out _));
        Assert.IsFalse(printer.TryReadPort(0x00FF, out _));
    }

    [TestMethod]
    public void PrinterRaisesTheStartLatchAfterMotorStartupDelay()
    {
        var printer = new ZxPrinter();
        printer.Connect();
        printer.HandlePortWrite(0x00FB, 0x00);

        printer.AdvanceTStates((ZxPrinter.FastPixelStepTStates * 55) - 1);
        printer.TryReadPort(0x00FB, out var statusBeforeLatch);
        Assert.AreEqual(0, statusBeforeLatch & 0x80);

        printer.AdvanceTStates(1);
        printer.TryReadPort(0x00FB, out var statusAfterLatch);
        Assert.AreEqual(0x80, statusAfterLatch & 0x80);
    }

    [TestMethod]
    public void PrinterWritesPixelsAtRomVisibleTiming()
    {
        var printer = new ZxPrinter();
        printer.Connect();

        printer.HandlePortWrite(0x00FB, 0x00);
        printer.AdvanceTStates(ZxPrinter.FastPixelStepTStates * 64);

        printer.HandlePortWrite(0x00FB, 0x80);
        printer.AdvanceTStates(ZxPrinter.FastPixelStepTStates);
        printer.HandlePortWrite(0x00FB, 0x80);
        printer.AdvanceTStates(ZxPrinter.FastPixelStepTStates);
        printer.HandlePortWrite(0x00FB, 0x00);
        printer.HandlePortWrite(0x00FB, 0x04);

        var snapshot = printer.GetPaperSnapshot();

        Assert.AreEqual(1, snapshot.Height);
        Assert.AreEqual(1, snapshot.Pixels[0]);
        Assert.AreEqual(1, snapshot.Pixels[1]);
        Assert.AreEqual(0, snapshot.Pixels[2]);
    }

    [TestMethod]
    public void PrinterSlowModeUsesLongerPixelTiming()
    {
        var printer = new ZxPrinter();
        printer.Connect();
        printer.HandlePortWrite(0x00FB, 0x02);

        printer.AdvanceTStates(ZxPrinter.SlowPixelStepTStates * 64);
        printer.HandlePortWrite(0x00FB, 0x80);
        printer.AdvanceTStates(ZxPrinter.SlowPixelStepTStates - 1);
        printer.TryReadPort(0x00FB, out var statusBeforePixel);
        Assert.AreEqual(0, statusBeforePixel & 0x01);

        printer.AdvanceTStates(1);
        printer.TryReadPort(0x00FB, out var statusAfterPixel);
        Assert.AreEqual(1, statusAfterPixel & 0x01);
    }

    [TestMethod]
    public void SpectrumRoutesPrinterAliasesThroughDedicatedDevice()
    {
        var speccy = new ZXSpectrum();
        speccy.ConnectZxPrinter();

        speccy.Out(0x00F3, 0x00, 0);
        speccy.TstateChange(ZxPrinter.FastPixelStepTStates * 64);
        speccy.Out(0x00EB, 0x80, 0);
        speccy.TstateChange(ZxPrinter.FastPixelStepTStates);
        speccy.Out(0x00F3, 0x00, 0);
        speccy.Out(0x00FB, 0x04, 0);

        var status = speccy.In(0x00EB);
        var snapshot = speccy.ZxPrinter.GetPaperSnapshot();

        Assert.AreEqual(0, status & 0x40);
        Assert.AreEqual(0, status & 0x80);
        Assert.AreEqual(1, snapshot.Height);
        Assert.AreEqual(1, snapshot.Pixels[0]);
    }

    [TestMethod]
    public void PrinterMatchesRomStyleBitBangSequence()
    {
        var printer = new ZxPrinter();
        printer.Connect();

        var lineBytes = new byte[32];
        lineBytes[0] = 0b1010_0101;
        lineBytes[1] = 0b0101_1010;
        lineBytes[2] = 0b1111_0000;
        lineBytes[3] = 0b0000_1111;

        PrintRomStyleRow(printer, lineBytes, 192);

        var snapshot = printer.GetPaperSnapshot();
        Assert.AreEqual(1, snapshot.Height);

        var expectedBits = ExpandBits(lineBytes);
        for (var pixel = 0; pixel < expectedBits.Length; pixel++)
        {
            Assert.AreEqual(expectedBits[pixel], snapshot.Pixels[pixel], $"Pixel {pixel} did not match the ROM-style printer sequence.");
        }
    }

    [TestMethod]
    public void PrinterProgressAdvancesOnTstatesRatherThanObservation()
    {
        var printer = new ZxPrinter();
        printer.Connect();
        printer.HandlePortWrite(0x00FB, 0x00);
        printer.AdvanceTStates(ZxPrinter.FastPixelStepTStates * 64);
        printer.HandlePortWrite(0x00FB, 0x80);
        printer.AdvanceTStates(ZxPrinter.FastPixelStepTStates);

        var versionBeforeObservation = printer.PaperVersion;
        var snapshot = printer.GetPaperSnapshot();

        Assert.AreEqual(1, snapshot.Height);
        Assert.AreEqual(1, snapshot.Pixels[0]);
        Assert.AreEqual(versionBeforeObservation, printer.PaperVersion);

        printer.TryReadPort(0x00FB, out _);
        Assert.AreEqual(versionBeforeObservation, printer.PaperVersion);
    }

    [TestMethod]
    public void ImmediateLprintProducesVisiblePaper()
    {
        var speccy = new ZXSpectrum();
        speccy.ConnectZxPrinter();
        speccy.Reset();

        RunFrames(speccy, 300);

        var eLine = ReadWord(speccy, 0x5C59);
        var line = new byte[] { 0xE0, 0x22, (byte)'T', (byte)'E', (byte)'S', (byte)'T', 0x22, 0x0D };
        WriteBytes(speccy, eLine, line);
        WriteWord(speccy, 0x5C61, eLine + line.Length);
        speccy.WriteByteToMemory(0x5C44, 0x01);
        speccy.WriteByteToMemory(0x5C3B, (byte)(speccy.ReadByteFromMemory(0x5C3B) | 0x80));
        speccy.PC = 0x1B8A;

        RunFrames(speccy, 300);

        var snapshot = speccy.ZxPrinter.GetPaperSnapshot();
        Assert.IsTrue(snapshot.Height > 0, "Immediate LPRINT should produce printer paper.");

        var foundInk = false;
        for (var index = 0; index < snapshot.Pixels.Length; index++)
        {
            if (snapshot.Pixels[index] != 0)
            {
                foundInk = true;
                break;
            }
        }

        Assert.IsTrue(foundInk, "Immediate LPRINT should leave visible printed pixels on the paper.");
    }

    private static byte[] ExpandBits(byte[] lineBytes)
    {
        var bits = new byte[ZxPrinter.PaperWidth];
        for (var byteIndex = 0; byteIndex < lineBytes.Length; byteIndex++)
        {
            var value = lineBytes[byteIndex];
            for (var bit = 0; bit < 8; bit++)
            {
                bits[(byteIndex * 8) + bit] = (byte)(((value << bit) & 0x80) != 0 ? 1 : 0);
            }
        }

        return bits;
    }

    private static void PrintRomStyleRow(ZxPrinter printer, byte[] lineBytes, byte remainingRows)
    {
        var d = remainingRows < 3 ? 0x20 : 0x00;
        printer.HandlePortWrite(0x00FB, d);
        WaitForStatus(printer, 0x80);

        foreach (var lineByte in lineBytes)
        {
            var e = lineByte;
            for (var bit = 0; bit < 8; bit++)
            {
                var carry = (d & 0x80) != 0;
                d = ((d << 1) & 0xFF);

                var nextCarry = (e & 0x80) != 0;
                e = (byte)((e << 1) & 0xFF);
                if (carry)
                {
                    e |= 0x01;
                }

                d = (d >> 1) & 0x7F;
                if (nextCarry)
                {
                    d |= 0x80;
                }

                WaitForStatus(printer, 0x01);
                printer.HandlePortWrite(0x00FB, d);
            }
        }

        printer.HandlePortWrite(0x00FB, 0x04);
    }

    private static void WaitForStatus(ZxPrinter printer, int mask)
    {
        for (var guard = 0; guard < 100000; guard++)
        {
            printer.TryReadPort(0x00FB, out var status);
            if ((status & mask) != 0)
            {
                return;
            }

            printer.AdvanceTStates(1);
        }

        throw new InvalidOperationException($"Printer status mask 0x{mask:X2} did not become ready.");
    }

    private static void RunFrames(ZXSpectrum speccy, int frameCount)
    {
        for (var frameIndex = 0; frameIndex < frameCount; frameIndex++)
        {
            speccy.DoInstructions(69888);
        }
    }

    private static int ReadWord(ZXSpectrum speccy, int address)
    {
        return speccy.ReadByteFromMemory((ushort)address) | (speccy.ReadByteFromMemory((ushort)(address + 1)) << 8);
    }

    private static void WriteWord(ZXSpectrum speccy, int address, int value)
    {
        speccy.WriteByteToMemory((ushort)address, (byte)(value & 0xFF));
        speccy.WriteByteToMemory((ushort)(address + 1), (byte)((value >> 8) & 0xFF));
    }

    private static void WriteBytes(ZXSpectrum speccy, int address, byte[] bytes)
    {
        for (var index = 0; index < bytes.Length; index++)
        {
            speccy.WriteByteToMemory((ushort)(address + index), bytes[index]);
        }
    }

}
