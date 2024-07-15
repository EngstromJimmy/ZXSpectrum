using System;
using ZXBox.Hardware.Interfaces;

namespace ZXBox.Monogame.Hardware
{
    public class JoystickFake : ZXBox.Hardware.Interfaces.IInput
    {
        public byte Input(ushort Port, int tact)
        {
            byte returnvalue = 0xFF;
            if ((Port & 0xff) == 0x1f)
            {
                returnvalue = 0x0;
            }
            return returnvalue;
        }

        public void AddTStates(int tstates) { }
    }
}