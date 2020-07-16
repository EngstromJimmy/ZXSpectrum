using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using ZXBox.Hardware.Interfaces;


namespace ZXBox.Hardware.Input
{
#if NETFX_CORE
    public class Keyboard : ZXBox_Core.IInput
#else
    public class Keyboard:IInput
#endif
    {
        private GamePadState State=new GamePadState();
        private KeyboardState ks;
        private KeyboardState virtualks;
        private bool virtalStateAvailible=false;
        private List<Keys> KeyBuffer = new List<Keys>();
        int NoKeyCounter = 0;
        bool SymbolShift = false;
        int sectionnumber=1;
        public JoystickTypeEnum JoystickType{get;set;}

        public void SetGamePadstate(GamePadState state)
        {
            State = state;
        }


        public void SetKeystate(KeyboardState state)
        {
            virtualks = state;
            //If no virtual buttons are pressed, use keyboard
            virtalStateAvailible=(virtualks.GetPressedKeys().Length != 0);
        }

        private bool IsNextKey(Keys key)
        {
            //if (KeyBuffer.Count > 0)
            //{
            //    if (KeyBuffer[0] == Keys.RightAlt) //(Symbol Shift)
            //    {
            //        SymbolShift = true;
            //        KeyBuffer.Remove(KeyBuffer[0]);
            //    }
            //}    
            //if (key == Keys.RightAlt)
            //{
            //    bool returnval = false;
            //    if (SymbolShift == true)
            //        returnval = true;
            //    SymbolShift = false;
            //    return returnval;
            //}

            //if (KeyBuffer.Count > 0)
            //{
            //    
            //    if (KeyBuffer[0] == key)
            //    {
            //        KeyBuffer.Remove(KeyBuffer[0]);
            //        return true;
            //    }

            //    if (KeyBuffer[0] == Keys.None)
            //    {
            //        if (key == Keys.B) //this is the last key to check
            //        {
            //            if (NoKeyCounter++ > 1)
            //            {
            //                KeyBuffer.Remove(KeyBuffer[0]);
            //                NoKeyCounter = 0;
            //            }
            //        }
            //    }
            //}
            return false;
        }

        public Keyboard()
        {
            //Initialize the keyboard
            ks=Microsoft.Xna.Framework.Input.Keyboard.GetState();
        }

        public int Input(int Port,int tstates)
        {

            if ((Port & 0xFF) == 0xFE)
            {
                //Only listen to the keybuffer when on different sections
                DateTime dt = DateTime.Now;
                if (sectionnumber++ == 1)
                {
                    if (virtalStateAvailible)
                    {
                        ks = virtualks;
                    }
                    else
                    {
                        ks = Microsoft.Xna.Framework.Input.Keyboard.GetState();
                    }
                }
                if (sectionnumber > 8)
                {   //Reset sectioncheck
                    sectionnumber = 1;
                    //virtalStateAvailible = false;
                }
                //TimeSpan ts = DateTime.Now - dt;
                //System.Diagnostics.Debug.WriteLine("Getstate:" + ts.Milliseconds);
                bool symbol = false;
                int returnvalue = 0xFF;

                //Special cace for any key
                if (((Port >> 8) & 0xFF) == 0x01 || ((Port >> 8) & 0xFF) == 0x00 || ((Port >> 8) & 0xFF) == 0x02)
                {   //Check for any key including joy, we might need to add more joy buttons later on
                    if (ks.GetPressedKeys().Length > 0 || (State.DPad.Down == ButtonState.Pressed || State.DPad.Up == ButtonState.Pressed || State.DPad.Left == ButtonState.Pressed || State.DPad.Right == ButtonState.Pressed || State.Buttons.A == ButtonState.Pressed))
                    {
                        return returnvalue &= ~1; 
                    }
                
                }

                switch ((Port >> 8) & 0x0F)
                {
                    case 0x0E: //SHIFT, Z, X, C, V
                        if (ks.IsKeyDown(Keys.RightShift) || ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.Back) || IsNextKey(Keys.Back) || IsNextKey(Keys.LeftShift) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Left == ButtonState.Pressed || State.DPad.Right == ButtonState.Pressed || State.DPad.Up == ButtonState.Pressed || State.DPad.Down == ButtonState.Pressed || State.Buttons.A == ButtonState.Pressed || ks.IsKeyDown(Keys.LeftControl)  || ks.IsKeyDown(Keys.RightControl))))
                            returnvalue &= ~1;
                        if (ks.IsKeyDown(Keys.Z) || IsNextKey(Keys.Z))
                            returnvalue &= ~2;
                        if (ks.IsKeyDown(Keys.X) || IsNextKey(Keys.X))
                            returnvalue &= ~4;
                        if (ks.IsKeyDown(Keys.C) || IsNextKey(Keys.Z))
                            returnvalue &= ~8;
                        if (ks.IsKeyDown(Keys.V) || IsNextKey(Keys.V))
                            returnvalue &= ~16;
                        break;
                    case 0x0D: //A, S, D, F, G
                        if (ks.IsKeyDown(Keys.A) || IsNextKey(Keys.A))
                            returnvalue &= ~1;
                        if (ks.IsKeyDown(Keys.S) || IsNextKey(Keys.S))
                            returnvalue &= ~2;
                        if (ks.IsKeyDown(Keys.D) || IsNextKey(Keys.D))
                            returnvalue &= ~4;
                        if (ks.IsKeyDown(Keys.F) || IsNextKey(Keys.F))
                            returnvalue &= ~8;
                        if (ks.IsKeyDown(Keys.G) || IsNextKey(Keys.G))
                            returnvalue &= ~16;
                        break;
                    case 0x0B: //Q, W, E, R, T
                        if (ks.IsKeyDown(Keys.Q) || IsNextKey(Keys.Q))
                            returnvalue &= ~1;
                        if (ks.IsKeyDown(Keys.W) || IsNextKey(Keys.W))
                            returnvalue &= ~2;
                        if (ks.IsKeyDown(Keys.E) || IsNextKey(Keys.E))
                            returnvalue &= ~4;
                        if (ks.IsKeyDown(Keys.R) || IsNextKey(Keys.R))
                            returnvalue &= ~8;
                        if (ks.IsKeyDown(Keys.T) || IsNextKey(Keys.T))
                            returnvalue &= ~16;
                        break;
                    case 0x07: //1, 2, 3, 4, 5n
                        if (ks.IsKeyDown(Keys.D1) || IsNextKey(Keys.D1) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.DPad.Left == ButtonState.Pressed || ks.IsKeyDown(Keys.Left))))
                            returnvalue &= ~1;
                        if (ks.IsKeyDown(Keys.D2) || IsNextKey(Keys.D2) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.DPad.Right == ButtonState.Pressed || ks.IsKeyDown(Keys.Right))))
                            returnvalue &= ~2;
                        if (ks.IsKeyDown(Keys.D3) || IsNextKey(Keys.D3) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.DPad.Down == ButtonState.Pressed || ks.IsKeyDown(Keys.Down))))
                            returnvalue &= ~4;
                        if (ks.IsKeyDown(Keys.D4) || IsNextKey(Keys.D4) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.DPad.Up == ButtonState.Pressed || ks.IsKeyDown(Keys.Up))))
                            returnvalue &= ~8;
                        if (ks.IsKeyDown(Keys.D5) || IsNextKey(Keys.D5) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.Buttons.A == ButtonState.Pressed || ks.IsKeyDown(Keys.LeftControl) || ks.IsKeyDown(Keys.RightControl))) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Left == ButtonState.Pressed || ks.IsKeyDown(Keys.Left))))
                            returnvalue &= ~16;
                        break;
                }

                switch ((Port >> 8) & 0xF0)
                {
                    case 0xE0: //0, 9, 8, 7, 6
                        if (ks.IsKeyDown(Keys.D0) || ks.IsKeyDown(Keys.Back) || IsNextKey(Keys.D0) || IsNextKey(Keys.Back) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.Buttons.A == ButtonState.Pressed || ks.IsKeyDown(Keys.LeftControl) || ks.IsKeyDown(Keys.RightControl))) || (JoystickType == JoystickTypeEnum.Cursor && (State.Buttons.A == ButtonState.Pressed || ks.IsKeyDown(Keys.LeftControl) || ks.IsKeyDown(Keys.RightControl))))
                            returnvalue &= ~1;
                        if (ks.IsKeyDown(Keys.D9) || IsNextKey(Keys.D9) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.DPad.Up == ButtonState.Pressed || ks.IsKeyDown(Keys.Up))))
                            returnvalue &= ~2;
                        if (ks.IsKeyDown(Keys.D8) || IsNextKey(Keys.D8) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.DPad.Down == ButtonState.Pressed || ks.IsKeyDown(Keys.Down))) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Right == ButtonState.Pressed || ks.IsKeyDown(Keys.Right))))
                            returnvalue &= ~4;
                        if (ks.IsKeyDown(Keys.D7) || IsNextKey(Keys.D7) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.DPad.Right == ButtonState.Pressed || ks.IsKeyDown(Keys.Right))) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Up == ButtonState.Pressed || ks.IsKeyDown(Keys.Up))))
                            returnvalue &= ~8;
                        if (ks.IsKeyDown(Keys.D6) || IsNextKey(Keys.D6) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.DPad.Left == ButtonState.Pressed || ks.IsKeyDown(Keys.Left))) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Down == ButtonState.Pressed || ks.IsKeyDown(Keys.Down))))
                            returnvalue &= ~16;
                        break;
                    case 0xD0: //P, O, I, U, Y
                        if (ks.IsKeyDown(Keys.P) || IsNextKey(Keys.P))
                            returnvalue &= ~1;
                        if (ks.IsKeyDown(Keys.O) || IsNextKey(Keys.O))
                            returnvalue &= ~2;
                        if (ks.IsKeyDown(Keys.I) || IsNextKey(Keys.I))
                            returnvalue &= ~4;
                        if (ks.IsKeyDown(Keys.U) || IsNextKey(Keys.U))
                            returnvalue &= ~8;
                        if (ks.IsKeyDown(Keys.Y) || IsNextKey(Keys.Y))
                            returnvalue &= ~16;
                        break;
                    case 0xB0: //ENTER, L, K, J, H
                        if (ks.IsKeyDown(Keys.Enter) || ks.IsKeyDown(Keys.Enter) || IsNextKey(Keys.Enter))
                            returnvalue &= ~1;
                        if (ks.IsKeyDown(Keys.L) || IsNextKey(Keys.L))
                            returnvalue &= ~2;
                        if (ks.IsKeyDown(Keys.K) || IsNextKey(Keys.K))
                            returnvalue &= ~4;
                        if (ks.IsKeyDown(Keys.J) || IsNextKey(Keys.J))
                            returnvalue &= ~8;
                        if (ks.IsKeyDown(Keys.H) || IsNextKey(Keys.H))
                            returnvalue &= ~16;
                        break;
                    case 0x70: //SPACE, SYM SHIFT, M, N, B
                        if (ks.IsKeyDown(Keys.Space) || IsNextKey(Keys.Space))
                            returnvalue &= ~1;
                        if (ks.IsKeyDown(Keys.Tab) || ks.IsKeyDown(Keys.Tab))
                        {
                            returnvalue &= ~2;
                        }
                        //if (SymbolShift)
                        //{
                        //    returnvalue &= ~2;
                        //}
                            
                        if (ks.IsKeyDown(Keys.M) || IsNextKey(Keys.M))
                            returnvalue &= ~4;
                        if (ks.IsKeyDown(Keys.N) || IsNextKey(Keys.N))
                            returnvalue &= ~8;
                        if (ks.IsKeyDown(Keys.B) || IsNextKey(Keys.B))
                            returnvalue &= ~16;
                        break;
                    default:
                        //Knas!

                        break;
                }
                if (returnvalue != 255)
                    returnvalue = returnvalue;
                return returnvalue;
            }
            return 0xFF;
        }
        public void EnterText(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                KeyBuffer.Clear();
                foreach (char c in text.ToLower())
                {
                    switch (c)
                    {
                        case '1':
                            KeyBuffer.Add(Keys.D1);
                            break;
                        case '2':
                            KeyBuffer.Add(Keys.D2);
                            break;
                        case '3':
                            KeyBuffer.Add(Keys.D3);
                            break;
                        case '4':
                            KeyBuffer.Add(Keys.D4);
                            break;
                        case '5':
                            KeyBuffer.Add(Keys.D5);
                            break;
                        case '6':
                            KeyBuffer.Add(Keys.D6);
                            break;
                    }
                    KeyBuffer.Add(Keys.None);
                }
            }
        }

    }
}