namespace ZXBox.Snapshot;

public interface ISnapshot
{
    void LoadSnapshot(byte[] snapshotbytes, Zilog.Z80 cpu);
    byte[] SaveSnapshot(Zilog.Z80 cpu);
}
