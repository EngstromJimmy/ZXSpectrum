using System;
using Zilog;

namespace ZXBox.Snapshot;

public class GBFileFormat : ISnapshot
{
    public void LoadSnapshot(byte[] snapshotbytes, Z80 cpu)
    {
        throw new NotImplementedException();
    }

    public byte[] SaveSnapshot(Z80 cpu)
    {
        throw new NotImplementedException();
    }
}
