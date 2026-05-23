namespace ZXBox.Hardware.Interfaces;

public interface IInput
{
    byte Input(ushort Port, int tact);
    void AddTStates(int tstates);
}
