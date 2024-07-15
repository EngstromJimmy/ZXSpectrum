using System;
using System.Collections.Generic;

namespace ZXBox.Snapshot;

public class Z80FileFormat : ISnapshot
{
    #region ISnapshot Members
    public void LoadSnapshot(byte[] snapshotbytes, Zilog.Z80 cpu)
    {
        List<MemoryBlock> MemoryBlocks = new List<MemoryBlock>();
        int snappshotposition = 0;
        //Read header bytes
        cpu.A = snapshotbytes[0];
        cpu.F = snapshotbytes[1];
        cpu.C = snapshotbytes[2];
        cpu.B = snapshotbytes[3];
        cpu.L = snapshotbytes[4];
        cpu.H = snapshotbytes[5];
        cpu.PC = (ushort)((snapshotbytes[7] << 8) | snapshotbytes[6]);
        cpu.SP = (ushort)((snapshotbytes[9] << 8) | snapshotbytes[8]);
        cpu.I = snapshotbytes[10];
        cpu.R = snapshotbytes[11];

        int bytetwelve = snapshotbytes[12];
        if (bytetwelve == 255)
        {
            bytetwelve = 1;
        }

        //Set border color
        //cpu.Out(254, ((bytetwelve >> 1) & 0x07), 0);

        //Set it 7 on refresh register
        if ((bytetwelve & 0x01) != 0)
        {
            cpu.R7 = 0x80;
        }

        //Is snapshot comressed
        bool isCompressed = ((bytetwelve & 0x20) != 0);

        cpu.E = snapshotbytes[13];
        cpu.D = snapshotbytes[14];

        //Load Prim registers
        cpu.CPrim = snapshotbytes[15];
        cpu.BPrim = snapshotbytes[16];
        cpu.EPrim = snapshotbytes[17];
        cpu.DPrim = snapshotbytes[18];
        cpu.LPrim = snapshotbytes[19];
        cpu.HPrim = snapshotbytes[20];

        cpu.APrim = snapshotbytes[21];
        cpu.FPrim = snapshotbytes[22];

        cpu.IY = (ushort)(snapshotbytes[23] | (snapshotbytes[24] << 8));
        cpu.IX = (ushort)(snapshotbytes[25] | (snapshotbytes[26] << 8));

        cpu.IFF = (snapshotbytes[27] != 0);
        cpu.IFF2 = (snapshotbytes[28] != 0);

        switch (snapshotbytes[29] & 0x03)
        {
            case 0:
                cpu.IM = 0;
                break;
            case 1:
                cpu.IM = 1;
                break;
            default:
                cpu.IM = 2;
                break;
        }
        /*
          29      1       Bit 0-1: Interrupt mode (0, 1 or 2)
                                Bit 2  : 1=Issue 2 emulation
                                Bit 3  : 1=Double interrupt frequency
                                Bit 4-5: 1=High video synchronisation
                                         3=Low video synchronisation
                                         0,2=Normal
                                Bit 6-7: 0=Cursor/Protek/AGF joystick
                                         1=Kempston joystick
                                         2=Sinclair 2 Left joystick (or user
                                           defined, for version 3 .z80 files)
                                         3=Sinclair 2 Right joystick
            No need to read an support all of these.
         */

        snappshotposition = 30;
        if (cpu.PC == 0)
        {/*
          * Extended format
            Most bytes in the extended section will be discarded since there are no support for them
          */
            int numberofheaderbytes = snapshotbytes[snappshotposition++] | (snapshotbytes[snappshotposition++] << 8);
            cpu.PC = (ushort)(snapshotbytes[32] | (snapshotbytes[33] << 8));

            //The rest of the header information is not relevant for this emulator
            snappshotposition += numberofheaderbytes;

            while (snappshotposition < snapshotbytes.Length)
            { //Load memory blocks
                MemoryBlock mb = new MemoryBlock();
                int datalength = (snapshotbytes[snappshotposition++]) | (snapshotbytes[snappshotposition++] << 8);
                int MemoryBlockNumber = snapshotbytes[snappshotposition++];
                if (datalength == 0xffff)
                {   //Not compressed
                    datalength = 16384;
                    isCompressed = false;
                }
                else
                {
                    isCompressed = true;
                }
                mb = GetMemoryBlock(snapshotbytes, snappshotposition, datalength, isCompressed, MemoryBlockNumber);
                snappshotposition += datalength;
                MemoryBlocks.Add(mb);
            }

        }
        else //After the first 30 bytes a memory dump och the 48k Spectrum follows.
        {
            MemoryBlock mb = GetMemoryBlock(snapshotbytes, 30, snapshotbytes.Length - 30, isCompressed, -1);
            //Memoryblock = -1 since this is no "real" block
            MemoryBlocks.Add(mb);
        }

        //Debug.WriteLineIf(MemoryBlocks.Count <= 3, "48K Z80");
        //Debug.WriteLineIf(MemoryBlocks.Count > 3, "128K Z80");
        //Debug.WriteLineIf((snapshotbytes[29] >> 0x06) == 0, "Cursor");
        //Debug.WriteLineIf((snapshotbytes[29] >> 0x06) == 1, "Kempston");
        //Debug.WriteLineIf((snapshotbytes[29] >> 0x06) == 2, "Sinclair 2 Left joystick");
        //Debug.WriteLineIf((snapshotbytes[29] >> 0x06) == 3, "Sinclair 2 Right joystick");

        //Load Memoryblocks into memory
        foreach (MemoryBlock mb in MemoryBlocks)
        {
            switch (mb.MemoryBlockNumber)
            {
                case -1:    //All Ram
                    MemoryHandler.LoadBytesintoMemory(mb.MemoryData.ToArray(), 16384, cpu);
                    break;
                case 0:     //Rom
                    MemoryHandler.LoadBytesintoMemory(mb.MemoryData.ToArray(), 0, cpu);
                    break;
                case 1:     //Interface 1
                case 2:     //Not used for 48
                case 3:     //Not used for 48
                case 6:     //Not used for 48
                case 7:     //Not used for 48
                case 9:     //Not used for 48
                case 10:    //Not used for 48
                case 11:    //Multiface rom
                    //Currently no support, using a file with these blocks might result in errors
                    break;
                case 4:     //Normal 8000-bfff
                    MemoryHandler.LoadBytesintoMemory(mb.MemoryData.ToArray(), 0x8000, cpu);
                    break;
                case 5:     //Normal c000-ffff
                    MemoryHandler.LoadBytesintoMemory(mb.MemoryData.ToArray(), 0xc000, cpu);
                    break;
                //case 3:
                case 8:     //Normal 4000-7ffff
                    //Loaded from 3
                    MemoryHandler.LoadBytesintoMemory(mb.MemoryData.ToArray(), 0x4000, cpu);
                    break;
            }

        }

    }

