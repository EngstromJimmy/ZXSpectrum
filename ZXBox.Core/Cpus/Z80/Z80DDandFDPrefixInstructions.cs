namespace Zilog;

/// <summary>
/// Class for handeling DD and FD prefixed instructions.
/// Since the only difference between these two acctually are usage if IX or IY
/// </summary>
public partial class Z80
{
    public enum IndexRegistryEnum
    {
        IX = 0, IY = 1
    }

    ushort ixd;
    int index;
    sbyte dvalue;
    ushort tmp;
    byte tmpValue = 0;
    public void DoDDorFDPrefixInstruction(IndexRegistryEnum IRindex)
    {
        ixd = 0;
        index = (int)IRindex;

        Refresh(1);
        switch (opcode)
        {
            case 0x84:      //ADD A,IXH*
                if (IRindex == IndexRegistryEnum.IX)
                    A = ADDADC8(A, IXH, false, 4);
                else
                    A = ADDADC8(A, IYH, false, 4);
                break;
            case 0x8C:      //ADC A,IXH*
                if (IRindex == IndexRegistryEnum.IX)
                    A = ADDADC8(A, IXH, true, 4);
                else
                    A = ADDADC8(A, IYH, true, 4);
                break;
            case 0x8D:      //ADC A,IXL*
                if (IRindex == IndexRegistryEnum.IX)
                    A = ADDADC8(A, IXL, true, 4);
                else
                    A = ADDADC8(A, IYL, true, 4);
                break;
            case 0x85:      //ADD A,IXL*
                if (IRindex == IndexRegistryEnum.IX)
                    A = ADDADC8(A, IXL, false, 4);
                else
                    A = ADDADC8(A, IYL, false, 4);
                break;
            case 0xA4:      //AND IXYH*
                if (IRindex == IndexRegistryEnum.IX)
                    A = AND8(A, IXH, 4);
                else
                    A = AND8(A, IYH, 4);
                break;
            case 0xA5:      //AND IXYL*
                if (IRindex == IndexRegistryEnum.IX)
                    A = AND8(A, IXL, 4);
                else
                    A = AND8(A, IYL, 4);
                break;
            case 0x2A:      //LD IX,(nn)
                IndexRegistry[index] = ReadWordFromMemory(GetNextPCWord());
                SubtractNumberOfTStatesLeft(20);
                break;
            case 0x22:      //LD (nn),IX
                WriteWordToMemory(GetNextPCWord(), IndexRegistry[index]);
                SubtractNumberOfTStatesLeft(14);
                break;
            case 0x21:      //LD IX,nn
                IndexRegistry[index] = GetNextPCWord();
                SubtractNumberOfTStatesLeft(14);
                break;
            case 0x36:      //LD (IX+d),n
                WriteByteToMemory((ushort)(IndexRegistry[index] + d), GetNextPCByte());
                SubtractNumberOfTStatesLeft(19);
                break;
            case 0x8E:      //ADC A,(IX+d) With PrefixDD
                A = ADDADC8(A, ReadByteFromMemory((ushort)(IndexRegistry[index] + d)), true, 19);
                break;
            // case 0xCE:      //ADC A,n
            //     A = ADDADC8(A, Memory[PC++], true, 7);
            //     break;
            case 0x86:      //ADD A,(IX+d)
                A = ADDADC8(A, ReadByteFromMemory((ushort)(IndexRegistry[index] + d)), false, 19);
                break;
            case 0x09:      //ADD IX,BC	
                IndexRegistry[index] = ADDADC16(IndexRegistry[index], BC, false, 15);
                break;
            case 0x19:      //ADD IX,DE
                IndexRegistry[index] = ADDADC16(IndexRegistry[index], DE, false, 15);
                break;
            case 0x29:      //ADD IX,IX
                IndexRegistry[index] = ADDADC16(IndexRegistry[index], IndexRegistry[index], false, 15);
                break;
            case 0x39:      //ADD IX,SP
                IndexRegistry[index] = ADDADC16(IndexRegistry[index], SP, false, 15);
                break;
            case 0xA6:      //AND (IX+d)
                A = AND8(A, ReadByteFromMemory((ushort)(IndexRegistry[index] + d)), 19);
                break;
            case 0xBE:      //CP (IX+d)
                CP(ReadByteFromMemory((ushort)(IndexRegistry[index] + d)), 19);
                break;
            case 0xBC:      //CP IXH*
                if (IRindex == IndexRegistryEnum.IX)
                    CP(IXH, 7);
                else
                    CP(IYH, 7);
                break;
            case 0xBD:      //CP IXH*
                if (IRindex == IndexRegistryEnum.IX)
                    CP(IXL, 7);
                else
                    CP(IYL, 7);
                break;
            case 0x35:      //DEC (IX+d)
                ixd = (ushort)(IndexRegistry[index] + d);
                WriteByteToMemory(ixd, DEC8(ReadByteFromMemory(ixd), 23));
                break;
            case 0x25:      //DEC IXH*
                if (IRindex == IndexRegistryEnum.IX)
                    IXH = DEC8(IXH, 4); //TODO: Verify tstates
                else
                    IYH = DEC8(IYH, 4); //TODO: Verify tstates
                break;
            case 0x2B:      //DEC IX
                IndexRegistry[index] = DEC16(IndexRegistry[index], 10);
                break;
            case 0x2D:      //DEC IXL
                if (IRindex == IndexRegistryEnum.IX)
                    IXL = DEC8(IXL, 6); //TODO: Verify tstates
                else
                    IYL = DEC8(IYL, 6); //TODO: Verify tstates
                break;
            case 0xE3:      //EX (SP),IX
                tmp = ReadWordFromMemory(SP);
                WriteWordToMemory(SP, IndexRegistry[index]);
                IndexRegistry[index] = tmp;
                SubtractNumberOfTStatesLeft(23);
                break;
            case 0x34:      //INC (IX+d)
                dvalue = d;
                WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), INC8(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 0));
                SubtractNumberOfTStatesLeft(23);
                break;
            case 0x24:      //INC IXH*
                if (IRindex == IndexRegistryEnum.IX)
                    IXH = INC8(IXH, 4);
                else
                    IYH = INC8(IYH, 4);
                break;
            case 0x23:      //INC IX
                IndexRegistry[index] = INC16(IndexRegistry[index], 10);
                break;
            case 0x2C:      //INC IXL*
                if (IRindex == IndexRegistryEnum.IX)
                    IXL = INC8(IXL, 4);
                else
                    IYL = INC8(IYL, 4);
                break;
            case 0xE9:      //JP (IX)
                JP(true, IndexRegistry[index], 8);
                break;
            case 0x77:      //LD (IYX + d),A
            case 0x70:      //LD (IYX + d),B
            case 0x71:      //LD (IYX + d),C
            case 0x72:      //LD (IYX + d),D
            case 0x73:      //LD (IYX + d),E
            case 0x74:      //LD (IYX + d),H
            case 0x75:      //LD (IYX + d),L
                WriteByteToMemory((ushort)(IndexRegistry[index] + d), RegisterValueFromOP(0));
                SubtractNumberOfTStatesLeft(19);
                break;
            case 0x7E:      //LD A,(IX+d)
                A = ReadByteFromMemory((ushort)(IndexRegistry[index] + d));
                SubtractNumberOfTStatesLeft(19);
                break;
            case 0x7C:      //LD A,IXH*
                if (IRindex == IndexRegistryEnum.IX)
                    A = IXH;
                else
                    A = IYH;
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x7D:      //LD A,IXL*
                if (IRindex == IndexRegistryEnum.IX)
                    A = IXL;
                else
                    A = IYL;
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x46:      //LD B,(IX+d)
                B = ReadByteFromMemory((ushort)(IndexRegistry[index] + d));
                SubtractNumberOfTStatesLeft(19);
                break;
            case 0x44:      //LD B,IXH*
                if (IRindex == IndexRegistryEnum.IX)
                    B = IXH;
                else
                    B = IYH;
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x45:      //LD B,IXL*
                if (IRindex == IndexRegistryEnum.IX)
                    B = IXL;
                else
                    B = IYL;
                SubtractNumberOfTStatesLeft(4);
                break;
            //C register
            case 0x4E:      //LD C,(IX+d)
                C = ReadByteFromMemory((ushort)(IndexRegistry[index] + d));
                SubtractNumberOfTStatesLeft(19);
                break;
            case 0x4C:      //LD C,IXH*
                if (IRindex == IndexRegistryEnum.IX)
                    C = IXH;
                else
                    C = IYH;
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x4D:      //LD B,IXL*
                if (IRindex == IndexRegistryEnum.IX)
                    C = IXL;
                else
                    C = IYL;
                SubtractNumberOfTStatesLeft(4);
                break;
            //D Register
            case 0x56:      //LD D,(IX+d)
                D = ReadByteFromMemory((ushort)(IndexRegistry[index] + d));
                SubtractNumberOfTStatesLeft(19);
                break;
            case 0x54:      //LD D,IXH*
                if (IRindex == IndexRegistryEnum.IX)
                    D = IXH;
                else
                    D = IYH;
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x55:      //LD D,IXL*
                if (IRindex == IndexRegistryEnum.IX)
                    D = IXL;
                else
                    D = IYL;
                SubtractNumberOfTStatesLeft(4);
                break;
            //E Register
            case 0x5E:      //LD E,(IX+d)
                E = ReadByteFromMemory((ushort)(IndexRegistry[index] + d));
                SubtractNumberOfTStatesLeft(19);
                break;
            case 0x5C:      //LD D,IXH*
                if (IRindex == IndexRegistryEnum.IX)
                    E = IXH;
                else
                    E = IYH;
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x5D:      //LD E,IXL*
                if (IRindex == IndexRegistryEnum.IX)
                    E = IXL;
                else
                    E = IYL;
                SubtractNumberOfTStatesLeft(4);
                break;
            //H Register
            case 0x66:      //LD H,(IX+d)
                H = ReadByteFromMemory((ushort)(IndexRegistry[index] + d));
                SubtractNumberOfTStatesLeft(19);
                break;
            case 0x67:      //LD IXH,A*   
                if (IRindex == IndexRegistryEnum.IX)
                    IXH = A;
                else
                    IYH = A;
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x60:      //LD IXH,B*
                if (IRindex == IndexRegistryEnum.IX)
                    IXH = B;
                else
                    IYH = B;
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x61:      //LD IXH,C*
                if (IRindex == IndexRegistryEnum.IX)
                    IXH = C;
                else
                    IYH = C;
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x62:      //LD IXH,D*
                if (IRindex == IndexRegistryEnum.IX)
                    IXH = D;
                else
                    IYH = D;
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x63:      //LD IXH,E*
                if (IRindex == IndexRegistryEnum.IX)
                    IXH = E;
                else
                    IYH = E;
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x64:      //LD IXYH,IYXH*
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x65:      //LD IXH,IXL*
                if (IRindex == IndexRegistryEnum.IX)
                    IXH = IXL;
                else
                    IYH = IYL;
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x26:      //LD IXH,n*
                if (IRindex == IndexRegistryEnum.IX)
                    IXH = GetNextPCByte();
                else
                    IYH = GetNextPCByte();
                SubtractNumberOfTStatesLeft(4);
                break;
            //L Register
            case 0x6E:      //LD L,(IX+d)
                L = ReadByteFromMemory((ushort)(IndexRegistry[index] + d));
                SubtractNumberOfTStatesLeft(19);
                break;
            case 0x6F:      //LD IXL,A*
                if (IRindex == IndexRegistryEnum.IX)
                    IXL = A;
                else
                    IYL = A;
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x68:      //LD IXL,B*
                if (IRindex == IndexRegistryEnum.IX)
                    IXL = B;
                else
                    IYL = B;
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x69:      //LD IXL,C*
                if (IRindex == IndexRegistryEnum.IX)
                    IXL = C;
                else
                    IYL = C;
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x6A:      //LD IXL,D*
                if (IRindex == IndexRegistryEnum.IX)
                    IXL = D;
                else
                    IYL = D;
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x6B:      //LD IXL,E*
                if (IRindex == IndexRegistryEnum.IX)
                    IXL = E;
                else
                    IYL = E;
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x6C:      //LD IXL,IXH*
                if (IRindex == IndexRegistryEnum.IX)
                    IXL = IXH;
                else
                    IYL = IYH;
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x6D:      //LD IXL,IXL*                    
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0x2E:      //LD IXL,n*
                if (IRindex == IndexRegistryEnum.IX)
                    IXL = GetNextPCByte();
                else
                    IYL = GetNextPCByte();
                SubtractNumberOfTStatesLeft(4);
                break;
            case 0xF9:      //LD SP,IX
                SP = IndexRegistry[index];
                SubtractNumberOfTStatesLeft(10);
                break;
            case 0xB6:      //OR (IX+d)
                OR(ReadByteFromMemory((ushort)(IndexRegistry[index] + d)), 7);
                break;
            case 0xB4:      //OR IXH*
                if (IRindex == IndexRegistryEnum.IX)
                    OR(IXH, 7);
                else
                    OR(IYH, 7);
                break;
            case 0xB5:      //OR IXL*
                if (IRindex == IndexRegistryEnum.IX)
                    OR(IXL, 7);
                else
                    OR(IYL, 7);
                break;
            case 0xCB:
                dvalue = d;
                tmpValue = 0;
                NextOpcode();
                switch (opcode)
                {
                    case 0x00:  //LD B,RLC (IX+d)*
                        tmpValue = RLC(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        B = tmpValue;
                        break;
                    case 0x01:  //LD C,RLC (IX+d)*
                        tmpValue = RLC(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        C = tmpValue;
                        break;
                    case 0x02:  //LD D,RLC (IX+d)*
                        tmpValue = RLC(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        D = tmpValue;
                        break;
                    case 0x03:  //LD E,RLC (IX+d)*
                        tmpValue = RLC(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        E = tmpValue;
                        break;
                    case 0x04:  //LD H,RLC (IX+d)*
                        tmpValue = RLC(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        H = tmpValue;
                        break;
                    case 0x05:  //LD L,RLC (IX+d)*
                        tmpValue = RLC(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        L = tmpValue;
                        break;
                    case 0x06:  //RLC (IX+d)
                        tmpValue = RLC(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        break;
                    case 0x22:  //LD D,SLA (IX+d)*
                        tmpValue = SLA(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        D = tmpValue;
                        break;
                    case 0x07:  //LD A,RLC (IX+d)*
                        tmpValue = RLC(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        A = tmpValue;
                        break;
                    case 0x08:  //LD B,RRC (IX+d)*
                        tmpValue = RRC(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        B = tmpValue;
                        break;
                    case 0x09:  //LD C,RRC (IX+d)*
                        tmpValue = RRC(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        C = tmpValue;
                        break;
                    case 0x0A:  //LD D,RRC (IX+d)*
                        tmpValue = RRC(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        D = tmpValue;
                        break;
                    case 0x0B:  //LD E,RRC (IX+d)*
                        tmpValue = RRC(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        E = tmpValue;
                        break;
                    case 0x0C:  //LD H,RRC (IX+d)*
                        tmpValue = RRC(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        H = tmpValue;
                        break;
                    case 0x0D:  //LD L,RRC (IX+d)*
                        tmpValue = RRC(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        L = tmpValue;
                        break;
                    case 0x0E:  //RRC (IX+d)
                        tmpValue = RRC(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        break;
                    case 0x0F:  //LD A,RRC (IX+d)*
                        tmpValue = RRC(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        A = tmpValue;
                        break;
                    case 0x10:  //LD B,RL (IX+d)*
                        tmpValue = RL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        B = tmpValue;
                        break;
                    case 0x11:  //LD C,RL (IX+d)*
                        tmpValue = RL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        C = tmpValue;
                        break;
                    case 0x12:  //LD D,RL (IX+d)*
                        tmpValue = RL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        D = tmpValue;
                        break;
                    case 0x13:  //LD E,RL (IX+d)*
                        tmpValue = RL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        E = tmpValue;
                        break;
                    case 0x14:  //LD H,RL (IX+d)*
                        tmpValue = RL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        H = tmpValue;
                        break;
                    case 0x15:  //LD L,RL (IX+d)*
                        tmpValue = RL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        L = tmpValue;
                        break;
                    case 0x16:  //RL (IX+d)
                        tmpValue = RL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        break;
                    case 0x17:  //LD A,RL (IX+d)*
                        tmpValue = RL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        A = tmpValue;
                        break;
                    case 0x18:  //LD B,RR (IX+d)*
                        tmpValue = RR(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        B = tmpValue;
                        break;
                    case 0x19:  //LD C,RR (IX+d)*
                        tmpValue = RR(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        C = tmpValue;
                        break;
                    case 0x1A:  //LD D,RR (IX+d)*
                        tmpValue = RR(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        D = tmpValue;
                        break;
                    case 0x1B:  //LD E,RR (IX+d)*
                        tmpValue = RR(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        E = tmpValue;
                        break;
                    case 0x1C:  //LD H,RR (IX+d)*
                        tmpValue = RR(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        H = tmpValue;
                        break;
                    case 0x1D:  //LD L,RR (IX+d)*
                        tmpValue = RR(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        L = tmpValue;
                        break;
                    case 0x1E:  //RR (IX+d)
                        tmpValue = RR(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        break;
                    case 0x1F:  //LD A,RR (IX+d)*
                        tmpValue = RR(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        A = tmpValue;
                        break;
                    case 0x20:  //LD B,SLA (IX+d)*
                        tmpValue = SLA(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        B = tmpValue;
                        break;
                    case 0x21:  //LD C,SLA (IX+d)*
                        tmpValue = SLA(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        C = tmpValue;
                        break;
                    case 0x23:  //LD E,SLA (IX+d)*
                        tmpValue = SLA(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        E = tmpValue;
                        break;
                    case 0x24:  //LD H,SLA (IX+d)*
                        tmpValue = SLA(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        H = tmpValue;
                        break;
                    case 0x25:  //LD L,SLA (IX+d)*
                        tmpValue = SLA(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        L = tmpValue;
                        break;
                    case 0x26:  //SLA (IX+d)
                        tmpValue = SLA(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        break;
                    case 0x27:  //LD A,SLA (IX+d)*
                        tmpValue = SLA(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        A = tmpValue;
                        break;
                    case 0x28:  //LD B,SRA (IX+d)*
                        tmpValue = SRA(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        B = tmpValue;
                        break;
                    case 0x29:  //LD C,SRA (IX+d)*
                        tmpValue = SRA(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        C = tmpValue;
                        break;
                    case 0x2A:  //LD D,SRA (IX+d)*
                        tmpValue = SRA(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        D = tmpValue;
                        break;
                    case 0x2B:  //LD E,SRA (IX+d)*
                        tmpValue = SRA(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        E = tmpValue;
                        break;
                    case 0x2C:  //LD H,SRA (IX+d)*
                        tmpValue = SRA(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        H = tmpValue;
                        break;
                    case 0x2D:  //LD L,SRA (IX+d)*
                        tmpValue = SRA(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        L = tmpValue;
                        break;
                    case 0x2E:  //SRA (IX+d)
                        tmpValue = SRA(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        break;
                    case 0x2F:  //LD A,SRA (IX+d)*
                        tmpValue = SRA(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        A = tmpValue;
                        break;
                    case 0x30:  //LD B,SLL (IX+d)*
                        tmpValue = SLL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        B = tmpValue;
                        break;
                    case 0x31:  //LD C,SLL (IX+d)*
                        tmpValue = SLL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        C = tmpValue;
                        break;
                    case 0x32:  //LD D,SLL (IX+d)*
                        tmpValue = SLL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        D = tmpValue;
                        break;
                    case 0x33:  //LD E,SLL (IX+d)*
                        tmpValue = SLL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        E = tmpValue;
                        break;
                    case 0x34:  //LD H,SLL (IX+d)*
                        tmpValue = SLL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        H = tmpValue;
                        break;
                    case 0x35:  //LD L,SLL (IX+d)*
                        tmpValue = SLL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        L = tmpValue;
                        break;
                    case 0x36:  //SLL (IX+d)*
                        tmpValue = SLL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        break;
                    case 0x37:  //LD A,SLL (IX+d)*
                        tmpValue = SLL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        A = tmpValue;
                        break;
                    case 0x38:  //LD B,SRL (IX+d)*
                        tmpValue = SRL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        B = tmpValue;
                        break;
                    case 0x39:  //LD C,SRL (IX+d)*
                        tmpValue = SRL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        C = tmpValue;
                        break;
                    case 0x3A:  //LD D,SRL (IX+d)*
                        tmpValue = SRL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        D = tmpValue;
                        break;
                    case 0x3B:  //LD E,SRL (IX+d)*
                        tmpValue = SRL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        E = tmpValue;
                        break;
                    case 0x3C:  //LD H,SRL (IX+d)*
                        tmpValue = SRL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        H = tmpValue;
                        break;
                    case 0x3D:  //LD L,SRL (IX+d)*
                        tmpValue = SRL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        L = tmpValue;
                        break;
                    case 0x3E:  //SRL (IX+d)
                        tmpValue = SRL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        break;
                    case 0x3F:  //LD A,SRL (IX+d)*
                        tmpValue = SRL(ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        A = tmpValue;
                        break;
                    case 0x40:  //BIT 0,(IX+d)*
                    case 0x41:  //BIT 0,(IX+d)*
                    case 0x42:  //BIT 0,(IX+d)*
                    case 0x43:  //BIT 0,(IX+d)*
                    case 0x44:  //BIT 0,(IX+d)*
                    case 0x45:  //BIT 0,(IX+d)*
                    case 0x46:  //BIT 0,(IX+d)
                    case 0x47:  //BIT 0,(IX+d)*
                        BITixyd(0, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), (ushort)(IndexRegistry[index] + dvalue), 20);
                        break;
                    case 0x48:  //BIT 1,(IX+d)*
                    case 0x49:  //BIT 1,(IX+d)*
                    case 0x4A:  //BIT 1,(IX+d)*
                    case 0x4B:  //BIT 1,(IX+d)*
                    case 0x4C:  //BIT 1,(IX+d)*
                    case 0x4D:  //BIT 1,(IX+d)*
                    case 0x4E:  //BIT 1,(IX+d)
                    case 0x4F:  //BIT 1,(IX+d)*
                        BITixyd(1, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), (ushort)(IndexRegistry[index] + dvalue), 20);
                        break;
                    case 0x50:  //BIT 2,(IX+d)*
                    case 0x51:  //BIT 2,(IX+d)*
                    case 0x52:  //BIT 2,(IX+d)*
                    case 0x53:  //BIT 2,(IX+d)*
                    case 0x54:  //BIT 2,(IX+d)*
                    case 0x55:  //BIT 2,(IX+d)*
                    case 0x56:  //BIT 2,(IX+d)
                    case 0x57:  //BIT 2,(IX+d)*
                        BITixyd(2, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), (ushort)(IndexRegistry[index] + dvalue), 20);
                        break;
                    case 0x58:  //BIT 3,(IX+d)*
                    case 0x59:  //BIT 3,(IX+d)*
                    case 0x5A:  //BIT 3,(IX+d)*
                    case 0x5B:  //BIT 3,(IX+d)*
                    case 0x5C:  //BIT 3,(IX+d)*
                    case 0x5D:  //BIT 3,(IX+d)*
                    case 0x5E:  //BIT 3,(IX+d)
                        BITixyd(3, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), (ushort)(IndexRegistry[index] + dvalue), 20);
                        break;
                    case 0x5F:  //BIT 3,(IX+d)*
                    case 0x60:  //BIT 4,(IX+d)*
                    case 0x61:  //BIT 4,(IX+d)*
                    case 0x62:  //BIT 4,(IX+d)*
                    case 0x63:  //BIT 4,(IX+d)*
                    case 0x64:  //BIT 4,(IX+d)*
                    case 0x65:  //BIT 4,(IX+d)*
                    case 0x66:  //BIT 4,(IX+d)
                    case 0x67:  //BIT 4,(IX+d)*
                        BITixyd(4, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), (ushort)(IndexRegistry[index] + dvalue), 20);
                        break;
                    case 0x68:  //BIT 5,(IX+d)*
                    case 0x69:  //BIT 5,(IX+d)*
                    case 0x6A:  //BIT 5,(IX+d)*
                    case 0x6B:  //BIT 5,(IX+d)*
                    case 0x6C:  //BIT 5,(IX+d)*
                    case 0x6D:  //BIT 5,(IX+d)*
                    case 0x6E:  //BIT 5,(IX+d)
                    case 0x6F:  //BIT 5,(IX+d)*
                        BITixyd(5, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), (ushort)(IndexRegistry[index] + dvalue), 20);
                        break;
                    case 0x70:  //BIT 6,(IX+d)*
                    case 0x71:  //BIT 6,(IX+d)*
                    case 0x72:  //BIT 6,(IX+d)*
                    case 0x73:  //BIT 6,(IX+d)*
                    case 0x74:  //BIT 6,(IX+d)*
                    case 0x75:  //BIT 6,(IX+d)*
                    case 0x76:  //BIT 6,(IX+d)
                    case 0x77:  //BIT 6,(IX+d)*
                        BITixyd(6, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), (ushort)(IndexRegistry[index] + dvalue), 20);
                        break;
                    case 0x78:  //BIT 7,(IX+d)*
                    case 0x79:  //BIT 7,(IX+d)*
                    case 0x7A:  //BIT 7,(IX+d)*
                    case 0x7B:  //BIT 7,(IX+d)*
                    case 0x7C:  //BIT 7,(IX+d)*
                    case 0x7D:  //BIT 7,(IX+d)*
                    case 0x7E:  //BIT 7,(IX+d)
                    case 0x7F:  //BIT 7,(IX+d)*
                        BITixyd(7, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), (ushort)(IndexRegistry[index] + dvalue), 20);
                        break;
                    case 0x87:  //LD A,RES 0,(IX+d)*
                    case 0x8F:  //LD A,RES 1,(IX+d)*
                    case 0x97:  //LD A,RES 2,(IX+d)*
                    case 0x9F:  //LD A,RES 3,(IX+d)*
                    case 0xA7:  //LD A,RES 4,(IX+d)*
                    case 0xAF:  //LD A,RES 5,(IX+d)*
                    case 0xB7:  //LD A,RES 6,(IX+d)*
                    case 0xBF:  //LD A,RES 7,(IX+d)*
                        tmpValue = RES(BitValueFromOP, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        A = tmpValue;
                        break;
                    case 0xC7:  //LD A,SET 0,(IX+d)*
                    case 0xCF:  //LD A,SET 1,(IX+d)*
                    case 0xD7:  //LD A,SET 2,(IX+d)*
                    case 0xDF:  //LD A,SET 3,(IX+d)*
                    case 0xE7:  //LD A,SET 4,(IX+d)*
                    case 0xEF:  //LD A,SET 5,(IX+d)*
                    case 0xF7:  //LD A,SET 6,(IX+d)*
                    case 0xFF:  //LD A,SET 7,(IX+d)*
                        tmpValue = SET(BitValueFromOP, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        A = tmpValue;
                        break;
                    case 0x80:  //LD B,RES 0,(IX+d)*
                    case 0x88:  //LD B,RES 1,(IX+d)*
                    case 0x90:  //LD B,RES 2,(IX+d)*
                    case 0x98:  //LD B,RES 3,(IX+d)*
                    case 0xA0:  //LD B,RES 4,(IX+d)*
                    case 0xA8:  //LD B,RES 5,(IX+d)*
                    case 0xB0:  //LD B,RES 6,(IX+d)*
                    case 0xB8:  //LD B,RES 7,(IX+d)*
                        tmpValue = RES(BitValueFromOP, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        B = tmpValue;
                        break;
                    case 0xC0:  //LD B,SET 0,(IX+d)*
                    case 0xC8:  //LD B,SET 1,(IX+d)*
                    case 0xD0:  //LD B,SET 2,(IX+d)*
                    case 0xD8:  //LD B,SET 3,(IX+d)*
                    case 0xE0:  //LD B,SET 4,(IX+d)*
                    case 0xE8:  //LD B,SET 5,(IX+d)*
                    case 0xF0:  //LD B,SET 6,(IX+d)*
                    case 0xF8:  //LD B,SET 7,(IX+d)*
                        tmpValue = SET(BitValueFromOP, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        B = tmpValue;
                        break;
                    case 0x81:  //LD C,RES 0,(IX+d)*
                    case 0x89:  //LD C,RES 1,(IX+d)*
                    case 0x91:  //LD C,RES 2,(IX+d)*
                    case 0x99:  //LD C,RES 3,(IX+d)*
                    case 0xA1:  //LD C,RES 4,(IX+d)*
                    case 0xA9:  //LD C,RES 5,(IX+d)*
                    case 0xB1:  //LD C,RES 6,(IX+d)*
                    case 0xB9:  //LD C,RES 7,(IX+d)*
                        tmpValue = RES(BitValueFromOP, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        C = tmpValue;
                        break;
                    case 0xC1:  //LD C,SET 0,(IX+d)*
                    case 0xC9:  //LD C,SET 1,(IX+d)*
                    case 0xD1:  //LD C,SET 2,(IX+d)*
                    case 0xD9:  //LD C,SET 3,(IX+d)*
                    case 0xE1:  //LD C,SET 4,(IX+d)*
                    case 0xE9:  //LD C,SET 5,(IX+d)*
                    case 0xF1:  //LD C,SET 6,(IX+d)*
                    case 0xF9:  //LD C,SET 7,(IX+d)*
                        tmpValue = SET(BitValueFromOP, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        C = tmpValue;
                        break;
                    case 0x82:  //LD D,RES 0,(IX+d)*
                    case 0x8A:  //LD D,RES 1,(IX+d)*
                    case 0x92:  //LD D,RES 2,(IX+d)*
                    case 0x9A:  //LD D,RES 3,(IX+d)*
                    case 0xA2:  //LD D,RES 4,(IX+d)*
                    case 0xAA:  //LD D,RES 5,(IX+d)*
                    case 0xB2:  //LD D,RES 6,(IX+d)*
                    case 0xBA:  //LD D,RES 7,(IX+d)*
                        tmpValue = RES(BitValueFromOP, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        D = tmpValue;
                        break;
                    case 0xC2:  //LD D,SET 0,(IX+d)*
                    case 0xCA:  //LD D,SET 1,(IX+d)*
                    case 0xD2:  //LD D,SET 2,(IX+d)*
                    case 0xDA:  //LD D,SET 3,(IX+d)*
                    case 0xE2:  //LD D,SET 4,(IX+d)*
                    case 0xEA:  //LD D,SET 5,(IX+d)*
                    case 0xF2:  //LD D,SET 6,(IX+d)*
                    case 0xFA:  //LD D,SET 7,(IX+d)*
                        tmpValue = SET(BitValueFromOP, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        D = tmpValue;
                        break;
                    case 0x83:  //LD E,RES 0,(IX+d)*
                    case 0x8B:  //LD E,RES 1,(IX+d)*
                    case 0x93:  //LD E,RES 2,(IX+d)*
                    case 0x9B:  //LD E,RES 3,(IX+d)*
                    case 0xA3:  //LD E,RES 4,(IX+d)*
                    case 0xAB:  //LD E,RES 5,(IX+d)*
                    case 0xB3:  //LD E,RES 6,(IX+d)*
                    case 0xBB:  //LD E,RES 7,(IX+d)*
                        tmpValue = RES(BitValueFromOP, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        E = tmpValue;
                        break;
                    case 0xC3:  //LD E,SET 0,(IX+d)*
                    case 0xCB:  //LD E,SET 1,(IX+d)*
                    case 0xD3:  //LD E,SET 2,(IX+d)*
                    case 0xDB:  //LD E,SET 3,(IX+d)*
                    case 0xE3:  //LD E,SET 4,(IX+d)*
                    case 0xEB:  //LD E,SET 5,(IX+d)*
                    case 0xF3:  //LD E,SET 6,(IX+d)*
                    case 0xFB:  //LD E,SET 7,(IX+d)*
                        tmpValue = SET(BitValueFromOP, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        E = tmpValue;
                        break;
                    case 0x84:  //LD H,RES 0,(IX+d)*
                    case 0x8C:  //LD H,RES 1,(IX+d)*
                    case 0x94:  //LD H,RES 2,(IX+d)*
                    case 0x9C:  //LD H,RES 3,(IX+d)*
                    case 0xA4:  //LD H,RES 4,(IX+d)*
                    case 0xAC:  //LD H,RES 5,(IX+d)*
                    case 0xB4:  //LD H,RES 6,(IX+d)*
                    case 0xBC:  //LD H,RES 7,(IX+d)*
                        tmpValue = RES(BitValueFromOP, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        H = tmpValue;
                        break;
                    case 0xC4:  //LD H,SET 0,(IX+d)*
                    case 0xCC:  //LD H,SET 1,(IX+d)*
                    case 0xD4:  //LD H,SET 2,(IX+d)*
                    case 0xDC:  //LD H,SET 3,(IX+d)*
                    case 0xE4:  //LD H,SET 4,(IX+d)*
                    case 0xEC:  //LD H,SET 5,(IX+d)*
                    case 0xF4:  //LD H,SET 6,(IX+d)*
                    case 0xFC:  //LD H,SET 7,(IX+d)*
                        tmpValue = SET(BitValueFromOP, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        H = tmpValue;
                        break;
                    case 0x85:  //LD L,RES 0,(IX+d)*
                    case 0x8D:  //LD L,RES 1,(IX+d)*
                    case 0x95:  //LD L,RES 2,(IX+d)*
                    case 0x9D:  //LD L,RES 3,(IX+d)*
                    case 0xA5:  //LD L,RES 4,(IX+d)*
                    case 0xAD:  //LD L,RES 5,(IX+d)*
                    case 0xB5:  //LD L,RES 6,(IX+d)*
                    case 0xBD:  //LD L,RES 7,(IX+d)*
                        tmpValue = RES(BitValueFromOP, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        L = tmpValue;
                        break;
                    case 0xC5:  //LD L,SET 0,(IX+d)*
                    case 0xCD:  //LD L,SET 1,(IX+d)*
                    case 0xD5:  //LD L,SET 2,(IX+d)*
                    case 0xDD:  //LD L,SET 3,(IX+d)*
                    case 0xE5:  //LD L,SET 4,(IX+d)*
                    case 0xED:  //LD L,SET 5,(IX+d)*
                    case 0xF5:  //LD L,SET 6,(IX+d)*
                    case 0xFD:  //LD L,SET 7,(IX+d)*
                        tmpValue = SET(BitValueFromOP, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        L = tmpValue;
                        break;
                    case 0x86:  //RES 0,(IX+d)
                    case 0x8E:  //RES 1,(IX+d)
                    case 0x96:  //RES 2,(IX+d)
                    case 0x9E:  //RES 3,(IX+d)
                    case 0xA6:  //RES 4,(IX+d)
                    case 0xAE:  //RES 5,(IX+d)
                    case 0xB6:  //RES 6,(IX+d)
                    case 0xBE:  //RES 7,(IX+d)
                        tmpValue = RES(BitValueFromOP, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        break;
                    case 0xC6:  //SET 0,(IX+d)
                    case 0xCE:  //SET 1,(IX+d)
                    case 0xD6:  //SET 2,(IX+d)
                    case 0xDE:  //SET 3,(IX+d)
                    case 0xE6:  //SET 4,(IX+d)
                    case 0xEE:  //SET 5,(IX+d)
                    case 0xF6:  //SET 6,(IX+d)
                    case 0xFE:  //SET 7,(IX+d)
                        tmpValue = SET(BitValueFromOP, ReadByteFromMemory((ushort)(IndexRegistry[index] + dvalue)), 23);
                        WriteByteToMemory((ushort)(IndexRegistry[index] + dvalue), tmpValue);
                        break;
                }
                break;
            case 0xE1:      //POP IX
                IndexRegistry[index] = POP();
                SubtractNumberOfTStatesLeft(14);
                break;
            case 0xE5:      //PUSH IX
                PUSH(IndexRegistry[index]);
                SubtractNumberOfTStatesLeft(15);
                break;
            case 0x9E:      //SBC A,(IX+d)
                A = SBC8(ReadByteFromMemory((ushort)(IndexRegistry[index] + d)), 19);
                break;
            case 0x9C:      //SBC A,IXH*
                if (IRindex == IndexRegistryEnum.IX)
                    A = SBC8(IXH, 4);
                else
                    A = SBC8(IYH, 4);
                break;
            case 0x9D:      //SBC A,IXL*
                if (IRindex == IndexRegistryEnum.IX)
                    A = SBC8(IXL, 4);
                else
                    A = SBC8(IYL, 4);
                break;
            case 0x96:      //SUB (IX+d)
                SUB(ReadByteFromMemory((ushort)(IndexRegistry[index] + d)), 7);
                break;
            case 0x94:      //SUB IXH*
                if (IRindex == IndexRegistryEnum.IX)
                    SUB(IXH, 4);
                else
                    SUB(IYH, 4);
                break;
            case 0x95:      //SUB IXL*
                if (IRindex == IndexRegistryEnum.IX)
                    SUB(IXL, 4);
                else
                    SUB(IYL, 4);
                break;
            case 0xAE:      //XOR (IX+d)
                XOR(ReadByteFromMemory((ushort)(IndexRegistry[index] + d)), 19);
                break;
            case 0xAC:      //XOR IXH*
                if (IRindex == IndexRegistryEnum.IX)
                    XOR(IXH, 4);
                else
                    XOR(IYH, 4);
                break;
            case 0xAD:      //XOR IXL*
                if (IRindex == IndexRegistryEnum.IX)
                    XOR(IXL, 19);
                else
                    XOR(IYL, 19);
                break;
            default:
                //Console.WriteLine("DDFD" + opcode.ToString());
                break;
        }
    }
}