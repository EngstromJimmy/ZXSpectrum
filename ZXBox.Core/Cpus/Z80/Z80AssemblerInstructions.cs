using System;

namespace Zilog;

/// <summary>
/// This partial class contains all the instructions that are carried out by the op codes.
/// </summary>
public partial class Z80
{

    private int[] bitArray = { 1, 2, 4, 8, 16, 32, 64, 128 };

    #region Instructions
    //Assembler commands
    public void Refresh()
    { }

    public bool interruptTriggered(int tstates)
    {
        return tstates <= 0;
    }

    public void PUSH(ushort word)
    {
        var pushsp = (ushort)(SP - 2 & 0xffff);
        SP = pushsp;
        WriteWordToMemory(pushsp, word);
    }

    private byte RES(int bit, byte value, int tstates)
    {
        SubtractNumberOfTStatesLeft(tstates);
        return (byte)(value & ~bitArray[bit]);
    }

    public byte RL(byte value, int tstates)
    {
        var rlc = (value & 0x80) != 0;

        if (fC)
        {
            value = (byte)((value << 1) | 0x01);
        }
        else
        {
            value <<= 1;
        }
        value &= 0xff;

        fS = (value & F_S) != 0;
        f3 = (value & F_3) != 0;
        f5 = (value & F_5) != 0;
        fZ = value == 0;
        fPV = Parity[value];
        fH = false;
        fN = false;
        fC = rlc;

        SubtractNumberOfTStatesLeft(tstates);
        return value;
    }

    public void RLCA()
    {
        var rlcavalue = A;
        bool c = (rlcavalue & 0x80) != 0;

        if (c)
        {
            rlcavalue = (byte)((rlcavalue << 1) | 0x01);
        }
        else
        {
            rlcavalue <<= 1;
        }
        rlcavalue &= 0xff;

        f3 = (rlcavalue & F_3) != 0;
        f5 = (rlcavalue & F_5) != 0;
        fN = false;
        fH = false;
        fC = c;
        SubtractNumberOfTStatesLeft(4);
        A = rlcavalue;
    }

    private byte SBC8(byte b, int tstates)
    {
        var sbc8a = A;
        var sbc8c = fC ? 1 : 0;
        var value = sbc8a - b - sbc8c;
        var sbc8truncated = (byte)(value & 0xff);

        fS = (sbc8truncated & F_S) != 0;
        f3 = (sbc8truncated & F_3) != 0;
        f5 = (sbc8truncated & F_5) != 0;
        fZ = sbc8truncated == 0;
        fC = (value & 0x100) != 0;
        fPV = ((sbc8a ^ b) & (sbc8a ^ sbc8truncated) & 0x80) != 0;
        fH = (((sbc8a & 0x0f) - (b & 0x0f) - sbc8c) & F_H) != 0;
        fN = true;

        SubtractNumberOfTStatesLeft(tstates);

        return sbc8truncated;
    }

    private ushort SBC16(ushort a, ushort b, int tstates)
    {
        var sbc16c = fC ? 1 : 0;
        var sbc16value = (a - b - sbc16c);
        var sbc16truncated = (ushort)(sbc16value & 0xffff);

        fS = (sbc16truncated & (F_S << 8)) != 0;
        f3 = (sbc16truncated & (F_3 << 8)) != 0;
        f5 = (sbc16truncated & (F_5 << 8)) != 0;
        fZ = sbc16truncated == 0;
        fC = (sbc16value & 0x10000) != 0; // TODO: Does this even make sense?
        fPV = ((a ^ b) & (a ^ sbc16truncated) & 0x8000) != 0;
        fH = (((a & 0x0fff) - (b & 0x0fff) - sbc16c) & 0x1000) != 0;
        fN = true;

        SubtractNumberOfTStatesLeft(tstates);
        return sbc16truncated;
    }

    public byte RLC(byte value, int tstates)
    {
        var rlcc = (value & 0x80) != 0;

        if (rlcc)
        {
            value = (byte)((value << 1) | 0x01);
        }
        else
        {
            value <<= 1;
        }
        value &= 0xff;

        fS = (value & F_S) != 0;
        f3 = (value & F_3) != 0;
        f5 = (value & F_5) != 0;
        fZ = value == 0;
        fPV = Parity[value];
        fH = false;
        fN = false;
        fC = rlcc;

        SubtractNumberOfTStatesLeft(tstates);
        return value;
    }

