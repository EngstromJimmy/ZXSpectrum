namespace Zilog;

public partial class Z80
{
    public void DoCBPrefixInstruction()
    {

        //TODO: Check i values are read from the right places
        Refresh(1);
        switch (opcode)
        {
            case 0x36:		//SLL (HL)*
                WriteByteToMemory(HL, SLL(ReadByteFromMemory(HL), 12));
                break;
            case 0x37:		//SLL A*
                A = SLL(A, 8);
                break;
            case 0x30:		//SLL B*
                B = SLL(B, 8);
                break;
            case 0x31:		//SLL C*
                C = SLL(C, 8);
                break;
            case 0x32:		//SLL D*
                D = SLL(D, 8);
                break;
            case 0x33:		//SLL E*
                E = SLL(E, 8);
                break;
            case 0x34:		//SLL H*
                H = SLL(H, 8);
                break;
            case 0x35:		//SLL L*
                L = SLL(L, 8);
                break;
            case 0x46:          //BIT 0,(HL)
            case 0x4E:          //BIT 1,(HL)
            case 0x56:          //BIT 2,(HL)
            case 0x5E:          //BIT 3,(HL)
            case 0x66:          //BIT 4,(HL)
            case 0x6E:          //BIT 5,(HL)
            case 0x76:          //BIT 6,(HL)
            case 0x7E:          //BIT 7,(HL)
                BIT(BitValueFromOP, ReadByteFromMemory(HL), 12);
                break;
            case 0x47:	 	    //BIT 0,A
            case 0x40:		    //BIT 0,B
            case 0x41:		    //BIT 0,C
            case 0x42:		    //BIT 0,D
            case 0x43:		    //BIT 0,E
            case 0x44:	 	    //BIT 0,H
            case 0x45:	 	    //BIT 0,L
            case 0x4F:	 	    //BIT 1,A
            case 0x48:		    //BIT 1,B
            case 0x49:	 	    //BIT 1,C
            case 0x4A:	 	    //BIT 1,D
            case 0x4B:	 	    //BIT 1,E
            case 0x4C:	 	    //BIT 1,H
            case 0x4D:	 	    //BIT 1,L
            case 0x57:	 	    //BIT 2,A
            case 0x50:		    //BIT 2,B
            case 0x51:	 	    //BIT 2,C
            case 0x52:	 	    //BIT 2,D
            case 0x53:	 	    //BIT 2,E
            case 0x54:	    	//BIT 2,H
            case 0x55:  	 	//BIT 2,L
            case 0x5F:	     	//BIT 3,A
            case 0x58:		    //BIT 3,B
            case 0x59:  	 	//BIT 3,C
            case 0x5A:	     	//BIT 3,D
            case 0x5B:	 	    //BIT 3,E
            case 0x5C:  	 	//BIT 3,H
            case 0x5D:	     	//BIT 3,L
            case 0x67:	 	    //BIT 4,A
            case 0x60:  		//BIT 4,B
            case 0x61:	     	//BIT 4,C
            case 0x62:	 	    //BIT 4,D
            case 0x63:	 	    //BIT 4,E
            case 0x64:  	 	//BIT 4,H
            case 0x65:	     	//BIT 4,L
            case 0x6F:	 	    //BIT 5,A
            case 0x68:  		//BIT 5,B
            case 0x69:	     	//BIT 5,C
            case 0x6A:	 	    //BIT 5,D
            case 0x6B:  	 	//BIT 5,E
            case 0x6C:	     	//BIT 5,H
            case 0x6D:	 	    //BIT 5,L
            case 0x77:  	 	//BIT 6,A
            case 0x70:	    	//BIT 6,B
            case 0x71:	 	    //BIT 6,C
            case 0x72:  	 	//BIT 6,D
            case 0x73:	     	//BIT 6,E
            case 0x74:	 	    //BIT 6,H
            case 0x75:  	 	//BIT 6,L
            case 0x7F:	     	//BIT 7,A
            case 0x78:		    //BIT 7,B
            case 0x79:  	 	//BIT 7,C
            case 0x7A:	     	//BIT 7,D
            case 0x7B:	 	    //BIT 7,E
            case 0x7C:  	 	//BIT 7,H
            case 0x7D:	     	//BIT 7,L
                BIT(BitValueFromOP, RegisterValueFromOP(0), 8);
                break;
            case 0x86:     //RES 0,(HL)
            case 0x8E: //RES 1,(HL)
            case 0x96: //RES 2,(HL)
            case 0x9E: //RES 3,(HL)
            case 0xA6: //RES 4,(HL)
            case 0xAE: //RES 5,(HL)
            case 0xB6: //RES 6,(HL)
            case 0xBE: //RES 7,(HL)
                WriteByteToMemory(HL, RES(BitValueFromOP, ReadByteFromMemory(HL), 15));
                break;
            case 0x87:      //RES 0,A
            case 0x8F: //RES 1,A
            case 0x97: //RES 2,A
            case 0x9F: //RES 3,A
            case 0xA7: //RES 4,A
            case 0xAF: //RES 5,A
            case 0xB7: //RES 6,A
            case 0xBF: //RES 7,A
                A = RES(BitValueFromOP, A, 8);
                break;
            case 0x80:      //RES 0,B
            case 0x88:      //RES 1,B
            case 0x90:      //RES 2,B
            case 0x98:      //RES 3,B
            case 0xA0:      //RES 4,B
            case 0xA8:      //RES 5,B
            case 0xB0:      //RES 6,B
            case 0xB8:      //RES 7,B
                B = RES(BitValueFromOP, B, 8);
                break;
            case 0x81:      //RES 0,C
            case 0x89:      //RES 1,C
            case 0x91:      //RES 2,C
            case 0x99:      //RES 3,C
            case 0xA1:      //RES 4,C
            case 0xA9:      //RES 5,C
            case 0xB1:      //RES 6,C
            case 0xB9:      //RES 7,C
                C = RES(BitValueFromOP, C, 8);
                break;
            case 0x82:      //RES 0,D
            case 0x8A:      //RES 1,D
            case 0x92:      //RES 2,D
            case 0x9A:      //RES 3,D
            case 0xA2:      //RES 4,D
            case 0xAA:      //RES 5,D
            case 0xB2:      //RES 6,D
            case 0xBA:      //RES 7,D
                D = RES(BitValueFromOP, D, 8);
                break;
            case 0x83:      //RES 0,E
            case 0x8B:      //RES 1,E
            case 0x93:      //RES 2,E
            case 0x9B:      //RES 3,E
            case 0xA3:      //RES 4,E
            case 0xAB:      //RES 5,E
            case 0xB3:      //RES 6,E
            case 0xBB:      //RES 7,E
                E = RES(BitValueFromOP, E, 8);
                break;
            case 0x84:      //RES 0,H
            case 0x8C:      //RES 1,H
            case 0x94:      //RES 2,H
            case 0x9C:      //RES 3,H
            case 0xA4:      //RES 4,H
            case 0xAC:      //RES 5,H
            case 0xB4:      //RES 6,H
            case 0xBC:      //RES 7,H
                H = RES(BitValueFromOP, H, 8);
                break;
            case 0x85:      //RES 0,L
            case 0x8D:      //RES 1,L
            case 0x95:      //RES 2,L
            case 0x9D:      //RES 3,L
            case 0xA5:      //RES 4,L
            case 0xAD:      //RES 5,L
            case 0xB5:      //RES 6,L
            case 0xBD:      //RES 7,L
                L = RES(BitValueFromOP, L, 8);
                break;
            case 0x16:		//RL (HL)
                WriteByteToMemory(HL, RL(ReadByteFromMemory(HL), 15));
                break;
            case 0x17:		//RL A
                A = RL(A, 8);
                break;
            case 0x10:		//RL B
                B = RL(B, 8);
                break;
            case 0x11:		//RL C
                C = RL(C, 8);
                break;
            case 0x12:		//RL D
                D = RL(D, 8);
                break;
            case 0x13:		//RL E
                E = RL(E, 8);
                break;
            case 0x14:		//RL H
                H = RL(H, 8);
                break;
            case 0x15:		//RL L
                L = RL(L, 8);
                break;
            case 0x06:		//RLC (HL)
                WriteByteToMemory(HL, RLC(ReadByteFromMemory(HL), 15));
                break;
            case 0x07:		//RLC A
                A = RLC(A, 8);
                break;
            case 0x00:		//RLC B
                B = RLC(B, 8);
                break;
            case 0x01:		//RLC C
                C = RLC(C, 8);
                break;
            case 0x02:		//RLC D
                D = RLC(D, 8);
                break;
            case 0x03:		//RLC E
                E = RLC(E, 8);
                break;
            case 0x04:		//RLC H
                H = RLC(H, 8);
                break;
            case 0x05:		//RLC L
                L = RLC(L, 8);
                break;
            case 0x1E:		//RR (HL)
                WriteByteToMemory(HL, RR(ReadByteFromMemory(HL), 15));
                break;
            case 0x1F:		//RR A
                A = RR(A, 8);
                break;
            case 0x18:		//RR B
                B = RR(B, 8);
                break;
            case 0x19:		//RR C
                C = RR(C, 8);
                break;
            case 0x1A:		//RR D
                D = RR(D, 8);
                break;
            case 0x1B:		//RR E
                E = RR(E, 8);
                break;
            case 0x1C:		//RR H
                H = RR(H, 8);
                break;
            case 0x1D:		//RR L
                L = RR(L, 8);
                break;
            case 0x0E:		//RRC (HL)
                WriteByteToMemory(HL, RRC(ReadByteFromMemory(HL), 15));
                break;
            case 0x0F:		//RRC A
                A = RRC(A, 8);
                break;
            case 0x08:		//RRC B
                B = RRC(B, 8);
                break;
            case 0x09:		//RRC C
                C = RRC(C, 8);
                break;
            case 0x0A:		//RRC D
                D = RRC(D, 8);
                break;
            case 0x0B:		//RRC E
                E = RRC(E, 8);
                break;
            case 0x0C:		//RRC H
                H = RRC(H, 8);
                break;
            case 0x0D:		//RRC L
                L = RRC(L, 8);
                break;

            case 0xC6:     // SET 0,(HL)
            case 0xCE:     //SET 1,(HL)
            case 0xD6:     //SET 2,(HL)
            case 0xDE:     //SET 3,(HL)
            case 0xE6:     //SET 4,(HL)
            case 0xEE:     //SET 5,(HL)
            case 0xF6:     //SET 6,(HL)
            case 0xFE:     //SET 7,(HL)
                WriteByteToMemory(HL, SET(BitValueFromOP, ReadByteFromMemory(HL), 15));
                break;
            case 0xC7:     //SET 0,A
            case 0xCF:     //SET 1,A
            case 0xD7:     //SET 2,A
            case 0xDF:     //SET 3,A
            case 0xE7:     //SET 4,A
            case 0xEF:     //SET 5,A
            case 0xF7:     //SET 6,A
            case 0xFF:     //SET 7,A
                A = SET(BitValueFromOP, A, 8);
                break;

            case 0xC0:		//SET 0,B
            case 0xC8:		//SET 1,B
            case 0xD0:		//SET 2,B
            case 0xD8:		//SET 3,B
            case 0xE0:		//SET 4,B
            case 0xE8:		//SET 5,B
            case 0xF0:		//SET 6,B
            case 0xF8:		//SET 7,B
                B = SET(BitValueFromOP, B, 8);
                break;

            case 0xC1:		//SET 0,C
            case 0xC9:     //SET 1,C
            case 0xD1:     //SET 2,C
            case 0xD9:     //SET 3,C
            case 0xE1:     //SET 4,C
            case 0xE9:     //SET 5,C
            case 0xF1:     //SET 6,C
            case 0xF9:     //SET 7,C
                C = SET(BitValueFromOP, C, 8);
                break;
            case 0xC2:		//SET 0,D
            case 0xCA:     //SET 1,D
            case 0xD2:     //SET 2,D
            case 0xDA:     //SET 3,D'
            case 0xE2:     //SET 4,D
            case 0xEA:     //SET 5,D
            case 0xF2:     //SET 6,D
            case 0xFA:     //SET 7,D
                D = SET(BitValueFromOP, D, 8);
                break;
            case 0xC3:		//SET 0,E
            case 0xCB:     //SET 1,E
            case 0xD3:     //SET 2,E
            case 0xDB:     //SET 3,E
            case 0xE3:     //SET 4,E
            case 0xEB:     //SET 5,E
            case 0xF3:     //SET 6,E
            case 0xFB:     //SET 7,E
                E = SET(BitValueFromOP, E, 8);
                break;
            case 0xC4:     //SET 0,H
            case 0xCC:     //SET 1,H
            case 0xD4:     //SET 2,H
            case 0xDC:     //SET 3,H
            case 0xE4:     //SET 4,H
            case 0xEC:     //SET 5,H
            case 0xF4:     //SET 6,H
            case 0xFC:     //SET 7,H
                H = SET(BitValueFromOP, H, 8);
                break;
            case 0xC5:     //SET 0,L
            case 0xCD:     //SET 1,L
            case 0xD5:     //SET 2,L
            case 0xDD:     //SET 3,L
            case 0xE5:     //SET 4,L
            case 0xED:     //SET 5,L
            case 0xF5:     //SET 6,L
            case 0xFD:     //SET 7,L
                L = SET(BitValueFromOP, L, 8);
                break;
            case 0x26:		//SLA (HL)
                WriteByteToMemory(HL, SLA(ReadByteFromMemory(HL), 15));
                break;
            case 0x27:		//SLA A
                A = SLA(A, 8); break;
            case 0x20:		//SLA B
                B = SLA(B, 8); break;
            case 0x21:		//SLA C
                C = SLA(C, 8); break;
            case 0x22:		//SLA D
                D = SLA(D, 8); break;
            case 0x23:		//SLA E
                E = SLA(E, 8); break;
            case 0x24:		//SLA H
                H = SLA(H, 8); break;
            case 0x25:		//SLA L
                L = SLA(L, 8); break;
            case 0x2E:     //SRA (HL)
                WriteByteToMemory(HL, SRA(ReadByteFromMemory(HL), 15)); break;
            case 0x2F:     //SRA A
                A = SRA(A, 8); break;
            case 0x28:     //SRA B
                B = SRA(B, 8); break;
            case 0x29:     //SRA C
                C = SRA(C, 8); break;
            case 0x2A:     //SRA D
                D = SRA(D, 8); break;
            case 0x2B:     //SRA E
                E = SRA(E, 8); break;
            case 0x2C:     //SRA H
                H = SRA(H, 8); break;
            case 0x2D:     //SRA L
                L = SRA(L, 8); break;
            case 0x3E:     //SRL (HL)
                WriteByteToMemory(HL, SRL(ReadByteFromMemory(HL), 15)); break;
            case 0x3F:     //SRL A
                A = SRL(A, 8); break;
            case 0x38:     //SRL B
                B = SRL(B, 8); break;
            case 0x39:     //SRL C
                C = SRL(C, 8); break;
            case 0x3A:     //SRL D
                D = SRL(D, 8); break;
            case 0x3B:     //SRL E
                E = SRL(E, 8); break;
            case 0x3C:     //SRL H
                H = SRL(H, 8); break;
            case 0x3D:     //SRL L
                L = SRL(L, 8); break;
            default:
                //Console.WriteLine("CB" + opcode.ToString());
                break;

        }
    }
}
