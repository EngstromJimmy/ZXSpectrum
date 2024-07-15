using System;
using System.IO;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZXBox.Core.Tests
{
    public class CoreTest
    {
        public static void RunTest()
        {
            foreach (string file in Directory.GetFiles(@"C:\Projects\ZXBox\ZXBox.Core.Tests\Testfiles\", "*.in"))
            {
                Debug.WriteLine("[TestMethod]");
                Debug.WriteLine("public void Test" + Path.GetFileNameWithoutExtension(file) + "()");
                Debug.WriteLine("{");
                Debug.WriteLine("Assert.IsTrue(ZXBox.Core.Tests.CoreTest.TestInstruction(@\"" + Path.GetFileName(file) + "\"));");
                Debug.WriteLine("}");
                //TestInstruction(file);
            }
        }

        public static bool TestInstruction(string file)
        {
            file = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}/Testfiles/" + file;
            ZXSpectrum z80 = new ZXSpectrum();
            z80.Reset();

            TestState ts = TestfileHandler.ReadINFile(file);
            z80.AF = ts.af;
            z80.AFPrim = ts.af_;
            z80.BC = ts.bc;
            z80.BCPrim = ts.bc_;
            z80.DE = ts.de;
            z80.DEPrim = ts.de_;
            z80.HL = ts.hl;
            z80.HLPrim = ts.hl_;
            z80.I = ts.i;
            z80.IFF = ts.iff1;
            z80.IFF2 = ts.iff2;
            z80.IM = ts.im;
            z80.IX = ts.ix; 
            z80.IY = ts.iy;

            //z80.Memory = ts.Memory;
            for(int m=0;m< ts.Memory.Length; m++)
            {
                z80.WriteByteToMemory((ushort)m, ts.Memory[m]);
            }
            z80.PC = ts.pc;
            z80.R = ts.r;
            z80.SP = ts.sp;

            z80.DoInstructions(ts.end_tstates2);

            TestState tsout = TestfileHandler.ReadOUTFile(Path.GetDirectoryName(file) + "/" + Path.GetFileNameWithoutExtension(file) + ".out", ts.Memory);

            //Compare
            bool result = CoreTest.CompareFunction(tsout, ExtractState(z80));
            if(!result)
                Debug.WriteLine(Path.GetFileNameWithoutExtension(file) + " failed");
            else
                Debug.WriteLine(Path.GetFileNameWithoutExtension(file) + " succeeded");

            return result;
        }

        public static bool CompareFunction(TestState ts, TestState z80)
        {
            bool comparetrue = true;
            //Assert.AreEqual(ts.af,z80.af,"af");
            //Assert.AreEqual(ts.af_, z80.af_,"af_");

            Assert.AreEqual (ts.bc,z80.bc,"bc");
            Assert.AreEqual (ts.bc_, z80.bc_,"bc_");
                
            Assert.AreEqual (ts.de, z80.de,"de");
            Assert.AreEqual (ts.de_ , z80.de_,"de_");
            Assert.AreEqual (ts.hl , z80.hl,"hl");
            Assert.AreEqual (ts.hl_ , z80.hl_,"hl_");
            Assert.AreEqual (ts.i , z80.i, "i");
            //Assert.AreEqual (ts.iff1 , z80.iff1, "iff1");
            //Assert.AreEqual (ts.iff2 , z80.iff2, "iff2");
            Assert.AreEqual (ts.im , z80.im, "im");
            Assert.AreEqual (ts.ix , z80.ix, "ix");
            Assert.AreEqual( ts.iy, z80.iy, "iy");

            //Not comparing this due to not fully implemented contended memory.
            //Assert.AreEqual(ts.end_tstates2, z80.end_tstates2,"tstates2");

            bool equalMemory = true;
            for (int a=0;a<ts.Memory.Length;a++)
            {
                if (ts.Memory[a] != z80.Memory[a])
                {
                    equalMemory = false;
                    break;
                }
            }
            Assert.IsTrue(equalMemory,"memory");
            //Assert.AreEqual(ts.pc, z80.pc, "pc");
            //Assert.AreEqual(ts.r, z80.r, "r");
            //Assert.AreEqual(ts.sp, z80.sp, "sp");
                

            return comparetrue;
        }


        /*
         public static bool CompareFunction(TestState ts, TestState z80)
        {
            bool comparetrue = true;
            if ((ts.af & 0xD7) != (z80.af & 0xD7))
            {
                comparetrue = false;
                Debug.WriteLine("af");
                Assert.AreEqual((ts.af & 0xD7), (z80.af & 0xD7));
            }
            if (ts.af_ != z80.af_)
            {
                comparetrue = false;
                Assert.AreEqual(ts.af_, z80.af_);
            }
            if (ts.bc != z80.bc)
                comparetrue=false;
            if (ts.bc_ != z80.bc_)
                comparetrue=false;
            if (ts.de != z80.de)
                comparetrue=false;
            if (ts.de_ != z80.de_)
                comparetrue=false;
            if (ts.hl != z80.hl)
                comparetrue=false;
            if (ts.hl_ != z80.hl_)
                comparetrue=false;
            if (ts.i != z80.i)
                comparetrue=false;
            if (ts.iff1 != z80.iff1)
                comparetrue=false;
            if (ts.iff2 != z80.iff2)
                comparetrue=false;
            if (ts.im != z80.im)
                comparetrue=false;
            if (ts.ix != z80.ix)
                comparetrue=false;
            if (ts.iy != z80.iy)
                comparetrue=false;
            
            if(ts.end_tstates2!=z80.end_tstates2)
            {
                comparetrue = false;
                //Debug.WriteLine("Tstates: "+ts.end_tstates2 + "-" + z80.end_tstates2);
            }

            for (int a=0;a<ts.Memory.Length;a++)
            {
                if (ts.Memory[a] != z80.Memory[a])
                {
                    comparetrue = false;
                    break;
                }
            }

            if (ts.pc != z80.pc)
            {
                comparetrue = false;
                Assert.AreEqual(ts.pc, z80.pc,"PC");
            }
                
            if (ts.r != z80.r)
                comparetrue=false;
            if (ts.sp != z80.sp)
                comparetrue = false;

            return comparetrue;
        }
         */
        public static TestState ExtractState(Zilog.Z80 z80)
        {
            TestState ts = new TestState();
            ts.af = z80.AF;
            ts.af_ = z80.AFPrim;
            ts.bc = z80.BC;
            ts.bc_ = z80.BCPrim;
            ts.de = z80.DE;
            ts.de_ = z80.DEPrim;
            ts.hl = z80.HL;
            ts.hl_ = z80.HLPrim;
            ts.i = z80.I;
            ts.iff1 = z80.IFF;
            ts.iff2 = z80.IFF2;
            ts.im = z80.IM;
            ts.ix = z80.IX;
            ts.iy = z80.IY;
            for (int m = 0; m < ts.Memory.Length; m++)
            {
                ts.Memory[m] = z80.ReadByteFromMemory((ushort)m);
            }
            ts.pc = z80.PC;
            ts.r = z80.R;
            ts.sp = z80.SP;
            ts.end_tstates2 = z80.EndTstates2;
            return ts;
        }
    }
}