    public void RLA()
    {
        var rlavalue = A;
        var rlac = (rlavalue & 0x80) != 0;

        if (fC)
        {
            rlavalue = (byte)((rlavalue << 1) | 0x01);
        }
        else
        {
            rlavalue <<= 1;
        }

        rlavalue &= 0xff;

        f3 = (rlavalue & F_3) != 0;
        f5 = (rlavalue & F_5) != 0;
        fN = false;
        fH = false;
        fC = rlac;
        SubtractNumberOfTStatesLeft(4);
        A = rlavalue;
    }

    public byte RR(byte value, int tstates)
    {
        var rrc = (value & 0x01) != 0;

        if (fC)
        {
            value = (byte)((value >> 1) | 0x80);
        }
        else
        {
            value >>= 1;
        }

        fS = (value & F_S) != 0;
        f3 = (value & F_3) != 0;
        f5 = (value & F_5) != 0;
        fZ = value == 0;
        fPV = Parity[value];
        fH = false;
        fN = false;
        fC = rrc;

        SubtractNumberOfTStatesLeft(tstates);
        return value;
    }

    public void Halt()
    {
        tmphaltsToInterrupt = ((_numberOfTStatesLeft - 1) / 4) + 1;
        SubtractNumberOfTStatesLeft(tmphaltsToInterrupt * 4);
        Refresh(tmphaltsToInterrupt - 1);
    }

    public void RST(ushort position)
    {
        PUSH(PC);
        PC = position;
        SubtractNumberOfTStatesLeft(11);
    }

    public void RRD()
    {
        var rrdvalue = A;
        var rrdm = ReadByteFromMemory(HL);
        var rrdq = rrdm;

        rrdm = (byte)((rrdm >> 4) | (rrdvalue << 4));
        rrdvalue = (byte)((rrdvalue & 0xf0) | (rrdq & 0x0f));
        WriteByteToMemory(HL, rrdm);

        fS = (rrdvalue & F_S) != 0;
        f3 = (rrdvalue & F_3) != 0;
        f5 = (rrdvalue & F_5) != 0;
        fZ = rrdvalue == 0;
        fPV = Parity[rrdvalue];
        fH = false;
        fN = false;
        SubtractNumberOfTStatesLeft(18);
        A = rrdvalue;
    }

    public void RRCA()
    {
        var rrcavalue = A;
        var rrcac = (rrcavalue & 0x01) != 0;

        if (rrcac)
        {
            rrcavalue = (byte)((rrcavalue >> 1) | 0x80);
        }
        else
        {
            rrcavalue >>= 1;
        }

        f3 = (rrcavalue & F_3) != 0;
        f5 = (rrcavalue & F_5) != 0;
        fN = false;
        fH = false;
        fC = rrcac;

        SubtractNumberOfTStatesLeft(4);
        A = rrcavalue;
    }

    public byte RRC(byte value, int tstates)
    {
        var rrcc = (value & 0x01) != 0;

        if (rrcc)
        {
            value = (byte)((value >> 1) | 0x80);
        }
        else
        {
            value >>= 1;
        }

        fS = (value & F_S) != 0;
        f3 = (value & F_3) != 0;
        f5 = (value & F_5) != 0;
        fZ = value == 0;
        fPV = Parity[value];
        fH = false;
        fN = false;
        fC = rrcc;

        SubtractNumberOfTStatesLeft(tstates);
        return (byte)value;
    }

    public void RRA()
    {
        var rravalue = A;
        var rrac = (rravalue & 0x01) != 0;

        if (fC)
        {
            rravalue = (byte)((rravalue >> 1) | 0x80);
        }
        else
        {
            rravalue >>= 1;
        }

        f3 = (rravalue & F_3) != 0;
        f5 = (rravalue & F_5) != 0;
        fN = false;
        fH = false;
        fC = rrac;

        SubtractNumberOfTStatesLeft(4);
        A = (byte)rravalue;
    }
    
    public void RLD()
    {
        var rldvalue = A;
        var rldm = ReadByteFromMemory(HL);
        var rldq = rldm;

        rldm = (byte)((rldm << 4) | (rldvalue & 0x0f));
        rldvalue = (byte)((rldvalue & 0xf0) | (rldq >> 4));
        WriteByteToMemory(HL, (byte)(rldm & 0xff));

        fS = (rldvalue & F_S) != 0;
        f3 = (rldvalue & F_3) != 0;
        f5 = (rldvalue & F_5) != 0;
        fZ = rldvalue == 0;
        fPV = Parity[rldvalue];
        fH = false;
        fN = false;

        SubtractNumberOfTStatesLeft(18);
        A = (byte)rldvalue;
    }

