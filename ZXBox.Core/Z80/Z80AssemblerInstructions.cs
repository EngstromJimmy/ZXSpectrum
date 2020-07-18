using System;

namespace Zilog
{
    /// <summary>
    /// This partial class contains all the instructions that are carried out by the op codes.
    /// </summary>
    public partial class  Z80
    {

        private int[] bitArray = { 1, 2, 4, 8, 16, 32, 64, 128 };

        #region Instructions
        //Assembler commands
        public void Refresh()
        { }

        public bool interruptTriggered(int tstates)
        {
            return (tstates <= 0);
        }

        int pushsp;
        public void PUSH(int word)
        {
            //    int sp = ((SP() - 2) & 0xffff);
            //    SP(sp);
            //    PokeW(sp, word);
            pushsp= (SP - 2 & 0xffff);
            SP= pushsp;
            WriteWordToMemory(pushsp, word);
        }

        private int RES(int bit, int value,int tstates) 
        {
            NumberOfTStatesLeft -= tstates;
            return value & ~ bitArray[bit]; 
        }

        bool rlc;
        public int RL(int value,int tstates)
        {
            rlc = (value & 0x80) != 0;

            if (fC)
            {
                value = (value << 1) | 0x01;
            }
            else
            {
                value <<= 1;
            }
            value &= 0xff;

            fS=(value & F_S) != 0;
            f3=((value & F_3) != 0);
            f5=((value & F_5) != 0);
            fZ=((value) == 0);
            fPV=Parity[value];
            fH=false;
            fN=false;
            fC=rlc;

            NumberOfTStatesLeft -=tstates;
            return (value);
        }

        int rlcavalue;
        public void RLCA()
        {
            rlcavalue = A;
            bool c = (rlcavalue & 0x80) != 0;

            if (c)
            {
                rlcavalue = (rlcavalue << 1) | 0x01;
            }
            else
            {
                rlcavalue <<= 1;
            }
            rlcavalue &= 0xff;

            f3=((rlcavalue & F_3) != 0);
            f5=((rlcavalue & F_5) != 0);
            fN=false;
            fH=false;
            fC=c;
            NumberOfTStatesLeft -= 4;
            A=rlcavalue;
        }

        int sbc8a;
        int sbc8c;
        int value;
        int sbc8truncated;
        private int SBC8(int b,int tstates)
        {
            sbc8a = A;
            sbc8c = fC ? 1 : 0;
            value = sbc8a - b - sbc8c;
            sbc8truncated = value & 0xff;

            fS=((sbc8truncated & F_S) != 0);
            f3=((sbc8truncated & F_3) != 0);
            f5=((sbc8truncated & F_5) != 0);
            fZ=((sbc8truncated) == 0);
            fC=((value & 0x100) != 0);
            fPV=(((sbc8a ^ b) & (sbc8a ^ sbc8truncated) & 0x80) != 0);
            fH=((((sbc8a & 0x0f) - (b & 0x0f) - sbc8c) & F_H) != 0);
            fN=true;

            NumberOfTStatesLeft -= tstates;

            return sbc8truncated;
        }


        int sbc16c;
        int sbc16value;
        int sbc16truncated;
        private int SBC16(int a, int b,int tstates)
        {
            sbc16c = fC ? 1 : 0;
            sbc16value = a - b - sbc16c;
            sbc16truncated = sbc16value & 0xffff;

            fS=((sbc16truncated & (F_S << 8)) != 0);
            f3=((sbc16truncated & (F_3 << 8)) != 0);
            f5=((sbc16truncated & (F_5 << 8)) != 0);
            fZ=((sbc16truncated) == 0);
            fC=((sbc16value & 0x10000) != 0);
            fPV=(((a ^ b) & (a ^ sbc16truncated) & 0x8000) != 0);
            fH=((((a & 0x0fff) - (b & 0x0fff) - sbc16c) & 0x1000) != 0);
            fN=true;

            NumberOfTStatesLeft -= tstates;
            return sbc16truncated;
        }

        bool rlcc;
        public int RLC (int value,int tstates)
        {
            rlcc = (value & 0x80) != 0;

            if (rlcc)
            {
                value = (value << 1) | 0x01;
            }
            else
            {
                value <<= 1;
            }
            value &= 0xff;

            fS=((value & F_S) != 0);
            f3=((value & F_3) != 0);
            f5=((value & F_5) != 0);
            fZ=((value) == 0);
            fPV=(Parity[value]);
            fH=false;
            fN=false;
            fC=rlcc;

            NumberOfTStatesLeft -= tstates;
            return value;
        }

        int rlavalue; bool rlac;
        public void RLA()
        {
            rlavalue = A;
            rlac = (rlavalue & 0x80) != 0;

            if (fC)
            {
                rlavalue = (rlavalue << 1) | 0x01;
            }
            else
            {
                rlavalue <<= 1;
            }

            rlavalue &= 0xff;

            f3=((rlavalue & F_3) != 0);
            f5=((rlavalue & F_5) != 0);
            fN=false;
            fH =false;
            fC= rlac;
            NumberOfTStatesLeft -= 4;
            A=rlavalue;
        }

