using System;
using System.Collections.Generic;
using System.Text;

namespace ZXBox.Snapshot
{
    public interface ISnapshot
    {

#if NETFX_CORE
        void LoadSnapshot(byte[] snapshotbytes, ZXBox_Core.ZXSpectrum48 cpu);
        byte[] SaveSnapshot(ZXBox_Core.ZXSpectrum48 cpu);
#else
        void LoadSnapshot(byte[] snapshotbytes, Zilog.Z80 cpu);
        byte[] SaveSnapshot(Zilog.Z80 cpu);
#endif
    }
}