    public byte SRA(int value, int tstates)
    {
        var srac = (value & 0x01) != 0;
        value = (value >> 1) | (value & 0x80);

        fS = (value & F_S) != 0;
        f3 = (value & F_3) != 0;
        f5 = (value & F_5) != 0;
        fZ = value == 0;
        fPV = Parity[value];
        fH = false;
        fN = false;
        fC = srac;

        SubtractNumberOfTStatesLeft(tstates);
        return (byte)value;
    }

    public void XOR(int value, int tstates)
    {
        var xornewvalue = (A ^ value) & 0xff;

        fS = (xornewvalue & F_S) != 0;
        f3 = (xornewvalue & F_3) != 0;
        f5 = (xornewvalue & F_5) != 0;
        fH = false;
        fPV = Parity[xornewvalue];
        fZ = xornewvalue == 0;
        fN = false;
        fC = false;

        SubtractNumberOfTStatesLeft(tstates);

        A = (byte)xornewvalue;
    }

    private byte SRL(int value, int tstates)
    {
        var srlc = (value & 0x01) != 0;
        value = value >> 1;

        fS = (value & F_S) != 0;
        f3 = (value & F_3) != 0;
        f5 = (value & F_5) != 0;
        fZ = value == 0;
        fPV = Parity[value];
        fH = false;
        fN = false;
        fC = srlc;
        SubtractNumberOfTStatesLeft(tstates);

        return (byte)value;
    }

    public byte SLA(int value, int tstates)
    {
        var slac = (value & 0x80) != 0;
        value = (value << 1) & 0xff;

        fS = (value & F_S) != 0;
        f3 = (value & F_3) != 0;
        f5 = (value & F_5) != 0;
        fZ = value == 0;
        fPV = Parity[value];
        fH = false;
        fN = false;
        fC = slac;
        SubtractNumberOfTStatesLeft(tstates);

        return (byte)value;
    }

    private byte SET(int bit, int value, int tstates)
    {
        SubtractNumberOfTStatesLeft(tstates);
        return (byte)(value | bitArray[bit]);
    }

    public void RET(bool condition, int tstates, int notmettstates)
    {
        if (condition)
        {
            PC = POP();
            SubtractNumberOfTStatesLeft(tstates);
        }
        else
        {
            SubtractNumberOfTStatesLeft(notmettstates);
        }
    }

    public ushort POP()
    {
        var popsp = SP;
        ushort popt = ReadByteFromMemory(popsp);
        popsp++;
        popt |= (ushort)(ReadByteFromMemory((ushort)(popsp & 0xffff)) << 8);
        SP = (ushort)(++popsp & 0xffff);
        return popt;
    }

    public void OUTI()
    {
        B = DEC8(B, 0);

        SubtractNumberOfTStatesLeft(9);
        Out(BC, ReadByteFromMemory(HL), NumberOfTstates - Math.Abs(_numberOfTStatesLeft));
        HL = INC16(HL, 0);

        fZ = B == 0;
        fN = true;
        if (ReadByteFromMemory(HL) + L > 255)
        {
            fH = true;
            fC = true;
        }
        else
        {
            fH = false;
            fC = false;
        }
        fPV = Parity[((ReadByteFromMemory(HL) + L) & 7) ^ B];

        SubtractNumberOfTStatesLeft(7);
    }

    public void OUTD()
    {
        B = DEC8(B, 0);

        var outdvalue = ReadByteFromMemory(HL);
        SubtractNumberOfTStatesLeft(9);
        Out(BC, outdvalue, NumberOfTstates - Math.Abs(_numberOfTStatesLeft));
        HL = DEC16(HL, 0);

        fZ = B == 0;
        fN = (outdvalue >> 7 & 0x01) != 1;
        if ((outdvalue + L) > 255)
        {
            fH = true;
            fC = true;
        }
        else
        {
            fH = false;
            fC = false;
        }
        fPV = Parity[((outdvalue + L) & 7) ^ B];
        SubtractNumberOfTStatesLeft(7);
    }