        bool rrc;
        public int RR(int value,int tstates)
        {
            rrc = (value & 0x01) != 0;

            if (fC)
            {
                value = (value >> 1) | 0x80;
            }
            else
            {
                value >>= 1;
            }

            fS=((value & F_S) != 0);
            f3=((value & F_3) != 0);
            f5=((value & F_5) != 0);
            fZ=((value) == 0);
            fPV=Parity[value];
            fH=false;
            fN=false;
            fC=rrc;

            NumberOfTStatesLeft -= tstates;
            return value;
        }

        public void RST(int position)
        {
            PUSH(PC);
            PC = position;
            NumberOfTStatesLeft -= 11;
        }

        int rrdvalue; int rrdm; int rrdq;
        public void RRD()
        {
            rrdvalue = A;
            rrdm = ReadByteFromMemory(HL);
            rrdq = rrdm;

            rrdm = (rrdm >> 4) | (rrdvalue << 4);
            rrdvalue = (rrdvalue & 0xf0) | (rrdq & 0x0f);
            WriteByteToMemory(HL, rrdm);

            fS=((rrdvalue & F_S) != 0);
            f3=((rrdvalue & F_3) != 0);
            f5=((rrdvalue & F_5) != 0);
            fZ=(rrdvalue == 0);
            fPV=Parity[rrdvalue];
            fH=false;
            fN=false;
            NumberOfTStatesLeft -= 18;
            A=rrdvalue;
        }
        int rrcavalue; bool rrcac;
        public void RRCA()
        {
            rrcavalue = A;
            rrcac = (rrcavalue & 0x01) != 0;

            if (rrcac)
            {
                rrcavalue = (rrcavalue >> 1) | 0x80;
            }
            else
            {
                rrcavalue >>= 1;
            }

            f3=((rrcavalue & F_3) != 0);
            f5=((rrcavalue & F_5) != 0);
            fN=false;
            fH=false;
            fC=rrcac;

            NumberOfTStatesLeft -= 4;
            A = rrcavalue;
        }

        bool rrcc;
        public int RRC(int value, int tstates)
        {
            rrcc = (value & 0x01) != 0;

            if (rrcc)
            {
                value = (value >> 1) | 0x80;
            }
            else
            {
                value >>= 1;
            }

            fS=((value & F_S) != 0);
            f3=((value & F_3) != 0);
            f5=((value & F_5) != 0);
            fZ=((value) == 0);
            fPV=(Parity[value]);
            fH=false;
            fN=false;
            fC=rrcc;

            NumberOfTStatesLeft -= tstates;
            return value;
        }

        int rravalue; bool rrac;
        public void RRA()
        {
            rravalue = A;
            rrac = (rravalue & 0x01) != 0;

            if (fC)
            {
                rravalue = (rravalue >> 1) | 0x80;
            }
            else
            {
                rravalue >>= 1;
            }

            f3=((rravalue & F_3) != 0);
            f5=((rravalue & F_5) != 0);
            fN=(false);
            fH=(false);
            fC=(rrac);

            NumberOfTStatesLeft -= 4; 
            A=rravalue;
        }
        int rldm;int rldq; int rldvalue;
        public void RLD()
        {
            rldvalue = A;
            rldm = ReadByteFromMemory(HL);
            rldq = rldm;

            rldm = (rldm << 4) | (rldvalue & 0x0f);
            rldvalue = (rldvalue & 0xf0) | (rldq >> 4);
            WriteByteToMemory(HL,rldm & 0xff);

            fS=((rldvalue & F_S) != 0);
            f3=((rldvalue & F_3) != 0);
            f5=((rldvalue & F_5) != 0);
            fZ=(rldvalue == 0);
            fPV=Parity[rldvalue];
            fH=false;
            fN=false;

            NumberOfTStatesLeft -= 18;
            A=rldvalue;
        }

        bool srac;
        public int SRA(int value,int tstates)
        {
            srac = (value & 0x01) != 0;
            value = (value >> 1) | (value & 0x80);

            fS=((value & F_S) != 0);
            f3=((value & F_3) != 0);
            f5=((value & F_5) != 0);
            fZ=((value) == 0);
            fPV=Parity[value];
            fH=false;
            fN=false;
            fC=srac;

            NumberOfTStatesLeft -= tstates;
            return value;
        }

        //public void SUB(int value,int tstates)
        //{
        //    int a = A;
        //    int subtracted = a - value;
        //    int truncated = wans & 0xff;

