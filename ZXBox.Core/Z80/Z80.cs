using System;
using System.Text;

namespace Zilog;

public abstract partial class Z80
{

    public void WriteByteToMemoryOverridden(int address, int b)
    {
        this.WriteByteToMemory(address, b);
    }

    public Z80()
    {
        //Initiate Parity table
        for (int a = 0; a < 256; a++)
        {
            bool p = true;
            for (int b = 0; b < 8; b++)
            {
                if ((a & (1 << b)) != 0)
                {
                    p = !p;
                }
            }
            Parity[a] = p;
        }
    }

    #region Ports
    //Override these
    public virtual int In(int port)
    {
        return 0xff;
    }

    public virtual void Out(int Port, int ByteValue, int tStates)
    {
    }

    #endregion

    #region Registers and access to them

    #region Flags
    //Flags
    private bool fS = false;
    public bool fZ = false;
    private bool f5 = false;
    private bool fH = false;
    private bool f3 = false;
    private bool fPV = false;
    private bool fN = false;
    private bool fC = false;

    public int F
    {
        get
        {
            return (fS ? F_S : 0) |
                (fZ ? F_Z : 0) |
                (f5 ? F_5 : 0) |
                (fH ? F_H : 0) |
                (f3 ? F_3 : 0) |
                (fPV ? F_PV : 0) |
                (fN ? F_N : 0) |
                (fC ? F_C : 0);
        }
        set
        {
            fS = (value & F_S) != 0;
            fZ = (value & F_Z) != 0;
            f5 = (value & F_5) != 0;
            fH = (value & F_H) != 0;
            f3 = (value & F_3) != 0;
            fPV = (value & F_PV) != 0;
            fN = (value & F_N) != 0;
            fC = (value & F_C) != 0;
        }
    }

    private static int F_C = 0x01;
    private static int F_N = 0x02;
    private static int F_PV = 0x04;
    private static int F_3 = 0x08;
    private static int F_H = 0x10;
    private static int F_5 = 0x20;
    private static int F_Z = 0x40;
    private static int F_S = 0x80;

    #endregion

    //Registers
    private static int R_A = 0x07;
    private static int R_B = 0x00;
    private static int R_C = 0x01;
    private static int R_D = 0x02;
    private static int R_E = 0x03;
    private static int R_H = 0x04;
    private static int R_L = 0x05;
    //Index Registers
    public int[] IndexRegistry = new int[2];
    public int IX
    {
        get { return IndexRegistry[(int)IndexRegistryEnum.IX]; }
        set { IndexRegistry[(int)IndexRegistryEnum.IX] = value; }
    }
    public int IY
    {
        get { return IndexRegistry[(int)IndexRegistryEnum.IY]; }
        set { IndexRegistry[(int)IndexRegistryEnum.IY] = value; }
    }

    //Interrupt Register
    public int I = 0;
    public int[] Registers = new int[10];

    //Registers Prim
    public int[] RegitersPrim = new int[10];
    public int Fprim = 0;
    //8bit
    public int A
    {
        get { return Registers[R_A]; }
        set { Registers[R_A] = value; }
    }

    public int B
    {
        get { return Registers[R_B]; }
        set { Registers[R_B] = value; }
    }

    public int C
    {
        get { return Registers[R_C]; }
        set { Registers[R_C] = value; }
    }

    public int D
    {
        get { return Registers[R_D]; }
        set { Registers[R_D] = value; }
    }

    public int E
    {
        get { return Registers[R_E]; }
        set { Registers[R_E] = value; }
    }

    public int H
    {
        get { return Registers[R_H]; }
        set { Registers[R_H] = value; }
    }

    public int L
    {
        get { return Registers[R_L]; }
        set { Registers[R_L] = value; }
    }

    public int APrim
    {
        get { return RegitersPrim[R_A]; }
        set { RegitersPrim[R_A] = value; }
    }

    public int BPrim
    {
        get { return RegitersPrim[R_B]; }
        set { RegitersPrim[R_B] = value; }
    }

    public int CPrim
    {
        get { return RegitersPrim[R_C]; }
        set { RegitersPrim[R_C] = value; }
    }

    public int DPrim
    {
        get { return RegitersPrim[R_D]; }
        set { RegitersPrim[R_D] = value; }
    }

    public int EPrim
    {
        get { return RegitersPrim[R_E]; }
        set { RegitersPrim[R_E] = value; }
    }

    public int HPrim
    {
        get { return RegitersPrim[R_H]; }
        set { RegitersPrim[R_H] = value; }
    }