    public static MemoryBlock GetMemoryBlock(byte[] SnapshotBytes, int StartPosition, int Length, bool Compressed, int MemoryBlockNumber)
    {
        List<byte> Uncompressedmemory = new List<byte>();
        if (Compressed)
        {//if the Z80 format has compressed memory we need to decompress it
            byte b = 0x00;
            for (int a = StartPosition; a < StartPosition + Length && a < SnapshotBytes.Length;)
            {
                b = SnapshotBytes[a++];
                if (b != 0xED) //Not compressed
                {
                    Uncompressedmemory.Add(b);
                }
                else
                {
                    b = SnapshotBytes[a++];
                    if (b != 0xED)
                    {
                        Uncompressedmemory.Add(0xED);
                        //Not compressed, single ED go back one step
                        a--;
                    }
                    else
                    {
                        int count;
                        count = SnapshotBytes[a++];
                        if (count > 0)
                        {
                            b = SnapshotBytes[a++];
                            while ((count--) != 0)
                            {
                                Uncompressedmemory.Add(b);
                            }
                        }
                    }
                }
            }
        }
        else
        {

            for (int a = StartPosition; a < StartPosition + Length; a++)
                Uncompressedmemory.Add(SnapshotBytes[a]);
        }
        MemoryBlock mb = new MemoryBlock();
        mb.MemoryBlockNumber = MemoryBlockNumber;
        mb.MemoryData = Uncompressedmemory;

        if (MemoryBlockNumber != -1 && Uncompressedmemory.Count > (16 * 1024))
        {
            mb.MemoryData.RemoveRange((16 * 1024), (16 * 1024) - Uncompressedmemory.Count);
        }
        return mb;
    }

    #endregion

    public byte[] SaveSnapshot(Zilog.Z80 cpu)
    {
        throw new NotImplementedException();
    }

    //public byte[] SaveSnapshot(ZXSpectrum48 cpu)
    //{
    //    throw new NotImplementedException();
    //}
}