        //    fS=((truncated & F_S) != 0);
        //    f3=((truncated & F_3) != 0);
        //    f5=((truncated & F_5) != 0);
        //    fZ=((truncated) == 0);
        //    fC=((wans & 0x100) != 0);
        //    fPV=(((a ^ value) & (a ^ truncated) & 0x80) != 0);
        //    fH=((((a & 0x0f) - (value & 0x0f)) & F_H) != 0);
        //    fN=true;

        //    NumberOfTStatesLeft -= tstates;

        //    A=truncated;
        //}

        int xornewvalue;
        public  void XOR(int value,int tstates)
        {
            xornewvalue = (A ^ value) & 0xff;

            fS=((xornewvalue & F_S) != 0);
            f3=((xornewvalue & F_3) != 0);
            f5=((xornewvalue & F_5) != 0);
            fH=(false);
            fPV=(Parity[xornewvalue]);
            fZ=(xornewvalue == 0);
            fN=false;
            fC=false;

            NumberOfTStatesLeft -= tstates;

            A=xornewvalue;
        }



        bool srlc;
        private int SRL(int value,int tstates)
        {
            srlc = (value & 0x01) != 0;
            value = value >> 1;

            fS=((value & F_S) != 0);
            f3=((value & F_3) != 0);
            f5=((value & F_5) != 0);
            fZ=((value) == 0);
            fPV=Parity[value];
            fH=false;
            fN=false;
            fC=srlc;
            NumberOfTStatesLeft -= tstates;

            return (value);
        }

        bool slac;
        public int SLA(int value,int tstates)
        {
            slac = (value & 0x80) != 0;
            value = (value << 1) & 0xff;

            fS=((value & F_S) != 0);
            f3=((value & F_3) != 0);
            f5=((value & F_5) != 0);
            fZ=((value) == 0);
            fPV=Parity[value];
            fH=false;
            fN=false;
            fC=slac;
            NumberOfTStatesLeft -= tstates;
            
            return value;
        }

        private int  SET(int bit, int value,int tstates) 
        {
            NumberOfTStatesLeft -= tstates;
            return value | bitArray[bit]; 
        }

        public void RET(bool condition, int tstates,int notmettstates)
        {
            if (condition)
            {
                PC = POP();
                NumberOfTStatesLeft -= tstates;
            }
            else
            {
                NumberOfTStatesLeft -= notmettstates;
            }
        }

        int popsp; int popt;
        public int POP()
        {
            popsp = SP;
            popt = ReadByteFromMemory(popsp);
            popsp++;
            popt |= (ReadByteFromMemory(popsp & 0xffff) << 8);
            SP=(++popsp & 0xffff);
            return popt;

            //int retval = ReadWordFromMemory(SP);
            //SP=(SP+2 & 0xffff);
            //return retval;
        }

        public void OUTI()
        {
            B=DEC8(B,0);
            
            NumberOfTStatesLeft -= 9;
            Out(BC, ReadByteFromMemory(HL), NumberOfTstates - Math.Abs(NumberOfTStatesLeft));
            HL=INC16(HL,0);

            fZ=(B == 0);
            fN=true;
            if (ReadByteFromMemory(HL) + L > 255)
            {
                fH=true;
                fC=true;
            }
            else
            {
                fH=false;
                fC=false;
            }
            fPV=Parity[(((ReadByteFromMemory(HL) + L) & 7) ^ B)];

            NumberOfTStatesLeft -= 7;
        }

        int outdvalue;
        public void OUTD()
        {
            B=DEC8(B,0);
            
            outdvalue = ReadByteFromMemory(HL);
            NumberOfTStatesLeft -= 9;
            Out(BC, outdvalue, NumberOfTstates - Math.Abs(NumberOfTStatesLeft));
            HL=DEC16(HL,0);

            fZ=(B == 0);
            fN=(outdvalue>>7&0x01)!=1;
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
            fPV=Parity[(((outdvalue + L) & 7) ^B)];
            NumberOfTStatesLeft -= 7;
        }

        int otirvalue;
        public void OTIR()
        {
            otirvalue = ReadByteFromMemory(HL);
            NumberOfTStatesLeft -= 9;
            B = DEC8(B, 0);
            Out(BC, otirvalue, NumberOfTstates - Math.Abs(NumberOfTStatesLeft));
            HL = INC16(HL, 0);

			fN = ((otirvalue>>7) & 0x01)==1 ;
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
            fPV=Parity[(((otirvalue + L) & 7 )^B)];

            if (B != 0)
            {
                PC = (PC - 2) & 0xffff;
                NumberOfTStatesLeft -= 12;
            }
            else 
            {
                NumberOfTStatesLeft -= 7; 
            }
            
        }

        public void OTDR()
        {
            B = DEC8(B,0);
            NumberOfTStatesLeft -= 9;
            Out(BC, ReadByteFromMemory(HL), NumberOfTstates - Math.Abs(NumberOfTStatesLeft));
            HL=DEC16(HL,0);

            fZ=true;
            fZ=true;
            if (B != 0)
            {
                PC = (PC - 2) & 0xffff;
                NumberOfTStatesLeft -= 12;
            }
            else
            {
                NumberOfTStatesLeft -= 7;
            }
        }

