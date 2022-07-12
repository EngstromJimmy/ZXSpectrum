using System;

namespace Zilog;

public partial class Z80
{

    int tmpw;
    int tmpAF;
    int tmpDE;
    int tmphaltsToInterrupt;
    int tmpport;

    public void DoNoPrefixInstruction()
    {
        switch (opcode)
        {
            case 0x2A:          //LD HL,(nn)
                HL = ReadWordFromMemory(GetNextPCWord());
                NumberOfTStatesLeft -= 20;
                break;
            case 0x21:  		//LD HL,nn
                HL = GetNextPCWord();
                NumberOfTStatesLeft -= 10;
                break;
            //Adc A, r
            case 0x8F:		//ADC A,A
            case 0x88:		//ADC A,B
            case 0x89:		//ADC A,C
            case 0x8A:		//ADC A,D
            case 0x8B:		//ADC A,E
            case 0x8C:		//ADC A,H
            case 0x8D:		//ADC A,L
                A = ADDADC8(A, RegisterValueFromOP(0), true, 4); break;
            case 0x8E:  //ADC A,(HL)
                A = ADDADC8(A, ReadByteFromMemory(HL), true, 7);
                break;
            case 0xCE:  //ADC A,n
                A = ADDADC8(A, GetNextPCByte(), true, 7); break;
            //Add a,ss
            case 0x86://ADD A,(HL)
                A = ADDADC8(A, ReadByteFromMemory(HL), false, 7);
                break;
            case 0x87:		//ADD A,A     
            case 0x80:		//ADD A,B     
            case 0x81:		//ADD A,C
            case 0x82:		//ADD A,D
            case 0x83:		//ADD A,E
            case 0x84:		//ADD A,H
            case 0x85:		//ADD A,L
                A = ADDADC8(A, RegisterValueFromOP(0), false, 4);
                break;
            case 0xC6:		//ADD A,n
                A = ADDADC8(A, GetNextPCByte(), false, 7);
                break;
            case 0x09://ADD HL,BC
                HL = ADDADC16(HL, BC, false, 11);
                break;
            case 0x19://ADD HL,DE
                HL = ADDADC16(HL, DE, false, 11);
                break;
            case 0x29:   //ADD HL,HL
                HL = ADDADC16(HL, HL, false, 11);
                break;
            case 0x39://ADD HL,SP
                HL = ADDADC16(HL, SP, false, 11);
                break;
            case 0xA6://AND (HL) 
                A = AND8(A, ReadByteFromMemory(HL), 7);
                break;
            case 0xA7:       //AND A
            case 0xA0:       //AND B
            case 0xA1:       //AND C
            case 0xA2:       //AND D
            case 0xA3:       //AND E
            case 0xA4:       //AND H
            case 0xA5:       //AND L
                A = AND8(A, RegisterValueFromOP(0), 4);
                break;
            case 0xE6:      //AND n
                A = AND8(A, GetNextPCByte(), 12);
                break;
            case 0xCD:   //CALL (nn)
                CALLnn();
                break;
            case 0xDC:  		//CALL C,(nn)
                CALL(fC);
                break;
            case 0xFC:  		//CALL M,(nn)
                CALL(fS);
                break;
            case 0xD4:  		//CALL NC,(nn)
                CALL(!fC);
                break;
            case 0xC4:  		//CALL NZ,(nn)
                CALL(!fZ);
                break;
            case 0xF4:      	//CALL P,(nn)
                CALL(!fS);
                break;
            case 0xEC:  		//CALL PE,(nn)
                CALL(fPV);
                break;
            case 0xE4:  		//CALL PO,(nn)
                CALL(!fPV);
                break;
            case 0xCC:  		//CALL Z,(nn)
                CALL(fZ);
                break;
            case 0x3F:          //CCF
                CCF();
                break;
            case 0xBE: //CP (HL)
                CP(ReadByteFromMemory(HL), 7);
                break;
            case 0xBF:		//CP A
            case 0xB8:		//CP B
            case 0xB9:		//CP C
            case 0xBA:		//CP D
            case 0xBB:		//CP E
                CP(RegisterValueFromOP(0), 4);
                break;
            //TODO: Check number of tstates for undocumented function
            case 0xBC: //CP H
                CP(H, 4);
                break;
            case 0xBD: //CP H
                CP(L, 4);
                break;
            case 0xFE: 		//CP n
                CP(GetNextPCByte(), 7);
                break;
            case 0x2F:      //CPL
                CPL();
                break;
            case 0x27:      //DAA
                DAA();
                break;
            case 0x35:  //DEC(HL)
                WriteByteToMemory(HL, DEC8(ReadByteFromMemory(HL), 11));
                break;
            case 0x3D:		//DEC A
                A = DEC8(A, 4); break;
            case 0x05:		//DEC B
                B = DEC8(B, 4); break;
            case 0x0B:		//DEC BC
                BC = DEC16(BC, 6); break;
            case 0x0D:		//DEC C
                C = DEC8(C, 4); break;
            case 0x15:		//DEC D
                D = DEC8(D, 4); break;
            case 0x1B:		//DEC DE
                DE = DEC16(DE, 6); break;
            case 0x1D:		//DEC E
                E = DEC8(E, 6); break;
            case 0x25:      //DEC H
                H = DEC8(H, 6); break;
            case 0x2B: //DEC HL
                HL = DEC16(HL, 6);
                break;
            case 0x2D: //DEC L
                L = DEC8(L, 6);
                break;
            case 0x3B:		//DEC SP
                SP = DEC16(SP, 6);
                break;
            case 0xF3:      //DI
                IFF = IFF2 = false;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x10:      //DNJZ
                DNJZ();
                break;
            case 0xFB:      //EI
                IFF = IFF2 = true;
                NumberOfTStatesLeft -= 4;
                break;
            case 0xE3:		//EX (SP),HL
                tmpw = ReadWordFromMemory(SP);
                WriteWordToMemory(SP, HL);
                HL = tmpw;
                NumberOfTStatesLeft -= 19;
                break;
            case 0x08:
                tmpAF = AF;
                AF = AFPrim;
                AFPrim = tmpAF;
                NumberOfTStatesLeft -= 4;
                break;
            case 0xEB:      //EX DE,HL
                tmpDE = DE;
                DE = HL;
                HL = tmpDE;
                NumberOfTStatesLeft -= 4;
                break;
            case 0xD9:      //EXX
                EXX();
                break;
            case 0x76:      //Halt
                Halt();
                break;
            case 0xDB:     //IN A,(n)

                tmpport = (A << 8) | GetNextPCByte();
                A = In(tmpport);
                NumberOfTStatesLeft -= 11;
                break;
            case 0x34:  //INC (HL)
                WriteByteToMemory(HL, INC8(ReadByteFromMemory(HL), 0));
                NumberOfTStatesLeft -= 11;
                break;
            case 0x3C:		    //INC A
                A = INC8(A, 4); break;
            case 0x04:		    //INC B
                B = INC8(B, 4); break;
            case 0x03:		    //INC BC
                BC = INC16(BC, 6); break;
            case 0x0C:		    //INC C
                C = INC8(C, 4); break;
            case 0x14:		    //INC D
                D = INC8(D, 4); break;
            case 0x13:		    //INC DE
                DE = INC16(DE, 6); break;
            case 0x1C:		    //INC E
                E = INC8(E, 4); break;
            case 0x24:          //INC H
                H = INC8(H, 4);
                break;
            case 0x23://INC HL
                HL = INC16(HL, 6);
                break;

            case 0x2C: //INC L
                L = INC8(L, 4);
                break;
            case 0x33:		    //INC SP
                SP = INC16(SP, 6); break;
            case 0xE9:  //JP (HL)
                JP(true, HL, 4);
                break;
            case 0xC3:          //JP (nn)
                JP(true, GetNextPCWord(), 10); break;
            case 0xDA:		    //JP C,(nn)
                JP(fC, GetNextPCWord(), 10); break;
            case 0xFA:  		//JP M,(nn)
                JP(fS, GetNextPCWord(), 10); break;
            case 0xD2:		    //JP NC,(nn)
                JP(!fC, GetNextPCWord(), 10); break;
            case 0xC2:  		//JP NZ,(nn)
                JP(!fZ, GetNextPCWord(), 10); break;
            case 0xF2:  		//JP P,(nn)
                JP(!fS, GetNextPCWord(), 10); break;
            case 0xEA:		    //JP PE,(nn)
                JP(fPV, GetNextPCWord(), 10); break;
            case 0xE2:  		//JP PO,(nn)
                JP(!fPV, GetNextPCWord(), 10); break;
            case 0xCA:		    //JP Z,(nn)
                JP(fZ, GetNextPCWord(), 10); break;
            case 0x18:      //JR (PC+e)
                JR(true, GetNextPCByte(), 12); break;
            case 0x38:		//JR C,(PC+e)
                JR(fC, GetNextPCByte(), fC ? 12 : 7); break;
            case 0x30:		//JR NC,(PC+e)
                JR(!fC, GetNextPCByte(), !fC ? 12 : 7); break;
            case 0x20:		//JR NZ,(PC+e)
                JR(!fZ, GetNextPCByte(), !fZ ? 12 : 7); break;
            case 0x28:		//JR Z,(PC+e)
                JR(fZ, GetNextPCByte(), fZ ? 12 : 7); break;
            case 0x02:		//LD (BC),A
                WriteByteToMemory(BC, A); NumberOfTStatesLeft -= 7; break;
            case 0x12:		//LD (DE),A
                WriteByteToMemory(DE, A); NumberOfTStatesLeft -= 7; break;
            case 0x77:		//LD (HL),A
            case 0x70:		//LD (HL),B
            case 0x71:		//LD (HL),C
            case 0x72:		//LD (HL),D
            case 0x73:		//LD (HL),E
            case 0x74:		//LD (HL),H
            case 0x75:      //LD (HL),L
                WriteByteToMemory(HL, RegisterValueFromOP(0)); NumberOfTStatesLeft -= 7; break;
            case 0x36:  		//LD (HL),n
                WriteByteToMemory(HL, GetNextPCByte()); NumberOfTStatesLeft -= 10; break;
            case 0x32:          //LD (nn),A
                WriteByteToMemory(GetNextPCWord(), A); NumberOfTStatesLeft -= 13; break;
            case 0x22: 		    //LD (nn),HL
                WriteWordToMemory(GetNextPCWord(), HL);
                NumberOfTStatesLeft -= 20;
                break;
            case 0x0A:		//LD A,(BC)
                A = ReadByteFromMemory(BC);
                NumberOfTStatesLeft -= 7;
                break;
            case 0x1A:		//LD A,(DE)
                A = ReadByteFromMemory(DE);
                NumberOfTStatesLeft -= 7;
                break;
            case 0x7E://LD A,(HL)
                A = ReadByteFromMemory(HL);
                NumberOfTStatesLeft -= 7;
                break;
            case 0x3A:		//LD A,(nn)
                A = ReadByteFromMemory(GetNextPCWord());
                NumberOfTStatesLeft -= 13;
                break;
            case 0x7F:		//LD A,A
            case 0x78:		//LD A,B
            case 0x79:		//LD A,C
            case 0x7A:		//LD A,D
            case 0x7B:		//LD A,E
                A = RegisterValueFromOP(0);
                NumberOfTStatesLeft -= 4;
                break;
            case 0x7C:  //LD A,H
                A = H;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x7D: //LD A,L
                A = L;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x3E:      //LD A,n
                A = GetNextPCByte();
                NumberOfTStatesLeft -= 7;
                break;
            case 0x46:  //LD B,(HL)
                B = ReadByteFromMemory(HL);
                NumberOfTStatesLeft -= 7;
                break;
            case 0x47:		//LD B,A
            case 0x40:		//LD B,B
            case 0x41:		//LD B,C
            case 0x42:		//LD B,D
            case 0x43:		//LD B,E
                B = RegisterValueFromOP(0);
                NumberOfTStatesLeft -= 4;
                break;
            case 0x44:      //LD B,H
                B = H;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x45://LD B,L
                B = L;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x06:             //LD B,n
                B = GetNextPCByte();
                NumberOfTStatesLeft -= 7;
                break;
            case 0x01:		//LD BC,nn
                BC = GetNextPCWord();
                NumberOfTStatesLeft -= 10;
                break;
            case 0x4E:      //LD C,(HL)
                C = ReadByteFromMemory(HL);
                NumberOfTStatesLeft -= 7;
                break;
            case 0x4F:		//LD C,A
            case 0x48:		//LD C,B
            case 0x49:		//LD C,C
            case 0x4A:		//LD C,D
            case 0x4B:		//LD C,E
                C = RegisterValueFromOP(0);
                NumberOfTStatesLeft -= 4;
                break;
            case 0x4C:      //LD C,H
                C = H;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x4D://LD C,L
                C = L;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x0E:             //LD C,n
                C = GetNextPCByte();
                NumberOfTStatesLeft -= 7;
                break;
            case 0x56:      //LD D,(HL)
                D = ReadByteFromMemory(HL);
                NumberOfTStatesLeft -= 7;
                break;
            case 0x57:		//LD D,A
            case 0x50:		//LD D,B
            case 0x51:		//LD D,C
            case 0x52:		//LD D,D
            case 0x53:		//LD D,E
                D = RegisterValueFromOP(0);
                NumberOfTStatesLeft -= 4;
                break;
            case 0x54:      //LD D,H
                D = H;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x55: //LD D,L
                D = L;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x16:             //LD D,n
                D = GetNextPCByte();
                NumberOfTStatesLeft -= 7;
                break;
            case 0x11:                 //LD DE,nn
                DE = GetNextPCWord();
                NumberOfTStatesLeft -= 7;
                break;
            case 0x5E:      //LD E,(HL)
                E = ReadByteFromMemory(HL);
                NumberOfTStatesLeft -= 7;
                break;
            case 0x5F:		//LD E,A
            case 0x58:		//LD E,B
            case 0x59:		//LD E,C
            case 0x5A:		//LD E,D
            case 0x5B:		//LD E,E
                E = RegisterValueFromOP(0);
                NumberOfTStatesLeft -= 4;
                break;
            case 0x5C:      //LD E,H
                E = H;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x5D://LD E,L
                E = L;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x1E:             //LD E,n
                E = GetNextPCByte();
                NumberOfTStatesLeft -= 7;
                break;
            case 0x66://LD H,(HL)
                H = ReadByteFromMemory(HL);
                NumberOfTStatesLeft -= 7;
                break;
            case 0x67: //LD H,A
                H = A;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x60:      //LD H,B
                H = B;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x61:      //LD H,C
                H = C;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x62:      //LD H,D
                H = D;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x63:      //LD H,E
                H = E;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x64:  //LD H,H
                H = H;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x65://LD H,L
                H = L;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x26: //LD H,n
                H = GetNextPCByte();
                NumberOfTStatesLeft -= 7;
                break;
            case 0x6E:      //LD L,(HL)
                L = ReadByteFromMemory(HL);
                NumberOfTStatesLeft -= 7;
                break;
            case 0x6F://LD L,A
                L = A;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x68:         //LD L,B
                L = B;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x69:         //LD L,C
                L = C;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x6A:      //LD L,D
                L = D;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x6B:      //LD L,E
                L = E;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x6C:      //LD L,H
                L = H;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x6D://LD L,L
                L = L;
                NumberOfTStatesLeft -= 4;
                break;
            case 0x2E: //LD L ,n
                L = GetNextPCByte();
                NumberOfTStatesLeft -= 7;
                break;
            case 0xF9: //LD SP HL
                SP = HL;
                NumberOfTStatesLeft -= 6;
                break;
            case 0x31:     //LD SP,nn
                SP = GetNextPCWord();
                NumberOfTStatesLeft -= 10;
                break;
            case 0x00:     //Nop
                NOP(); break;
            case 0xB6:  //OR (HL)
                OR(ReadByteFromMemory(HL), 7);
                break;
            case 0xB7:		//OR A
            case 0xB0:		//OR B
            case 0xB1:		//OR C
            case 0xB2:		//OR D
            case 0xB3:		//OR E
                OR(RegisterValueFromOP(0), 4); break;
            case 0xB4://OR H
                OR(H, 4);
                break;
            case 0xB5://OR L
                OR(L, 4);
                break;
            case 0xF6:		//OR n
                OR(GetNextPCByte(), 7); break;
            case 0xD3:    //OUT (n),A
                NumberOfTStatesLeft -= 7;
                Out(GetNextPCByte(), A, NumberOfTstates - Math.Abs(NumberOfTStatesLeft));
                NumberOfTStatesLeft -= 4;
                break;
            case 0xF1:		//POP AF
                AF = POP();
                NumberOfTStatesLeft -= 10;
                break;
            case 0xC1:		//POP BC
                BC = POP();
                NumberOfTStatesLeft -= 10;
                break;
            case 0xD1:		//POP DE
                DE = POP();
                NumberOfTStatesLeft -= 10;
                break;
            case 0xE1:		//POP HL
                HL = POP();
                NumberOfTStatesLeft -= 10;
                break;
            case 0xF5:		//PUSH AF
                PUSH(AF);
                NumberOfTStatesLeft -= 11;
                break;
            case 0xC5:		//PUSH BC
                PUSH(BC);
                NumberOfTStatesLeft -= 11;
                break;
            case 0xD5:		//PUSH DE
                PUSH(DE);
                NumberOfTStatesLeft -= 11;
                break;
            case 0xE5:		//PUSH HL
                PUSH(HL);
                NumberOfTStatesLeft -= 11;
                break;
            case 0xC9: //RET
                RET(true, 10, 0);
                break;
            case 0xD8:		//RET C                    
                RET(fC, 10, 5);
                break;
            case 0xF8:		//RET M
                RET(fS, 11, 5);
                break;
            case 0xD0:		//RET NC
                RET(!fC, 11, 5);
                break;
            case 0xC0:		//RET NZ
                RET(!fZ, 11, 5);
                break;
            case 0xF0:		//RET P
                RET(!fS, 11, 5);
                break;
            case 0xE8:		//RET PE
                RET(fPV, 11, 5);
                break;
            case 0xE0:		//RET PO
                RET(!fPV, 11, 5);
                break;
            case 0xC8:		//RET Z
                RET(fZ, 11, 5);
                break;
            case 0x17:     //RLA
                RLA();
                break;
            case 0x07:     //RLCA
                RLCA();
                break;
            case 0x1F:     //RRA
                RRA();
                break;
            case 0x0F:       //RRCA
                RRCA();
                break;
            case 0xC7:		//RST 0H
                RST(0x00); break;
            case 0xCF:		//RST 8H
                RST(0x08); break;
            case 0xD7:		//RST 10H
                RST(0x10); break;
            case 0xDF:		//RST 18H
                RST(0x18); break;
            case 0xE7:     //RST 20H
                RST(0x20); break;
            case 0xEF:		//RST 28H
                RST(0x28); break;
            case 0xF7:		//RST 30H
                RST(0x30); break;
            case 0xFF:		//RST 38H
                RST(0x38); break;
            case 0x9E:     //SBC A,(HL)
                A = SBC8(ReadByteFromMemory(HL), 7);
                break;
            case 0x9F:         //SBC A,A
            case 0x98:		    //SBC A,B
            case 0x99:		    //SBC A,C
            case 0x9A:		    //SBC A,D
            case 0x9B:		    //SBC A,E
            case 0x9C:		    //SBC A,H
            case 0x9D:		    //SBC A,L
                A = SBC8(RegisterValueFromOP(0), 4);
                break;
            case 0xDE://SBC a,n
                A = SBC8(GetNextPCByte(), 7);
                break;
            case 0x37:         //SCF
                fC = true;
                fN = false;
                fH = false;
                f3 = ((A & F_3) != 0);
                f5 = ((A & F_5) != 0);

                NumberOfTStatesLeft -= 4;
                break;
            case 0x96:     //SUB (HL)
                SUB(ReadByteFromMemory(HL), 7);
                break;
            case 0x97:		//SUB A
            case 0x90:		//SUB B
            case 0x91:		//SUB C
            case 0x92:		//SUB D
            case 0x93:		//SUB E
            case 0x94:		//SUB H
            case 0x95:		//SUB L
                SUB(RegisterValueFromOP(0), 4);
                break;
            case 0xD6:
                SUB(GetNextPCByte(), 7);
                break;
            case 0xAE:		//XOR (HL)
                XOR(ReadByteFromMemory(HL), 7);
                break;
            case 0xAF:		//XOR A
            case 0xA8:		//XOR B
            case 0xA9:		//XOR C
            case 0xAA:	//XOR D
            case 0xAB:		//XOR E
            case 0xAC:	//XOR H
            case 0xAD:	//XOR L
                XOR(RegisterValueFromOP(0), 4);
                break;
            case 0xEE:      //XOR n
                XOR(GetNextPCByte(), 7);
                break;
            default:
                //Console.WriteLine("NO" + opcode.ToString());
                break;
        }
        //if (opcode!=0)
        //Console.WriteLine("just 0x" +  opcode.ToString("x"));
    }
}
