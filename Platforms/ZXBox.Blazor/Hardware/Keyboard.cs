using System;
//using Microsoft.Xna.Framework.Input;
using ZXBox.Hardware.Interfaces;

namespace ZXBox.Hardware.Input
{
    public class JavaScriptKeyboard : IInput
    {
        private const ulong ShiftKey = 1UL << 0;
        private const ulong BackspaceKey = 1UL << 1;
        private const ulong AltKey = 1UL << 2;
        private const ulong EnterKey = 1UL << 3;
        private const ulong SpaceKey = 1UL << 4;
        private const ulong ArrowUpKey = 1UL << 5;
        private const ulong ArrowDownKey = 1UL << 6;
        private const ulong ArrowLeftKey = 1UL << 7;
        private const ulong ArrowRightKey = 1UL << 8;
        private const ulong ZKey = 1UL << 9;
        private const ulong XKey = 1UL << 10;
        private const ulong CKey = 1UL << 11;
        private const ulong VKey = 1UL << 12;
        private const ulong AKey = 1UL << 13;
        private const ulong SKey = 1UL << 14;
        private const ulong DKey = 1UL << 15;
        private const ulong FKey = 1UL << 16;
        private const ulong GKey = 1UL << 17;
        private const ulong QKey = 1UL << 18;
        private const ulong WKey = 1UL << 19;
        private const ulong EKey = 1UL << 20;
        private const ulong RKey = 1UL << 21;
        private const ulong TKey = 1UL << 22;
        private const ulong Digit1Key = 1UL << 23;
        private const ulong Digit2Key = 1UL << 24;
        private const ulong Digit3Key = 1UL << 25;
        private const ulong Digit4Key = 1UL << 26;
        private const ulong Digit5Key = 1UL << 27;
        private const ulong Digit0Key = 1UL << 28;
        private const ulong Digit9Key = 1UL << 29;
        private const ulong Digit8Key = 1UL << 30;
        private const ulong Digit7Key = 1UL << 31;
        private const ulong Digit6Key = 1UL << 32;
        private const ulong PKey = 1UL << 33;
        private const ulong OKey = 1UL << 34;
        private const ulong IKey = 1UL << 35;
        private const ulong UKey = 1UL << 36;
        private const ulong YKey = 1UL << 37;
        private const ulong LKey = 1UL << 38;
        private const ulong KKey = 1UL << 39;
        private const ulong JKey = 1UL << 40;
        private const ulong HKey = 1UL << 41;
        private const ulong MKey = 1UL << 42;
        private const ulong NKey = 1UL << 43;
        private const ulong BKey = 1UL << 44;

        private ulong _keyMask;
        public JoystickTypeEnum JoystickType { get; set; }

        public void SetKeyMask(ulong keyMask)
        {
            _keyMask = keyMask;
        }

        public void AddTStates(int tstates) { }

