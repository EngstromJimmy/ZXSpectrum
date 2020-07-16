using System;

namespace Zilog
{
    public partial class Z80
    {
        public void DoEDPrefixInstruction()
        {
                    Refresh(1);
                    switch (opcode)
                    {
                        case 0x4A:	    //ADC HL,BC
                            HL = ADDADC16(HL, BC, true, 15); break;
                        case 0x5A:      //ADC HL,DE
                            HL = ADDADC16(HL, DE, true, 15); break;
                        case 0x6A:	    //ADC HL,HL
                            HL = ADDADC16(HL, HL, true, 15); break;
                        case 0x7A:		//ADC HL,SP
                            HL = ADDADC16(HL, SP, true, 15); break;
                        case 0xA9:
                            CPD(); break;
                        case 0xB9:		//CPDR
                            CPDR(); break;
                        case 0xA1:		//CPI
                            CPI(); break;
                        case 0xB1:		//CPIR
                            CPIR(); break;
                        case 0x4E:		//IM 0/1*
                        case 0x6E:		//IM 0/1*
                        case 0x46:      //IM0
                        case 0x66:      //IM0
                            IM = 0;
                            NumberOfTStatesLeft -= 8;
                            break;
                        case 0x56:     //IM 1
                        case 0x76:     //IM 1*
                            IM = 1;
                            NumberOfTStatesLeft -= 8;
                            break;
                        case 0x5E:  //IM 2
                        case 0x7E: //IM 2*
                            IM = 2;
                            NumberOfTStatesLeft -= 8;
                            break;
                        case 0x63:  //LD (nn),HL
                            WriteWordToMemory(GetNextPCWord(), HL);
                            NumberOfTStatesLeft -= 16;
                            break;
                        case 0x6B:  //LD HL,(nn)
                            HL = ReadWordFromMemory(GetNextPCWord());
                            NumberOfTStatesLeft -= 20;
                            break;
                        case 0x73:  //LD (nn),SP
                            WriteWordToMemory(GetNextPCWord(), SP);
                            NumberOfTStatesLeft -= 16;
                            break;
                        case 0x7B:  //LD SP,(nn)
                            SP = ReadWordFromMemory(GetNextPCWord());
                            NumberOfTStatesLeft -= 20;
                            break;
                        case 0xA2:     //INI
                            INI(16); break;
                        case 0x78:      //IN A,(C)
                            A = INBC(12); break;
                        case 0x40:		//IN B,(C)
                            B = INBC(12); break;
                        case 0x48:		//IN C,(C)
                            C = INBC(12); break;
                        case 0x50:		//IN D,(C)
                            D = INBC(12); break;
                        case 0x58:		//IN E,(C)
                            E = INBC(12); break;
                        case 0x70:		//IN F,(C)* / IN (C)*
                            INBC(12); break;
                        case 0x60:		//IN H,(C)
                            H = INBC(12); break;
                        case 0x68:		//IN L,(C)F
                            L = INBC(12); break;
                        case 0xAA:      //IND
                            IND(16); break;
                        case 0xBA:      //INDR
                            INDR(); break;
                        case 0x57:	//LD A,I
                            LDAI(); break;
                        case 0x5F:		//LD A,R
                            LDAR(); break;
                        case 0x47:     //LD I,A
                            I = A; NumberOfTStatesLeft -= 9; break;
                        case 0x4F:     //LD R,A
                            R = A; NumberOfTStatesLeft -= 9; break;
                        case 0xA8:    //LDD
                            LDD(); break;
                        case 0xB8: 	//LDDR
                            LDDR(); break;
                        case 0xA0:		//LDI
                            LDI(); break;
                        case 0xB0:		//LDIR
                            LDIR(); break;
                        case 0x44:		//NEG
                        case 0x4C:		//NEG*
                        case 0x54:		//NEG*
                        case 0x5C:		//NEG*
                        case 0x64:		//NEG*
                        case 0x6C:		//NEG*
                        case 0x74:		//NEG*
                        case 0x7C:		//NEG*
                            NEG(); break;
                        case 0xBB:
                            OTDR(); break;
                        case 0xB3:
                            OTIR(); break;
                        case 0x71:		//OUT (C),0*
                            NumberOfTStatesLeft -= 8;
                            Out(BC, 0, NumberOfTstates - Math.Abs(NumberOfTStatesLeft));
                            NumberOfTStatesLeft -= 4;
                            break;
                        case 0x79:		//OUT (C),A
                        case 0x41:		//OUT (C),B
                        case 0x49:		//OUT (C),C
                        case 0x51:		//OUT (C),D
                        case 0x59:		//OUT (C),E
                        case 0x61:		//OUT (C),H
                        case 0x69:		//OUT (C),L
                            NumberOfTStatesLeft -= 8;
                            Out(BC, RegisterValueFromOP(3), NumberOfTstates - Math.Abs(NumberOfTStatesLeft));
                            NumberOfTStatesLeft -= 4; //12
                            break;
                        case 0xAB:  //OUTD
                            OUTD(); break;
                        case 0xA3:  //OUTI
                            OUTI();
                            break;
                        case 0x4D:  //RETI
                                RET(true, 14, 0);
                            break;
                        case 0x45:		//RETN
                        case 0x55:		//RETN*
                        case 0x5D:		//RETN*
                        case 0x65:		//RETN*
                        case 0x6D:		//RETN*
                        case 0x75:		//RETN*
                        case 0x7D:		//RETN*
                            RET(true,14,0);
                            IFF=IFF2;
                        break;
                        case 0x6F:  //RLD
                            RLD();
                            break;
                        case 0x67://RRD
                            RRD(); 
                            break;
                        case 0x42:		//SBC HL,BC
                            HL = SBC16(HL, BC, 15);
                            break;
                        case 0x52:		//SBC HL,DE
                            HL = SBC16(HL, DE, 15);
                            break;
                        case 0x62:		//SBC HL,HL
                            HL = SBC16(HL, HL, 15);
                            break;
                        case 0x72:		//SBC HL,SP
                            HL = SBC16(HL, SP, 15);
                            break;
                        case 0x53:	        //LD (nn),DE
                            WriteWordToMemory(GetNextPCWord(),DE);
                            NumberOfTStatesLeft -= 20;
                            break;
                        case 0x43:	 //LD (nn),BC
                                WriteWordToMemory(GetNextPCWord(), BC);
                                NumberOfTStatesLeft -= 20;
                                break;                        
                        case 0x4B:     //LD BC,(nn)
                            BC = ReadWordFromMemory(GetNextPCWord());
                            NumberOfTStatesLeft -= 10;
                            break;
                        case 0x5B:  //LD DE,(nn)
                            DE = ReadWordFromMemory(GetNextPCWord());
                            NumberOfTStatesLeft -= 10;
                            break;
                        case 0xB2: //INIR
                            INIR();
                            break;
                        default:
                            //Console.WriteLine("ED" + opcode.ToString());
                            break;

                    }
            }
    }
}