    public int LPrim
    {
        get { return RegitersPrim[R_L]; }
        set { RegitersPrim[R_L] = value; }
    }

    public int FPrim
    {
        get { return Fprim; }
        set { Fprim = value; }
    }

    //16 bit
    public int HL
    {
        get
        {
            return Registers[R_H] << 8 | Registers[R_L];
        }
        set
        {
            Registers[R_H] = value >> 8;
            Registers[R_L] = value & 0xff;
            //if (value == 23672)
            //{
            //    Console.WriteLine("SET hl:" + HL.ToString());
            //    Console.WriteLine("PC: " + PC.ToString());
            //    Console.WriteLine("OP: 0x" + opcode.ToString("x"));
            //}
        }
    }

    public int HLPrim
    {
        get
        {
            return RegitersPrim[R_H] << 8 | RegitersPrim[R_L];
        }
        set
        {
            RegitersPrim[R_H] = value >> 8;
            RegitersPrim[R_L] = value & 0xff;
        }
    }

    public int DE
    {
        get
        {
            return Registers[R_D] << 8 | Registers[R_E];
        }
        set
        {
            Registers[R_D] = value >> 8;
            Registers[R_E] = value & 0xff;
        }
    }

    public int DEPrim
    {
        get
        {
            return RegitersPrim[R_D] << 8 | RegitersPrim[R_E];
        }
        set
        {
            RegitersPrim[R_D] = value >> 8;
            RegitersPrim[R_E] = value & 0xff;
        }
    }

    public int BC
    {
        get
        {
            return Registers[R_B] << 8 | Registers[R_C];
        }
        set
        {
            Registers[R_B] = value >> 8;
            Registers[R_C] = value & 0xff;
        }
    }

    public int BCPrim
    {
        get
        {
            return RegitersPrim[R_B] << 8 | RegitersPrim[R_C];
        }
        set
        {
            RegitersPrim[R_B] = value >> 8;
            RegitersPrim[R_C] = value & 0xff;
        }
    }

    public int AF
    {
        get
        {
            return Registers[R_A] << 8 | F;
        }
        set
        {
            Registers[R_A] = value >> 8;
            F = value & 0xff;
        }
    }

    public int AFPrim
    {
        get
        {
            return RegitersPrim[R_A] << 8 | FPrim;
        }
        set
        {
            RegitersPrim[R_A] = value >> 8;
            Fprim = value & 0xff;
        }
    }

    public int IXL
    {
        get { return IX & 0xff; }
        set { IX = (IX & 0xff00) | (value); }
    }

    public int IXH
    {
        get { return (IX >> 8) & 0xff; }
        set { IX = (IX & 0xff) | (value << 8); }
    }

    public int IYL
    {
        get { return IY & 0xff; }
        set { IY = (IY & 0xff00) | (value); }
    }

    public int IYH
    {
        get { return (IY >> 8) & 0xff; }
        set { IY = (IY & 0xff) | (value << 8); }
    }

    /// <summary>
    /// Returns the value in register accoording to current opcode
    /// </summary>
    /// <param name="rpos"></param>
    /// <returns></returns>
    public int RegisterValueFromOP(int rpos)
    {
        return Registers[(opcode >> rpos) & 7];
    }

    public int BitValueFromOP
    {
        get { return ((opcode >> 3) & 7); }
    }

    private int d
    {
        get { return Sign(ReadByteFromMemory(PC++)); }
    }

    #endregion

    #region Program Counter
    //PC Stack call
    public void PCToStack()
    {
        StackpushWord(PC);
    }
    public void PCFromStack()
    {
        PC = StackpopWord();
    }

    private int GetNextPCByte()
    {
        int b = ReadByteFromMemory(PC);
        PC = PC + 1 & 0xffff;
        return b;
    }
    private int GetNextPCWord()
    {
        int w = ReadByteFromMemory(PC++);
        w |= (ReadByteFromMemory(PC++ & 0xffff) << 8);
        return w;
    }
    #endregion

    #region Stack
    public void StackpushWord(int word)
    {
        int sp = ((SP - 2) & 0xffff);
        SP = sp;
        WriteWordToMemory(sp, word);
    }
    public int StackpopWord()
    {
        int w = ReadByteFromMemory(SP);
        SP++;
        w |= (ReadByteFromMemory(SP & 0xffff) << 8);
        SP = (SP + 1 & 0xffff);
        return w;
    }
    #endregion

    #region Memory
    public abstract void WriteWordToMemory(int address, int word);

    public abstract void WriteByteToMemory(int address, int bytetowrite);

    public abstract int ReadByteFromMemory(int address);