        int orvalue;
        public void OR(int b, int tstates)
        {
            orvalue = A | b;

            fS = (orvalue & F_S) != 0;
            f3 = (orvalue & F_3) != 0;
            f5 = (orvalue & F_5) != 0;
            fH = false;
            fPV = Parity[orvalue];
            fZ = orvalue == 0;
            fN = false;
            fC = false;
            NumberOfTStatesLeft -= tstates; 
            A = orvalue;
        }

        public void NOP()
        {
            NumberOfTStatesLeft -= 4;
        }

        int negtmp;
        public void NEG()
        {
            negtmp = A;

            A = 0;
            SUB(negtmp, 0);
            NumberOfTStatesLeft -= 8;
        }

        int lddmemval; int lddn;
        public void LDD()
        {
            lddmemval = ReadByteFromMemory(HL);
            WriteByteToMemory(DE, lddmemval);
            DE = DEC16(DE, 0);
            HL = DEC16(HL, 0);
            BC = DEC16(BC, 0);

            fPV = BC != 0;
            fH = false;
            fN = false;

            lddn = lddmemval + A;

            f5 = ((lddn & 0x01) == 1);
            f3 = ((lddn >> 3 & 0x01) == 1);
            

            NumberOfTStatesLeft -= 16;
        }

        int lddr_local_tstates;int lddrcount, lddrdest, lddrfrom;
        public void LDDR()
        {  //TODO:fix this
            lddr_local_tstates = 0;

            lddrcount = BC;
            lddrdest = DE;
            lddrfrom = HL;
            Refresh(-2);
            do
            {
                WriteByteToMemory(lddrdest,ReadByteFromMemory(lddrfrom));
                lddrfrom = DEC16(lddrfrom, 0);
                lddrdest = DEC16(lddrdest, 0);
                lddrcount = DEC16(lddrcount, 0);

                lddr_local_tstates += (21);
                Refresh(2);
                if (interruptTriggered(lddr_local_tstates))
                {
                    break;
                }
            }
            while (lddrcount != 0);

            if (lddrcount != 0)
            {
                PC = ((PC - 2) & 0xffff);
                fH = false;
                fN = false;
                fPV = true;
            }
            else
            {
                lddr_local_tstates += (-5);
                fH = false;
                fN = false;
                fPV = false;
            }
            DE = lddrdest;
            HL = lddrfrom;
            BC = lddrcount;
            NumberOfTStatesLeft-=lddr_local_tstates;
            //int tstates = (5 + 16 * (BC)); 
            //int tmp1 = DE; 
            //int tmp2 = HL;	// LDDR - ok
            //Refresh(-2);
            //for (int n = (BC); n > 0; n--)
            //{
            //    WriteByteToMemory(tmp1--, ReadByteFromMemory(tmp2--));
            //    Refresh(2);
            //}
            //E = tmp1 & 0xff; 
            //D = tmp1 >> 8; 
            //B = C = 0; 
            //L = tmp2 & 0xff; 
            //H = tmp2 >> 8; 
            //F &= 0xe9;

            //NumberOfTStatesLeft -= tstates;
        }

        public void LDI()
        {
            int ldimemval = ReadByteFromMemory(HL);
            WriteByteToMemory(DE, ldimemval);
            DE = INC16(DE, 0);
            HL = INC16(HL, 0);
            BC = DEC16(BC, 0);

            int n = ldimemval + A;

            fPV = BC != 0;
            fH = false;
            fN = false;
            f5 = ((n & 0x01) == 1);
            f3 = ((n >> 3 & 0x01) == 1);
            
            NumberOfTStatesLeft -= 16;
        }

