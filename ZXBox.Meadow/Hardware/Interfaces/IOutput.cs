using System;

namespace ZXBox.Hardware.Interfaces
{
    public interface IOutput
    {
        void Output(ushort port, byte byteValue, int tState);
    }
}
