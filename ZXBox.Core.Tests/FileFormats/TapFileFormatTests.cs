using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using ZXBox.Core.Hardware.Input;
using ZXBox.Core.Tape;

namespace ZXBox.Core.Tests.FileFormats;

[TestClass]
public class TapFileFormatTests
{
    [TestMethod]
    public void LoadTapFileTest()
    {
        var filename = @"Binaries\froggers.tap";
        var tf = new TapFormat();
        var bytes = File.ReadAllBytes(filename);
        tf.ReadFile(bytes);

        Assert.AreEqual(tf.Blocks.Count, 2);
    }

    [TestMethod]
    public void DecodeTapFileTest()
    {

        var tp = new TapePlayer();

    }
}