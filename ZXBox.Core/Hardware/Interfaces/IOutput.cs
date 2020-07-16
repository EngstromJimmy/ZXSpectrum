using System;

namespace ZXBox.Hardware.Interfaces
{
    public interface IOutput
    {
        void Output(int Port, int ByteValue, int tState);
    }
}
