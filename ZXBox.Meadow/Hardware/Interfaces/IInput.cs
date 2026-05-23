using System;

namespace ZXBox.Hardware.Interfaces
{
    public interface IInput
    {
        byte Input(ushort port, int tact);
    }
}