    public int ReadWordFromMemory(int address)
    {
        return (ReadByteFromMemory(address + 1 & 0xffff) << 8 | ReadByteFromMemory(address & 0xffff)) & 0xffff;
    }
    #endregion

    //Program Counter
    public int PC;
    //Stack Pointer
    public int SP = 0x10000;
    int opcode = 0;
    public int NumberOfTStatesLeft = 0;
    public bool[] Parity = new bool[256];

    private int _EndTstates2 = 0;
    public int EndTstates2
    {
        get
        {
            return _EndTstates2 + (NumberOfTStatesLeft * -1);
        }
    }
    //Interupts and memory
    public bool BlockINT = true;
    public bool IFF = false;
    public bool IFF2 = false;
    public int IM = 2;
    public int _R7 = 0;
    public int _R = 0;
    public int R7
    {
        get { return _R7; }
        set { _R7 = value; }
    }

    public int R
    {
        get { return (_R & 0x7f) | _R7; }
        set
        {
            _R = value;
            _R7 = value & 0x80;
        }
    }

    public void Refresh(int t)
    {
        //            _R += t;
        _R = (byte)(((_R + 1) & 0x7F) | (_R & 0x80));
        //NumberOfTStatesLeft -= 1;
    }

    public void Reset()
    {
        PC = 0;
        SP = 0;

        A = 0;
        F = 0;
        BC = 0;
        DE = 0;
        HL = 0;

        EXX();
        AFPrim = 0;

        A = 0;
        F = 0;
        BC = 0;
        DE = 0;
        HL = 0;

        IX = 0;
        IY = 0;

        R = 0;

        I = 0;
        IFF = false;
        IFF2 = false;
        IM = 0;
        NumberOfTStatesLeft = 0;
        this.Out(254, 5, 0); //Border Color

        //Clear Memory
        for (int a = 16384; a < 3 * 16384; a++)
        {
            WriteByteToMemory(a, 0);
        }
    }

    //System.Text.StringBuilder sb = new StringBuilder();
    public void NextOpcode()
    {
        opcode = (ReadByteFromMemory(PC) & 0xff);
        PC = (PC + 1) & 0xffff;
    }

    public int interrupt()
    {
        // If it's not a masking interrupt
        if (!IFF)
        {
            return 0;
        }

        switch (IM)
        {
            case 0:
            case 1:
                PCToStack();
                IFF = false;
                IFF2 = false;
                PC = 56;
                return 13;
            case 2:
                PCToStack();
                IFF = false;
                IFF2 = false;
                int t = (I << 8) | 0x00ff;
                PC = ReadWordFromMemory(t);
                return 19;
        }

        return 0;
    }
    public int NumberOfTstates = 0;
    public StringBuilder sb = new StringBuilder();
    private DateTime start;
    public void DoIntructions(int numberOfTStates)
    {
        DoIntructions(numberOfTStates, null);
    }

    public void DoIntructions(int numberOfTStates, Func<Z80, int> gameSpecificFunc)
    {
        //sb = new StringBuilder();
        NumberOfTstates = numberOfTStates;
        NumberOfTStatesLeft += numberOfTStates;
        _EndTstates2 = numberOfTStates;
        //start = DateTime.Now;
        while (true)
        {

            if (interruptTriggered(NumberOfTStatesLeft))
            {
                //NumberOfTStatesLeft += (NumberOfTStates - interrupt());
                NumberOfTStatesLeft -= interrupt();
                break;
            }

            //Refresh(1);

            NextOpcode();
            if (gameSpecificFunc != null)
            {
                NumberOfTStatesLeft -= gameSpecificFunc(this);
            }
            //if(PC==58)
            //    Debug.WriteLine(PC);
            //sb.Append(PC);
            //sb.Append("\t");
            //sb.Append(opcode.ToString());
            //sb.Append("\t");
            //sb.Append(NumberOfTStatesLeft);
            //sb.Append("\r\n");
            switch (opcode)
            {
                case 0xCB:
                    NextOpcode();
                    DoCBPrefixInstruction();
                    break;
                case 0xDD:
                    Refresh(1);
                    NextOpcode();
                    DoDDorFDPrefixInstruction(IndexRegistryEnum.IX);
                    break;
                case 0xED:
                    Refresh(1);
                    NextOpcode();
                    DoEDPrefixInstruction();
                    break;
                case 0xFD:
                    Refresh(1);
                    NextOpcode();
                    DoDDorFDPrefixInstruction(IndexRegistryEnum.IY);
                    break;
                default:
                    Refresh(1);
                    DoNoPrefixInstruction();
                    break;
            }
        }
    }
}