        int ldir_local_tstates, ldircount, ldirdest, ldirfrom;
        public void LDIR()
        {
            ldir_local_tstates = 0;

            ldircount = BC;
            ldirdest = DE;
            ldirfrom = HL;
            Refresh(-2);
            
            
            //var timeleft = NumberOfTStatesLeft/21;
            //int lenght = count;
            //if (timeleft < count)
            //{
            //    lenght = timeleft;
            //    if (lenght == 0) //Always let it run once
            //    {
            //        lenght = 1;
            //    }
            //}

            ////Buffer.BlockCopy(Memory, from * sizeof(int), Memory, dest * sizeof(int), lenght * sizeof(int));
            //Array.Copy(Memory, from , Memory, dest , lenght);

            //count = (count - lenght) & 0xffff;
            //from = (from + lenght) & 0xffff;
            //dest = (dest + lenght) & 0xffff;

            //_local_tstates = 21 * lenght;
            //Refresh(2 * lenght);

            do
            {
                WriteByteToMemory(ldirdest, ReadByteFromMemory(ldirfrom));
                //Memory[ldirdest] = Memory[ldirfrom];
                ldirfrom = INC16(ldirfrom, 0);
                ldirdest = INC16(ldirdest, 0);
              ldircount = DEC16(ldircount, 0);

                ldir_local_tstates += (21);
              Refresh(2);
               if (interruptTriggered(NumberOfTStatesLeft-ldir_local_tstates))
                {
                    break;
                }
            } while (ldircount != 0);

            if (ldircount != 0)
            {
                PC = (PC - 2) & 0xffff;
                fH = false;
                fN = false;
                fPV = true;
            }
            else
            {
                ldir_local_tstates += (-5);
                fH = false;
                fN = false;
                fPV = false;
            }
            DE = ldirdest;
            HL = ldirfrom;
            BC = ldircount;

            /*int tstates = 0;
            int tmp1 = DE; 
            int tmp2 = HL;
            int nvalue = 0;
            int count = BC;
            Refresh(-2);
            do //            for (int n = (BC); n > 0; n--)
            {
                int b = ReadByteFromMemory(tmp2);
                WriteByteToMemory(tmp1, b);
                tmp1 = (tmp1 + 1) & 0xffff; 
                tmp2 = (tmp2 + 1) & 0xffff;
                nvalue = b + A;
                Refresh(2);
                tstates += (21);
                if (interruptTriggered(tstates))
                {
                    break;
                }
            }while (count != 0);

            if (count != 0)
            {
                PC = PC - 2;
                fH = false;
                fN = false;
                fPV = true;
            }
            else
            {
                tstates += (-5);
                f5=((nvalue & 0x01)==1);
                fH=false;
                f3=((nvalue>>3 & 0x01)==1);
                fPV = (BC != 0);
            }
            E = tmp1 & 0xff;
            D = tmp1 >> 8;
            //B = C = 0; 
            L = tmp2 & 0xff;
            H = tmp2 >> 8;*/

            NumberOfTStatesLeft -= ldir_local_tstates;
        }

        int ldarvalue;
        private void LDAR()
        {
            ldarvalue = R;

            fS = ((ldarvalue & F_S) != 0);
            f3 = ((ldarvalue & F_3) != 0);
            f5 = ((ldarvalue & F_5) != 0);
            fZ = (ldarvalue == 0);
            fPV = IFF2;
            fH = false;
            fN = false;

            NumberOfTStatesLeft -= 9;
            A = ldarvalue;
        }
        int ldaivalue;
        public void LDAI()
        {
            ldaivalue = I;

            fS = ((ldaivalue & F_S) != 0);
            f3 = ((ldaivalue & F_3) != 0);
            f5 = ((ldaivalue & F_5) != 0);
            fZ = ldaivalue == 0;
            fPV = IFF2;
            fH = false;
            fN = false;

            NumberOfTStatesLeft -= 9;

            A = ldaivalue;
        }
        public void JP(bool argument, int position, int tstates)
        {
            if (argument)
                PC = position;
            NumberOfTStatesLeft -= tstates;
        }

        private int Sign(int nn)
        {
            return nn - ((nn & 128) << 1);
        }

        public void JR(bool argument, int position, int tstates)
        {
            if (argument)
            {
                PC = (PC + Sign(position)) & 0xFFFF;
            }
            NumberOfTStatesLeft -= tstates;
        }

        int inbcvalue;
        public int INBC(int tstates)
        {
            inbcvalue = In(BC);

            NumberOfTStatesLeft -= tstates;

            fZ = (inbcvalue == 0);
            fS = ((inbcvalue & F_S) != 0);
            f3 = ((inbcvalue & F_3) != 0);
            f5 = ((inbcvalue & F_5) != 0);
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
                PC = PC - 2;
                NumberOfTStatesLeft -= 21;
            }
            else
                NumberOfTStatesLeft -= 16;
        }

        int indb;
        public void IND(int tstates)
        {
            indb = DEC8(B,0);
            WriteByteToMemory(HL,In(BC));
            B = indb;
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
            fPV = Parity[(((ReadByteFromMemory(HL) + ((C - 1) & 255)) & 7) ^ B)];
            NumberOfTStatesLeft -= tstates;
        }

        int inib, inival;
        public void INI(int tstates)
        {
            inib = DEC8(B,0);
            inival = In(BC);
            WriteByteToMemory(HL, inival);
            B = inib;
            HL = INC16(HL, 0);
             
            fZ = inib == 0;
            fN = true;
            if (inival  + ((C + 1) & 255) > 255)
            {
                fC = true;
                fH = true;
            }
            else
            {
                fC = false;
                fH = false;
            }
            fPV=Parity[(((inival + ((C+1) & 255)) & 7)^B)];
            NumberOfTStatesLeft -= tstates;
        }

        public void INIR()
        {
            INI(0);
            if (B != 0)
            {
                NumberOfTStatesLeft -= 21;
                PC = PC - 2;
            }
            else
            {
                NumberOfTStatesLeft -= 16;
            }

        }

