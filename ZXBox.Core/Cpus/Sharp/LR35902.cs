using System;
using Zilog;

namespace ZXBox.Core.Cpus.Sharp
{
    public class LR35902 : Z80
    {
        public override byte ReadByteFromMemory(ushort address)
        {
            throw new System.NotImplementedException();
        }

        public override void WriteByteToMemory(ushort address, byte bytetowrite)
        {
            throw new System.NotImplementedException();
        }

        public override void WriteWordToMemory(ushort address, ushort word)
        {
            throw new System.NotImplementedException();
        }

        //This is implemented base on information found here https://gbdev.io/pandocs/CPU_Comparison_with_Z80.html
        public override void DoInstructions(int numberOfTStates, Func<Z80, int> gameSpecificFunc)
        {
            NumberOfTstates = numberOfTStates;
            _numberOfTStatesLeft += numberOfTStates;
            _EndTstates2 = numberOfTStates;
            while (true)
            {
                if (interruptTriggered(_numberOfTStatesLeft))
                {
                    SubtractNumberOfTStatesLeft(interrupt());
                    break;
                }

                //Refresh(1);

                NextOpcode();
                if (gameSpecificFunc != null)
                {
                    SubtractNumberOfTStatesLeft(gameSpecificFunc(this));
                }

                switch (opcode)
                {
                    case 0xCB:
                        NextOpcode();
                        DoCBPrefixInstruction();
                        break;
                    case 0xDD:
                        //Removed
                        break;
                    case 0xED:
                        Refresh(1);
                        NextOpcode();
                        DoEDPrefixInstruction();
                        break;
                    case 0xFD:
                        //Removed
                        break;
                    default:
                        Refresh(1);
                        switch (opcode)
                        {
                            case 0x08:
                                WriteWordToMemory(GetNextPCWord(), SP);
                                //WriteByteToMemory(GetNextPCWord(), SP);
                                SubtractNumberOfTStatesLeft(13);
                                break;
                            case 0x10://STOP
                                Halt();
                                break;
                            case 0x22://LDI(HL),A
                                WriteByteToMemory(HL, A);
                                HL++;
                                break;
                            case 0x2A://LDI  A,(HL)
                                A = ReadByteFromMemory(HL);
                                HL++;
                                break;
                            case 0x32://LDD(HL),A
                                WriteByteToMemory(HL, A);
                                HL--;
                                break;
                            case 0x3A://LDD  A,(HL)
                                A = ReadByteFromMemory(HL);
                                HL--;
                                break;
                            case 0xD3:// -
                                break;
                            case 0xD9://RETI
                                RET(true, 14, 0);
                                break;
                            case 0xDB://-
                                break;
                            case 0xE0://LD(FF00 + n),A
                                WriteByteToMemory((ushort)(0xFF00 + GetNextPCByte()), A);
                                break;
                            case 0xE2://LD(FF00 + C),A
                                WriteByteToMemory((ushort)(0xFF00 + C), A);
                                break;
                            case 0xE3://-
                                break;
                            case 0xE4://-
                                break;
                            case 0xE8://ADD SP, dd

                                break;
                            case 0xEA://LD(nn),A
                                WriteByteToMemory(GetNextPCWord(), A);
                                SubtractNumberOfTStatesLeft(13);
                                break;
                            case 0xEB://-
                                break;
                            case 0xEC://-
                                break;
                            case 0xED://-
                                break;
                            case 0xF0://LD A,(FF00 + n)
                                A = ReadByteFromMemory((ushort)(0xFF00 + GetNextPCByte()));
                                SubtractNumberOfTStatesLeft(7);
                                break;
                            case 0xF2://LD   A,(FF00 + C)
                                A = ReadByteFromMemory((ushort)(0xFF00 + C));
                                SubtractNumberOfTStatesLeft(7);
                                break;
                            case 0xF4://-
                                break;
                            case 0xF8://LD HL, SP+dd
                                break;
                            case 0xFA: //LD   A,(nn)
                                A = ReadByteFromMemory(GetNextPCWord());
                                SubtractNumberOfTStatesLeft(7);
                                break;
                            case 0xFC:// -
                                break;
                        }
                        DoNoPrefixInstruction();
                        break;
                }
            }
        }
    }
}
