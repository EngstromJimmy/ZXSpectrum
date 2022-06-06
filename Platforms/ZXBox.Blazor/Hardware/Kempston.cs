using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Toolbelt.Blazor.Gamepad;
using ZXBox.Hardware.Interfaces;

namespace ZXBox.Hardware.Input.Joystick
{
    /// <summary>
    /// The Kempston joystick interface differs from the other common types in that it does not map to the ZX Spectrum keyboard directly. Rather, it maps to a particular hardware port (0x1f) and support must therefore be 'built-in' to the software. Fortunately, the Kempston joystick interface was enormously popular, and support was very easy to provide, making Kempston control a common, almost standard, feature of most games.
    /// Assuming an appropriate interface is attached, reading from port 0x1f returns the current state of the Kempston joystick in the form 000FUDLR, with active bits high.
    /// © www.worldofspectrum.com
    /// </summary>

    public class Kempston:IInput
    {
        
        public IReadOnlyList<Gamepad> Gamepads { get; set; }
        Gamepad Gamepad;
        
        public Kempston()
        {
        
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

                this.Gamepad = Gamepads.FirstOrDefault();
                if (Gamepad == null)
                {
                    return returnvalue;
                }
                //GamePadState currentState = GamePad.GetState(playerIndex);
                //000FUDLR
                if (Gamepad.Buttons.Count() > 0)
                    {
                    if (Gamepad.Buttons[0].Pressed)
                        returnvalue |= 16;
                    if ((Gamepad.Buttons.Count >= 12 && Gamepad.Buttons[12].Pressed) || (Gamepad.Axes.Count > 1 && Gamepad.Axes[1] > 0.5))//up
                        returnvalue |= 8;
                    if ((Gamepad.Buttons.Count >= 13 && Gamepad.Buttons[13].Pressed) || (Gamepad.Axes.Count > 1 && Gamepad.Axes[1] < -0.5))//down
                        returnvalue |= 4;
                    if ((Gamepad.Buttons.Count >= 14 && Gamepad.Buttons[14].Pressed) || (Gamepad.Axes.Count > 0 && Gamepad.Axes[0] < -0.5))//left
                        returnvalue |= 2;
                    if ((Gamepad.Buttons.Count >= 15 && Gamepad.Buttons[15].Pressed) || (Gamepad.Axes.Count > 0 && Gamepad.Axes[0] > 0.5))//right
                        returnvalue |= 1;
                }
            }
            return returnvalue;
        }

        #endregion
    }
}