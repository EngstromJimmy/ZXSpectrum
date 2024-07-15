using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
//using Microsoft.Xna.Framework.Input;
using ZXBox.Hardware.Interfaces;

namespace ZXBox.Hardware.Input
{
    public class JavaScriptKeyboard : IInput
    {
        public List<string> KeyBuffer = new List<string>();
        int NoKeyCounter = 0;
        bool SymbolShift = false;
        int sectionnumber = 1;
        bool up = false;
        bool down = false;
        bool left = false;
        bool right = false;
        bool fire = false;
        public JoystickTypeEnum JoystickType { get; set; }

        public bool GetKeyStatus(string key)
        {
            return KeyBuffer.Any(k => k.ToLower() == key.ToLower());
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
                if (GetKeyStatus("arrowup"))
                {
                    up = true;
                }

                if (GetKeyStatus("arrowdown"))
                {
                    down = true;
                }

                if (GetKeyStatus("arrowleft"))
                {
                    left = true;
                }

                if (GetKeyStatus("arrowright"))
                {
                    right = true;
                }

                var returnvalue = 0xFF;

                //Special cace for any key
                if (((Port >> 8) & 0xFF) == 0x01 || ((Port >> 8) & 0xFF) == 0x00 || ((Port >> 8) & 0xFF) == 0x02)
                {   //Check for any key including joy, we might need to add more joy buttons later on

                    if (KeyBuffer.Count() > 0)
                    {
                        return (byte)(returnvalue &= ~1);
                    }

                }

                switch ((Port >> 8) & 0x0F)
                {
                    case 0x0E: //SHIFT, Z, X, C, V
                        //if (GetKeyStatus("shift") || ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.Back) || IsNextKey(Keys.Back) || IsNextKey(Keys.LeftShift) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Left == ButtonState.Pressed || State.DPad.Right == ButtonState.Pressed || State.DPad.Up == ButtonState.Pressed || State.DPad.Down == ButtonState.Pressed || State.Buttons.A == ButtonState.Pressed || ks.IsKeyDown(Keys.LeftControl) || ks.IsKeyDown(Keys.RightControl))))
                        if (GetKeyStatus("shift") || GetKeyStatus("backspace") || up || down || left || right)
                            returnvalue &= ~1;
                        if (GetKeyStatus("z"))
                            returnvalue &= ~2;
                        if (GetKeyStatus("x"))
                            returnvalue &= ~4;
                        if (GetKeyStatus("c"))
                            returnvalue &= ~8;
                        if (GetKeyStatus("v"))
                            returnvalue &= ~16;
                        break;
                    case 0x0D: //A, S, D, F, G
                        if (GetKeyStatus("a"))
                            returnvalue &= ~1;
                        if (GetKeyStatus("s"))
                            returnvalue &= ~2;
                        if (GetKeyStatus("d"))
                            returnvalue &= ~4;
                        if (GetKeyStatus("f"))
                            returnvalue &= ~8;
                        if (GetKeyStatus("g"))
                            returnvalue &= ~16;
                        break;
                    case 0x0B: //Q, W, E, R, T
                        if (GetKeyStatus("q"))
                            returnvalue &= ~1;
                        if (GetKeyStatus("w"))
                            returnvalue &= ~2;
                        if (GetKeyStatus("e"))
                            returnvalue &= ~4;
                        if (GetKeyStatus("r"))
                            returnvalue &= ~8;
                        if (GetKeyStatus("t"))
                            returnvalue &= ~16;
                        break;
                    case 0x07: //1, 2, 3, 4, 5
                        //if (ks.IsKeyDown(Keys.D1) || IsNextKey(Keys.D1) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.DPad.Left == ButtonState.Pressed || ks.IsKeyDown(Keys.Left))))
                        if (GetKeyStatus("1"))
                            returnvalue &= ~1;
                        //if (ks.IsKeyDown(Keys.D2) || IsNextKey(Keys.D2) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.DPad.Right == ButtonState.Pressed || ks.IsKeyDown(Keys.Right))))
                        if (GetKeyStatus("2"))
                            returnvalue &= ~2;
                        //if (ks.IsKeyDown(Keys.D3) || IsNextKey(Keys.D3) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.DPad.Down == ButtonState.Pressed || ks.IsKeyDown(Keys.Down))))
                        if (GetKeyStatus("3"))
                            returnvalue &= ~4;
                        //if (ks.IsKeyDown(Keys.D4) || IsNextKey(Keys.D4) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.DPad.Up == ButtonState.Pressed || ks.IsKeyDown(Keys.Up))))
                        if (GetKeyStatus("4"))
                            returnvalue &= ~8;
                        //if (ks.IsKeyDown(Keys.D5) || IsNextKey(Keys.D5) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.Buttons.A == ButtonState.Pressed || ks.IsKeyDown(Keys.LeftControl) || ks.IsKeyDown(Keys.RightControl))) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Left == ButtonState.Pressed || ks.IsKeyDown(Keys.Left))))
                        if (GetKeyStatus("5") || left)
                            returnvalue &= ~16;
                        break;
                }

                switch ((Port >> 8) & 0xF0)
                {
                    case 0xE0: //0, 9, 8, 7, 6
                        //if (ks.IsKeyDown(Keys.D0) || ks.IsKeyDown(Keys.Back) || IsNextKey(Keys.D0) || IsNextKey(Keys.Back) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.Buttons.A == ButtonState.Pressed || ks.IsKeyDown(Keys.LeftControl) || ks.IsKeyDown(Keys.RightControl))) || (JoystickType == JoystickTypeEnum.Cursor && (State.Buttons.A == ButtonState.Pressed || ks.IsKeyDown(Keys.LeftControl) || ks.IsKeyDown(Keys.RightControl))))
                        if (GetKeyStatus("0") || GetKeyStatus("backspace"))
                            returnvalue &= ~1;
                        //if (ks.IsKeyDown(Keys.D9) || IsNextKey(Keys.D9) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.DPad.Up == ButtonState.Pressed || ks.IsKeyDown(Keys.Up))))
                        if (GetKeyStatus("9"))
                            returnvalue &= ~2;
                        //if (ks.IsKeyDown(Keys.D8) || IsNextKey(Keys.D8) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.DPad.Down == ButtonState.Pressed || ks.IsKeyDown(Keys.Down))) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Right == ButtonState.Pressed || ks.IsKeyDown(Keys.Right))))
                        if (GetKeyStatus("8") || right)
                            returnvalue &= ~4;
                        //if (ks.IsKeyDown(Keys.D7) || IsNextKey(Keys.D7) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.DPad.Right == ButtonState.Pressed || ks.IsKeyDown(Keys.Right))) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Up == ButtonState.Pressed || ks.IsKeyDown(Keys.Up))))
                        if (GetKeyStatus("7") || up)
                            returnvalue &= ~8;
                        //if (ks.IsKeyDown(Keys.D6) || IsNextKey(Keys.D6) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.DPad.Left == ButtonState.Pressed || ks.IsKeyDown(Keys.Left))) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Down == ButtonState.Pressed || ks.IsKeyDown(Keys.Down))))
                        if (GetKeyStatus("6") || down)
                            returnvalue &= ~16;
                        break;
                    case 0xD0: //P, O, I, U, Y
                        if (GetKeyStatus("p"))
                            returnvalue &= ~1;
                        if (GetKeyStatus("o"))
                            returnvalue &= ~2;
                        if (GetKeyStatus("i"))
                            returnvalue &= ~4;
                        if (GetKeyStatus("u"))
                            returnvalue &= ~8;
                        if (GetKeyStatus("y"))
                            returnvalue &= ~16;
                        break;
                    case 0xB0: //ENTER, L, K, J, H
                        if (GetKeyStatus("enter"))
                            returnvalue &= ~1;
                        if (GetKeyStatus("l"))
                            returnvalue &= ~2;
                        if (GetKeyStatus("k"))
                            returnvalue &= ~4;
                        if (GetKeyStatus("j"))
                            returnvalue &= ~8;
                        if (GetKeyStatus("h"))
                            returnvalue &= ~16;
                        break;
                    case 0x70: //SPACE, SYM SHIFT, M, N, B
                        if (GetKeyStatus("space"))
                            returnvalue &= ~1;
                        if (GetKeyStatus("alt"))
                        {
                            returnvalue &= ~2;
                        }
                        //if (SymbolShift)
                        //{
                        //    returnvalue &= ~2;
                        //}

                        if (GetKeyStatus("m"))
                            returnvalue &= ~4;
                        if (GetKeyStatus("n"))
                            returnvalue &= ~8;
                        if (GetKeyStatus("b"))
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

    internal static class AsyncHelper
    {
        private static readonly TaskFactory _myTaskFactory = new
          TaskFactory(CancellationToken.None,
                      TaskCreationOptions.None,
                      TaskContinuationOptions.None,
                      TaskScheduler.Default);

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncHelper._myTaskFactory
              .StartNew<Task<TResult>>(func)
              .Unwrap<TResult>()
              .GetAwaiter()
              .GetResult();
        }

        public static void RunSync(Func<Task> func)
        {
            AsyncHelper._myTaskFactory
              .StartNew<Task>(func)
              .Unwrap()
              .GetAwaiter()
              .GetResult();
        }
    }
}