using System;
using ZXBox.Hardware.Interfaces;

namespace ZXBox.Hardware.Input.Joystick
{
    /// <summary>
    /// The Kempston joystick interface differs from the other common types in that it does not map to the ZX Spectrum keyboard directly. Rather, it maps to a particular hardware port (0x1f) and support must therefore be 'built-in' to the software. Fortunately, the Kempston joystick interface was enormously popular, and support was very easy to provide, making Kempston control a common, almost standard, feature of most games.
    /// Assuming an appropriate interface is attached, reading from port 0x1f returns the current state of the Kempston joystick in the form 000FUDLR, with active bits high.
    /// © www.worldofspectrum.com
    /// </summary>
#if NETFX_CORE
    public class Kempston:ZXBox_Core.IInput
#else
    public class Kempston:IInput
#endif
    {
        GamePadState currentState;
        public Kempston()
        {
        }

        public void UpdateState(GamePadState state)
        {
            currentState = state;
        }

        private bool _Enabled;

        public bool Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; }
        }
	

        #region IInput Members

        public int Input(int Port,int tact)
        {
            int returnvalue = 0xFF;
            if ((Port &0xff) == 0x1f)
            {
                returnvalue = 0x0;
                //if (!Enabled)
                //    return returnvalue;

                //GamePadState currentState = GamePad.GetState(playerIndex);
                    //000FUDLR
                


                    if (currentState.Buttons.A == ButtonState.Pressed)
                        returnvalue |= 16;
                    if (currentState.DPad.Up == ButtonState.Pressed) 
                        returnvalue |= 8;
                    if (currentState.DPad.Down == ButtonState.Pressed)
                        returnvalue |= 4;
                    if (currentState.DPad.Left == ButtonState.Pressed)
                        returnvalue |= 2;
                    if (currentState.DPad.Right == ButtonState.Pressed)
                        returnvalue |= 1;
            }
            return returnvalue;
        }

        #endregion
    }
}