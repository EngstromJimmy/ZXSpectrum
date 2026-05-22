using System.IO;

namespace ZXBox.Core.Tape;

public class TapFormat
{
    public TapeImage ReadFile(byte[] data)
    {
        var tapeImage = new TapeImage();

        using MemoryStream ms = new(data);
        using BinaryReader reader = new(ms);

        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            var length = reader.ReadUInt16();
            var blockData = reader.ReadBytes(length);

            tapeImage.Blocks.Add(new TapeDataBlock
            {
                Data = blockData,
                PilotPulseLength = 2168,
                PilotPulseCount = blockData.Length > 0 && blockData[0] < 0x80 ? 8063 : 3223,
                SyncFirstPulseLength = 667,
                SyncSecondPulseLength = 735,
                ZeroBitPulseLength = 855,
                OneBitPulseLength = 1710,
                UsedBitsInLastByte = 8,
                PauseAfterMilliseconds = 1000
            });
        }

        return tapeImage;
    }
}