    public void OTIR()
    {
        var otirvalue = ReadByteFromMemory(HL);
        SubtractNumberOfTStatesLeft(9);
        B = DEC8(B, 0);
        Out(BC, otirvalue, NumberOfTstates - Math.Abs(_numberOfTStatesLeft));
        HL = INC16(HL, 0);

        fN = ((otirvalue >> 7) & 0x01) == 1;
        if ((otirvalue + L) > 255)
        {
            fH = true;
            fC = true;
        }
        else
        {
            fH = true;
            fC = true;
        }
        fPV = Parity[((otirvalue + L) & 7) ^ B];

        if (B != 0)
        {
            PC = (ushort)((PC - 2) & 0xffff);
            SubtractNumberOfTStatesLeft(12);
        }
        else
        {
            SubtractNumberOfTStatesLeft(7);
        }

    }

    public void OTDR()
    {
        B = DEC8(B, 0);
        SubtractNumberOfTStatesLeft(9);
        Out(BC, ReadByteFromMemory(HL), NumberOfTstates - Math.Abs(_numberOfTStatesLeft));
        HL = DEC16(HL, 0);

        fZ = true;
        fZ = true;
        if (B != 0)
        {
            PC = (ushort)((PC - 2) & 0xffff);
            SubtractNumberOfTStatesLeft(12);
        }
        else
        {
            SubtractNumberOfTStatesLeft(7);
        }
    }

    public void OR(int b, int tstates)
    {
        var orvalue = A | b;

        fS = (orvalue & F_S) != 0;
        f3 = (orvalue & F_3) != 0;
        f5 = (orvalue & F_5) != 0;
        fH = false;
        fPV = Parity[orvalue];
        fZ = orvalue == 0;
        fN = false;
        fC = false;
        SubtractNumberOfTStatesLeft(tstates);
        A = (byte)orvalue;
    }

    public void NOP()
    {
        SubtractNumberOfTStatesLeft(4);
    }

    public void NEG()
    {
        var negtmp = A;

        A = 0;
        SUB(negtmp, 0);
        SubtractNumberOfTStatesLeft(8);
    }

    public void LDD()
    {
        var lddmemval = ReadByteFromMemory(HL);
        WriteByteToMemory(DE, (byte)lddmemval);
        DE = DEC16(DE, 0);
        HL = DEC16(HL, 0);
        BC = DEC16(BC, 0);

        fPV = BC != 0;
        fH = false;
        fN = false;

        var lddn = lddmemval + A;

        f5 = (lddn & 0x01) == 1;
        f3 = (lddn >> 3 & 0x01) == 1;

        SubtractNumberOfTStatesLeft(16);
    }

    public void LDDR()
    {  //TODO:fix this
        var lddr_local_tstates = 0;

        var lddrcount = BC;
        var lddrdest = DE;
        var lddrfrom = HL;
        Refresh(-2);
        do
        {
            WriteByteToMemory(lddrdest, ReadByteFromMemory(lddrfrom));
            lddrfrom = DEC16(lddrfrom, 0);
            lddrdest = DEC16(lddrdest, 0);
            lddrcount = DEC16(lddrcount, 0);

            lddr_local_tstates += 21;
            Refresh(2);
            if (interruptTriggered(lddr_local_tstates))
            {
                break;
            }
        }
        while (lddrcount != 0);

        if (lddrcount != 0)
        {
            PC = (ushort)((PC - 2) & 0xffff);
            fH = false;
            fN = false;
            fPV = true;
        }
        else
        {
            lddr_local_tstates += -5;
            fH = false;
            fN = false;
            fPV = false;
        }
        DE = (ushort)lddrdest;
        HL = (ushort)lddrfrom;
        BC = (ushort)lddrcount;
        SubtractNumberOfTStatesLeft(lddr_local_tstates);
    }

    public void LDI()
    {
        int ldimemval = ReadByteFromMemory(HL);
        WriteByteToMemory(DE, (byte)ldimemval);
        DE = INC16(DE, 0);
        HL = INC16(HL, 0);
        BC = DEC16(BC, 0);

        int n = ldimemval + A;

        fPV = BC != 0;
        fH = false;
        fN = false;
        f5 = (n & 0x01) == 1;
        f3 = (n >> 3 & 0x01) == 1;

        SubtractNumberOfTStatesLeft(16);
    }

