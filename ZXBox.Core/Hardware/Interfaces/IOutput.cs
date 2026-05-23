namespace ZXBox.Hardware.Interfaces;

public interface IOutput
{
    void Output(ushort Port, byte ByteValue, int tState);
}
