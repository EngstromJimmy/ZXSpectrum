namespace ZXBox.Hardware.Interfaces;

public interface IInput
{
    int Input(int Port, int tact);
    void AddTStates(int tstates)
    { }
}
