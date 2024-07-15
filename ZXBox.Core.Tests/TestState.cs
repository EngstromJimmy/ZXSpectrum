using System;

namespace ZXBox.Core.Tests
{
    public class TestState
    {
        public ushort af, bc, de, hl, af_, bc_, de_, hl_, ix, iy, sp, pc;
        public byte i, r;
        public int im;
        public bool iff1, iff2,halted;
        public int end_tstates2;
        public byte[] Memory=new byte[4*16 * 1024];
    }
}