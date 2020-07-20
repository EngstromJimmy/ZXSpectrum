using System;

namespace ZXBox.Core.Tests
{
    public class TestState
    {
        public int af, bc, de, hl, af_, bc_, de_, hl_, ix, iy, sp, pc;
        public int i, r, im;
        public bool iff1, iff2,halted;
        public int end_tstates2;
        public int[] Memory=new int[64 * 1024];
    }
}