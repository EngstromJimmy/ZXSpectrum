﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ZXBox.Core.Tape;
using ZXBox.Hardware.Interfaces;

namespace ZXBox.Core.Hardware.Input
{
    /// <summary>
    ///A leader consisting of 8063 (for header blocks) or 3223 (data blocks) pulses, each of which has a duration of 2168 tstates.
    ///A first sync pulse of 667 tstates.
    ///A second sync pulse of 735 tstates.
    ///The block data: a reset bit is encoded as two pulses of 855 tstates each, a set bit as two pulses of 1710 tstates each.The lowest byte in memory is first on tape, with the most significant bit first within each byte.
    /// </summary>
    public class TapePlayer : IInput
    {
        public TapFormat tf = new TapFormat();

        public void LoadTape(byte[] data)
        {
            tf.ReadFile(data);
            bool ear = false;
            long tstate = 0;
            foreach (var block in tf.Blocks)
            {
                EarValues.Add(new EarValue() { Ear = ear, TState = 0, Pulse = PulseTypeEnum.Pilot });
                for (int pilotcount = 0; pilotcount < (block.Data[0] < 128 ? 8063 : 3223); pilotcount++)
                {
                    ear = !ear;
                    tstate += 2168;
                    EarValues.Add(new EarValue() { Ear = ear, TState = tstate, Pulse = PulseTypeEnum.Pilot });

                }

                //Add sync1
                ear = !ear;
                tstate += 667;
                EarValues.Add(new EarValue() { Ear = ear, TState = tstate, Pulse = PulseTypeEnum.Sync1 });

                //Add sync2
                ear = !ear;
                tstate += 735;
                EarValues.Add(new EarValue() { Ear = ear, TState = tstate, Pulse = PulseTypeEnum.Sync2 });

                BitArray bits = new BitArray(block.Data);
                for (int i = 0; i < bits.Length; i++)
                {
                    //Add two pulses
                    ear = !ear;
                    tstate += bits[i] ? 1710 : 855;
                    EarValues.Add(new() { Ear = ear, TState = tstate, Pulse = PulseTypeEnum.Data });

                    ear = !ear;
                    tstate += bits[i] ? 1710 : 855;
                    EarValues.Add(new() { Ear = ear, TState = tstate, Pulse = PulseTypeEnum.Data });
                }
                //ear = !ear;
                //Pause
                tstate += 3500 * 1000; //1second;
                EarValues.Add(new() { Ear = ear, TState = tstate, Pulse = PulseTypeEnum.Pause });

            }

            //Add Termination 
            ear = !ear;
            tstate += 947;
            EarValues.Add(new() { Ear = ear, TState = tstate, Pulse = PulseTypeEnum.Stop });
            //foreach (var e in EarValues)
            //{
            //    Console.WriteLine($"{e.TState} \t {e.Ear} \t {e.Pulse}");
            //}
        }

        public void AddTStates(int tstates)
        {
            if (IsPlaying)
            {
                currentTstate += tstates;
            }
        }

        public List<EarValue> EarValues = new();
        public void Play()
        {
            IsPlaying = true;
        }
        bool IsPlaying = false;

        private long currentTstate = 0;

        private long lastTstate = 0;
        private long diff = 0;
        int returnvalue = 0xff;
        EarValue ear;
        bool firstread = true;
        public int Input(int Port, int tact)
        {
            if (IsPlaying)
            {
                //if (tact < lastTstate)
                //{
                //    diff = Math.Abs((69888 - lastTstate) - tact);
                //}
                //else
                //{
                //    diff = Math.Abs(lastTstate - tact);
                //}
                //lastTstate = tact;
                //currentTstate += diff;
                returnvalue = 0xff;
                if ((Port & 0xff) == 0xfe)
                {
                    if (firstread)
                    {
                        currentTstate = 0;
                        firstread = false;
                    }
                    ear = EarValues.TakeWhile(e => e.TState <= currentTstate).LastOrDefault();
                    if (ear != null)
                    {
                        if (ear.Pulse == PulseTypeEnum.Stop)
                        {
                            IsPlaying = false;
                        }
                        if (ear.Ear)
                            return returnvalue |= 1 << 6;
                        else
                            return returnvalue &= ~(1 << 6);
                    }
                }
            }

            return returnvalue;
        }
    }
    public class EarValue
    {
        public long TState { get; set; }
        public bool Ear { get; set; }
        public PulseTypeEnum Pulse { get; set; }
    }
    public enum PulseTypeEnum
    {
        Data,
        Termination,
        Pilot,
        Sync1,
        Sync2,
        Stop,
        Pause
    }

}