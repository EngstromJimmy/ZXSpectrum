using System;
using System.Collections.Generic;
using System.Linq;

namespace ZXBox.Hardware.Output;

public class Beeper<T> : Interfaces.IOutput where T : IComparable, IComparable<T>, IConvertible, IEquatable<T>
{

    public Beeper(T low, T high, int samplesPerFrame, int channels)
    {
        bufferCount = samplesPerFrame;
        highBuffer = Enumerable.Repeat<T>(high, bufferCount).ToArray();
        lowBuffer = Enumerable.Repeat<T>(low, bufferCount).ToArray();
        this.samplesPerFrame = samplesPerFrame;
        this.high = high;
        this.low = low;
        this.channels = channels;
        returnbuffer = new T[samplesPerFrame * channels];
        buffer = new T[samplesPerFrame];
    }

    public T[] highBuffer = null;
    public T[] lowBuffer = null;
    private Queue<T[]> BufferQueue = new Queue<T[]>();
    private int bufferCount;
    private int lastTstate;
    private int samplesPerFrame;
    private T lastValue;
    private T low;
    private T high;
    private T[] buffer;
    private T[] returnbuffer;
    private int bufferPosition;
    private int channels;

    public T[] GetSoundBuffer()
    {
        if (BufferQueue.Count > 0)
        {
            return BufferQueue.Dequeue();
        }
        else
        {
            return lowBuffer;
        }
    }

    public void GenerateSound(int tStates = 69888)
    {
        if (lastTstate < tStates)
        {
            if (bufferPosition <= bufferCount)
            {
                if (lastValue.CompareTo(low) > 0)
                {
                    Array.Copy(highBuffer, 0, buffer, bufferPosition, bufferCount - bufferPosition);
                }
                else
                {
                    Array.Copy(lowBuffer, 0, buffer, bufferPosition, bufferCount - bufferPosition);
                }
            }
        }
        else
        {

        }
        counter = 0;
        for (c = 0; c < bufferCount; c++)
        {
            for (int channel = 0; channel < channels; channel++)
            {
                returnbuffer[counter++] = buffer[c];
            }
        }

        BufferQueue.Enqueue(returnbuffer);
        lastTstate = 0;
        buffer = new T[samplesPerFrame];
    }

    int lastbufferPosition = 0;
    int c = 0;
    int counter = 0;
    #region IOutput Members
    //The output is is not dependent on the way the sound will be outputted but rather all the values the buzzer would have at any given tstate
    public void Output(int Port, int ByteValue, int tState)
    {
        double buffertstate = Convert.ToDouble(samplesPerFrame) / 69888d;

        if ((Port & 0xff) == 0xfe)
        {
            if (lastTstate > tState)
            {
                lastTstate = 0;
            }

            lastbufferPosition = Convert.ToInt32(lastTstate * buffertstate);
            bufferPosition = Convert.ToInt32(tState * buffertstate);

            if (bufferPosition >= buffer.Count())
            {
                bufferPosition = buffer.Count() - 1;
            }

            int diff = bufferPosition - lastbufferPosition;

            if (bufferPosition < bufferCount && diff > 0)
            {
                if (lastValue.CompareTo(low) > 0)
                {
                    Array.Copy(highBuffer, 0, buffer, lastbufferPosition, diff);
                }
                else
                {
                    Array.Copy(lowBuffer, 0, buffer, lastbufferPosition, diff);
                }
            }
            //}

            lastValue = (T)((((ByteValue & 16) == 16)) ? high : low);
            //Debug.WriteLine(tState + " - " + (tState / Samplesperframe));

            buffer[bufferPosition] = lastValue;

            lastTstate = tState;
        }
    }

    #endregion
}