        int addadc8c, addadc8newvalue, addadc8truncated;
        public int ADDADC8(int a, int b, bool Carry, int tStates)
        {
            addadc8c = 0;
            if (Carry)
                addadc8c = fC ? 1 : 0; //Add 1 if carry is set

            addadc8newvalue = a + b + addadc8c;
            addadc8truncated = addadc8newvalue & 0xff;

            //Set flags
            fS = ((addadc8truncated & F_S) != 0);
            f3 = ((addadc8truncated & F_3) != 0);
            f5 = ((addadc8truncated & F_5) != 0);
            fZ = ((addadc8truncated) == 0);
            fC = ((addadc8newvalue & 0x100) != 0);
            fPV = (((a ^ ~b) & (a ^ addadc8truncated) & 0x80) != 0);
            fH = ((((a & 0x0f) + (b & 0x0f) + addadc8c) & F_H) != 0);
            fN = false;

            NumberOfTStatesLeft -= tStates;

            return addadc8truncated;
        }

        int addadc16c, addadc16added, addadc16truncated;
        private int ADDADC16(int a, int b, bool Carry, int tStates)
        {
            addadc16c = fC && Carry ? 1 : 0;
            addadc16added = a + b + addadc16c;
            addadc16truncated = addadc16added & 0xffff;

            
            f3 = (addadc16truncated & (F_3 << 8)) != 0;
            f5 = (addadc16truncated & (F_5 << 8)) != 0;
            fC = (addadc16added & 0x10000) != 0;
            fH = (((a & 0x0fff) + (b & 0x0fff) + addadc16c) & 0x1000) != 0;
            fN = false;
            if (Carry)
            {
                fS = (addadc16truncated & (F_S << 8)) != 0;
                fPV = ((a ^ ~b) & (a ^ addadc16truncated) & 0x8000) != 0;
                fZ = (addadc16truncated) == 0;
            }
            NumberOfTStatesLeft -= tStates;
            return (addadc16truncated);
        }

        int and8newvalue;
        public int AND8(int a, int b, int tStates)
        {
            and8newvalue = a & b;

            fS = (and8newvalue & F_S) != 0;
            f3 = (and8newvalue & F_3) != 0;
            f5 = (and8newvalue & F_5) != 0;
            fH = true;
            fPV = Parity[and8newvalue];
            fZ = and8newvalue == 0;
            fN = false;
            fC = false;

            NumberOfTStatesLeft -= tStates;
            return and8newvalue;
        }

        bool bitbitIsSet;
        public void BIT(int bit, int regvalue, int tStates)
        {
            bitbitIsSet = ((regvalue & bitArray[bit]) != 0);

            //fN = false;
            //fH = true;
            //if (bit == 3)
            //    f3 = bitIsSet;
            //if (bit == 5)
            //    f5 = bitIsSet;
            //fS = ((bit == 7) ? bitIsSet : false);
            //fZ = !bitIsSet;
            //fPV = !bitIsSet;        

            NumberOfTStatesLeft -= tStates;
            //fN=false;
            //fH=true;
            //f3=((regvalue & F_3) != 0);
            //f5=((regvalue & F_5) != 0);
            //fS=((bit == F_S) ? bitIsSet : false);
            //fZ=(!bitIsSet);
            //fPV=(!bitIsSet);
            F=(byte)((F & F_C) |
                            F_H |
                            (regvalue & (F_3 |F_5)) |
                            ((regvalue & (0x01 << bit)) != 0 ? 0 : (F_PV | F_Z)));


        }

        bool bitixydbitIsSet;
        public void BITixyd(int bit, int regvalue,int ixyd, int tStates)
        {
            bitixydbitIsSet = ((regvalue & bitArray[bit]) != 0);

            fN = false;
            fH = true;
            f3 = ((ixyd >> 11 & 0x01) == 1) ? true : false;
            f5 = ((ixyd >> 13 & 0x01) == 1) ? true : false;
            fS = ((bit == 7) ? bitixydbitIsSet : false);
            fZ = !bitixydbitIsSet;
            fPV = !bitixydbitIsSet;
            NumberOfTStatesLeft -= tStates;
        }

        int callnnw;
        public void CALLnn()
        {
            callnnw = GetNextPCWord();
            PCToStack();
            PC = callnnw;
            NumberOfTStatesLeft -= 17;
        }

        int sllc;
        private int SLL(int value,int tstates)
        {
            sllc = (value & 0x80) >> 7;
            value = ((value << 1) | 1) & 0xff;

            fS = ((value & F_S) != 0);
            f3 = ((value & F_3) != 0);
            f5 = ((value & F_5) != 0);
            fZ = ((value) == 0);
            fPV = Parity[value];
            fH = false;
            fN = false;
            fC = (sllc==1);
            NumberOfTStatesLeft -= tstates;
            return value;
        }


