using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using XnaInput = Microsoft.Xna.Framework.Input;
using ZXBox.Hardware.Interfaces;

namespace ZXBox.Monogame.Hardware;

public class Keyboard : GameComponent, IInput{
    private XnaInput.KeyboardState oldState;
    bool up = false;
    bool down = false;
    bool left = false;
    bool right = false;
    public Keyboard(Game game) : base(game) {}

    public override void Update(GameTime gameTime)
    {
        oldState = XnaInput.Keyboard.GetState();
    }
    public void AddTStates(int tstates) { }
    
    public byte Input(ushort Port, int tstates)
    {
        if ((Port & 0xFF) == 0xFE)
        {
            down = false;
            up = false;
            right = false;
            left = false;

            //Joystick
            if (oldState.IsKeyDown(XnaInput.Keys.Up))
            {
                up = true;  
            }

            if (oldState.IsKeyDown(XnaInput.Keys.Down))
            {
                down = true;
            }

            if (oldState.IsKeyDown(XnaInput.Keys.Left))
            {
                left = true;
            }

            if (oldState.IsKeyDown(XnaInput.Keys.Right))
            {
                right = true;
            }

            // bool symbol = false;
            var returnvalue = 0xFF;

            switch ((Port >> 8) & 0x0F)
            {
                case 0x0E: //SHIFT, Z, X, C, V
                    //if (GetKeyStatus("shift") || ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.Back) || IsNextKey(Keys.Back) || IsNextKey(Keys.LeftShift) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Left == ButtonState.Pressed || State.DPad.Right == ButtonState.Pressed || State.DPad.Up == ButtonState.Pressed || State.DPad.Down == ButtonState.Pressed || State.Buttons.A == ButtonState.Pressed || ks.IsKeyDown(Keys.LeftControl) || ks.IsKeyDown(Keys.RightControl))))
                    if (oldState.IsKeyDown(XnaInput.Keys.LeftShift) || 
                        oldState.IsKeyDown(XnaInput.Keys.RightShift) || 
                        oldState.IsKeyDown(XnaInput.Keys.Back) || up || down || left || right)
                        returnvalue &= ~1;
                    if (oldState.IsKeyDown(XnaInput.Keys.Z))
                        returnvalue &= ~2;
                    if (oldState.IsKeyDown(XnaInput.Keys.X))
                        returnvalue &= ~4;
                    if (oldState.IsKeyDown(XnaInput.Keys.C))
                        returnvalue &= ~8;
                    if (oldState.IsKeyDown(XnaInput.Keys.V))
                        returnvalue &= ~16;
                    break;
                case 0x0D: //A, S, D, F, G
                    if (oldState.IsKeyDown(XnaInput.Keys.A))
                        returnvalue &= ~1;
                    if (oldState.IsKeyDown(XnaInput.Keys.S))
                        returnvalue &= ~2;
                    if (oldState.IsKeyDown(XnaInput.Keys.D))
                        returnvalue &= ~4;
                    if (oldState.IsKeyDown(XnaInput.Keys.F))
                        returnvalue &= ~8;
                    if (oldState.IsKeyDown(XnaInput.Keys.G))
                        returnvalue &= ~16;
                    break;
                case 0x0B: //Q, W, E, R, T
                    if (oldState.IsKeyDown(XnaInput.Keys.Q))
                        returnvalue &= ~1;
                    if (oldState.IsKeyDown(XnaInput.Keys.W))
                        returnvalue &= ~2;
                    if (oldState.IsKeyDown(XnaInput.Keys.E))
                        returnvalue &= ~4;
                    if (oldState.IsKeyDown(XnaInput.Keys.R))
                        returnvalue &= ~8;
                    if (oldState.IsKeyDown(XnaInput.Keys.T))
                        returnvalue &= ~16;
                    break;
                case 0x07: //1, 2, 3, 4, 5
                    //if (ks.IsKeyDown(Keys.D1) || IsNextKey(Keys.D1) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.DPad.Left == ButtonState.Pressed || ks.IsKeyDown(Keys.Left))))
                    if (oldState.IsKeyDown(XnaInput.Keys.D1))
                        returnvalue &= ~1;
                    //if (ks.IsKeyDown(Keys.D2) || IsNextKey(Keys.D2) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.DPad.Right == ButtonState.Pressed || ks.IsKeyDown(Keys.Right))))
                    if (oldState.IsKeyDown(XnaInput.Keys.D2))
                        returnvalue &= ~2;
                    //if (ks.IsKeyDown(Keys.D3) || IsNextKey(Keys.D3) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.DPad.Down == ButtonState.Pressed || ks.IsKeyDown(Keys.Down))))
                    if (oldState.IsKeyDown(XnaInput.Keys.D3))
                        returnvalue &= ~4;
                    //if (ks.IsKeyDown(Keys.D4) || IsNextKey(Keys.D4) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.DPad.Up == ButtonState.Pressed || ks.IsKeyDown(Keys.Up))))
                    if (oldState.IsKeyDown(XnaInput.Keys.D4))
                        returnvalue &= ~8;
                    //if (ks.IsKeyDown(Keys.D5) || IsNextKey(Keys.D5) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.Buttons.A == ButtonState.Pressed || ks.IsKeyDown(Keys.LeftControl) || ks.IsKeyDown(Keys.RightControl))) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Left == ButtonState.Pressed || ks.IsKeyDown(Keys.Left))))
                    if (oldState.IsKeyDown(XnaInput.Keys.D5) || left)
                        returnvalue &= ~16;
                    break;
            }

            switch ((Port >> 8) & 0xF0)
            {
                case 0xE0: //0, 9, 8, 7, 6
                    //if (ks.IsKeyDown(Keys.D0) || ks.IsKeyDown(Keys.Back) || IsNextKey(Keys.D0) || IsNextKey(Keys.Back) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.Buttons.A == ButtonState.Pressed || ks.IsKeyDown(Keys.LeftControl) || ks.IsKeyDown(Keys.RightControl))) || (JoystickType == JoystickTypeEnum.Cursor && (State.Buttons.A == ButtonState.Pressed || ks.IsKeyDown(Keys.LeftControl) || ks.IsKeyDown(Keys.RightControl))))
                    if (oldState.IsKeyDown(XnaInput.Keys.D0) || oldState.IsKeyDown(XnaInput.Keys.Back))
                        returnvalue &= ~1;
                    //if (ks.IsKeyDown(Keys.D9) || IsNextKey(Keys.D9) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.DPad.Up == ButtonState.Pressed || ks.IsKeyDown(Keys.Up))))
                    if (oldState.IsKeyDown(XnaInput.Keys.D9))
                        returnvalue &= ~2;
                    //if (ks.IsKeyDown(Keys.D8) || IsNextKey(Keys.D8) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.DPad.Down == ButtonState.Pressed || ks.IsKeyDown(Keys.Down))) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Right == ButtonState.Pressed || ks.IsKeyDown(Keys.Right))))
                    if (oldState.IsKeyDown(XnaInput.Keys.D8) || right)
                        returnvalue &= ~4;
                    //if (ks.IsKeyDown(Keys.D7) || IsNextKey(Keys.D7) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.DPad.Right == ButtonState.Pressed || ks.IsKeyDown(Keys.Right))) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Up == ButtonState.Pressed || ks.IsKeyDown(Keys.Up))))
                    if (oldState.IsKeyDown(XnaInput.Keys.D7) || up)
                        returnvalue &= ~8;
                    //if (ks.IsKeyDown(Keys.D6) || IsNextKey(Keys.D6) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.DPad.Left == ButtonState.Pressed || ks.IsKeyDown(Keys.Left))) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Down == ButtonState.Pressed || ks.IsKeyDown(Keys.Down))))
                    if (oldState.IsKeyDown(XnaInput.Keys.D6) || down)
                        returnvalue &= ~16;
                    break;
                case 0xD0: //P, O, I, U, Y
                    // if (GetKeyStatus("p"))
                    if (oldState.IsKeyDown(XnaInput.Keys.P))
                        returnvalue &= ~1;
                    if (oldState.IsKeyDown(XnaInput.Keys.O))
                        returnvalue &= ~2;
                    if (oldState.IsKeyDown(XnaInput.Keys.I))
                        returnvalue &= ~4;
                    if (oldState.IsKeyDown(XnaInput.Keys.U))
                        returnvalue &= ~8;
                    if (oldState.IsKeyDown(XnaInput.Keys.Y))
                        returnvalue &= ~16;
                    break;
                case 0xB0: //ENTER, L, K, J, H
                    if (oldState.IsKeyDown(XnaInput.Keys.Enter))
                        returnvalue &= ~1;
                    if (oldState.IsKeyDown(XnaInput.Keys.L))
                        returnvalue &= ~2;
                    if (oldState.IsKeyDown(XnaInput.Keys.K))
                        returnvalue &= ~4;
                    if (oldState.IsKeyDown(XnaInput.Keys.J))
                        returnvalue &= ~8;
                    if (oldState.IsKeyDown(XnaInput.Keys.H))
                        returnvalue &= ~16;
                    break;
                case 0x70: //SPACE, SYM SHIFT, M, N, B
                    if (oldState.IsKeyDown(XnaInput.Keys.Space))
                        returnvalue &= ~1;
                    if (oldState.IsKeyDown(XnaInput.Keys.LeftAlt) ||
                        oldState.IsKeyDown(XnaInput.Keys.RightAlt))
                    {
                        returnvalue &= ~2;
                    }
                    //if (SymbolShift)
                    //{
                    //    returnvalue &= ~2;
                    //}

                    if (oldState.IsKeyDown(XnaInput.Keys.M))
                        returnvalue &= ~4;
                    if (oldState.IsKeyDown(XnaInput.Keys.N))
                        returnvalue &= ~8;
                    if (oldState.IsKeyDown(XnaInput.Keys.B))
                        returnvalue &= ~16;
                    break;
                default:
                    //Knas!

                    break;
            }

            return (byte)returnvalue;
        }
        return 0xFF;
    }
}
