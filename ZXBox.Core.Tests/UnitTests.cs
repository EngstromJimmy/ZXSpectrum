using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Diagnostics;

namespace ZXBox.Core.Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void Test00()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"00.in"));
        }
        [TestMethod]
        public void Test01()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"01.in"));
        }
        [TestMethod]
        public void Test02()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"02.in"));
        }
        [TestMethod]
        public void Test03()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"03.in"));
        }
        [TestMethod]
        public void Test04()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"04.in"));
        }
        [TestMethod]
        public void Test05()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"05.in"));
        }
        [TestMethod]
        public void Test06()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"06.in"));
        }
        [TestMethod]
        public void Test07()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"07.in"));
        }
        [TestMethod]
        public void Test08()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"08.in"));
        }
        [TestMethod]
        public void Test09()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"09.in"));
        }
        [TestMethod]
        public void Test0a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"0a.in"));
        }
        [TestMethod]
        public void Test0b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"0b.in"));
        }
        [TestMethod]
        public void Test0c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"0c.in"));
        }
        [TestMethod]
        public void Test0d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"0d.in"));
        }
        [TestMethod]
        public void Test0e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"0e.in"));
        }
        [TestMethod]
        public void Test0f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"0f.in"));
        }
        [TestMethod]
        public void Test10()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"10.in"));
        }
        [TestMethod]
        public void Test11()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"11.in"));
        }
        [TestMethod]
        public void Test12()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"12.in"));
        }
        [TestMethod]
        public void Test13()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"13.in"));
        }
        [TestMethod]
        public void Test14()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"14.in"));
        }
        [TestMethod]
        public void Test15()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"15.in"));
        }
        [TestMethod]
        public void Test16()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"16.in"));
        }
        [TestMethod]
        public void Test17()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"17.in"));
        }
        [TestMethod]
        public void Test18()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"18.in"));
        }
        [TestMethod]
        public void Test19()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"19.in"));
        }
        [TestMethod]
        public void Test1a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"1a.in"));
        }
        [TestMethod]
        public void Test1b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"1b.in"));
        }
        [TestMethod]
        public void Test1c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"1c.in"));
        }
        [TestMethod]
        public void Test1d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"1d.in"));
        }
        [TestMethod]
        public void Test1e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"1e.in"));
        }
        [TestMethod]
        public void Test1f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"1f.in"));
        }
        [TestMethod]
        public void Test20_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"20_1.in"));
        }
        [TestMethod]
        public void Test20_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"20_2.in"));
        }
        [TestMethod]
        public void Test21()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"21.in"));
        }
        [TestMethod]
        public void Test22()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"22.in"));
        }
        [TestMethod]
        public void Test23()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"23.in"));
        }
        [TestMethod]
        public void Test24()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"24.in"));
        }
        [TestMethod]
        public void Test25()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"25.in"));
        }
        [TestMethod]
        public void Test26()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"26.in"));
        }
        [TestMethod]
        public void Test27()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"27.in"));
        }
        [TestMethod]
        public void Test28_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"28_1.in"));
        }
        [TestMethod]
        public void Test28_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"28_2.in"));
        }
        [TestMethod]
        public void Test29()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"29.in"));
        }
        [TestMethod]
        public void Test2a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"2a.in"));
        }
        [TestMethod]
        public void Test2b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"2b.in"));
        }
        [TestMethod]
        public void Test2c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"2c.in"));
        }
        [TestMethod]
        public void Test2d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"2d.in"));
        }
        [TestMethod]
        public void Test2e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"2e.in"));
        }
        [TestMethod]
        public void Test2f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"2f.in"));
        }
        [TestMethod]
        public void Test30_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"30_1.in"));
        }
        [TestMethod]
        public void Test30_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"30_2.in"));
        }
        [TestMethod]
        public void Test31()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"31.in"));
        }
        [TestMethod]
        public void Test32()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"32.in"));
        }
        [TestMethod]
        public void Test33()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"33.in"));
        }
        [TestMethod]
        public void Test34()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"34.in"));
        }
        [TestMethod]
        public void Test35()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"35.in"));
        }
        [TestMethod]
        public void Test36()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"36.in"));
        }
        [TestMethod]
        public void Test37()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"37.in"));
        }
        [TestMethod]
        public void Test37_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"37_1.in"));
        }
        [TestMethod]
        public void Test37_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"37_2.in"));
        }
        [TestMethod]
        public void Test37_3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"37_3.in"));
        }
        [TestMethod]
        public void Test38_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"38_1.in"));
        }
        [TestMethod]
        public void Test38_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"38_2.in"));
        }
        [TestMethod]
        public void Test39()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"39.in"));
        }
        [TestMethod]
        public void Test3a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"3a.in"));
        }
        [TestMethod]
        public void Test3b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"3b.in"));
        }
        [TestMethod]
        public void Test3c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"3c.in"));
        }
        [TestMethod]
        public void Test3d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"3d.in"));
        }
        [TestMethod]
        public void Test3e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"3e.in"));
        }
        [TestMethod]
        public void Test3f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"3f.in"));
        }
        [TestMethod]
        public void Test40()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"40.in"));
        }
        [TestMethod]
        public void Test41()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"41.in"));
        }
        [TestMethod]
        public void Test42()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"42.in"));
        }
        [TestMethod]
        public void Test43()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"43.in"));
        }
        [TestMethod]
        public void Test44()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"44.in"));
        }
        [TestMethod]
        public void Test45()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"45.in"));
        }
        [TestMethod]
        public void Test46()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"46.in"));
        }
        [TestMethod]
        public void Test47()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"47.in"));
        }
        [TestMethod]
        public void Test48()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"48.in"));
        }
        [TestMethod]
        public void Test49()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"49.in"));
        }
        [TestMethod]
        public void Test4a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"4a.in"));
        }
        [TestMethod]
        public void Test4b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"4b.in"));
        }
        [TestMethod]
        public void Test4c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"4c.in"));
        }
        [TestMethod]
        public void Test4d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"4d.in"));
        }
        [TestMethod]
        public void Test4e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"4e.in"));
        }
        [TestMethod]
        public void Test4f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"4f.in"));
        }
        [TestMethod]
        public void Test50()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"50.in"));
        }
        [TestMethod]
        public void Test51()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"51.in"));
        }
        [TestMethod]
        public void Test52()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"52.in"));
        }
        [TestMethod]
        public void Test53()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"53.in"));
        }
        [TestMethod]
        public void Test54()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"54.in"));
        }
        [TestMethod]
        public void Test55()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"55.in"));
        }
        [TestMethod]
        public void Test56()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"56.in"));
        }
        [TestMethod]
        public void Test57()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"57.in"));
        }
        [TestMethod]
        public void Test58()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"58.in"));
        }
        [TestMethod]
        public void Test59()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"59.in"));
        }
        [TestMethod]
        public void Test5a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"5a.in"));
        }
        [TestMethod]
        public void Test5b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"5b.in"));
        }
        [TestMethod]
        public void Test5c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"5c.in"));
        }
        [TestMethod]
        public void Test5d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"5d.in"));
        }
        [TestMethod]
        public void Test5e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"5e.in"));
        }
        [TestMethod]
        public void Test5f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"5f.in"));
        }
        [TestMethod]
        public void Test60()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"60.in"));
        }
        [TestMethod]
        public void Test61()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"61.in"));
        }
        [TestMethod]
        public void Test62()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"62.in"));
        }
        [TestMethod]
        public void Test63()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"63.in"));
        }
        [TestMethod]
        public void Test64()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"64.in"));
        }
        [TestMethod]
        public void Test65()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"65.in"));
        }
        [TestMethod]
        public void Test66()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"66.in"));
        }
        [TestMethod]
        public void Test67()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"67.in"));
        }
        [TestMethod]
        public void Test68()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"68.in"));
        }
        [TestMethod]
        public void Test69()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"69.in"));
        }
        [TestMethod]
        public void Test6a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"6a.in"));
        }
        [TestMethod]
        public void Test6b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"6b.in"));
        }
        [TestMethod]
        public void Test6c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"6c.in"));
        }
        [TestMethod]
        public void Test6d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"6d.in"));
        }
        [TestMethod]
        public void Test6e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"6e.in"));
        }
        [TestMethod]
        public void Test6f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"6f.in"));
        }
        [TestMethod]
        public void Test70()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"70.in"));
        }
        [TestMethod]
        public void Test71()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"71.in"));
        }
        [TestMethod]
        public void Test72()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"72.in"));
        }
        [TestMethod]
        public void Test73()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"73.in"));
        }
        [TestMethod]
        public void Test74()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"74.in"));
        }
        [TestMethod]
        public void Test75()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"75.in"));
        }
        [TestMethod]
        public void Test76()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"76.in"));
        }
        [TestMethod]
        public void Test77()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"77.in"));
        }
        [TestMethod]
        public void Test78()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"78.in"));
        }
        [TestMethod]
        public void Test79()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"79.in"));
        }
        [TestMethod]
        public void Test7a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"7a.in"));
        }
        [TestMethod]
        public void Test7b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"7b.in"));
        }
        [TestMethod]
        public void Test7c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"7c.in"));
        }
        [TestMethod]
        public void Test7d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"7d.in"));
        }
        [TestMethod]
        public void Test7e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"7e.in"));
        }
        [TestMethod]
        public void Test7f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"7f.in"));
        }
        [TestMethod]
        public void Test80()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"80.in"));
        }
        [TestMethod]
        public void Test81()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"81.in"));
        }
        [TestMethod]
        public void Test82()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"82.in"));
        }
        [TestMethod]
        public void Test83()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"83.in"));
        }
        [TestMethod]
        public void Test84()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"84.in"));
        }
        [TestMethod]
        public void Test85()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"85.in"));
        }
        [TestMethod]
        public void Test86()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"86.in"));
        }
        [TestMethod]
        public void Test87()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"87.in"));
        }
        [TestMethod]
        public void Test88()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"88.in"));
        }
        [TestMethod]
        public void Test89()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"89.in"));
        }
        [TestMethod]
        public void Test8a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"8a.in"));
        }
        [TestMethod]
        public void Test8b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"8b.in"));
        }
        [TestMethod]
        public void Test8c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"8c.in"));
        }
        [TestMethod]
        public void Test8d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"8d.in"));
        }
        [TestMethod]
        public void Test8e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"8e.in"));
        }
        [TestMethod]
        public void Test8f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"8f.in"));
        }
        [TestMethod]
        public void Test90()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"90.in"));
        }
        [TestMethod]
        public void Test91()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"91.in"));
        }
        [TestMethod]
        public void Test92()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"92.in"));
        }
        [TestMethod]
        public void Test93()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"93.in"));
        }
        [TestMethod]
        public void Test94()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"94.in"));
        }
        [TestMethod]
        public void Test95()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"95.in"));
        }
        [TestMethod]
        public void Test96()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"96.in"));
        }
        [TestMethod]
        public void Test97()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"97.in"));
        }
        [TestMethod]
        public void Test98()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"98.in"));
        }
        [TestMethod]
        public void Test99()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"99.in"));
        }
        [TestMethod]
        public void Test9a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"9a.in"));
        }
        [TestMethod]
        public void Test9b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"9b.in"));
        }
        [TestMethod]
        public void Test9c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"9c.in"));
        }
        [TestMethod]
        public void Test9d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"9d.in"));
        }
        [TestMethod]
        public void Test9e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"9e.in"));
        }
        [TestMethod]
        public void Test9f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"9f.in"));
        }
        [TestMethod]
        public void Testa0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"a0.in"));
        }
        [TestMethod]
        public void Testa1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"a1.in"));
        }
        [TestMethod]
        public void Testa2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"a2.in"));
        }
        [TestMethod]
        public void Testa3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"a3.in"));
        }
        [TestMethod]
        public void Testa4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"a4.in"));
        }
        [TestMethod]
        public void Testa5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"a5.in"));
        }
        [TestMethod]
        public void Testa6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"a6.in"));
        }
        [TestMethod]
        public void Testa7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"a7.in"));
        }
        [TestMethod]
        public void Testa8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"a8.in"));
        }
        [TestMethod]
        public void Testa9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"a9.in"));
        }
        [TestMethod]
        public void Testaa()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"aa.in"));
        }
        [TestMethod]
        public void Testab()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ab.in"));
        }
        [TestMethod]
        public void Testac()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ac.in"));
        }
        [TestMethod]
        public void Testad()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ad.in"));
        }
        [TestMethod]
        public void Testae()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ae.in"));
        }
        [TestMethod]
        public void Testaf()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"af.in"));
        }
        [TestMethod]
        public void Testb0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"b0.in"));
        }
        [TestMethod]
        public void Testb1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"b1.in"));
        }
        [TestMethod]
        public void Testb2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"b2.in"));
        }
        [TestMethod]
        public void Testb3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"b3.in"));
        }
        [TestMethod]
        public void Testb4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"b4.in"));
        }
        [TestMethod]
        public void Testb5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"b5.in"));
        }
        [TestMethod]
        public void Testb6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"b6.in"));
        }
        [TestMethod]
        public void Testb7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"b7.in"));
        }
        [TestMethod]
        public void Testb8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"b8.in"));
        }
        [TestMethod]
        public void Testb9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"b9.in"));
        }
        [TestMethod]
        public void Testba()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ba.in"));
        }
        [TestMethod]
        public void Testbb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"bb.in"));
        }
        [TestMethod]
        public void Testbc()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"bc.in"));
        }
        [TestMethod]
        public void Testbd()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"bd.in"));
        }
        [TestMethod]
        public void Testbe()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"be.in"));
        }
        [TestMethod]
        public void Testbf()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"bf.in"));
        }
        [TestMethod]
        public void Testc0_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"c0_1.in"));
        }
        [TestMethod]
        public void Testc0_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"c0_2.in"));
        }
        [TestMethod]
        public void Testc1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"c1.in"));
        }
        [TestMethod]
        public void Testc2_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"c2_1.in"));
        }
        [TestMethod]
        public void Testc2_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"c2_2.in"));
        }
        [TestMethod]
        public void Testc3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"c3.in"));
        }
        [TestMethod]
        public void Testc4_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"c4_1.in"));
        }
        [TestMethod]
        public void Testc4_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"c4_2.in"));
        }
        [TestMethod]
        public void Testc5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"c5.in"));
        }
        [TestMethod]
        public void Testc6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"c6.in"));
        }
        [TestMethod]
        public void Testc7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"c7.in"));
        }
        [TestMethod]
        public void Testc8_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"c8_1.in"));
        }
        [TestMethod]
        public void Testc8_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"c8_2.in"));
        }
        [TestMethod]
        public void Testc9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"c9.in"));
        }
        [TestMethod]
        public void Testca_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ca_1.in"));
        }
        [TestMethod]
        public void Testca_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ca_2.in"));
        }
        [TestMethod]
        public void Testcb00()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb00.in"));
        }
        [TestMethod]
        public void Testcb01()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb01.in"));
        }
        [TestMethod]
        public void Testcb02()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb02.in"));
        }
        [TestMethod]
        public void Testcb03()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb03.in"));
        }
        [TestMethod]
        public void Testcb04()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb04.in"));
        }
        [TestMethod]
        public void Testcb05()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb05.in"));
        }
        [TestMethod]
        public void Testcb06()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb06.in"));
        }
        [TestMethod]
        public void Testcb07()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb07.in"));
        }
        [TestMethod]
        public void Testcb08()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb08.in"));
        }
        [TestMethod]
        public void Testcb09()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb09.in"));
        }
        [TestMethod]
        public void Testcb0a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb0a.in"));
        }
        [TestMethod]
        public void Testcb0b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb0b.in"));
        }
        [TestMethod]
        public void Testcb0c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb0c.in"));
        }
        [TestMethod]
        public void Testcb0d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb0d.in"));
        }
        [TestMethod]
        public void Testcb0e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb0e.in"));
        }
        [TestMethod]
        public void Testcb0f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb0f.in"));
        }
        [TestMethod]
        public void Testcb10()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb10.in"));
        }
        [TestMethod]
        public void Testcb11()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb11.in"));
        }
        [TestMethod]
        public void Testcb12()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb12.in"));
        }
        [TestMethod]
        public void Testcb13()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb13.in"));
        }
        [TestMethod]
        public void Testcb14()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb14.in"));
        }
        [TestMethod]
        public void Testcb15()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb15.in"));
        }
        [TestMethod]
        public void Testcb16()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb16.in"));
        }
        [TestMethod]
        public void Testcb17()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb17.in"));
        }
        [TestMethod]
        public void Testcb18()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb18.in"));
        }
        [TestMethod]
        public void Testcb19()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb19.in"));
        }
        [TestMethod]
        public void Testcb1a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb1a.in"));
        }
        [TestMethod]
        public void Testcb1b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb1b.in"));
        }
        [TestMethod]
        public void Testcb1c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb1c.in"));
        }
        [TestMethod]
        public void Testcb1d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb1d.in"));
        }
        [TestMethod]
        public void Testcb1e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb1e.in"));
        }
        [TestMethod]
        public void Testcb1f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb1f.in"));
        }
        [TestMethod]
        public void Testcb20()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb20.in"));
        }
        [TestMethod]
        public void Testcb21()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb21.in"));
        }
        [TestMethod]
        public void Testcb22()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb22.in"));
        }
        [TestMethod]
        public void Testcb23()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb23.in"));
        }
        [TestMethod]
        public void Testcb24()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb24.in"));
        }
        [TestMethod]
        public void Testcb25()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb25.in"));
        }
        [TestMethod]
        public void Testcb26()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb26.in"));
        }
        [TestMethod]
        public void Testcb27()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb27.in"));
        }
        [TestMethod]
        public void Testcb28()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb28.in"));
        }
        [TestMethod]
        public void Testcb29()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb29.in"));
        }
        [TestMethod]
        public void Testcb2a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb2a.in"));
        }
        [TestMethod]
        public void Testcb2b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb2b.in"));
        }
        [TestMethod]
        public void Testcb2c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb2c.in"));
        }
        [TestMethod]
        public void Testcb2d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb2d.in"));
        }
        [TestMethod]
        public void Testcb2e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb2e.in"));
        }
        [TestMethod]
        public void Testcb2f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb2f.in"));
        }
        [TestMethod]
        public void Testcb30()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb30.in"));
        }
        [TestMethod]
        public void Testcb31()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb31.in"));
        }
        [TestMethod]
        public void Testcb32()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb32.in"));
        }
        [TestMethod]
        public void Testcb33()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb33.in"));
        }
        [TestMethod]
        public void Testcb34()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb34.in"));
        }
        [TestMethod]
        public void Testcb35()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb35.in"));
        }
        [TestMethod]
        public void Testcb36()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb36.in"));
        }
        [TestMethod]
        public void Testcb37()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb37.in"));
        }
        [TestMethod]
        public void Testcb38()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb38.in"));
        }
        [TestMethod]
        public void Testcb39()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb39.in"));
        }
        [TestMethod]
        public void Testcb3a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb3a.in"));
        }
        [TestMethod]
        public void Testcb3b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb3b.in"));
        }
        [TestMethod]
        public void Testcb3c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb3c.in"));
        }
        [TestMethod]
        public void Testcb3d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb3d.in"));
        }
        [TestMethod]
        public void Testcb3e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb3e.in"));
        }
        [TestMethod]
        public void Testcb3f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb3f.in"));
        }
        [TestMethod]
        public void Testcb40()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb40.in"));
        }
        [TestMethod]
        public void Testcb41()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb41.in"));
        }
        [TestMethod]
        public void Testcb42()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb42.in"));
        }
        [TestMethod]
        public void Testcb43()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb43.in"));
        }
        [TestMethod]
        public void Testcb44()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb44.in"));
        }
        [TestMethod]
        public void Testcb45()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb45.in"));
        }
        [TestMethod]
        public void Testcb46()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb46.in"));
        }
        [TestMethod]
        public void Testcb47()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb47.in"));
        }
        [TestMethod]
        public void Testcb47_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb47_1.in"));
        }
        [TestMethod]
        public void Testcb48()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb48.in"));
        }
        [TestMethod]
        public void Testcb49()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb49.in"));
        }
        [TestMethod]
        public void Testcb4a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb4a.in"));
        }
        [TestMethod]
        public void Testcb4b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb4b.in"));
        }
        [TestMethod]
        public void Testcb4c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb4c.in"));
        }
        [TestMethod]
        public void Testcb4d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb4d.in"));
        }
        [TestMethod]
        public void Testcb4e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb4e.in"));
        }
        [TestMethod]
        public void Testcb4f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb4f.in"));
        }
        [TestMethod]
        public void Testcb4f_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb4f_1.in"));
        }
        [TestMethod]
        public void Testcb50()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb50.in"));
        }
        [TestMethod]
        public void Testcb51()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb51.in"));
        }
        [TestMethod]
        public void Testcb52()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb52.in"));
        }
        [TestMethod]
        public void Testcb53()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb53.in"));
        }
        [TestMethod]
        public void Testcb54()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb54.in"));
        }
        [TestMethod]
        public void Testcb55()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb55.in"));
        }
        [TestMethod]
        public void Testcb56()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb56.in"));
        }
        [TestMethod]
        public void Testcb57()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb57.in"));
        }
        [TestMethod]
        public void Testcb57_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb57_1.in"));
        }
        [TestMethod]
        public void Testcb58()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb58.in"));
        }
        [TestMethod]
        public void Testcb59()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb59.in"));
        }
        [TestMethod]
        public void Testcb5a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb5a.in"));
        }
        [TestMethod]
        public void Testcb5b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb5b.in"));
        }
        [TestMethod]
        public void Testcb5c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb5c.in"));
        }
        [TestMethod]
        public void Testcb5d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb5d.in"));
        }
        [TestMethod]
        public void Testcb5e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb5e.in"));
        }
        [TestMethod]
        public void Testcb5f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb5f.in"));
        }
        [TestMethod]
        public void Testcb5f_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb5f_1.in"));
        }
        [TestMethod]
        public void Testcb60()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb60.in"));
        }
        [TestMethod]
        public void Testcb61()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb61.in"));
        }
        [TestMethod]
        public void Testcb62()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb62.in"));
        }
        [TestMethod]
        public void Testcb63()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb63.in"));
        }
        [TestMethod]
        public void Testcb64()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb64.in"));
        }
        [TestMethod]
        public void Testcb65()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb65.in"));
        }
        [TestMethod]
        public void Testcb66()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb66.in"));
        }
        [TestMethod]
        public void Testcb67()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb67.in"));
        }
        [TestMethod]
        public void Testcb67_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb67_1.in"));
        }
        [TestMethod]
        public void Testcb68()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb68.in"));
        }
        [TestMethod]
        public void Testcb69()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb69.in"));
        }
        [TestMethod]
        public void Testcb6a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb6a.in"));
        }
        [TestMethod]
        public void Testcb6b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb6b.in"));
        }
        [TestMethod]
        public void Testcb6c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb6c.in"));
        }
        [TestMethod]
        public void Testcb6d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb6d.in"));
        }
        [TestMethod]
        public void Testcb6e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb6e.in"));
        }
        [TestMethod]
        public void Testcb6f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb6f.in"));
        }
        [TestMethod]
        public void Testcb6f_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb6f_1.in"));
        }
        [TestMethod]
        public void Testcb70()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb70.in"));
        }
        [TestMethod]
        public void Testcb71()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb71.in"));
        }
        [TestMethod]
        public void Testcb72()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb72.in"));
        }
        [TestMethod]
        public void Testcb73()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb73.in"));
        }
        [TestMethod]
        public void Testcb74()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb74.in"));
        }
        [TestMethod]
        public void Testcb75()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb75.in"));
        }
        [TestMethod]
        public void Testcb76()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb76.in"));
        }
        [TestMethod]
        public void Testcb77()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb77.in"));
        }
        [TestMethod]
        public void Testcb77_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb77_1.in"));
        }
        [TestMethod]
        public void Testcb78()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb78.in"));
        }
        [TestMethod]
        public void Testcb79()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb79.in"));
        }
        [TestMethod]
        public void Testcb7a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb7a.in"));
        }
        [TestMethod]
        public void Testcb7b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb7b.in"));
        }
        [TestMethod]
        public void Testcb7c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb7c.in"));
        }
        [TestMethod]
        public void Testcb7d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb7d.in"));
        }
        [TestMethod]
        public void Testcb7e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb7e.in"));
        }
        [TestMethod]
        public void Testcb7f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb7f.in"));
        }
        [TestMethod]
        public void Testcb7f_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb7f_1.in"));
        }
        [TestMethod]
        public void Testcb80()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb80.in"));
        }
        [TestMethod]
        public void Testcb81()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb81.in"));
        }
        [TestMethod]
        public void Testcb82()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb82.in"));
        }
        [TestMethod]
        public void Testcb83()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb83.in"));
        }
        [TestMethod]
        public void Testcb84()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb84.in"));
        }
        [TestMethod]
        public void Testcb85()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb85.in"));
        }
        [TestMethod]
        public void Testcb86()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb86.in"));
        }
        [TestMethod]
        public void Testcb87()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb87.in"));
        }
        [TestMethod]
        public void Testcb88()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb88.in"));
        }
        [TestMethod]
        public void Testcb89()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb89.in"));
        }
        [TestMethod]
        public void Testcb8a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb8a.in"));
        }
        [TestMethod]
        public void Testcb8b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb8b.in"));
        }
        [TestMethod]
        public void Testcb8c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb8c.in"));
        }
        [TestMethod]
        public void Testcb8d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb8d.in"));
        }
        [TestMethod]
        public void Testcb8e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb8e.in"));
        }
        [TestMethod]
        public void Testcb8f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb8f.in"));
        }
        [TestMethod]
        public void Testcb90()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb90.in"));
        }
        [TestMethod]
        public void Testcb91()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb91.in"));
        }
        [TestMethod]
        public void Testcb92()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb92.in"));
        }
        [TestMethod]
        public void Testcb93()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb93.in"));
        }
        [TestMethod]
        public void Testcb94()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb94.in"));
        }
        [TestMethod]
        public void Testcb95()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb95.in"));
        }
        [TestMethod]
        public void Testcb96()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb96.in"));
        }
        [TestMethod]
        public void Testcb97()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb97.in"));
        }
        [TestMethod]
        public void Testcb98()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb98.in"));
        }
        [TestMethod]
        public void Testcb99()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb99.in"));
        }
        [TestMethod]
        public void Testcb9a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb9a.in"));
        }
        [TestMethod]
        public void Testcb9b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb9b.in"));
        }
        [TestMethod]
        public void Testcb9c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb9c.in"));
        }
        [TestMethod]
        public void Testcb9d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb9d.in"));
        }
        [TestMethod]
        public void Testcb9e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb9e.in"));
        }
        [TestMethod]
        public void Testcb9f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cb9f.in"));
        }
        [TestMethod]
        public void Testcba0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cba0.in"));
        }
        [TestMethod]
        public void Testcba1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cba1.in"));
        }
        [TestMethod]
        public void Testcba2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cba2.in"));
        }
        [TestMethod]
        public void Testcba3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cba3.in"));
        }
        [TestMethod]
        public void Testcba4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cba4.in"));
        }
        [TestMethod]
        public void Testcba5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cba5.in"));
        }
        [TestMethod]
        public void Testcba6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cba6.in"));
        }
        [TestMethod]
        public void Testcba7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cba7.in"));
        }
        [TestMethod]
        public void Testcba8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cba8.in"));
        }
        [TestMethod]
        public void Testcba9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cba9.in"));
        }
        [TestMethod]
        public void Testcbaa()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbaa.in"));
        }
        [TestMethod]
        public void Testcbab()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbab.in"));
        }
        [TestMethod]
        public void Testcbac()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbac.in"));
        }
        [TestMethod]
        public void Testcbad()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbad.in"));
        }
        [TestMethod]
        public void Testcbae()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbae.in"));
        }
        [TestMethod]
        public void Testcbaf()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbaf.in"));
        }
        [TestMethod]
        public void Testcbb0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbb0.in"));
        }
        [TestMethod]
        public void Testcbb1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbb1.in"));
        }
        [TestMethod]
        public void Testcbb2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbb2.in"));
        }
        [TestMethod]
        public void Testcbb3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbb3.in"));
        }
        [TestMethod]
        public void Testcbb4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbb4.in"));
        }
        [TestMethod]
        public void Testcbb5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbb5.in"));
        }
        [TestMethod]
        public void Testcbb6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbb6.in"));
        }
        [TestMethod]
        public void Testcbb7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbb7.in"));
        }
        [TestMethod]
        public void Testcbb8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbb8.in"));
        }
        [TestMethod]
        public void Testcbb9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbb9.in"));
        }
        [TestMethod]
        public void Testcbba()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbba.in"));
        }
        [TestMethod]
        public void Testcbbb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbbb.in"));
        }
        [TestMethod]
        public void Testcbbc()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbbc.in"));
        }
        [TestMethod]
        public void Testcbbd()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbbd.in"));
        }
        [TestMethod]
        public void Testcbbe()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbbe.in"));
        }
        [TestMethod]
        public void Testcbbf()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbbf.in"));
        }
        [TestMethod]
        public void Testcbc0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbc0.in"));
        }
        [TestMethod]
        public void Testcbc1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbc1.in"));
        }
        [TestMethod]
        public void Testcbc2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbc2.in"));
        }
        [TestMethod]
        public void Testcbc3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbc3.in"));
        }
        [TestMethod]
        public void Testcbc4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbc4.in"));
        }
        [TestMethod]
        public void Testcbc5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbc5.in"));
        }
        [TestMethod]
        public void Testcbc6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbc6.in"));
        }
        [TestMethod]
        public void Testcbc7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbc7.in"));
        }
        [TestMethod]
        public void Testcbc8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbc8.in"));
        }
        [TestMethod]
        public void Testcbc9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbc9.in"));
        }
        [TestMethod]
        public void Testcbca()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbca.in"));
        }
        [TestMethod]
        public void Testcbcb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbcb.in"));
        }
        [TestMethod]
        public void Testcbcc()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbcc.in"));
        }
        [TestMethod]
        public void Testcbcd()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbcd.in"));
        }
        [TestMethod]
        public void Testcbce()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbce.in"));
        }
        [TestMethod]
        public void Testcbcf()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbcf.in"));
        }
        [TestMethod]
        public void Testcbd0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbd0.in"));
        }
        [TestMethod]
        public void Testcbd1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbd1.in"));
        }
        [TestMethod]
        public void Testcbd2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbd2.in"));
        }
        [TestMethod]
        public void Testcbd3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbd3.in"));
        }
        [TestMethod]
        public void Testcbd4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbd4.in"));
        }
        [TestMethod]
        public void Testcbd5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbd5.in"));
        }
        [TestMethod]
        public void Testcbd6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbd6.in"));
        }
        [TestMethod]
        public void Testcbd7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbd7.in"));
        }
        [TestMethod]
        public void Testcbd8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbd8.in"));
        }
        [TestMethod]
        public void Testcbd9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbd9.in"));
        }
        [TestMethod]
        public void Testcbda()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbda.in"));
        }
        [TestMethod]
        public void Testcbdb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbdb.in"));
        }
        [TestMethod]
        public void Testcbdc()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbdc.in"));
        }
        [TestMethod]
        public void Testcbdd()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbdd.in"));
        }
        [TestMethod]
        public void Testcbde()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbde.in"));
        }
        [TestMethod]
        public void Testcbdf()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbdf.in"));
        }
        [TestMethod]
        public void Testcbe0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbe0.in"));
        }
        [TestMethod]
        public void Testcbe1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbe1.in"));
        }
        [TestMethod]
        public void Testcbe2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbe2.in"));
        }
        [TestMethod]
        public void Testcbe3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbe3.in"));
        }
        [TestMethod]
        public void Testcbe4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbe4.in"));
        }
        [TestMethod]
        public void Testcbe5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbe5.in"));
        }
        [TestMethod]
        public void Testcbe6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbe6.in"));
        }
        [TestMethod]
        public void Testcbe7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbe7.in"));
        }
        [TestMethod]
        public void Testcbe8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbe8.in"));
        }
        [TestMethod]
        public void Testcbe9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbe9.in"));
        }
        [TestMethod]
        public void Testcbea()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbea.in"));
        }
        [TestMethod]
        public void Testcbeb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbeb.in"));
        }
        [TestMethod]
        public void Testcbec()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbec.in"));
        }
        [TestMethod]
        public void Testcbed()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbed.in"));
        }
        [TestMethod]
        public void Testcbee()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbee.in"));
        }
        [TestMethod]
        public void Testcbef()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbef.in"));
        }
        [TestMethod]
        public void Testcbf0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbf0.in"));
        }
        [TestMethod]
        public void Testcbf1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbf1.in"));
        }
        [TestMethod]
        public void Testcbf2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbf2.in"));
        }
        [TestMethod]
        public void Testcbf3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbf3.in"));
        }
        [TestMethod]
        public void Testcbf4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbf4.in"));
        }
        [TestMethod]
        public void Testcbf5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbf5.in"));
        }
        [TestMethod]
        public void Testcbf6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbf6.in"));
        }
        [TestMethod]
        public void Testcbf7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbf7.in"));
        }
        [TestMethod]
        public void Testcbf8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbf8.in"));
        }
        [TestMethod]
        public void Testcbf9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbf9.in"));
        }
        [TestMethod]
        public void Testcbfa()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbfa.in"));
        }
        [TestMethod]
        public void Testcbfb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbfb.in"));
        }
        [TestMethod]
        public void Testcbfc()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbfc.in"));
        }
        [TestMethod]
        public void Testcbfd()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbfd.in"));
        }
        [TestMethod]
        public void Testcbfe()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbfe.in"));
        }
        [TestMethod]
        public void Testcbff()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cbff.in"));
        }
        [TestMethod]
        public void Testcc_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cc_1.in"));
        }
        [TestMethod]
        public void Testcc_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cc_2.in"));
        }
        [TestMethod]
        public void Testcd()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cd.in"));
        }
        [TestMethod]
        public void Testce()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ce.in"));
        }
        [TestMethod]
        public void Testcf()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"cf.in"));
        }
        [TestMethod]
        public void Testd0_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"d0_1.in"));
        }
        [TestMethod]
        public void Testd0_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"d0_2.in"));
        }
        [TestMethod]
        public void Testd1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"d1.in"));
        }
        [TestMethod]
        public void Testd2_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"d2_1.in"));
        }
        [TestMethod]
        public void Testd2_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"d2_2.in"));
        }
        [TestMethod]
        public void Testd3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"d3.in"));
        }
        [TestMethod]
        public void Testd4_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"d4_1.in"));
        }
        [TestMethod]
        public void Testd4_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"d4_2.in"));
        }
        [TestMethod]
        public void Testd5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"d5.in"));
        }
        [TestMethod]
        public void Testd6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"d6.in"));
        }
        [TestMethod]
        public void Testd7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"d7.in"));
        }
        [TestMethod]
        public void Testd8_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"d8_1.in"));
        }
        [TestMethod]
        public void Testd8_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"d8_2.in"));
        }
        [TestMethod]
        public void Testd9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"d9.in"));
        }
        [TestMethod]
        public void Testda_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"da_1.in"));
        }
        [TestMethod]
        public void Testda_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"da_2.in"));
        }
        [TestMethod]
        public void Testdb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"db.in"));
        }
        [TestMethod]
        public void Testdc_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dc_1.in"));
        }
        [TestMethod]
        public void Testdc_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dc_2.in"));
        }
        [TestMethod]
        public void Testdd09()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd09.in"));
        }
        [TestMethod]
        public void Testdd19()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd19.in"));
        }
        [TestMethod]
        public void Testdd21()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd21.in"));
        }
        [TestMethod]
        public void Testdd22()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd22.in"));
        }
        [TestMethod]
        public void Testdd23()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd23.in"));
        }
        [TestMethod]
        public void Testdd24()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd24.in"));
        }
        [TestMethod]
        public void Testdd25()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd25.in"));
        }
        [TestMethod]
        public void Testdd26()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd26.in"));
        }
        [TestMethod]
        public void Testdd29()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd29.in"));
        }
        [TestMethod]
        public void Testdd2a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd2a.in"));
        }
        [TestMethod]
        public void Testdd2b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd2b.in"));
        }
        [TestMethod]
        public void Testdd2c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd2c.in"));
        }
        [TestMethod]
        public void Testdd2d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd2d.in"));
        }
        [TestMethod]
        public void Testdd2e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd2e.in"));
        }
        [TestMethod]
        public void Testdd34()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd34.in"));
        }
        [TestMethod]
        public void Testdd35()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd35.in"));
        }
        [TestMethod]
        public void Testdd36()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd36.in"));
        }
        [TestMethod]
        public void Testdd39()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd39.in"));
        }
        [TestMethod]
        public void Testdd44()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd44.in"));
        }
        [TestMethod]
        public void Testdd45()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd45.in"));
        }
        [TestMethod]
        public void Testdd46()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd46.in"));
        }
        [TestMethod]
        public void Testdd4c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd4c.in"));
        }
        [TestMethod]
        public void Testdd4d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd4d.in"));
        }
        [TestMethod]
        public void Testdd4e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd4e.in"));
        }
        [TestMethod]
        public void Testdd54()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd54.in"));
        }
        [TestMethod]
        public void Testdd55()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd55.in"));
        }
        [TestMethod]
        public void Testdd56()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd56.in"));
        }
        [TestMethod]
        public void Testdd5c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd5c.in"));
        }
        [TestMethod]
        public void Testdd5d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd5d.in"));
        }
        [TestMethod]
        public void Testdd5e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd5e.in"));
        }
        [TestMethod]
        public void Testdd60()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd60.in"));
        }
        [TestMethod]
        public void Testdd61()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd61.in"));
        }
        [TestMethod]
        public void Testdd62()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd62.in"));
        }
        [TestMethod]
        public void Testdd63()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd63.in"));
        }
        [TestMethod]
        public void Testdd64()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd64.in"));
        }
        [TestMethod]
        public void Testdd65()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd65.in"));
        }
        [TestMethod]
        public void Testdd66()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd66.in"));
        }
        [TestMethod]
        public void Testdd67()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd67.in"));
        }
        [TestMethod]
        public void Testdd68()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd68.in"));
        }
        [TestMethod]
        public void Testdd69()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd69.in"));
        }
        [TestMethod]
        public void Testdd6a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd6a.in"));
        }
        [TestMethod]
        public void Testdd6b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd6b.in"));
        }
        [TestMethod]
        public void Testdd6c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd6c.in"));
        }
        [TestMethod]
        public void Testdd6d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd6d.in"));
        }
        [TestMethod]
        public void Testdd6e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd6e.in"));
        }
        [TestMethod]
        public void Testdd6f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd6f.in"));
        }
        [TestMethod]
        public void Testdd70()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd70.in"));
        }
        [TestMethod]
        public void Testdd71()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd71.in"));
        }
        [TestMethod]
        public void Testdd72()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd72.in"));
        }
        [TestMethod]
        public void Testdd73()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd73.in"));
        }
        [TestMethod]
        public void Testdd74()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd74.in"));
        }
        [TestMethod]
        public void Testdd75()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd75.in"));
        }
        [TestMethod]
        public void Testdd77()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd77.in"));
        }
        [TestMethod]
        public void Testdd7c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd7c.in"));
        }
        [TestMethod]
        public void Testdd7d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd7d.in"));
        }
        [TestMethod]
        public void Testdd7e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd7e.in"));
        }
        [TestMethod]
        public void Testdd84()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd84.in"));
        }
        [TestMethod]
        public void Testdd85()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd85.in"));
        }
        [TestMethod]
        public void Testdd86()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd86.in"));
        }
        [TestMethod]
        public void Testdd8c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd8c.in"));
        }
        [TestMethod]
        public void Testdd8d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd8d.in"));
        }
        [TestMethod]
        public void Testdd8e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd8e.in"));
        }
        [TestMethod]
        public void Testdd94()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd94.in"));
        }
        [TestMethod]
        public void Testdd95()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd95.in"));
        }
        [TestMethod]
        public void Testdd96()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd96.in"));
        }
        [TestMethod]
        public void Testdd9c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd9c.in"));
        }
        [TestMethod]
        public void Testdd9d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd9d.in"));
        }
        [TestMethod]
        public void Testdd9e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dd9e.in"));
        }
        [TestMethod]
        public void Testdda4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dda4.in"));
        }
        [TestMethod]
        public void Testdda5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dda5.in"));
        }
        [TestMethod]
        public void Testdda6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dda6.in"));
        }
        [TestMethod]
        public void Testddac()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddac.in"));
        }
        [TestMethod]
        public void Testddad()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddad.in"));
        }
        [TestMethod]
        public void Testddae()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddae.in"));
        }
        [TestMethod]
        public void Testddb4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddb4.in"));
        }
        [TestMethod]
        public void Testddb5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddb5.in"));
        }
        [TestMethod]
        public void Testddb6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddb6.in"));
        }
        [TestMethod]
        public void Testddbc()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddbc.in"));
        }
        [TestMethod]
        public void Testddbd()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddbd.in"));
        }
        [TestMethod]
        public void Testddbe()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddbe.in"));
        }
        [TestMethod]
        public void Testddcb00()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb00.in"));
        }
        [TestMethod]
        public void Testddcb01()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb01.in"));
        }
        [TestMethod]
        public void Testddcb02()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb02.in"));
        }
        [TestMethod]
        public void Testddcb03()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb03.in"));
        }
        [TestMethod]
        public void Testddcb04()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb04.in"));
        }
        [TestMethod]
        public void Testddcb05()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb05.in"));
        }
        [TestMethod]
        public void Testddcb06()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb06.in"));
        }
        [TestMethod]
        public void Testddcb07()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb07.in"));
        }
        [TestMethod]
        public void Testddcb08()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb08.in"));
        }
        [TestMethod]
        public void Testddcb09()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb09.in"));
        }
        [TestMethod]
        public void Testddcb0a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb0a.in"));
        }
        [TestMethod]
        public void Testddcb0b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb0b.in"));
        }
        [TestMethod]
        public void Testddcb0c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb0c.in"));
        }
        [TestMethod]
        public void Testddcb0d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb0d.in"));
        }
        [TestMethod]
        public void Testddcb0e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb0e.in"));
        }
        [TestMethod]
        public void Testddcb0f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb0f.in"));
        }
        [TestMethod]
        public void Testddcb10()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb10.in"));
        }
        [TestMethod]
        public void Testddcb11()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb11.in"));
        }
        [TestMethod]
        public void Testddcb12()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb12.in"));
        }
        [TestMethod]
        public void Testddcb13()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb13.in"));
        }
        [TestMethod]
        public void Testddcb14()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb14.in"));
        }
        [TestMethod]
        public void Testddcb15()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb15.in"));
        }
        [TestMethod]
        public void Testddcb16()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb16.in"));
        }
        [TestMethod]
        public void Testddcb17()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb17.in"));
        }
        [TestMethod]
        public void Testddcb18()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb18.in"));
        }
        [TestMethod]
        public void Testddcb19()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb19.in"));
        }
        [TestMethod]
        public void Testddcb1a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb1a.in"));
        }
        [TestMethod]
        public void Testddcb1b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb1b.in"));
        }
        [TestMethod]
        public void Testddcb1c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb1c.in"));
        }
        [TestMethod]
        public void Testddcb1d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb1d.in"));
        }
        [TestMethod]
        public void Testddcb1e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb1e.in"));
        }
        [TestMethod]
        public void Testddcb1f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb1f.in"));
        }
        [TestMethod]
        public void Testddcb20()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb20.in"));
        }
        [TestMethod]
        public void Testddcb21()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb21.in"));
        }
        [TestMethod]
        public void Testddcb22()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb22.in"));
        }
        [TestMethod]
        public void Testddcb23()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb23.in"));
        }
        [TestMethod]
        public void Testddcb24()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb24.in"));
        }
        [TestMethod]
        public void Testddcb25()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb25.in"));
        }
        [TestMethod]
        public void Testddcb26()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb26.in"));
        }
        [TestMethod]
        public void Testddcb27()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb27.in"));
        }
        [TestMethod]
        public void Testddcb28()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb28.in"));
        }
        [TestMethod]
        public void Testddcb29()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb29.in"));
        }
        [TestMethod]
        public void Testddcb2a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb2a.in"));
        }
        [TestMethod]
        public void Testddcb2b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb2b.in"));
        }
        [TestMethod]
        public void Testddcb2c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb2c.in"));
        }
        [TestMethod]
        public void Testddcb2d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb2d.in"));
        }
        [TestMethod]
        public void Testddcb2e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb2e.in"));
        }
        [TestMethod]
        public void Testddcb2f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb2f.in"));
        }
        [TestMethod]
        public void Testddcb30()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb30.in"));
        }
        [TestMethod]
        public void Testddcb31()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb31.in"));
        }
        [TestMethod]
        public void Testddcb32()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb32.in"));
        }
        [TestMethod]
        public void Testddcb33()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb33.in"));
        }
        [TestMethod]
        public void Testddcb34()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb34.in"));
        }
        [TestMethod]
        public void Testddcb35()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb35.in"));
        }
        [TestMethod]
        public void Testddcb36()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb36.in"));
        }
        [TestMethod]
        public void Testddcb37()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb37.in"));
        }
        [TestMethod]
        public void Testddcb38()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb38.in"));
        }
        [TestMethod]
        public void Testddcb39()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb39.in"));
        }
        [TestMethod]
        public void Testddcb3a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb3a.in"));
        }
        [TestMethod]
        public void Testddcb3b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb3b.in"));
        }
        [TestMethod]
        public void Testddcb3c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb3c.in"));
        }
        [TestMethod]
        public void Testddcb3d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb3d.in"));
        }
        [TestMethod]
        public void Testddcb3e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb3e.in"));
        }
        [TestMethod]
        public void Testddcb3f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb3f.in"));
        }
        [TestMethod]
        public void Testddcb40()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb40.in"));
        }
        [TestMethod]
        public void Testddcb41()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb41.in"));
        }
        [TestMethod]
        public void Testddcb42()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb42.in"));
        }
        [TestMethod]
        public void Testddcb43()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb43.in"));
        }
        [TestMethod]
        public void Testddcb44()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb44.in"));
        }
        [TestMethod]
        public void Testddcb45()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb45.in"));
        }
        [TestMethod]
        public void Testddcb46()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb46.in"));
        }
        [TestMethod]
        public void Testddcb47()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb47.in"));
        }
        [TestMethod]
        public void Testddcb48()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb48.in"));
        }
        [TestMethod]
        public void Testddcb49()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb49.in"));
        }
        [TestMethod]
        public void Testddcb4a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb4a.in"));
        }
        [TestMethod]
        public void Testddcb4b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb4b.in"));
        }
        [TestMethod]
        public void Testddcb4c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb4c.in"));
        }
        [TestMethod]
        public void Testddcb4d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb4d.in"));
        }
        [TestMethod]
        public void Testddcb4e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb4e.in"));
        }
        [TestMethod]
        public void Testddcb4f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb4f.in"));
        }
        [TestMethod]
        public void Testddcb50()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb50.in"));
        }
        [TestMethod]
        public void Testddcb51()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb51.in"));
        }
        [TestMethod]
        public void Testddcb52()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb52.in"));
        }
        [TestMethod]
        public void Testddcb53()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb53.in"));
        }
        [TestMethod]
        public void Testddcb54()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb54.in"));
        }
        [TestMethod]
        public void Testddcb55()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb55.in"));
        }
        [TestMethod]
        public void Testddcb56()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb56.in"));
        }
        [TestMethod]
        public void Testddcb57()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb57.in"));
        }
        [TestMethod]
        public void Testddcb58()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb58.in"));
        }
        [TestMethod]
        public void Testddcb59()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb59.in"));
        }
        [TestMethod]
        public void Testddcb5a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb5a.in"));
        }
        [TestMethod]
        public void Testddcb5b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb5b.in"));
        }
        [TestMethod]
        public void Testddcb5c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb5c.in"));
        }
        [TestMethod]
        public void Testddcb5d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb5d.in"));
        }
        [TestMethod]
        public void Testddcb5e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb5e.in"));
        }
        [TestMethod]
        public void Testddcb5f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb5f.in"));
        }
        [TestMethod]
        public void Testddcb60()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb60.in"));
        }
        [TestMethod]
        public void Testddcb61()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb61.in"));
        }
        [TestMethod]
        public void Testddcb62()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb62.in"));
        }
        [TestMethod]
        public void Testddcb63()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb63.in"));
        }
        [TestMethod]
        public void Testddcb64()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb64.in"));
        }
        [TestMethod]
        public void Testddcb65()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb65.in"));
        }
        [TestMethod]
        public void Testddcb66()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb66.in"));
        }
        [TestMethod]
        public void Testddcb67()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb67.in"));
        }
        [TestMethod]
        public void Testddcb68()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb68.in"));
        }
        [TestMethod]
        public void Testddcb69()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb69.in"));
        }
        [TestMethod]
        public void Testddcb6a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb6a.in"));
        }
        [TestMethod]
        public void Testddcb6b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb6b.in"));
        }
        [TestMethod]
        public void Testddcb6c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb6c.in"));
        }
        [TestMethod]
        public void Testddcb6d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb6d.in"));
        }
        [TestMethod]
        public void Testddcb6e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb6e.in"));
        }
        [TestMethod]
        public void Testddcb6f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb6f.in"));
        }
        [TestMethod]
        public void Testddcb70()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb70.in"));
        }
        [TestMethod]
        public void Testddcb71()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb71.in"));
        }
        [TestMethod]
        public void Testddcb72()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb72.in"));
        }
        [TestMethod]
        public void Testddcb73()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb73.in"));
        }
        [TestMethod]
        public void Testddcb74()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb74.in"));
        }
        [TestMethod]
        public void Testddcb75()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb75.in"));
        }
        [TestMethod]
        public void Testddcb76()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb76.in"));
        }
        [TestMethod]
        public void Testddcb77()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb77.in"));
        }
        [TestMethod]
        public void Testddcb78()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb78.in"));
        }
        [TestMethod]
        public void Testddcb79()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb79.in"));
        }
        [TestMethod]
        public void Testddcb7a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb7a.in"));
        }
        [TestMethod]
        public void Testddcb7b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb7b.in"));
        }
        [TestMethod]
        public void Testddcb7c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb7c.in"));
        }
        [TestMethod]
        public void Testddcb7d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb7d.in"));
        }
        [TestMethod]
        public void Testddcb7e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb7e.in"));
        }
        [TestMethod]
        public void Testddcb7f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb7f.in"));
        }
        [TestMethod]
        public void Testddcb80()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb80.in"));
        }
        [TestMethod]
        public void Testddcb81()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb81.in"));
        }
        [TestMethod]
        public void Testddcb82()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb82.in"));
        }
        [TestMethod]
        public void Testddcb83()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb83.in"));
        }
        [TestMethod]
        public void Testddcb84()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb84.in"));
        }
        [TestMethod]
        public void Testddcb85()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb85.in"));
        }
        [TestMethod]
        public void Testddcb86()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb86.in"));
        }
        [TestMethod]
        public void Testddcb87()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb87.in"));
        }
        [TestMethod]
        public void Testddcb88()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb88.in"));
        }
        [TestMethod]
        public void Testddcb89()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb89.in"));
        }
        [TestMethod]
        public void Testddcb8a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb8a.in"));
        }
        [TestMethod]
        public void Testddcb8b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb8b.in"));
        }
        [TestMethod]
        public void Testddcb8c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb8c.in"));
        }
        [TestMethod]
        public void Testddcb8d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb8d.in"));
        }
        [TestMethod]
        public void Testddcb8e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb8e.in"));
        }
        [TestMethod]
        public void Testddcb8f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb8f.in"));
        }
        [TestMethod]
        public void Testddcb90()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb90.in"));
        }
        [TestMethod]
        public void Testddcb91()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb91.in"));
        }
        [TestMethod]
        public void Testddcb92()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb92.in"));
        }
        [TestMethod]
        public void Testddcb93()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb93.in"));
        }
        [TestMethod]
        public void Testddcb94()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb94.in"));
        }
        [TestMethod]
        public void Testddcb95()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb95.in"));
        }
        [TestMethod]
        public void Testddcb96()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb96.in"));
        }
        [TestMethod]
        public void Testddcb97()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb97.in"));
        }
        [TestMethod]
        public void Testddcb98()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb98.in"));
        }
        [TestMethod]
        public void Testddcb99()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb99.in"));
        }
        [TestMethod]
        public void Testddcb9a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb9a.in"));
        }
        [TestMethod]
        public void Testddcb9b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb9b.in"));
        }
        [TestMethod]
        public void Testddcb9c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb9c.in"));
        }
        [TestMethod]
        public void Testddcb9d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb9d.in"));
        }
        [TestMethod]
        public void Testddcb9e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb9e.in"));
        }
        [TestMethod]
        public void Testddcb9f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcb9f.in"));
        }
        [TestMethod]
        public void Testddcba0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcba0.in"));
        }
        [TestMethod]
        public void Testddcba1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcba1.in"));
        }
        [TestMethod]
        public void Testddcba2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcba2.in"));
        }
        [TestMethod]
        public void Testddcba3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcba3.in"));
        }
        [TestMethod]
        public void Testddcba4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcba4.in"));
        }
        [TestMethod]
        public void Testddcba5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcba5.in"));
        }
        [TestMethod]
        public void Testddcba6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcba6.in"));
        }
        [TestMethod]
        public void Testddcba7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcba7.in"));
        }
        [TestMethod]
        public void Testddcba8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcba8.in"));
        }
        [TestMethod]
        public void Testddcba9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcba9.in"));
        }
        [TestMethod]
        public void Testddcbaa()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbaa.in"));
        }
        [TestMethod]
        public void Testddcbab()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbab.in"));
        }
        [TestMethod]
        public void Testddcbac()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbac.in"));
        }
        [TestMethod]
        public void Testddcbad()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbad.in"));
        }
        [TestMethod]
        public void Testddcbae()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbae.in"));
        }
        [TestMethod]
        public void Testddcbaf()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbaf.in"));
        }
        [TestMethod]
        public void Testddcbb0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbb0.in"));
        }
        [TestMethod]
        public void Testddcbb1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbb1.in"));
        }
        [TestMethod]
        public void Testddcbb2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbb2.in"));
        }
        [TestMethod]
        public void Testddcbb3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbb3.in"));
        }
        [TestMethod]
        public void Testddcbb4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbb4.in"));
        }
        [TestMethod]
        public void Testddcbb5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbb5.in"));
        }
        [TestMethod]
        public void Testddcbb6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbb6.in"));
        }
        [TestMethod]
        public void Testddcbb7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbb7.in"));
        }
        [TestMethod]
        public void Testddcbb8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbb8.in"));
        }
        [TestMethod]
        public void Testddcbb9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbb9.in"));
        }
        [TestMethod]
        public void Testddcbba()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbba.in"));
        }
        [TestMethod]
        public void Testddcbbb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbbb.in"));
        }
        [TestMethod]
        public void Testddcbbc()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbbc.in"));
        }
        [TestMethod]
        public void Testddcbbd()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbbd.in"));
        }
        [TestMethod]
        public void Testddcbbe()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbbe.in"));
        }
        [TestMethod]
        public void Testddcbbf()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbbf.in"));
        }
        [TestMethod]
        public void Testddcbc0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbc0.in"));
        }
        [TestMethod]
        public void Testddcbc1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbc1.in"));
        }
        [TestMethod]
        public void Testddcbc2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbc2.in"));
        }
        [TestMethod]
        public void Testddcbc3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbc3.in"));
        }
        [TestMethod]
        public void Testddcbc4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbc4.in"));
        }
        [TestMethod]
        public void Testddcbc5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbc5.in"));
        }
        [TestMethod]
        public void Testddcbc6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbc6.in"));
        }
        [TestMethod]
        public void Testddcbc7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbc7.in"));
        }
        [TestMethod]
        public void Testddcbc8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbc8.in"));
        }
        [TestMethod]
        public void Testddcbc9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbc9.in"));
        }
        [TestMethod]
        public void Testddcbca()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbca.in"));
        }
        [TestMethod]
        public void Testddcbcb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbcb.in"));
        }
        [TestMethod]
        public void Testddcbcc()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbcc.in"));
        }
        [TestMethod]
        public void Testddcbcd()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbcd.in"));
        }
        [TestMethod]
        public void Testddcbce()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbce.in"));
        }
        [TestMethod]
        public void Testddcbcf()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbcf.in"));
        }
        [TestMethod]
        public void Testddcbd0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbd0.in"));
        }
        [TestMethod]
        public void Testddcbd1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbd1.in"));
        }
        [TestMethod]
        public void Testddcbd2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbd2.in"));
        }
        [TestMethod]
        public void Testddcbd3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbd3.in"));
        }
        [TestMethod]
        public void Testddcbd4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbd4.in"));
        }
        [TestMethod]
        public void Testddcbd5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbd5.in"));
        }
        [TestMethod]
        public void Testddcbd6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbd6.in"));
        }
        [TestMethod]
        public void Testddcbd7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbd7.in"));
        }
        [TestMethod]
        public void Testddcbd8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbd8.in"));
        }
        [TestMethod]
        public void Testddcbd9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbd9.in"));
        }
        [TestMethod]
        public void Testddcbda()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbda.in"));
        }
        [TestMethod]
        public void Testddcbdb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbdb.in"));
        }
        [TestMethod]
        public void Testddcbdc()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbdc.in"));
        }
        [TestMethod]
        public void Testddcbdd()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbdd.in"));
        }
        [TestMethod]
        public void Testddcbde()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbde.in"));
        }
        [TestMethod]
        public void Testddcbdf()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbdf.in"));
        }
        [TestMethod]
        public void Testddcbe0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbe0.in"));
        }
        [TestMethod]
        public void Testddcbe1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbe1.in"));
        }
        [TestMethod]
        public void Testddcbe2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbe2.in"));
        }
        [TestMethod]
        public void Testddcbe3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbe3.in"));
        }
        [TestMethod]
        public void Testddcbe4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbe4.in"));
        }
        [TestMethod]
        public void Testddcbe5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbe5.in"));
        }
        [TestMethod]
        public void Testddcbe6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbe6.in"));
        }
        [TestMethod]
        public void Testddcbe7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbe7.in"));
        }
        [TestMethod]
        public void Testddcbe8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbe8.in"));
        }
        [TestMethod]
        public void Testddcbe9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbe9.in"));
        }
        [TestMethod]
        public void Testddcbea()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbea.in"));
        }
        [TestMethod]
        public void Testddcbeb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbeb.in"));
        }
        [TestMethod]
        public void Testddcbec()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbec.in"));
        }
        [TestMethod]
        public void Testddcbed()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbed.in"));
        }
        [TestMethod]
        public void Testddcbee()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbee.in"));
        }
        [TestMethod]
        public void Testddcbef()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbef.in"));
        }
        [TestMethod]
        public void Testddcbf0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbf0.in"));
        }
        [TestMethod]
        public void Testddcbf1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbf1.in"));
        }
        [TestMethod]
        public void Testddcbf2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbf2.in"));
        }
        [TestMethod]
        public void Testddcbf3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbf3.in"));
        }
        [TestMethod]
        public void Testddcbf4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbf4.in"));
        }
        [TestMethod]
        public void Testddcbf5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbf5.in"));
        }
        [TestMethod]
        public void Testddcbf6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbf6.in"));
        }
        [TestMethod]
        public void Testddcbf7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbf7.in"));
        }
        [TestMethod]
        public void Testddcbf8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbf8.in"));
        }
        [TestMethod]
        public void Testddcbf9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbf9.in"));
        }
        [TestMethod]
        public void Testddcbfa()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbfa.in"));
        }
        [TestMethod]
        public void Testddcbfb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbfb.in"));
        }
        [TestMethod]
        public void Testddcbfc()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbfc.in"));
        }
        [TestMethod]
        public void Testddcbfd()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbfd.in"));
        }
        [TestMethod]
        public void Testddcbfe()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbfe.in"));
        }
        [TestMethod]
        public void Testddcbff()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddcbff.in"));
        }
        [TestMethod]
        public void Testdde1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dde1.in"));
        }
        [TestMethod]
        public void Testdde3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dde3.in"));
        }
        [TestMethod]
        public void Testdde5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dde5.in"));
        }
        [TestMethod]
        public void Testdde9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"dde9.in"));
        }
        [TestMethod]
        public void Testddf9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ddf9.in"));
        }
        [TestMethod]
        public void Testde()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"de.in"));
        }
        [TestMethod]
        public void Testdf()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"df.in"));
        }
        [TestMethod]
        public void Teste0_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"e0_1.in"));
        }
        [TestMethod]
        public void Teste0_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"e0_2.in"));
        }
        [TestMethod]
        public void Teste1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"e1.in"));
        }
        [TestMethod]
        public void Teste2_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"e2_1.in"));
        }
        [TestMethod]
        public void Teste2_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"e2_2.in"));
        }
        [TestMethod]
        public void Teste3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"e3.in"));
        }
        [TestMethod]
        public void Teste4_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"e4_1.in"));
        }
        [TestMethod]
        public void Teste4_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"e4_2.in"));
        }
        [TestMethod]
        public void Teste5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"e5.in"));
        }
        [TestMethod]
        public void Teste6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"e6.in"));
        }
        [TestMethod]
        public void Teste7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"e7.in"));
        }
        [TestMethod]
        public void Teste8_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"e8_1.in"));
        }
        [TestMethod]
        public void Teste8_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"e8_2.in"));
        }
        [TestMethod]
        public void Teste9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"e9.in"));
        }
        [TestMethod]
        public void Testea_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ea_1.in"));
        }
        [TestMethod]
        public void Testea_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ea_2.in"));
        }
        [TestMethod]
        public void Testeb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"eb.in"));
        }
        [TestMethod]
        public void Testec_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ec_1.in"));
        }
        [TestMethod]
        public void Testec_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ec_2.in"));
        }
        [TestMethod]
        public void Tested40()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed40.in"));
        }
        [TestMethod]
        public void Tested41()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed41.in"));
        }
        [TestMethod]
        public void Tested42()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed42.in"));
        }
        [TestMethod]
        public void Tested43()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed43.in"));
        }
        [TestMethod]
        public void Tested44()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed44.in"));
        }
        [TestMethod]
        public void Tested45()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed45.in"));
        }
        [TestMethod]
        public void Tested46()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed46.in"));
        }
        [TestMethod]
        public void Tested47()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed47.in"));
        }
        [TestMethod]
        public void Tested48()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed48.in"));
        }
        [TestMethod]
        public void Tested49()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed49.in"));
        }
        [TestMethod]
        public void Tested4a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed4a.in"));
        }
        [TestMethod]
        public void Tested4b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed4b.in"));
        }
        [TestMethod]
        public void Tested4c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed4c.in"));
        }
        [TestMethod]
        public void Tested4d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed4d.in"));
        }
        [TestMethod]
        public void Tested4e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed4e.in"));
        }
        [TestMethod]
        public void Tested4f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed4f.in"));
        }
        [TestMethod]
        public void Tested50()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed50.in"));
        }
        [TestMethod]
        public void Tested51()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed51.in"));
        }
        [TestMethod]
        public void Tested52()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed52.in"));
        }
        [TestMethod]
        public void Tested53()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed53.in"));
        }
        [TestMethod]
        public void Tested54()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed54.in"));
        }
        [TestMethod]
        public void Tested55()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed55.in"));
        }
        [TestMethod]
        public void Tested56()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed56.in"));
        }
        [TestMethod]
        public void Tested57()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed57.in"));
        }
        [TestMethod]
        public void Tested58()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed58.in"));
        }
        [TestMethod]
        public void Tested59()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed59.in"));
        }
        [TestMethod]
        public void Tested5a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed5a.in"));
        }
        [TestMethod]
        public void Tested5b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed5b.in"));
        }
        [TestMethod]
        public void Tested5c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed5c.in"));
        }
        [TestMethod]
        public void Tested5d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed5d.in"));
        }
        [TestMethod]
        public void Tested5e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed5e.in"));
        }
        [TestMethod]
        public void Tested5f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed5f.in"));
        }
        [TestMethod]
        public void Tested60()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed60.in"));
        }
        [TestMethod]
        public void Tested61()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed61.in"));
        }
        [TestMethod]
        public void Tested62()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed62.in"));
        }
        [TestMethod]
        public void Tested63()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed63.in"));
        }
        [TestMethod]
        public void Tested64()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed64.in"));
        }
        [TestMethod]
        public void Tested65()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed65.in"));
        }
        [TestMethod]
        public void Tested66()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed66.in"));
        }
        [TestMethod]
        public void Tested67()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed67.in"));
        }
        [TestMethod]
        public void Tested68()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed68.in"));
        }
        [TestMethod]
        public void Tested69()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed69.in"));
        }
        [TestMethod]
        public void Tested6a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed6a.in"));
        }
        [TestMethod]
        public void Tested6b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed6b.in"));
        }
        [TestMethod]
        public void Tested6c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed6c.in"));
        }
        [TestMethod]
        public void Tested6d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed6d.in"));
        }
        [TestMethod]
        public void Tested6e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed6e.in"));
        }
        [TestMethod]
        public void Tested6f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed6f.in"));
        }
        [TestMethod]
        public void Tested70()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed70.in"));
        }
        [TestMethod]
        public void Tested71()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed71.in"));
        }
        [TestMethod]
        public void Tested72()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed72.in"));
        }
        [TestMethod]
        public void Tested73()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed73.in"));
        }
        [TestMethod]
        public void Tested74()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed74.in"));
        }
        [TestMethod]
        public void Tested75()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed75.in"));
        }
        [TestMethod]
        public void Tested76()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed76.in"));
        }
        [TestMethod]
        public void Tested78()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed78.in"));
        }
        [TestMethod]
        public void Tested79()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed79.in"));
        }
        [TestMethod]
        public void Tested7a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed7a.in"));
        }
        [TestMethod]
        public void Tested7b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed7b.in"));
        }
        [TestMethod]
        public void Tested7c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed7c.in"));
        }
        [TestMethod]
        public void Tested7d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed7d.in"));
        }
        [TestMethod]
        public void Tested7e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ed7e.in"));
        }
        [TestMethod]
        public void Testeda0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"eda0.in"));
        }
        [TestMethod]
        public void Testeda1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"eda1.in"));
        }
        [TestMethod]
        public void Testeda2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"eda2.in"));
        }
        [TestMethod]
        public void Testeda3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"eda3.in"));
        }
        [TestMethod]
        public void Testeda8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"eda8.in"));
        }
        [TestMethod]
        public void Testeda9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"eda9.in"));
        }
        [TestMethod]
        public void Testedaa()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"edaa.in"));
        }
        [TestMethod]
        public void Testedab()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"edab.in"));
        }
        [TestMethod]
        public void Testedb0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"edb0.in"));
        }
        [TestMethod]
        public void Testedb1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"edb1.in"));
        }
        [TestMethod]
        public void Testedb2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"edb2.in"));
        }
        [TestMethod]
        public void Testedb3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"edb3.in"));
        }
        [TestMethod]
        public void Testedb8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"edb8.in"));
        }
        [TestMethod]
        public void Testedb9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"edb9.in"));
        }
        [TestMethod]
        public void Testedba()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"edba.in"));
        }
        [TestMethod]
        public void Testedbb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"edbb.in"));
        }
        [TestMethod]
        public void Testee()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ee.in"));
        }
        [TestMethod]
        public void Testef()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ef.in"));
        }
        [TestMethod]
        public void Testf0_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"f0_1.in"));
        }
        [TestMethod]
        public void Testf0_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"f0_2.in"));
        }
        [TestMethod]
        public void Testf1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"f1.in"));
        }
        [TestMethod]
        public void Testf2_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"f2_1.in"));
        }
        [TestMethod]
        public void Testf2_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"f2_2.in"));
        }
        [TestMethod]
        public void Testf3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"f3.in"));
        }
        [TestMethod]
        public void Testf4_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"f4_1.in"));
        }
        [TestMethod]
        public void Testf4_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"f4_2.in"));
        }
        [TestMethod]
        public void Testf5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"f5.in"));
        }
        [TestMethod]
        public void Testf6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"f6.in"));
        }
        [TestMethod]
        public void Testf7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"f7.in"));
        }
        [TestMethod]
        public void Testf8_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"f8_1.in"));
        }
        [TestMethod]
        public void Testf8_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"f8_2.in"));
        }
        [TestMethod]
        public void Testf9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"f9.in"));
        }
        [TestMethod]
        public void Testfa_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fa_1.in"));
        }
        [TestMethod]
        public void Testfa_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fa_2.in"));
        }
        [TestMethod]
        public void Testfb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fb.in"));
        }
        [TestMethod]
        public void Testfc_1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fc_1.in"));
        }
        [TestMethod]
        public void Testfc_2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fc_2.in"));
        }
        [TestMethod]
        public void Testfd09()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd09.in"));
        }
        [TestMethod]
        public void Testfd19()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd19.in"));
        }
        [TestMethod]
        public void Testfd21()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd21.in"));
        }
        [TestMethod]
        public void Testfd22()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd22.in"));
        }
        [TestMethod]
        public void Testfd23()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd23.in"));
        }
        [TestMethod]
        public void Testfd24()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd24.in"));
        }
        [TestMethod]
        public void Testfd25()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd25.in"));
        }
        [TestMethod]
        public void Testfd26()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd26.in"));
        }
        [TestMethod]
        public void Testfd29()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd29.in"));
        }
        [TestMethod]
        public void Testfd2a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd2a.in"));
        }
        [TestMethod]
        public void Testfd2b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd2b.in"));
        }
        [TestMethod]
        public void Testfd2c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd2c.in"));
        }
        [TestMethod]
        public void Testfd2d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd2d.in"));
        }
        [TestMethod]
        public void Testfd2e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd2e.in"));
        }
        [TestMethod]
        public void Testfd34()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd34.in"));
        }
        [TestMethod]
        public void Testfd35()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd35.in"));
        }
        [TestMethod]
        public void Testfd36()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd36.in"));
        }
        [TestMethod]
        public void Testfd39()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd39.in"));
        }
        [TestMethod]
        public void Testfd44()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd44.in"));
        }
        [TestMethod]
        public void Testfd45()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd45.in"));
        }
        [TestMethod]
        public void Testfd46()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd46.in"));
        }
        [TestMethod]
        public void Testfd4c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd4c.in"));
        }
        [TestMethod]
        public void Testfd4d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd4d.in"));
        }
        [TestMethod]
        public void Testfd4e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd4e.in"));
        }
        [TestMethod]
        public void Testfd54()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd54.in"));
        }
        [TestMethod]
        public void Testfd55()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd55.in"));
        }
        [TestMethod]
        public void Testfd56()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd56.in"));
        }
        [TestMethod]
        public void Testfd5c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd5c.in"));
        }
        [TestMethod]
        public void Testfd5d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd5d.in"));
        }
        [TestMethod]
        public void Testfd5e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd5e.in"));
        }
        [TestMethod]
        public void Testfd60()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd60.in"));
        }
        [TestMethod]
        public void Testfd61()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd61.in"));
        }
        [TestMethod]
        public void Testfd62()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd62.in"));
        }
        [TestMethod]
        public void Testfd63()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd63.in"));
        }
        [TestMethod]
        public void Testfd64()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd64.in"));
        }
        [TestMethod]
        public void Testfd65()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd65.in"));
        }
        [TestMethod]
        public void Testfd66()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd66.in"));
        }
        [TestMethod]
        public void Testfd67()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd67.in"));
        }
        [TestMethod]
        public void Testfd68()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd68.in"));
        }
        [TestMethod]
        public void Testfd69()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd69.in"));
        }
        [TestMethod]
        public void Testfd6a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd6a.in"));
        }
        [TestMethod]
        public void Testfd6b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd6b.in"));
        }
        [TestMethod]
        public void Testfd6c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd6c.in"));
        }
        [TestMethod]
        public void Testfd6d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd6d.in"));
        }
        [TestMethod]
        public void Testfd6e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd6e.in"));
        }
        [TestMethod]
        public void Testfd6f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd6f.in"));
        }
        [TestMethod]
        public void Testfd70()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd70.in"));
        }
        [TestMethod]
        public void Testfd71()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd71.in"));
        }
        [TestMethod]
        public void Testfd72()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd72.in"));
        }
        [TestMethod]
        public void Testfd73()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd73.in"));
        }
        [TestMethod]
        public void Testfd74()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd74.in"));
        }
        [TestMethod]
        public void Testfd75()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd75.in"));
        }
        [TestMethod]
        public void Testfd77()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd77.in"));
        }
        [TestMethod]
        public void Testfd7c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd7c.in"));
        }
        [TestMethod]
        public void Testfd7d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd7d.in"));
        }
        [TestMethod]
        public void Testfd7e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd7e.in"));
        }
        [TestMethod]
        public void Testfd84()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd84.in"));
        }
        [TestMethod]
        public void Testfd85()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd85.in"));
        }
        [TestMethod]
        public void Testfd86()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd86.in"));
        }
        [TestMethod]
        public void Testfd8c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd8c.in"));
        }
        [TestMethod]
        public void Testfd8d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd8d.in"));
        }
        [TestMethod]
        public void Testfd8e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd8e.in"));
        }
        [TestMethod]
        public void Testfd94()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd94.in"));
        }
        [TestMethod]
        public void Testfd95()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd95.in"));
        }
        [TestMethod]
        public void Testfd96()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd96.in"));
        }
        [TestMethod]
        public void Testfd9c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd9c.in"));
        }
        [TestMethod]
        public void Testfd9d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd9d.in"));
        }
        [TestMethod]
        public void Testfd9e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fd9e.in"));
        }
        [TestMethod]
        public void Testfda4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fda4.in"));
        }
        [TestMethod]
        public void Testfda5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fda5.in"));
        }
        [TestMethod]
        public void Testfda6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fda6.in"));
        }
        [TestMethod]
        public void Testfdac()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdac.in"));
        }
        [TestMethod]
        public void Testfdad()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdad.in"));
        }
        [TestMethod]
        public void Testfdae()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdae.in"));
        }
        [TestMethod]
        public void Testfdb4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdb4.in"));
        }
        [TestMethod]
        public void Testfdb5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdb5.in"));
        }
        [TestMethod]
        public void Testfdb6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdb6.in"));
        }
        [TestMethod]
        public void Testfdbc()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdbc.in"));
        }
        [TestMethod]
        public void Testfdbd()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdbd.in"));
        }
        [TestMethod]
        public void Testfdbe()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdbe.in"));
        }
        [TestMethod]
        public void Testfdcb00()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb00.in"));
        }
        [TestMethod]
        public void Testfdcb01()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb01.in"));
        }
        [TestMethod]
        public void Testfdcb02()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb02.in"));
        }
        [TestMethod]
        public void Testfdcb03()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb03.in"));
        }
        [TestMethod]
        public void Testfdcb04()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb04.in"));
        }
        [TestMethod]
        public void Testfdcb05()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb05.in"));
        }
        [TestMethod]
        public void Testfdcb06()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb06.in"));
        }
        [TestMethod]
        public void Testfdcb07()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb07.in"));
        }
        [TestMethod]
        public void Testfdcb08()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb08.in"));
        }
        [TestMethod]
        public void Testfdcb09()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb09.in"));
        }
        [TestMethod]
        public void Testfdcb0a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb0a.in"));
        }
        [TestMethod]
        public void Testfdcb0b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb0b.in"));
        }
        [TestMethod]
        public void Testfdcb0c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb0c.in"));
        }
        [TestMethod]
        public void Testfdcb0d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb0d.in"));
        }
        [TestMethod]
        public void Testfdcb0e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb0e.in"));
        }
        [TestMethod]
        public void Testfdcb0f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb0f.in"));
        }
        [TestMethod]
        public void Testfdcb10()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb10.in"));
        }
        [TestMethod]
        public void Testfdcb11()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb11.in"));
        }
        [TestMethod]
        public void Testfdcb12()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb12.in"));
        }
        [TestMethod]
        public void Testfdcb13()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb13.in"));
        }
        [TestMethod]
        public void Testfdcb14()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb14.in"));
        }
        [TestMethod]
        public void Testfdcb15()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb15.in"));
        }
        [TestMethod]
        public void Testfdcb16()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb16.in"));
        }
        [TestMethod]
        public void Testfdcb17()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb17.in"));
        }
        [TestMethod]
        public void Testfdcb18()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb18.in"));
        }
        [TestMethod]
        public void Testfdcb19()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb19.in"));
        }
        [TestMethod]
        public void Testfdcb1a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb1a.in"));
        }
        [TestMethod]
        public void Testfdcb1b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb1b.in"));
        }
        [TestMethod]
        public void Testfdcb1c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb1c.in"));
        }
        [TestMethod]
        public void Testfdcb1d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb1d.in"));
        }
        [TestMethod]
        public void Testfdcb1e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb1e.in"));
        }
        [TestMethod]
        public void Testfdcb1f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb1f.in"));
        }
        [TestMethod]
        public void Testfdcb20()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb20.in"));
        }
        [TestMethod]
        public void Testfdcb21()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb21.in"));
        }
        [TestMethod]
        public void Testfdcb22()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb22.in"));
        }
        [TestMethod]
        public void Testfdcb23()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb23.in"));
        }
        [TestMethod]
        public void Testfdcb24()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb24.in"));
        }
        [TestMethod]
        public void Testfdcb25()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb25.in"));
        }
        [TestMethod]
        public void Testfdcb26()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb26.in"));
        }
        [TestMethod]
        public void Testfdcb27()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb27.in"));
        }
        [TestMethod]
        public void Testfdcb28()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb28.in"));
        }
        [TestMethod]
        public void Testfdcb29()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb29.in"));
        }
        [TestMethod]
        public void Testfdcb2a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb2a.in"));
        }
        [TestMethod]
        public void Testfdcb2b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb2b.in"));
        }
        [TestMethod]
        public void Testfdcb2c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb2c.in"));
        }
        [TestMethod]
        public void Testfdcb2d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb2d.in"));
        }
        [TestMethod]
        public void Testfdcb2e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb2e.in"));
        }
        [TestMethod]
        public void Testfdcb2f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb2f.in"));
        }
        [TestMethod]
        public void Testfdcb30()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb30.in"));
        }
        [TestMethod]
        public void Testfdcb31()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb31.in"));
        }
        [TestMethod]
        public void Testfdcb32()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb32.in"));
        }
        [TestMethod]
        public void Testfdcb33()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb33.in"));
        }
        [TestMethod]
        public void Testfdcb34()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb34.in"));
        }
        [TestMethod]
        public void Testfdcb35()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb35.in"));
        }
        [TestMethod]
        public void Testfdcb36()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb36.in"));
        }
        [TestMethod]
        public void Testfdcb37()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb37.in"));
        }
        [TestMethod]
        public void Testfdcb38()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb38.in"));
        }
        [TestMethod]
        public void Testfdcb39()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb39.in"));
        }
        [TestMethod]
        public void Testfdcb3a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb3a.in"));
        }
        [TestMethod]
        public void Testfdcb3b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb3b.in"));
        }
        [TestMethod]
        public void Testfdcb3c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb3c.in"));
        }
        [TestMethod]
        public void Testfdcb3d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb3d.in"));
        }
        [TestMethod]
        public void Testfdcb3e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb3e.in"));
        }
        [TestMethod]
        public void Testfdcb3f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb3f.in"));
        }
        [TestMethod]
        public void Testfdcb40()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb40.in"));
        }
        [TestMethod]
        public void Testfdcb41()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb41.in"));
        }
        [TestMethod]
        public void Testfdcb42()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb42.in"));
        }
        [TestMethod]
        public void Testfdcb43()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb43.in"));
        }
        [TestMethod]
        public void Testfdcb44()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb44.in"));
        }
        [TestMethod]
        public void Testfdcb45()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb45.in"));
        }
        [TestMethod]
        public void Testfdcb46()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb46.in"));
        }
        [TestMethod]
        public void Testfdcb47()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb47.in"));
        }
        [TestMethod]
        public void Testfdcb48()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb48.in"));
        }
        [TestMethod]
        public void Testfdcb49()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb49.in"));
        }
        [TestMethod]
        public void Testfdcb4a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb4a.in"));
        }
        [TestMethod]
        public void Testfdcb4b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb4b.in"));
        }
        [TestMethod]
        public void Testfdcb4c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb4c.in"));
        }
        [TestMethod]
        public void Testfdcb4d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb4d.in"));
        }
        [TestMethod]
        public void Testfdcb4e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb4e.in"));
        }
        [TestMethod]
        public void Testfdcb4f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb4f.in"));
        }
        [TestMethod]
        public void Testfdcb50()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb50.in"));
        }
        [TestMethod]
        public void Testfdcb51()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb51.in"));
        }
        [TestMethod]
        public void Testfdcb52()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb52.in"));
        }
        [TestMethod]
        public void Testfdcb53()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb53.in"));
        }
        [TestMethod]
        public void Testfdcb54()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb54.in"));
        }
        [TestMethod]
        public void Testfdcb55()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb55.in"));
        }
        [TestMethod]
        public void Testfdcb56()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb56.in"));
        }
        [TestMethod]
        public void Testfdcb57()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb57.in"));
        }
        [TestMethod]
        public void Testfdcb58()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb58.in"));
        }
        [TestMethod]
        public void Testfdcb59()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb59.in"));
        }
        [TestMethod]
        public void Testfdcb5a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb5a.in"));
        }
        [TestMethod]
        public void Testfdcb5b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb5b.in"));
        }
        [TestMethod]
        public void Testfdcb5c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb5c.in"));
        }
        [TestMethod]
        public void Testfdcb5d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb5d.in"));
        }
        [TestMethod]
        public void Testfdcb5e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb5e.in"));
        }
        [TestMethod]
        public void Testfdcb5f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb5f.in"));
        }
        [TestMethod]
        public void Testfdcb60()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb60.in"));
        }
        [TestMethod]
        public void Testfdcb61()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb61.in"));
        }
        [TestMethod]
        public void Testfdcb62()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb62.in"));
        }
        [TestMethod]
        public void Testfdcb63()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb63.in"));
        }
        [TestMethod]
        public void Testfdcb64()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb64.in"));
        }
        [TestMethod]
        public void Testfdcb65()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb65.in"));
        }
        [TestMethod]
        public void Testfdcb66()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb66.in"));
        }
        [TestMethod]
        public void Testfdcb67()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb67.in"));
        }
        [TestMethod]
        public void Testfdcb68()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb68.in"));
        }
        [TestMethod]
        public void Testfdcb69()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb69.in"));
        }
        [TestMethod]
        public void Testfdcb6a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb6a.in"));
        }
        [TestMethod]
        public void Testfdcb6b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb6b.in"));
        }
        [TestMethod]
        public void Testfdcb6c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb6c.in"));
        }
        [TestMethod]
        public void Testfdcb6d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb6d.in"));
        }
        [TestMethod]
        public void Testfdcb6e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb6e.in"));
        }
        [TestMethod]
        public void Testfdcb6f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb6f.in"));
        }
        [TestMethod]
        public void Testfdcb70()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb70.in"));
        }
        [TestMethod]
        public void Testfdcb71()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb71.in"));
        }
        [TestMethod]
        public void Testfdcb72()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb72.in"));
        }
        [TestMethod]
        public void Testfdcb73()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb73.in"));
        }
        [TestMethod]
        public void Testfdcb74()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb74.in"));
        }
        [TestMethod]
        public void Testfdcb75()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb75.in"));
        }
        [TestMethod]
        public void Testfdcb76()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb76.in"));
        }
        [TestMethod]
        public void Testfdcb77()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb77.in"));
        }
        [TestMethod]
        public void Testfdcb78()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb78.in"));
        }
        [TestMethod]
        public void Testfdcb79()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb79.in"));
        }
        [TestMethod]
        public void Testfdcb7a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb7a.in"));
        }
        [TestMethod]
        public void Testfdcb7b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb7b.in"));
        }
        [TestMethod]
        public void Testfdcb7c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb7c.in"));
        }
        [TestMethod]
        public void Testfdcb7d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb7d.in"));
        }
        [TestMethod]
        public void Testfdcb7e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb7e.in"));
        }
        [TestMethod]
        public void Testfdcb7f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb7f.in"));
        }
        [TestMethod]
        public void Testfdcb80()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb80.in"));
        }
        [TestMethod]
        public void Testfdcb81()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb81.in"));
        }
        [TestMethod]
        public void Testfdcb82()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb82.in"));
        }
        [TestMethod]
        public void Testfdcb83()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb83.in"));
        }
        [TestMethod]
        public void Testfdcb84()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb84.in"));
        }
        [TestMethod]
        public void Testfdcb85()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb85.in"));
        }
        [TestMethod]
        public void Testfdcb86()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb86.in"));
        }
        [TestMethod]
        public void Testfdcb87()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb87.in"));
        }
        [TestMethod]
        public void Testfdcb88()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb88.in"));
        }
        [TestMethod]
        public void Testfdcb89()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb89.in"));
        }
        [TestMethod]
        public void Testfdcb8a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb8a.in"));
        }
        [TestMethod]
        public void Testfdcb8b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb8b.in"));
        }
        [TestMethod]
        public void Testfdcb8c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb8c.in"));
        }
        [TestMethod]
        public void Testfdcb8d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb8d.in"));
        }
        [TestMethod]
        public void Testfdcb8e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb8e.in"));
        }
        [TestMethod]
        public void Testfdcb8f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb8f.in"));
        }
        [TestMethod]
        public void Testfdcb90()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb90.in"));
        }
        [TestMethod]
        public void Testfdcb91()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb91.in"));
        }
        [TestMethod]
        public void Testfdcb92()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb92.in"));
        }
        [TestMethod]
        public void Testfdcb93()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb93.in"));
        }
        [TestMethod]
        public void Testfdcb94()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb94.in"));
        }
        [TestMethod]
        public void Testfdcb95()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb95.in"));
        }
        [TestMethod]
        public void Testfdcb96()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb96.in"));
        }
        [TestMethod]
        public void Testfdcb97()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb97.in"));
        }
        [TestMethod]
        public void Testfdcb98()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb98.in"));
        }
        [TestMethod]
        public void Testfdcb99()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb99.in"));
        }
        [TestMethod]
        public void Testfdcb9a()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb9a.in"));
        }
        [TestMethod]
        public void Testfdcb9b()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb9b.in"));
        }
        [TestMethod]
        public void Testfdcb9c()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb9c.in"));
        }
        [TestMethod]
        public void Testfdcb9d()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb9d.in"));
        }
        [TestMethod]
        public void Testfdcb9e()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb9e.in"));
        }
        [TestMethod]
        public void Testfdcb9f()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcb9f.in"));
        }
        [TestMethod]
        public void Testfdcba0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcba0.in"));
        }
        [TestMethod]
        public void Testfdcba1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcba1.in"));
        }
        [TestMethod]
        public void Testfdcba2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcba2.in"));
        }
        [TestMethod]
        public void Testfdcba3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcba3.in"));
        }
        [TestMethod]
        public void Testfdcba4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcba4.in"));
        }
        [TestMethod]
        public void Testfdcba5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcba5.in"));
        }
        [TestMethod]
        public void Testfdcba6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcba6.in"));
        }
        [TestMethod]
        public void Testfdcba7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcba7.in"));
        }
        [TestMethod]
        public void Testfdcba8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcba8.in"));
        }
        [TestMethod]
        public void Testfdcba9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcba9.in"));
        }
        [TestMethod]
        public void Testfdcbaa()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbaa.in"));
        }
        [TestMethod]
        public void Testfdcbab()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbab.in"));
        }
        [TestMethod]
        public void Testfdcbac()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbac.in"));
        }
        [TestMethod]
        public void Testfdcbad()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbad.in"));
        }
        [TestMethod]
        public void Testfdcbae()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbae.in"));
        }
        [TestMethod]
        public void Testfdcbaf()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbaf.in"));
        }
        [TestMethod]
        public void Testfdcbb0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbb0.in"));
        }
        [TestMethod]
        public void Testfdcbb1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbb1.in"));
        }
        [TestMethod]
        public void Testfdcbb2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbb2.in"));
        }
        [TestMethod]
        public void Testfdcbb3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbb3.in"));
        }
        [TestMethod]
        public void Testfdcbb4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbb4.in"));
        }
        [TestMethod]
        public void Testfdcbb5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbb5.in"));
        }
        [TestMethod]
        public void Testfdcbb6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbb6.in"));
        }
        [TestMethod]
        public void Testfdcbb7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbb7.in"));
        }
        [TestMethod]
        public void Testfdcbb8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbb8.in"));
        }
        [TestMethod]
        public void Testfdcbb9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbb9.in"));
        }
        [TestMethod]
        public void Testfdcbba()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbba.in"));
        }
        [TestMethod]
        public void Testfdcbbb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbbb.in"));
        }
        [TestMethod]
        public void Testfdcbbc()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbbc.in"));
        }
        [TestMethod]
        public void Testfdcbbd()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbbd.in"));
        }
        [TestMethod]
        public void Testfdcbbe()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbbe.in"));
        }
        [TestMethod]
        public void Testfdcbbf()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbbf.in"));
        }
        [TestMethod]
        public void Testfdcbc0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbc0.in"));
        }
        [TestMethod]
        public void Testfdcbc1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbc1.in"));
        }
        [TestMethod]
        public void Testfdcbc2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbc2.in"));
        }
        [TestMethod]
        public void Testfdcbc3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbc3.in"));
        }
        [TestMethod]
        public void Testfdcbc4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbc4.in"));
        }
        [TestMethod]
        public void Testfdcbc5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbc5.in"));
        }
        [TestMethod]
        public void Testfdcbc6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbc6.in"));
        }
        [TestMethod]
        public void Testfdcbc7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbc7.in"));
        }
        [TestMethod]
        public void Testfdcbc8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbc8.in"));
        }
        [TestMethod]
        public void Testfdcbc9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbc9.in"));
        }
        [TestMethod]
        public void Testfdcbca()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbca.in"));
        }
        [TestMethod]
        public void Testfdcbcb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbcb.in"));
        }
        [TestMethod]
        public void Testfdcbcc()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbcc.in"));
        }
        [TestMethod]
        public void Testfdcbcd()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbcd.in"));
        }
        [TestMethod]
        public void Testfdcbce()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbce.in"));
        }
        [TestMethod]
        public void Testfdcbcf()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbcf.in"));
        }
        [TestMethod]
        public void Testfdcbd0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbd0.in"));
        }
        [TestMethod]
        public void Testfdcbd1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbd1.in"));
        }
        [TestMethod]
        public void Testfdcbd2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbd2.in"));
        }
        [TestMethod]
        public void Testfdcbd3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbd3.in"));
        }
        [TestMethod]
        public void Testfdcbd4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbd4.in"));
        }
        [TestMethod]
        public void Testfdcbd5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbd5.in"));
        }
        [TestMethod]
        public void Testfdcbd6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbd6.in"));
        }
        [TestMethod]
        public void Testfdcbd7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbd7.in"));
        }
        [TestMethod]
        public void Testfdcbd8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbd8.in"));
        }
        [TestMethod]
        public void Testfdcbd9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbd9.in"));
        }
        [TestMethod]
        public void Testfdcbda()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbda.in"));
        }
        [TestMethod]
        public void Testfdcbdb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbdb.in"));
        }
        [TestMethod]
        public void Testfdcbdc()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbdc.in"));
        }
        [TestMethod]
        public void Testfdcbdd()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbdd.in"));
        }
        [TestMethod]
        public void Testfdcbde()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbde.in"));
        }
        [TestMethod]
        public void Testfdcbdf()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbdf.in"));
        }
        [TestMethod]
        public void Testfdcbe0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbe0.in"));
        }
        [TestMethod]
        public void Testfdcbe1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbe1.in"));
        }
        [TestMethod]
        public void Testfdcbe2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbe2.in"));
        }
        [TestMethod]
        public void Testfdcbe3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbe3.in"));
        }
        [TestMethod]
        public void Testfdcbe4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbe4.in"));
        }
        [TestMethod]
        public void Testfdcbe5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbe5.in"));
        }
        [TestMethod]
        public void Testfdcbe6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbe6.in"));
        }
        [TestMethod]
        public void Testfdcbe7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbe7.in"));
        }
        [TestMethod]
        public void Testfdcbe8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbe8.in"));
        }
        [TestMethod]
        public void Testfdcbe9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbe9.in"));
        }
        [TestMethod]
        public void Testfdcbea()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbea.in"));
        }
        [TestMethod]
        public void Testfdcbeb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbeb.in"));
        }
        [TestMethod]
        public void Testfdcbec()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbec.in"));
        }
        [TestMethod]
        public void Testfdcbed()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbed.in"));
        }
        [TestMethod]
        public void Testfdcbee()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbee.in"));
        }
        [TestMethod]
        public void Testfdcbef()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbef.in"));
        }
        [TestMethod]
        public void Testfdcbf0()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbf0.in"));
        }
        [TestMethod]
        public void Testfdcbf1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbf1.in"));
        }
        [TestMethod]
        public void Testfdcbf2()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbf2.in"));
        }
        [TestMethod]
        public void Testfdcbf3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbf3.in"));
        }
        [TestMethod]
        public void Testfdcbf4()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbf4.in"));
        }
        [TestMethod]
        public void Testfdcbf5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbf5.in"));
        }
        [TestMethod]
        public void Testfdcbf6()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbf6.in"));
        }
        [TestMethod]
        public void Testfdcbf7()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbf7.in"));
        }
        [TestMethod]
        public void Testfdcbf8()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbf8.in"));
        }
        [TestMethod]
        public void Testfdcbf9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbf9.in"));
        }
        [TestMethod]
        public void Testfdcbfa()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbfa.in"));
        }
        [TestMethod]
        public void Testfdcbfb()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbfb.in"));
        }
        [TestMethod]
        public void Testfdcbfc()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbfc.in"));
        }
        [TestMethod]
        public void Testfdcbfd()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbfd.in"));
        }
        [TestMethod]
        public void Testfdcbfe()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbfe.in"));
        }
        [TestMethod]
        public void Testfdcbff()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdcbff.in"));
        }
        [TestMethod]
        public void Testfde1()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fde1.in"));
        }
        [TestMethod]
        public void Testfde3()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fde3.in"));
        }
        [TestMethod]
        public void Testfde5()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fde5.in"));
        }
        [TestMethod]
        public void Testfde9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fde9.in"));
        }
        [TestMethod]
        public void Testfdf9()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fdf9.in"));
        }
        [TestMethod]
        public void Testfe()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"fe.in"));
        }
        [TestMethod]
        public void Testff()
        {
            Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@"ff.in"));
        }

    }
}
