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
        private readonly byte[] _rowValues = new byte[8];
        private readonly byte[] _portResponses = new byte[256];
        private bool _hasAnyKey;
        public JoystickTypeEnum JoystickType { get; set; }

        public void SetKeyMask(ulong keyMask)
        {
            _keyMask = keyMask;
            RebuildRowValues();
        }

        public void AddTStates(int tstates) { }

        public byte Input(ushort Port, int tstates)
        {
            if ((Port & 0xFF) == 0xFE)
            {
                return _portResponses[(byte)(Port >> 8)];
            }
            return 0xFF;
        }

        private bool IsPressed(ulong keyMask)
        {
            return (_keyMask & keyMask) != 0;
        }

        private void RebuildRowValues()
        {
            var up = IsPressed(ArrowUpKey);
            var down = IsPressed(ArrowDownKey);
            var left = IsPressed(ArrowLeftKey);
            var right = IsPressed(ArrowRightKey);

            _rowValues[0] = BuildRow(
                IsPressed(ShiftKey) || IsPressed(BackspaceKey) || up || down || left || right,
                IsPressed(ZKey),
                IsPressed(XKey),
                IsPressed(CKey),
                IsPressed(VKey));

            _rowValues[1] = BuildRow(
                IsPressed(AKey),
                IsPressed(SKey),
                IsPressed(DKey),
                IsPressed(FKey),
                IsPressed(GKey));

            _rowValues[2] = BuildRow(
                IsPressed(QKey),
                IsPressed(WKey),
                IsPressed(EKey),
                IsPressed(RKey),
                IsPressed(TKey));

            _rowValues[3] = BuildRow(
                IsPressed(Digit1Key),
                IsPressed(Digit2Key),
                IsPressed(Digit3Key),
                IsPressed(Digit4Key),
                IsPressed(Digit5Key) || left);

            _rowValues[4] = BuildRow(
                IsPressed(Digit0Key) || IsPressed(BackspaceKey),
                IsPressed(Digit9Key),
                IsPressed(Digit8Key) || right,
                IsPressed(Digit7Key) || up,
                IsPressed(Digit6Key) || down);

            _rowValues[5] = BuildRow(
                IsPressed(PKey),
                IsPressed(OKey),
                IsPressed(IKey),
                IsPressed(UKey),
                IsPressed(YKey));

            _rowValues[6] = BuildRow(
                IsPressed(EnterKey),
                IsPressed(LKey),
                IsPressed(KKey),
                IsPressed(JKey),
                IsPressed(HKey));

            _rowValues[7] = BuildRow(
                IsPressed(SpaceKey),
                IsPressed(AltKey),
                IsPressed(MKey),
                IsPressed(NKey),
                IsPressed(BKey));

            _hasAnyKey = _keyMask != 0;
            RebuildPortResponses();
        }

        private void RebuildPortResponses()
        {
            for (var highByte = 0; highByte < _portResponses.Length; highByte++)
            {
                var value = 0xFF;

                if ((highByte == 0x01 || highByte == 0x00 || highByte == 0x02) && _hasAnyKey)
                {
                    value &= ~1;
                }

                for (var row = 0; row < _rowValues.Length; row++)
                {
                    if ((highByte & (1 << row)) == 0)
                    {
                        value &= _rowValues[row];
                    }
                }

                _portResponses[highByte] = (byte)value;
            }
        }

        private static byte BuildRow(bool bit0, bool bit1, bool bit2, bool bit3, bool bit4)
        {
            var value = 0xFF;

            if (bit0)
            {
                value &= ~1;
            }

            if (bit1)
            {
                value &= ~2;
            }

            if (bit2)
            {
                value &= ~4;
            }

            if (bit3)
            {
                value &= ~8;
            }

            if (bit4)
            {
                value &= ~16;
            }

            return (byte)value;
        }
    }
}