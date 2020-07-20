using System;
using System.IO;
namespace ZXBox.Core.Tests
{
    public class TestfileHandler
    {
        public static TestState ReadINFile(string Path)
        {
            TextReader tr = new StreamReader(Path);
            string file = tr.ReadToEnd();
            string[] rows = file.Split('\n');

            
            TestState ts = new TestState();
            
            //&af, &bc,&de, &hl, &af_, &bc_, &de_, &hl_, &ix, &iy, &sp, &pc 
            while (rows[0].IndexOf("  ") > 0)
                rows[0] = rows[0].Replace("  ", " ");
            string[] rowdata = rows[0].Split(' ');
            ts.af = Convert.ToInt32(rowdata[0], 16);
            ts.bc = Convert.ToInt32(rowdata[1], 16);
            ts.de = Convert.ToInt32(rowdata[2], 16);
            ts.hl = Convert.ToInt32(rowdata[3], 16);
            ts.af_ = Convert.ToInt32(rowdata[4], 16);
            ts.bc_ = Convert.ToInt32(rowdata[5], 16);
            ts.de_ = Convert.ToInt32(rowdata[6], 16);
            ts.hl_ = Convert.ToInt32(rowdata[7], 16);
            ts.ix = Convert.ToInt32(rowdata[8], 16);
            ts.iy = Convert.ToInt32(rowdata[9], 16);
            ts.sp = Convert.ToInt32(rowdata[10], 16);
            ts.pc = Convert.ToInt32(rowdata[11], 16);

            //&i, &r, &iff1, &iff2, &im, &z80.halted, &end_tstates2
            //Since C# lacks the fscanf function we need to do this in a ugly way
            //The file is formated very nicely until the last value.
            //To get this to work start with removing all double spaces that should 
            //leave us vis a nicely formated string that is easy to parse.
            //Sorry about this..
            while (rows[1].IndexOf("  ") > 0)
                rows[1]=rows[1].Replace("  ", " ");
            rowdata = rows[1].Split(' ');
            ts.i = Convert.ToInt32(rowdata[0], 16);
            ts.r = Convert.ToInt32(rowdata[1], 16);
            ts.iff1 = rowdata[2]=="0"?false:true;
            ts.iff2 = rowdata[3]=="0"?false:true;
            ts.im = Convert.ToInt32(rowdata[4]);
            ts.halted = rowdata[5]=="0"?false:true;
            ts.end_tstates2 = Convert.ToInt32(rowdata[6]);

            //To get a generic tester I have a copy of the Z80 memory in the teststate
            //Fill the memory with data
            for (int a = 2; a < rows.Length; a++)
            {
                rowdata = rows[a].Split(' ');
                if (rowdata[0].Length > 1)
                {
                    int mempos = Convert.ToInt32(rowdata[0],16);
                    for (int b = 1; b < rowdata.Length; b++)
                    {
                        if (rowdata[b] != "-1")
                            ts.Memory[mempos++] = Convert.ToInt32(rowdata[b], 16);
                    }
                }
            }
            return ts;
        }

        public static TestState ReadOUTFile(string Path,int[] MemoryPreset)
        {
            TextReader tr = new StreamReader(Path);
            string file = tr.ReadToEnd();
            string[] rows = file.Split('\n');
            int LineToStarttRead = 0;
            while (rows[LineToStarttRead].IndexOf("M") > 0 || rows[LineToStarttRead].IndexOf("P") > 0)
                LineToStarttRead++;


            TestState ts = new TestState();

            //&af, &bc,&de, &hl, &af_, &bc_, &de_, &hl_, &ix, &iy, &sp, &pc 
            string[] rowdata = rows[LineToStarttRead].Split(' ');
            ts.af = Convert.ToInt32(rowdata[0], 16);
            ts.bc = Convert.ToInt32(rowdata[1], 16);
            ts.de = Convert.ToInt32(rowdata[2], 16);
            ts.hl = Convert.ToInt32(rowdata[3], 16);
            ts.af_ = Convert.ToInt32(rowdata[4], 16);
            ts.bc_ = Convert.ToInt32(rowdata[5], 16);
            ts.de_ = Convert.ToInt32(rowdata[6], 16);
            ts.hl_ = Convert.ToInt32(rowdata[7], 16);
            ts.ix = Convert.ToInt32(rowdata[8], 16);
            ts.iy = Convert.ToInt32(rowdata[9], 16);
            ts.sp = Convert.ToInt32(rowdata[10], 16);
            ts.pc = Convert.ToInt32(rowdata[11], 16);

            //&i, &r, &iff1, &iff2, &im, &z80.halted, &end_tstates2
            //Since C# lacks the fscanf function we need to do this in a ugly way
            //The file is formated very nicely until the last value.
            //To get this to work start with removing all double spaces that should 
            //leave us vis a nicely formated string that is easy to parse.
            //Sorry about this..
            while (rows[LineToStarttRead + 1].IndexOf("  ") > 0)
                rows[LineToStarttRead + 1] = rows[LineToStarttRead+1].Replace("  ", " ");
            rowdata = rows[LineToStarttRead+1].Split(' ');
            ts.i = Convert.ToInt32(rowdata[0], 16);
            ts.r = Convert.ToInt32(rowdata[1], 16);
            ts.iff1 = rowdata[2] == "0" ? false : true;
            ts.iff2 = rowdata[3] == "0" ? false : true;
            ts.im = Convert.ToInt32(rowdata[4]);
            ts.halted = rowdata[5] == "0" ? false : true;
            ts.end_tstates2 = Convert.ToInt32(rowdata[6], 16); //int?

            //To get a generic tester I have a copy of the Z80 memory in the teststate
            //Fill the memory with data
            ts.Memory = MemoryPreset;
            for (int a = LineToStarttRead + 2; a < rows.Length; a++)
            {
                rowdata = rows[a].Split(' ');
                if (rowdata[0].Length > 1)
                {
                    int mempos = Convert.ToInt32(rowdata[0],16);
                    for (int b = 1; b < rowdata.Length; b++)
                    {
                        if (rowdata[b] != "-1")
                            ts.Memory[mempos++] = Convert.ToInt32(rowdata[b], 16);
                    }
                }
            }
            return ts;
        }
    }
}