        int cpa, cpwvalue, cpnewvalue;
        /// <summary>
        /// Compare operand s to ackumulator
        /// </summary>
        /// <param name="s">Operand</param>
        /// <param name="tStates">Number of tstates</param>
        private void CP(int value,int tstates)
        {
            cpa = A;
            cpwvalue = cpa - value;
            cpnewvalue = cpwvalue & 0xff;

            fS=((cpnewvalue & F_S) != 0);
            f3=((value & F_3) != 0);
            f5=((value & F_5) != 0);
            fN=(true);
            fZ=(cpnewvalue == 0);
            fC=((cpwvalue & 0x100) != 0);
            fH=((((cpa & 0x0f) - (value & 0x0f)) & F_H) != 0);
            fPV=(((cpa ^ value) & (cpa ^ cpnewvalue) & 0x80) != 0);

            NumberOfTStatesLeft -= tstates;
        }

        int cp2sub, cp2truncated;
        public void CP2(int s, int tStates)
        {
            NumberOfTStatesLeft -= tStates;
            cp2sub = A - s;
            cp2truncated = cp2sub & 0xff;

            fS = ((cp2truncated & F_S) != 0);
            f3 = ((s & F_3) != 0);
            f5 = ((s & F_5) != 0);
            fN = true;
            fZ = (cp2truncated == 0);
            fC = ((cp2sub & 0x100) != 0);
            fH = ((((A & 0x0f) - (s & 0x0f)) & F_H) != 0);
            fPV = (((A ^ s) & (A ^ cp2truncated) & 0x80) != 0);
        }

        int callw;
        public void CALL(bool argument)
        {
            if (argument)
            {
                callw = GetNextPCWord();
                PCToStack();
                PC = callw;
                NumberOfTStatesLeft -= 17;
            }
            else
            {
                PC = ((PC + 2) & 0xffff);
                NumberOfTStatesLeft -= 10;
            }
        }

        /// <summary>
        /// Compement Carry flag
        /// </summary>
        public void CCF()
        {
            f3 = ((A & F_3) != 0);
            f5 = ((A & F_5) != 0);
            fN = false;
            fC = !fC;
            NumberOfTStatesLeft -= 4;
        }

        bool cpdc, cpdpv;
        int cpdn;
        public void CPD()
        {
            cpdc = fC;

            CP(ReadByteFromMemory(HL), 0);
            HL = DEC16(HL, 0);
            BC = DEC16(BC, 0);

            fPV = (BC != 0);
            fC = cpdc;
            //---------------
            cpdn = A - ReadByteFromMemory(HL) - (fH ? 1 : 0);
            cpdpv = (BC != 0);

            fN = true;
            fC = cpdc;
            f5 = ((cpdn & 0x01) == 1);
            f3 = ((cpdn >> 3 & 0x01) == 1);
            //-----------------------

            NumberOfTStatesLeft -= 16;
        }
        bool cpic;int cpimemvalue; int cpin;
        public void CPI()
        {
            cpic = fC;
            cpimemvalue = ReadByteFromMemory(HL);
            CP(cpimemvalue, 0);
            HL = INC16(HL, 0);
            BC = DEC16(BC, 0);
            cpin = A - cpimemvalue - (fH ? 1 : 0);
            f5 = ((cpin & 0x01) == 1);
            f3 = ((cpin >> 3 & 0x01) == 1);
            fPV = (BC != 0);
            fC = cpic;
            NumberOfTStatesLeft -= 16;
        }

        bool cpirc; int cpirvalue; int cpirn; bool cpirpv;
        public void CPIR()
        {
            cpirc = fC;
            cpirvalue = ReadByteFromMemory(HL);
            CP(cpirvalue, 0);

            HL = INC16(HL, 0);
            BC = DEC16(BC, 0);

            cpirn = A - cpirvalue  - (fH?1:0);
            cpirpv = (BC != 0);

            fN = true;
            fPV = cpirpv;
            fC = cpirc;
            f5 = ((cpirn & 0x01) == 1);
            f3 = ((cpirn >> 3 & 0x01) == 1);

            if (BC != 0 && A != cpirvalue)
            {   //Repeat until BC ==0
                PC = ((PC - 2) & 0xffff);
                NumberOfTStatesLeft -= 21;
            }
            else
            {
                NumberOfTStatesLeft -= 16;
            } 
        }

        private int INC16(int value, int tStates)
        {
            NumberOfTStatesLeft -= tStates;
            return (value + 1) & 0xffff;
        }

        bool inc8pv, inc8h;
        private int INC8(int value, int tStates)
        {
            inc8pv = (value == 0x7f);
            inc8h = (((value & 0x0f) + 1) & F_H) != 0;
            value = (value + 1) & 0xff;

            fS = ((value & F_S) != 0);
            f3 = ((value & F_3) != 0);
            f5 = ((value & F_5) != 0);
            fZ = ((value) == 0);
            fPV = (inc8pv);
            fH = inc8h;
            fN = false;

            NumberOfTStatesLeft -= tStates;
            return value;
        }

