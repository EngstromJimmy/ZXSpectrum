using System;
using System.Text;
using Zilog;

namespace ZXBox.Snapshot;

public class GBFileFormat : ISnapshot
{
    public void LoadSnapshot(byte[] snapshotbytes, Z80 cpu)
    {
        var entrypoint = snapshotbytes.AsSpan(0x100, 3).ToArray();
        var title = ASCIIEncoding.ASCII.GetString(snapshotbytes.AsSpan(0x134..0x143));
        var cartridgeType = snapshotbytes.AsSpan(0x147, 1).ToArray();
        var romSize = snapshotbytes.AsSpan(0x148, 1).ToArray();
        var ramSize = snapshotbytes.AsSpan(0x149, 1).ToArray();
        var destination = snapshotbytes.AsSpan(0x14A, 1).ToArray();
        var licensee = snapshotbytes.AsSpan(0x14B, 1).ToArray();
        var version = snapshotbytes.AsSpan(0x14C, 1).ToArray();
        var checksum = snapshotbytes.AsSpan(0x14D, 1).ToArray();
        var globalChecksum = snapshotbytes.AsSpan(0x14E, 2).ToArray();
        var rom = snapshotbytes.AsSpan().ToArray();
        MemoryHandler.LoadBytesintoMemory(rom, 0, 0, cpu);
    }

    public byte[] SaveSnapshot(Z80 cpu)
    {
        throw new NotImplementedException();
    }
}
