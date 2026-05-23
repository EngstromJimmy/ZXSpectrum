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
        Assert.IsFalse(printer.HandlePortWrite(0x00FB, 0x80));

        printer.Connect();

        Assert.IsTrue(printer.TryReadPort(0x00FB, out var status));
        Assert.AreEqual(0, status & 0x40);
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
    public void PrinterUsesConfiguredFastPixelTiming()
    {
        var printer = new ZxPrinter();
        printer.Connect();

        printer.HandlePortWrite(0x00FB, 0x80);
        printer.TryReadPort(0x00FB, out var statusAfterWrite);
        Assert.AreEqual(0, statusAfterWrite & 0x01);
        Assert.AreEqual(0x80, statusAfterWrite & 0x80);

        printer.AdvanceTStates(ZxPrinter.FastPixelStepTStates - 1);
        printer.TryReadPort(0x00FB, out var statusBeforeStep);
        Assert.AreEqual(0, statusBeforeStep & 0x01);

        printer.AdvanceTStates(1);
        printer.TryReadPort(0x00FB, out var statusAfterStep);
        Assert.AreEqual(1, statusAfterStep & 0x01);

        var snapshot = printer.GetPaperSnapshot();
        Assert.AreEqual(1, snapshot.Height);
        Assert.AreEqual(1, snapshot.Pixels[0]);
    }

    [TestMethod]
    public void PrinterPoweringStylusOnRaisesPaperStartLatchWithoutMirroringStylusState()
    {
        var printer = new ZxPrinter();
        printer.Connect();

        printer.HandlePortWrite(0x00FB, 0x80);
        printer.TryReadPort(0x00FB, out var statusAfterPowerOn);
        Assert.AreEqual(0x80, statusAfterPowerOn & 0x80);

        printer.HandlePortWrite(0x00FB, 0x80);
        printer.TryReadPort(0x00FB, out var statusAfterSecondWrite);
        Assert.AreEqual(0, statusAfterSecondWrite & 0x80);
    }

    [TestMethod]
    public void PrinterCompletesScanlineAfter256Pixels()
    {
        var printer = new ZxPrinter();
        printer.Connect();
        printer.HandlePortWrite(0x00FB, 0x80);

        for (var pixel = 0; pixel < ZxPrinter.PaperWidth; pixel++)
        {
            printer.AdvanceTStates(ZxPrinter.FastPixelStepTStates);

            if (pixel < ZxPrinter.PaperWidth - 1)
            {
                printer.HandlePortWrite(0x00FB, 0x80);
            }
        }

        printer.TryReadPort(0x00FB, out var status);
        var snapshot = printer.GetPaperSnapshot();

        Assert.AreEqual(0x80, status & 0x80);
        Assert.AreEqual(1, snapshot.Height);
        Assert.AreEqual(1, snapshot.Pixels[ZxPrinter.PaperWidth - 1]);
    }

    [TestMethod]
    public void PrinterMotorStopFreezesPrinting()
    {
        var printer = new ZxPrinter();
        printer.Connect();
        printer.HandlePortWrite(0x00FB, 0x84);

        printer.AdvanceTStates(ZxPrinter.FastPixelStepTStates * 4);
        printer.TryReadPort(0x00FB, out var status);
        var snapshot = printer.GetPaperSnapshot();

        Assert.AreEqual(0, status & 0x01);
        Assert.AreEqual(0, snapshot.Height);
    }

    [TestMethod]
    public void PrinterSlowModeUsesLongerPixelTiming()
    {
        var printer = new ZxPrinter();
        printer.Connect();
        printer.HandlePortWrite(0x00FB, 0x82);

        printer.AdvanceTStates(ZxPrinter.SlowPixelStepTStates - 1);
        printer.TryReadPort(0x00FB, out var statusBeforeStep);
        Assert.AreEqual(0, statusBeforeStep & 0x01);

        printer.AdvanceTStates(1);
        printer.TryReadPort(0x00FB, out var statusAfterStep);
        var snapshot = printer.GetPaperSnapshot();

        Assert.AreEqual(1, statusAfterStep & 0x01);
        Assert.AreEqual(1, snapshot.Height);
        Assert.AreEqual(1, snapshot.Pixels[0]);
    }

    [TestMethod]
    public void SpectrumRoutesPrinterAliasesThroughDedicatedDevice()
    {
        var speccy = new ZXSpectrum();
        speccy.ConnectZxPrinter();

        speccy.Out(0x00F3, 0x80, 0);
        speccy.TstateChange(ZxPrinter.FastPixelStepTStates);

        var status = speccy.In(0x00EB);
        var snapshot = speccy.ZxPrinter.GetPaperSnapshot();

        Assert.AreEqual(0, status & 0x40);
        Assert.AreEqual(1, status & 0x01);
        Assert.AreEqual(1, snapshot.Height);
        Assert.AreEqual(1, snapshot.Pixels[0]);
    }
}
