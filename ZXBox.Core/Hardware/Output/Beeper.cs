using System;
using System.Collections.Generic;

namespace ZXBox.Hardware.Output;

public class Beeper<T> : Interfaces.IOutput where T : IComparable, IComparable<T>, IConvertible, IEquatable<T>
{

    public Beeper(T low, T high, int samplesPerFrame, int channels, int tStatesPerFrame = 69888)
    {
        bufferCount = samplesPerFrame;
        highBuffer = new T[bufferCount];
        lowBuffer = new T[bufferCount];
        Array.Fill(highBuffer, high);
        Array.Fill(lowBuffer, low);
        this.samplesPerFrame = samplesPerFrame;
        this.high = high;
        this.low = low;
        this.channels = channels;
        bufferTstate = (double)samplesPerFrame / tStatesPerFrame;
        outputBuffers = new[]
        {
            new T[samplesPerFrame * channels],
            new T[samplesPerFrame * channels]
        };
        silentOutputBuffer = new T[samplesPerFrame * channels];
        buffer = new T[samplesPerFrame];

        if (channels == 1)
        {
            Array.Copy(lowBuffer, silentOutputBuffer, bufferCount);
        }
        else
        {
            for (int sample = 0, outputIndex = 0; sample < bufferCount; sample++)
            {
                for (int channel = 0; channel < channels; channel++)
                {
                    silentOutputBuffer[outputIndex++] = low;
                }
            }
        }

        lastValue = low;
    }

    public T[] highBuffer = null;
    public T[] lowBuffer = null;
    private Queue<T[]> BufferQueue = new Queue<T[]>(2);
    private int bufferCount;
    private int lastTstate;
    private int samplesPerFrame;
    private T lastValue;
    private T low;
    private T high;
    private T[] buffer;
    private readonly T[][] outputBuffers;
    private readonly T[] silentOutputBuffer;
    private readonly double bufferTstate;
    private int outputBufferIndex;
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
            return silentOutputBuffer;
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
        var outputBuffer = outputBuffers[outputBufferIndex];
        if (channels > 1)
        {
            for (c = 0; c < bufferCount; c++)
            {
                for (int channel = 0; channel < channels; channel++)
                {
                    outputBuffer[counter++] = buffer[c];
                }
            }
        }
        else
        {
            Array.Copy(buffer, outputBuffer, bufferCount);
        }
        BufferQueue.Enqueue(outputBuffer);
        outputBufferIndex = (outputBufferIndex + 1) % outputBuffers.Length;

        lastTstate = 0;
        Array.Clear(buffer, 0, bufferCount);
        bufferPosition = 0;
    }

    int lastbufferPosition = 0;
    int c = 0;
    int counter = 0;
    #region IOutput Members
    //The output is is not dependent on the way the sound will be outputted but rather all the values the buzzer would have at any given tstate
    public void Output(ushort Port, byte ByteValue, int tState)
    {
        if ((Port & 0xff) == 0xfe)
        {
            if (lastTstate > tState)
            {
                lastTstate = 0;
            }

            lastbufferPosition = (int)(lastTstate * bufferTstate);
            bufferPosition = (int)(tState * bufferTstate);

            if (bufferPosition >= bufferCount)
            {
                bufferPosition = bufferCount - 1;
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

            lastValue = ((ByteValue & 16) == 16) ? high : low;
            //Debug.WriteLine(tState + " - " + (tState / Samplesperframe));

            buffer[bufferPosition] = lastValue;

            lastTstate = tState;
        }
    }

    #endregion
}