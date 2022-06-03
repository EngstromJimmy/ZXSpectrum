using System;

namespace ZXBox.Snapshot
{
    public class MemoryHandler
    {
        public static void LoadBytesintoMemory(Byte[] bytes, int MemoryStartIndex, Zilog.Z80 cpu)
        {
            //foreach (byte b in bytes)
            //    cpu.Memory[MemoryStartIndex++] = b;
            LoadBytesintoMemory(bytes, 0, MemoryStartIndex, cpu);
        }

        public static void LoadBytesintoMemory(Byte[] bytes, int ByteArrayStartIndex, int MemoryStartIndex, Zilog.Z80 cpu)
        {
            for (int a = ByteArrayStartIndex; a < bytes.Length && MemoryStartIndex < bytes.Length; a++)
            {
                cpu.WriteByteToMemory(MemoryStartIndex++, bytes[a]);
            }
        }

#if NETFX_CORE
        public static void LoadBytesintoMemory(Byte[] bytes, int MemoryStartIndex, ZXBox_Core.ZXSpectrum48 cpu)
        {
            //foreach (byte b in bytes)
            //    cpu.Memory[MemoryStartIndex++] = b;
            LoadBytesintoMemory(bytes, 0, MemoryStartIndex, cpu);
        }

        public static void LoadBytesintoMemory(Byte[] bytes, int ByteArrayStartIndex, int MemoryStartIndex, ZXBox_Core.ZXSpectrum48 cpu)
        {
            for (int a = ByteArrayStartIndex; a < bytes.Length && MemoryStartIndex < (64*1024); a++)
                cpu.WriteByteToMemoryOverridden(MemoryStartIndex++, bytes[a]);
        }
    
#endif
    }
}