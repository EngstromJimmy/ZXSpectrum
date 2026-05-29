using System;
using ZXBox.Hardware.Interfaces;

namespace ZXBox.Hardware.Output;

public sealed class BrowserBeeper : IOutput
{
    private const int InitialTransitionCapacity = 128;
    private const byte SpeakerMask = 1 << 4;

    private readonly float _bufferTState;
    private readonly float _gain;
    private int[] _transitionPositions = new int[InitialTransitionCapacity];
    private bool[] _transitionStates = new bool[InitialTransitionCapacity];
    private int _transitionCount;
    private int _lastTState;
    private bool _frameStartHigh;
    public BrowserBeeper(int samplesPerFrame, int tStatesPerFrame, float gain)
    {
        SampleCount = samplesPerFrame;
        _bufferTState = (float)samplesPerFrame / tStatesPerFrame;
        _gain = gain;
    }

    public int SampleCount { get; }

    public void Output(ushort port, byte byteValue, int tState)
    {
        if ((port & 0xff) != 0xfe)
        {
            return;
        }

        if (_lastTState > tState)
        {
            _lastTState = 0;
        }

        var bufferPosition = (int)(tState * _bufferTState);
        if (bufferPosition >= SampleCount)
        {
            bufferPosition = SampleCount - 1;
        }

        var nextHigh = (byteValue & SpeakerMask) == SpeakerMask;
        RecordTransition(bufferPosition, nextHigh);
        _lastTState = tState;
    }

    public void CompleteFrame(Span<float> destination)
    {
        if (destination.Length < SampleCount)
        {
            throw new ArgumentException("Destination buffer is smaller than the frame size.", nameof(destination));
        }

        var cursor = 0;
        var currentHigh = _frameStartHigh;
        var highSampleCount = 0;

        for (var i = 0; i < _transitionCount; i++)
        {
            var position = _transitionPositions[i];
            if (position > cursor && currentHigh)
            {
                highSampleCount += position - cursor;
            }

            currentHigh = _transitionStates[i];
            cursor = position;
        }

        if (cursor < SampleCount && currentHigh)
        {
            highSampleCount += SampleCount - cursor;
        }

        var average = (127f * highSampleCount) / SampleCount;
        var lowValue = Math.Clamp((-average / 63.5f) * _gain, -1f, 1f);
        var highValue = Math.Clamp(((127f - average) / 63.5f) * _gain, -1f, 1f);

        cursor = 0;
        currentHigh = _frameStartHigh;

        for (var i = 0; i < _transitionCount; i++)
        {
            var position = _transitionPositions[i];
            if (position > cursor)
            {
                destination[cursor..position].Fill(currentHigh ? highValue : lowValue);
            }

            currentHigh = _transitionStates[i];
            cursor = position;
        }

        if (cursor < SampleCount)
        {
            destination[cursor..SampleCount].Fill(currentHigh ? highValue : lowValue);
        }

        if (destination.Length > SampleCount)
        {
            destination[SampleCount..].Clear();
        }

        _frameStartHigh = currentHigh;
        _lastTState = 0;
        _transitionCount = 0;
    }

    private void RecordTransition(int position, bool state)
    {
        if (_transitionCount > 0 && _transitionPositions[_transitionCount - 1] == position)
        {
            _transitionStates[_transitionCount - 1] = state;
            return;
        }

        EnsureCapacity(_transitionCount + 1);
        _transitionPositions[_transitionCount] = position;
        _transitionStates[_transitionCount] = state;
        _transitionCount++;
    }

    private void EnsureCapacity(int requiredCapacity)
    {
        if (_transitionPositions.Length >= requiredCapacity)
        {
            return;
        }

        var newCapacity = _transitionPositions.Length * 2;
        while (newCapacity < requiredCapacity)
        {
            newCapacity *= 2;
        }

        Array.Resize(ref _transitionPositions, newCapacity);
        Array.Resize(ref _transitionStates, newCapacity);
    }
}
