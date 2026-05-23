using System;

namespace ZXBox.Snapshot
{
    public class MemoryHandler
    {
        public static void LoadBytesintoMemory(Byte[] bytes, ushort MemoryStartIndex, Zilog.Z80 cpu)
        {
            LoadBytesintoMemory(bytes, 0, MemoryStartIndex, cpu);
        }

        public static void LoadBytesintoMemory(Byte[] bytes, int ByteArrayStartIndex, ushort MemoryStartIndex, Zilog.Z80 cpu)
        {
            var memoryAddress = MemoryStartIndex;
            var bytesToCopy = Math.Min(bytes.Length - ByteArrayStartIndex, 0x10000 - memoryAddress);

            for (int a = ByteArrayStartIndex, end = ByteArrayStartIndex + bytesToCopy; a < end; a++)
            {
                cpu.WriteByteToMemory(memoryAddress++, bytes[a]);
            }
        }

#if NETFX_CORE
        public static void LoadBytesintoMemory(Byte[] bytes, ushort MemoryStartIndex, ZXBox_Core.ZXSpectrum48 cpu)
        {
            //foreach (byte b in bytes)
            //    cpu.Memory[MemoryStartIndex++] = b;
            LoadBytesintoMemory(bytes, 0, MemoryStartIndex, cpu);
        }

        public static void LoadBytesintoMemory(Byte[] bytes, int ByteArrayStartIndex, ushort MemoryStartIndex, ZXBox_Core.ZXSpectrum48 cpu)
        {
            var memoryAddress = MemoryStartIndex;
            var bytesToCopy = Math.Min(bytes.Length - ByteArrayStartIndex, 0x10000 - memoryAddress);

            for (int a = ByteArrayStartIndex, end = ByteArrayStartIndex + bytesToCopy; a < end; a++)
                cpu.WriteByteToMemoryOverridden(memoryAddress++, bytes[a]);
        }
    
#endif
    }
}