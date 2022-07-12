using System.Collections.Generic;
using System.IO;

namespace ZXBox.Core.Tape
{
    public class TapFormat
    {
        public void ReadFile(byte[] data)
        {
            using MemoryStream ms = new MemoryStream(data);
            using BinaryReader reader = new BinaryReader(ms);
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var lenght = reader.ReadUInt16();
                Blocks.Add(new TapBlock() { Data = reader.ReadBytes(lenght) });
            }
        }

        public List<TapBlock> Blocks = new();
    }

    public class TapBlock
    {
        public byte[] Data { get; set; }

    }

}
