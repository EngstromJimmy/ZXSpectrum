using System.Diagnostics;

namespace ZXBox.Snapshot
{
    public class SNAFileFormat : ISnapshot
    {
        #region ISnapshot Members

        public void LoadSnapshot(byte[] snapshotbytes, Zilog.Z80 cpu)
        {
            cpu.Reset();

            //cpu.I = snapshotbytes[0];
            //cpu.HLPrim = (ushort)(snapshotbytes[1] + 256 * snapshotbytes[2]);
            //cpu.DEPrim = (ushort)(snapshotbytes[3] + 256 * snapshotbytes[4]);
            //cpu.BCPrim = (ushort)(snapshotbytes[5] + 256 * snapshotbytes[6]);
            //cpu.AFPrim = (ushort)(snapshotbytes[7] + 256 * snapshotbytes[8]);

            //cpu.HL = (ushort)(snapshotbytes[9] + 256 * snapshotbytes[10]);
            //cpu.DE = (ushort)(snapshotbytes[11] + 256 * snapshotbytes[12]);
            //cpu.BC = (ushort)(snapshotbytes[13] + 256 * snapshotbytes[14]);
            //cpu.IY = (ushort)(snapshotbytes[15] + 256 * snapshotbytes[16]);
            //cpu.IX = (ushort)(snapshotbytes[17] + 256 * snapshotbytes[18]);
            //cpu.R = snapshotbytes[20];
            //cpu.AF = (ushort)(snapshotbytes[21] + 256 * snapshotbytes[22]);
            //cpu.SP = (ushort)(snapshotbytes[23] + 256 * snapshotbytes[24]);

            cpu.I = snapshotbytes[0];
            cpu.HLPrim = snapshotbytes[1] | (snapshotbytes[2] << 8);
            cpu.DEPrim = snapshotbytes[3] | (snapshotbytes[4] << 8);
            cpu.BCPrim = snapshotbytes[5] | (snapshotbytes[6] << 8);
            cpu.AFPrim = snapshotbytes[7] | (snapshotbytes[8] << 8);
            cpu.HL = snapshotbytes[9] | (snapshotbytes[10] << 8);
            cpu.DE = snapshotbytes[11] | (snapshotbytes[12] << 8);
            cpu.BC = snapshotbytes[13] | (snapshotbytes[14] << 8);
            cpu.IY = snapshotbytes[15] | (snapshotbytes[16] << 8);
            cpu.IX = snapshotbytes[17] | (snapshotbytes[18] << 8);
            cpu.IFF = cpu.IFF2 = ((snapshotbytes[19] & 0x04) == 0x04);
            cpu.R = snapshotbytes[20];
            cpu.AF = snapshotbytes[21] | (snapshotbytes[22] << 8);
            cpu.SP = snapshotbytes[23] | (snapshotbytes[24] << 8);
            cpu.IM = (byte)(snapshotbytes[25] & 0x03);
            if (cpu.IM > 2)
            {
                cpu.IM = 2;
            }

            cpu.Out(254, snapshotbytes[26], 0); //Border Color
            //Memory
            MemoryHandler.LoadBytesintoMemory(snapshotbytes, 27, 0x4000, cpu);

            int pc = cpu.ReadWordFromMemory(cpu.SP);
            Debug.WriteLine("Load PC:" + pc);
            //cpu.SP++;
            //cpu.SP++;
            cpu.PC = pc;
            cpu.RET(true, 0, 0);
        }

        public byte[] SaveSnapshot(Zilog.Z80 cpu)
        {
            byte[] snapshotData = new byte[49179];

            ushort tsp = (ushort)(cpu.SP - 2);
            snapshotData[0] = (byte)cpu.I;
            snapshotData[1] = (byte)(cpu.HLPrim & 0xFF);
            snapshotData[2] = (byte)(cpu.HLPrim >> 8);
            snapshotData[3] = (byte)(cpu.DEPrim & 0xFF);
            snapshotData[4] = (byte)(cpu.DEPrim >> 8);
            snapshotData[5] = (byte)(cpu.BCPrim & 0xFF);
            snapshotData[6] = (byte)(cpu.BCPrim >> 8);
            snapshotData[7] = (byte)(cpu.AFPrim & 0xFF);
            snapshotData[8] = (byte)(cpu.AFPrim >> 8);

            snapshotData[9] = (byte)(cpu.HL & 0xFF);
            snapshotData[10] = (byte)(cpu.HL >> 8);
            snapshotData[11] = (byte)(cpu.DE & 0xFF);
            snapshotData[12] = (byte)(cpu.DE >> 8);
            snapshotData[13] = (byte)(cpu.BC & 0xFF);
            snapshotData[14] = (byte)(cpu.BC >> 8);

            snapshotData[15] = (byte)(cpu.IY & 0xFF);
            snapshotData[16] = (byte)(cpu.IY >> 8);
            snapshotData[17] = (byte)(cpu.IX & 0xFF);
            snapshotData[18] = (byte)(cpu.IX >> 8);

            snapshotData[20] = (byte)cpu.R;
            snapshotData[21] = (byte)(cpu.AF & 0xFF);
            snapshotData[22] = (byte)(cpu.AF >> 8);
            snapshotData[23] = (byte)(tsp & 0xFF);
            snapshotData[24] = (byte)(tsp >> 8);

            snapshotData[25] = (byte)(cpu.IM & 0x03);
            snapshotData[19] = (byte)(cpu.IFF2 ? 0x04 : 0x00);

            snapshotData[26] = (byte)cpu.In(254);

            var t1 = cpu.ReadByteFromMemory(tsp);
            cpu.WriteByteToMemory(tsp++, (byte)(cpu.PC & 0xFF));
            var t2 = cpu.ReadByteFromMemory(tsp);
            cpu.WriteByteToMemory(tsp++, (byte)(cpu.PC >> 8));
            tsp -= 2;

            var mempos = 27;

            for (int a = 0x4001; a < 64 * 1024; a++)
            {
                snapshotData[mempos++] = (byte)cpu.ReadByteFromMemory(a);
            }
            //foreach (byte b in cpu.Memory.Skip(0x4000))
            //{
            //    snapshotData[mempos++] = b;
            //}

            int pc = cpu.ReadWordFromMemory(cpu.SP);
            Debug.WriteLine("save PC:" + pc);

            return snapshotData;

        }
        #endregion
    }
}