    public void LDIR()
    {
        var ldir_local_tstates = 0;

        var ldircount = BC;
        var ldirdest = DE;
        var ldirfrom = HL;
        Refresh(-2);

        do
        {
            WriteByteToMemory(ldirdest, ReadByteFromMemory(ldirfrom));
            //Memory[ldirdest] = Memory[ldirfrom];
            ldirfrom = INC16(ldirfrom, 0);
            ldirdest = INC16(ldirdest, 0);
            ldircount = DEC16(ldircount, 0);

            ldir_local_tstates += 21;
            Refresh(2);
            if (interruptTriggered(_numberOfTStatesLeft - ldir_local_tstates))
            {
                break;
            }
        } while (ldircount != 0);

        if (ldircount != 0)
        {
            PC = (ushort)((PC - 2) & 0xffff);
            fH = false;
            fN = false;
            fPV = true;
        }
        else
        {
            ldir_local_tstates += -5;
            fH = false;
            fN = false;
            fPV = false;
        }
        DE = ldirdest;
        HL = ldirfrom;
        BC = ldircount;

        SubtractNumberOfTStatesLeft(ldir_local_tstates);
    }

    private void LDAR()
    {
        var ldarvalue = R;

        fS = (ldarvalue & F_S) != 0;
        f3 = (ldarvalue & F_3) != 0;
        f5 = (ldarvalue & F_5) != 0;
        fZ = ldarvalue == 0;
        fPV = IFF2;
        fH = false;
        fN = false;

        SubtractNumberOfTStatesLeft(9);
        A = ldarvalue;
    }
    
    public void LDAI()
    {
        var ldaivalue = I;

        fS = (ldaivalue & F_S) != 0;
        f3 = (ldaivalue & F_3) != 0;
        f5 = (ldaivalue & F_5) != 0;
        fZ = ldaivalue == 0;
        fPV = IFF2;
        fH = false;
        fN = false;

        SubtractNumberOfTStatesLeft(9);

        A = ldaivalue;
    }
    public void JP(bool argument, int position, int tstates)
    {
        if (argument)
            PC = (ushort)position;
        SubtractNumberOfTStatesLeft(tstates);
    }

    private sbyte Sign(byte nn)
    {
        return (sbyte)(nn);// - ((nn & 128) << 1));
    }

    public void JR(bool argument, int position, int tstates)
    {
        if (argument)
        {
            PC = (ushort)((PC + Sign((byte)position)) & 0xFFFF);
        }
        SubtractNumberOfTStatesLeft(tstates);
    }

    public byte INBC(int tstates)
    {
        var inbcvalue = (byte)In(BC);

        SubtractNumberOfTStatesLeft(tstates);

        fZ = inbcvalue == 0;
        fS = (inbcvalue & F_S) != 0;
        f3 = (inbcvalue & F_3) != 0;
        f5 = (inbcvalue & F_5) != 0;
        fPV = Parity[inbcvalue];
        fN = false;
        fH = false;

        return inbcvalue;
    }

    public void INDR()
    {
        IND(0);

        if (B != 0)  //If B is not zero Do instruction again
        {
            PC = (ushort)(PC - 2);
            SubtractNumberOfTStatesLeft(21);
        }
        else
            SubtractNumberOfTStatesLeft(16);
    }

    public void IND(int tstates)
    {
        var indb = DEC8(B, 0);
        WriteByteToMemory(HL, (byte)In(BC));
        B = (byte)indb;
        HL = DEC16(HL, 0);

        fZ = indb == 0;
        fN = true;
        if (ReadByteFromMemory(HL) + ((C - 1) & 255) > 255)
        {
            fC = true;
            fH = true;
        }
        else
        {
            fC = false;
            fH = false;
        }
        fPV = Parity[((ReadByteFromMemory(HL) + ((C - 1) & 255)) & 7) ^ B];
        SubtractNumberOfTStatesLeft(tstates);
    }

    public void INI(int tstates)
    {
        var inib = DEC8(B, 0);
        var inival = (byte)In(BC);
        WriteByteToMemory(HL, (byte)inival);
        B = inib;
        HL = INC16(HL, 0);

        fZ = inib == 0;
        fN = true;
        if (inival + ((C + 1) & 255) > 255)
        {
            fC = true;
            fH = true;
        }
        else
        {
            fC = false;
            fH = false;
        }
        fPV = Parity[((inival + ((C + 1) & 255)) & 7) ^ B];
        SubtractNumberOfTStatesLeft(tstates);
    }

    public void INIR()
    {
        INI(0);
        if (B != 0)
        {
            SubtractNumberOfTStatesLeft(21);
            PC = (ushort)(PC - 2);
        }
        else
        {
            SubtractNumberOfTStatesLeft(16);
        }

    }