        /// <summary>
        /// Decrement for 16 bit regiter
        /// </summary>
        /// <param name="value">Value to decrement with</param>
        /// <param name="tStates">Number of tstates</param>
        /// <returns>Decremented value</returns>
        private int DEC16(int value, int tStates)
        {
            NumberOfTStatesLeft -= tStates;
            return (value - 1) & 0xffff;
        }

        bool dec8pv, dev8h;
        /// <summary>
        /// Decrement for 8bit register
        /// </summary>
        /// <param name="value">Value to decrement</param>
        /// <param name="tStates">Number of tstates</param>
        /// <returns>Decremented value</returns>
        private int DEC8(int value, int tStates)
        {
            NumberOfTStatesLeft -= tStates;
            dec8pv = (value == 0x80);
            dev8h = (((value & 0x0f) - 1) & F_H) != 0;
            value = (value - 1) & 0xff;

            fS = ((value & F_S) != 0);
            f3 = ((value & F_3) != 0);
            f5 = ((value & F_5) != 0);
            fZ = ((value) == 0);
            fPV = dec8pv;
            fH = dev8h;
            fN = true;

            return value;
        }


        //private int INC16NoFlags(int a) 
        //{
        //        return (a + 1) & 0xffff; 
        //}

        private int INC8NoFlags(int a)
        {
            return (a + 1) & 0xff;
        }


        private int DEC8NoFlags(int a)
        {
            return (a - 1) & 0xff;
        }

        bool cpdrc, cpdrpv;
        /// <summary>
        /// Block compare with decrement
        /// </summary>
        public void CPDR()
        {
            bool cpdrc = fC;

            CP(ReadByteFromMemory(HL), 0);
            HL = DEC16(HL, 0);
            BC = DEC16(BC, 0);

            cpdrpv = (BC != 0);

            fPV = cpdrpv;
            fC = cpdrc;
            if (cpdrpv && !fZ)
            {
                //Repeat until BC==0
                PC = ((PC - 2) & 0xffff);
                NumberOfTStatesLeft -= 21;
            }
            else
            {
                NumberOfTStatesLeft -= 16;
            }
        }

        /// <summary>
        /// Complement Accumulator (4 tStates)
        /// </summary>
        public void CPL()
        {
            NumberOfTStatesLeft -= 4;
            int comp = A ^ 0xff;

            f3 = (comp & F_3) != 0;
            f5 = (comp & F_5) != 0;
            fH = true;
            fN = true;

            A = comp;
        }

        int daaa,daaincrement;
        bool daac;
        /// <summary>
        /// Deciaml Adjust accumulator (4 tstates)
        /// </summary>
        public void DAA()
        {
            daaa = A;
            daaincrement = 0;
            daac = fC;

            if ((fH) || ((daaa & 0x0f) > 0x09))
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
                A=ADDADC8(A, daaincrement, false, 0);
            }

            fC = daac;
            fPV = Parity[A];
            NumberOfTStatesLeft -= 4;
        }

        int suba, subsubtracted, subtruncated;
        public void SUB(int b, int tStates)
        {
            suba = A;
            subsubtracted = suba - b;
            subtruncated = subsubtracted & 0xff;

            fS = ((subtruncated & F_S) != 0);
            f3 = ((subtruncated & F_3) != 0);
            f5 = ((subtruncated & F_5) != 0);
            fZ = ((subtruncated) == 0);
            fC = ((subsubtracted & 0x100) != 0);
            fPV = (((suba ^ b) & (suba ^ subtruncated) & 0x80) != 0);
            fH = ((((suba & 0x0f) - (b & 0x0f)) & F_H) != 0);
            fN = true;

            A = subtruncated;
            NumberOfTStatesLeft -= tStates;
        }

        public void DNJZ()
        {
            ////No effects on flags
            //int f = F;

            //B--;
            //if (B != 0)
            //{
            //    int d = GetNextPCByte();
            //    PC = (PC + d) & 0xffff;
            //    NumberOfTStatesLeft -= 13;
            //}
            //else
            //{
            //    PC = INC16(PC, 0);
            //    NumberOfTStatesLeft -= 8;
            //}
            //F = f;

            B = (B - 1) & 0xff;
            if (B != 0)
            {
                NumberOfTStatesLeft -= 13;
                PC += Sign(GetNextPCByte());
                PC++;
            }
            else
            {
                NumberOfTStatesLeft -= 8;
                PC++;
            }
        }

        int exxtmp;
        public void EXX()
        {
            exxtmp = BC;
            BC = BCPrim;
            BCPrim = exxtmp;

            exxtmp = DE;
            DE = DEPrim;
            DEPrim = exxtmp;

            exxtmp = HL;
            HL = HLPrim;
            HLPrim = exxtmp;


            NumberOfTStatesLeft -= 4;
        }
        #endregion

    }
}