        public byte Input(ushort Port, int tstates)
        {
            if ((Port & 0xFF) == 0xFE)
            {
                var up = IsPressed(ArrowUpKey);
                var down = IsPressed(ArrowDownKey);
                var left = IsPressed(ArrowLeftKey);
                var right = IsPressed(ArrowRightKey);

                var returnvalue = 0xFF;

                //Special cace for any key
                if (((Port >> 8) & 0xFF) == 0x01 || ((Port >> 8) & 0xFF) == 0x00 || ((Port >> 8) & 0xFF) == 0x02)
                {   //Check for any key including joy, we might need to add more joy buttons later on

                    if (_keyMask != 0)
                    {
                        return (byte)(returnvalue &= ~1);
                    }

                }

                switch ((Port >> 8) & 0x0F)
                {
                    case 0x0E: //SHIFT, Z, X, C, V
                        //if (GetKeyStatus("shift") || ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.Back) || IsNextKey(Keys.Back) || IsNextKey(Keys.LeftShift) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Left == ButtonState.Pressed || State.DPad.Right == ButtonState.Pressed || State.DPad.Up == ButtonState.Pressed || State.DPad.Down == ButtonState.Pressed || State.Buttons.A == ButtonState.Pressed || ks.IsKeyDown(Keys.LeftControl) || ks.IsKeyDown(Keys.RightControl))))
                        if (IsPressed(ShiftKey) || IsPressed(BackspaceKey) || up || down || left || right)
                            returnvalue &= ~1;
                        if (IsPressed(ZKey))
                            returnvalue &= ~2;
                        if (IsPressed(XKey))
                            returnvalue &= ~4;
                        if (IsPressed(CKey))
                            returnvalue &= ~8;
                        if (IsPressed(VKey))
                            returnvalue &= ~16;
                        break;
                    case 0x0D: //A, S, D, F, G
                        if (IsPressed(AKey))
                            returnvalue &= ~1;
                        if (IsPressed(SKey))
                            returnvalue &= ~2;
                        if (IsPressed(DKey))
                            returnvalue &= ~4;
                        if (IsPressed(FKey))
                            returnvalue &= ~8;
                        if (IsPressed(GKey))
                            returnvalue &= ~16;
                        break;
                    case 0x0B: //Q, W, E, R, T
                        if (IsPressed(QKey))
                            returnvalue &= ~1;
                        if (IsPressed(WKey))
                            returnvalue &= ~2;
                        if (IsPressed(EKey))
                            returnvalue &= ~4;
                        if (IsPressed(RKey))
                            returnvalue &= ~8;
                        if (IsPressed(TKey))
                            returnvalue &= ~16;
                        break;
                    case 0x07: //1, 2, 3, 4, 5
                        //if (ks.IsKeyDown(Keys.D1) || IsNextKey(Keys.D1) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.DPad.Left == ButtonState.Pressed || ks.IsKeyDown(Keys.Left))))
                        if (IsPressed(Digit1Key))
                            returnvalue &= ~1;
                        //if (ks.IsKeyDown(Keys.D2) || IsNextKey(Keys.D2) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.DPad.Right == ButtonState.Pressed || ks.IsKeyDown(Keys.Right))))
                        if (IsPressed(Digit2Key))
                            returnvalue &= ~2;
                        //if (ks.IsKeyDown(Keys.D3) || IsNextKey(Keys.D3) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.DPad.Down == ButtonState.Pressed || ks.IsKeyDown(Keys.Down))))
                        if (IsPressed(Digit3Key))
                            returnvalue &= ~4;
                        //if (ks.IsKeyDown(Keys.D4) || IsNextKey(Keys.D4) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.DPad.Up == ButtonState.Pressed || ks.IsKeyDown(Keys.Up))))
                        if (IsPressed(Digit4Key))
                            returnvalue &= ~8;
                        //if (ks.IsKeyDown(Keys.D5) || IsNextKey(Keys.D5) || (JoystickType == JoystickTypeEnum.Sinclair1 && (State.Buttons.A == ButtonState.Pressed || ks.IsKeyDown(Keys.LeftControl) || ks.IsKeyDown(Keys.RightControl))) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Left == ButtonState.Pressed || ks.IsKeyDown(Keys.Left))))
                        if (IsPressed(Digit5Key) || left)
                            returnvalue &= ~16;
                        break;
                }

                switch ((Port >> 8) & 0xF0)
                {
                    case 0xE0: //0, 9, 8, 7, 6
                        //if (ks.IsKeyDown(Keys.D0) || ks.IsKeyDown(Keys.Back) || IsNextKey(Keys.D0) || IsNextKey(Keys.Back) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.Buttons.A == ButtonState.Pressed || ks.IsKeyDown(Keys.LeftControl) || ks.IsKeyDown(Keys.RightControl))) || (JoystickType == JoystickTypeEnum.Cursor && (State.Buttons.A == ButtonState.Pressed || ks.IsKeyDown(Keys.LeftControl) || ks.IsKeyDown(Keys.RightControl))))
                        if (IsPressed(Digit0Key) || IsPressed(BackspaceKey))
                            returnvalue &= ~1;
                        //if (ks.IsKeyDown(Keys.D9) || IsNextKey(Keys.D9) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.DPad.Up == ButtonState.Pressed || ks.IsKeyDown(Keys.Up))))
                        if (IsPressed(Digit9Key))
                            returnvalue &= ~2;
                        //if (ks.IsKeyDown(Keys.D8) || IsNextKey(Keys.D8) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.DPad.Down == ButtonState.Pressed || ks.IsKeyDown(Keys.Down))) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Right == ButtonState.Pressed || ks.IsKeyDown(Keys.Right))))
                        if (IsPressed(Digit8Key) || right)
                            returnvalue &= ~4;
                        //if (ks.IsKeyDown(Keys.D7) || IsNextKey(Keys.D7) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.DPad.Right == ButtonState.Pressed || ks.IsKeyDown(Keys.Right))) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Up == ButtonState.Pressed || ks.IsKeyDown(Keys.Up))))
                        if (IsPressed(Digit7Key) || up)
                            returnvalue &= ~8;
                        //if (ks.IsKeyDown(Keys.D6) || IsNextKey(Keys.D6) || (JoystickType == JoystickTypeEnum.Sinclair2 && (State.DPad.Left == ButtonState.Pressed || ks.IsKeyDown(Keys.Left))) || (JoystickType == JoystickTypeEnum.Cursor && (State.DPad.Down == ButtonState.Pressed || ks.IsKeyDown(Keys.Down))))
                        if (IsPressed(Digit6Key) || down)
                            returnvalue &= ~16;
                        break;
                    case 0xD0: //P, O, I, U, Y
                        if (IsPressed(PKey))
                            returnvalue &= ~1;
                        if (IsPressed(OKey))
                            returnvalue &= ~2;
                        if (IsPressed(IKey))
                            returnvalue &= ~4;
                        if (IsPressed(UKey))
                            returnvalue &= ~8;
                        if (IsPressed(YKey))
                            returnvalue &= ~16;
                        break;
                    case 0xB0: //ENTER, L, K, J, H
                        if (IsPressed(EnterKey))
                            returnvalue &= ~1;
                        if (IsPressed(LKey))
                            returnvalue &= ~2;
                        if (IsPressed(KKey))
                            returnvalue &= ~4;
                        if (IsPressed(JKey))
                            returnvalue &= ~8;
                        if (IsPressed(HKey))
                            returnvalue &= ~16;
                        break;
                    case 0x70: //SPACE, SYM SHIFT, M, N, B
                        if (IsPressed(SpaceKey))
                            returnvalue &= ~1;
                        if (IsPressed(AltKey))
                        {
                            returnvalue &= ~2;
                        }
                        //if (SymbolShift)
                        //{
                        //    returnvalue &= ~2;
                        //}

                        if (IsPressed(MKey))
                            returnvalue &= ~4;
                        if (IsPressed(NKey))
                            returnvalue &= ~8;
                        if (IsPressed(BKey))
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

        private bool IsPressed(ulong keyMask)
        {
            return (_keyMask & keyMask) != 0;
        }
    }
}