    public byte ADDADC8(byte a, byte b, bool Carry, int tStates)
    {
        var addadc8c = 0;
        if (Carry)
            addadc8c = fC ? 1 : 0; //Add 1 if carry is set

        var addadc8newvalue = a + b + addadc8c;
        var addadc8truncated = (byte)(addadc8newvalue & 0xff);

        //Set flags
        fS = (addadc8truncated & F_S) != 0;
        f3 = (addadc8truncated & F_3) != 0;
        f5 = (addadc8truncated & F_5) != 0;
        fZ = addadc8truncated == 0;
        fC = (addadc8newvalue & 0x100) != 0;
        fPV = ((a ^ ~b) & (a ^ addadc8truncated) & 0x80) != 0;
        fH = (((a & 0x0f) + (b & 0x0f) + addadc8c) & F_H) != 0;
        fN = false;

        SubtractNumberOfTStatesLeft(tStates);

        return addadc8truncated;
    }

    private ushort ADDADC16(ushort a, ushort b, bool Carry, int tStates)
    {
        var addadc16c = fC && Carry ? 1 : 0;
        var addadc16added = a + b + addadc16c;
        var addadc16truncated = (ushort)(addadc16added & 0xffff);

        f3 = (addadc16truncated & (F_3 << 8)) != 0;
        f5 = (addadc16truncated & (F_5 << 8)) != 0;
        fC = (addadc16added & 0x10000) != 0;
        fH = (((a & 0x0fff) + (b & 0x0fff) + addadc16c) & 0x1000) != 0;
        fN = false;
        if (Carry)
        {
            fS = (addadc16truncated & (F_S << 8)) != 0;
            fPV = ((a ^ ~b) & (a ^ addadc16truncated) & 0x8000) != 0;
            fZ = addadc16truncated == 0;
        }
        SubtractNumberOfTStatesLeft(tStates);
        return addadc16truncated;
    }

    public byte AND8(byte a, byte b, int tStates)
    {
        var and8newvalue = a & b;

        fS = (and8newvalue & F_S) != 0;
        f3 = (and8newvalue & F_3) != 0;
        f5 = (and8newvalue & F_5) != 0;
        fH = true;
        fPV = Parity[and8newvalue];
        fZ = and8newvalue == 0;
        fN = false;
        fC = false;

        SubtractNumberOfTStatesLeft(tStates);
        return (byte)and8newvalue;
    }

    public void BIT(int bit, int regvalue, int tStates)
    {
        SubtractNumberOfTStatesLeft(tStates);
        F = (byte)((F & F_C) |
                    F_H |
                    (regvalue & (F_3 | F_5)) |
                    ((regvalue & (0x01 << bit)) != 0 ? 0 : (F_PV | F_Z)));
    }

    public void BITixyd(int bit, int regvalue, int ixyd, int tStates)
    {
        var bitixydbitIsSet = (regvalue & bitArray[bit]) != 0;

        fN = false;
        fH = true;
        f3 = (ixyd >> 11 & 0x01) == 1;
        f5 = (ixyd >> 13 & 0x01) == 1;
        fS = (bit == 7) && bitixydbitIsSet;
        fZ = !bitixydbitIsSet;
        fPV = !bitixydbitIsSet;
        SubtractNumberOfTStatesLeft(tStates);
    }

    public void CALLnn()
    {
        var callnnw = GetNextPCWord();
        PCToStack();
        PC = callnnw;
        SubtractNumberOfTStatesLeft(17);
    }

    private byte SLL(byte value, int tstates)
    {
        var sllc = (value & 0x80) >> 7;
        value = (byte)(((value << 1) | 1) & 0xff);

        fS = (value & F_S) != 0;
        f3 = (value & F_3) != 0;
        f5 = (value & F_5) != 0;
        fZ = value == 0;
        fPV = Parity[value];
        fH = false;
        fN = false;
        fC = sllc == 1;
        SubtractNumberOfTStatesLeft(tstates);
        return value;
    }

    /// <summary>
    /// Compare operand s to ackumulator
    /// </summary>
    /// <param name="s">Operand</param>
    /// <param name="tStates">Number of tstates</param>
    private void CP(byte value, int tstates)
    {
        var cpa = A;
        var cpwvalue = cpa - value;
        var cpnewvalue = (byte)(cpwvalue & 0xff);

        fS = (cpnewvalue & F_S) != 0;
        f3 = (value & F_3) != 0;
        f5 = (value & F_5) != 0;
        fN = true;
        fZ = cpnewvalue == 0;
        fC = (cpwvalue & 0x100) != 0;
        fH = (((cpa & 0x0f) - (value & 0x0f)) & F_H) != 0;
        fPV = ((cpa ^ value) & (cpa ^ cpnewvalue) & 0x80) != 0;

        SubtractNumberOfTStatesLeft(tstates);
    }

    public void CP2(byte s, int tStates)
    {
        SubtractNumberOfTStatesLeft(tStates);
        var cp2sub = A - s;
        var cp2truncated = (byte)(cp2sub & 0xff);

        fS = (cp2truncated & F_S) != 0;
        f3 = (s & F_3) != 0;
        f5 = (s & F_5) != 0;
        fN = true;
        fZ = cp2truncated == 0;
        fC = (cp2sub & 0x100) != 0;
        fH = (((A & 0x0f) - (s & 0x0f)) & F_H) != 0;
        fPV = ((A ^ s) & (A ^ cp2truncated) & 0x80) != 0;
    }

    public void CALL(bool argument)
    {
        if (argument)
        {
            var callw = GetNextPCWord();
            PCToStack();
            PC = callw;
            SubtractNumberOfTStatesLeft(17);
        }
        else
        {
            PC = (ushort)((PC + 2) & 0xffff);
            SubtractNumberOfTStatesLeft(10);
        }
    }

    /// <summary>
    /// Compement Carry flag
    /// </summary>
    public void CCF()
    {
        f3 = (A & F_3) != 0;
        f5 = (A & F_5) != 0;
        fN = false;
        fC = !fC;
        SubtractNumberOfTStatesLeft(4);
    }

    public void CPD()
    {
        var cpdc = fC;

        CP(ReadByteFromMemory(HL), 0);
        HL = DEC16(HL, 0);
        BC = DEC16(BC, 0);

        fPV = BC != 0;
        fC = cpdc;
        //---------------
        var cpdn = (byte)(A - ReadByteFromMemory(HL) - (fH ? 1 : 0));
        var cpdpv = BC != 0;

        fN = true;
        fC = cpdc;
        f5 = (cpdn & 0x01) == 1;
        f3 = (cpdn >> 3 & 0x01) == 1;
        //-----------------------

        SubtractNumberOfTStatesLeft(16);
    }

    public void CPI()
    {
        var cpic = fC;
        var cpimemvalue = ReadByteFromMemory(HL);
        CP(cpimemvalue, 0);
        HL = INC16(HL, 0);
        BC = DEC16(BC, 0);
        var cpin = A - cpimemvalue - (fH ? 1 : 0);
        f5 = (cpin & 0x01) == 1;
        f3 = (cpin >> 3 & 0x01) == 1;
        fPV = BC != 0;
        fC = cpic;
        SubtractNumberOfTStatesLeft(16);
    }

    public void CPIR()
    {
        var cpirc = fC;
        var cpirvalue = ReadByteFromMemory(HL);
        CP(cpirvalue, 0);

        HL = INC16(HL, 0);
        BC = DEC16(BC, 0);

        var cpirn = A - cpirvalue - (fH ? 1 : 0);
        var cpirpv = BC != 0;

        fN = true;
        fPV = cpirpv;
        fC = cpirc;
        f5 = (cpirn & 0x01) == 1;
        f3 = (cpirn >> 3 & 0x01) == 1;

        if (BC != 0 && A != cpirvalue)
        {   //Repeat until BC ==0
            PC = (ushort)((PC - 2) & 0xffff);
            SubtractNumberOfTStatesLeft(21);
        }
        else
        {
            SubtractNumberOfTStatesLeft(16);
        }
    }

    private ushort INC16(ushort value, int tStates)
    {
        SubtractNumberOfTStatesLeft(tStates);
        return (ushort)((value + 1) & 0xffff);
    }

    private byte INC8(byte value, int tStates)
    {
        var inc8pv = value == 0x7f;
        var inc8h = (((value & 0x0f) + 1) & F_H) != 0;
        value = (byte)((value + 1) & 0xff);

        fS = (value & F_S) != 0;
        f3 = (value & F_3) != 0;
        f5 = (value & F_5) != 0;
        fZ = value == 0;
        fPV = inc8pv;
        fH = inc8h;
        fN = false;

        SubtractNumberOfTStatesLeft(tStates);
        return value;
    }

    /// <summary>
    /// Decrement for 16 bit regiter
    /// </summary>
    /// <param name="value">Value to decrement with</param>
    /// <param name="tStates">Number of tstates</param>
    /// <returns>Decremented value</returns>
    private ushort DEC16(ushort value, int tStates)
    {
        SubtractNumberOfTStatesLeft(tStates);
        return (ushort)((value - 1) & 0xffff);
    }

    /// <summary>
    /// Decrement for 8bit register
    /// </summary>
    /// <param name="value">Value to decrement</param>
    /// <param name="tStates">Number of tstates</param>
    /// <returns>Decremented value</returns>
    private byte DEC8(byte value, int tStates)
    {
        SubtractNumberOfTStatesLeft(tStates);
        var dec8pv = value == 0x80;
        var dev8h = (((value & 0x0f) - 1) & F_H) != 0;
        value = (byte)((value - 1) & 0xff);

        fS = (value & F_S) != 0;
        f3 = (value & F_3) != 0;
        f5 = (value & F_5) != 0;
        fZ = value == 0;
        fPV = dec8pv;
        fH = dev8h;
        fN = true;

        return value;
    }

    private byte INC8NoFlags(byte a)
    {
        return (byte)((a + 1) & 0xff);
    }

    private byte DEC8NoFlags(byte a)
    {
        return (byte)((a - 1) & 0xff);
    }

    /// <summary>
    /// Block compare with decrement
    /// </summary>
    public void CPDR()
    {
        var cpdrc = fC;

        CP(ReadByteFromMemory(HL), 0);
        HL = DEC16(HL, 0);
        BC = DEC16(BC, 0);

        var cpdrpv = BC != 0;

        fPV = cpdrpv;
        fC = cpdrc;
        if (cpdrpv && !fZ)
        {
            //Repeat until BC==0
            PC = (ushort)((PC - 2) & 0xffff);
            SubtractNumberOfTStatesLeft(21);
        }
        else
        {
            SubtractNumberOfTStatesLeft(16);
        }
    }

    /// <summary>
    /// Complement Accumulator (4 tStates)
    /// </summary>
    public void CPL()
    {
        SubtractNumberOfTStatesLeft(4);
        var comp = (byte)(A ^ 0xff);

        f3 = (comp & F_3) != 0;
        f5 = (comp & F_5) != 0;
        fH = true;
        fN = true;

        A = comp;
    }

    /// <summary>
    /// Deciaml Adjust accumulator (4 tstates)
    /// </summary>
    public void DAA()
    {
        var daaa = A;
        byte daaincrement = 0;
        var daac = fC;

        if (fH || ((daaa & 0x0f) > 0x09))
        {
            daaincrement |= 0x06;
        }
        if (daac || (daaa > 0x9f) || ((daaa > 0x8f) && ((daaa & 0x0f) > 0x09)))
        {
            daaincrement |= 0x60;
        }
        if (daaa > 0x99)
        {
            daac = true;
        }
        if (fN)
        {
            SUB(daaincrement, 0);
        }
        else
        {
            A = (byte)ADDADC8(A, daaincrement, false, 0);
        }

        fC = daac;
        fPV = Parity[A];
        SubtractNumberOfTStatesLeft(4);
    }

    public void SUB(byte b, int tStates)
    {
        var suba = A;
        var subsubtracted = suba - b;
        var subtruncated = (byte)(subsubtracted & 0xff);

        fS = (subtruncated & F_S) != 0;
        f3 = (subtruncated & F_3) != 0;
        f5 = (subtruncated & F_5) != 0;
        fZ = subtruncated == 0;
        fC = (subsubtracted & 0x100) != 0;
        fPV = ((suba ^ b) & (suba ^ subtruncated) & 0x80) != 0;
        fH = (((suba & 0x0f) - (b & 0x0f)) & F_H) != 0;
        fN = true;

        A = subtruncated;
        SubtractNumberOfTStatesLeft(tStates);
    }

    public void DNJZ()
    {
        B = (byte)((B - 1) & 0xff);
        if (B != 0)
        {
            SubtractNumberOfTStatesLeft(13);
            PC = (ushort)(PC + Sign(GetNextPCByte()));
            PC++;
        }
        else
        {
            SubtractNumberOfTStatesLeft(8);
            PC++;
        }
    }

    public void EXX()
    {
        var exxtmp = BC;
        BC = BCPrim;
        BCPrim = exxtmp;

        exxtmp = DE;
        DE = DEPrim;
        DEPrim = exxtmp;

        exxtmp = HL;
        HL = HLPrim;
        HLPrim = exxtmp;

        SubtractNumberOfTStatesLeft(4);
    }
    #endregion
}
