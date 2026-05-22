using System;
using System.Collections.Generic;

namespace ZXBox.Hardware.Output;

public sealed class ZxPrinter
{
    public const int PaperWidth = 256;
    public const int FastPixelStepTStates = 12288;
    public const int SlowPixelStepTStates = FastPixelStepTStates * 2;

    private readonly object _syncRoot = new();
    private readonly List<byte[]> _completedLines = new();
    private readonly byte[] _currentLine = new byte[PaperWidth];

    private bool _connected;
    private bool _motorRunning;
    private bool _slowMotor;
    private bool _stylusPowered;
    private bool _paperStartLatch;
    private bool _nextPixelLatch;
    private int _headColumn = -1;
    private int _tStateAccumulator;
    private int _paperVersion;

    public bool Connected
    {
        get
        {
            lock (_syncRoot)
            {
                return _connected;
            }
        }
    }

    public int PaperHeight
    {
        get
        {
            lock (_syncRoot)
            {
                return _completedLines.Count + (_headColumn >= 0 ? 1 : 0);
            }
        }
    }

    public int PaperVersion
    {
        get
        {
            lock (_syncRoot)
            {
                return _paperVersion;
            }
        }
    }

    public void Connect()
    {
        lock (_syncRoot)
        {
            _connected = true;
            ResetRuntimeLocked();
        }
    }

    public void Disconnect()
    {
        lock (_syncRoot)
        {
            _connected = false;
            ResetRuntimeLocked();
        }
    }

    public void ResetRuntime()
    {
        lock (_syncRoot)
        {
            ResetRuntimeLocked();
        }
    }

    public void ClearPaper()
    {
        lock (_syncRoot)
        {
            _completedLines.Clear();
            Array.Clear(_currentLine);
            _headColumn = -1;
            _paperVersion++;
        }
    }

    public bool TryReadPort(int port, out int value)
    {
        lock (_syncRoot)
        {
            value = 0xFF;

            if (!_connected || !ClaimsPort(port))
            {
                return false;
            }

            value = BuildStatusByte();
            return true;
        }
    }

    public bool HandlePortWrite(int port, int value)
    {
        lock (_syncRoot)
        {
            if (!_connected || !ClaimsPort(port))
            {
                return false;
            }

            var stylusWasPowered = _stylusPowered;
            _paperStartLatch = false;
            _nextPixelLatch = false;
            _stylusPowered = (value & 0x80) != 0;
            _motorRunning = (value & 0x04) == 0;
            _slowMotor = (value & 0x02) != 0;

            if (!stylusWasPowered && _stylusPowered)
            {
                _paperStartLatch = true;
            }

            return true;
        }
    }

    public void AdvanceTStates(int diff)
    {
        if (diff <= 0)
        {
            return;
        }

        lock (_syncRoot)
        {
            if (!_connected || !_motorRunning)
            {
                return;
            }

            _tStateAccumulator += diff;
            var stepSize = _slowMotor ? SlowPixelStepTStates : FastPixelStepTStates;

            while (_tStateAccumulator >= stepSize)
            {
                _tStateAccumulator -= stepSize;
                AdvanceOnePixel();
            }
        }
    }

    public ZxPrinterPaperSnapshot GetPaperSnapshot()
    {
        lock (_syncRoot)
        {
            var height = _completedLines.Count + (_headColumn >= 0 ? 1 : 0);
            if (height == 0)
            {
                return new ZxPrinterPaperSnapshot(PaperWidth, 0, Array.Empty<byte>(), _paperVersion);
            }

            var pixels = new byte[PaperWidth * height];

            for (var lineIndex = 0; lineIndex < _completedLines.Count; lineIndex++)
            {
                Buffer.BlockCopy(_completedLines[lineIndex], 0, pixels, lineIndex * PaperWidth, PaperWidth);
            }

            if (_headColumn >= 0)
            {
                Buffer.BlockCopy(_currentLine, 0, pixels, _completedLines.Count * PaperWidth, PaperWidth);
            }

            return new ZxPrinterPaperSnapshot(PaperWidth, height, pixels, _paperVersion);
        }
    }

    private static bool ClaimsPort(int port)
    {
        return (port & 0x0004) == 0;
    }

    private int BuildStatusByte()
    {
        var status = 0x3E;

        if (_paperStartLatch)
        {
            status |= 0x80;
        }

        if (_nextPixelLatch)
        {
            status |= 0x01;
        }

        return status;
    }

    private void AdvanceOnePixel()
    {
        _nextPixelLatch = true;

        if (_headColumn < 0)
        {
            _headColumn = 0;
        }

        if (_stylusPowered)
        {
            _currentLine[_headColumn] = 1;
        }

        if (_headColumn == PaperWidth - 1)
        {
            CommitLine();
            _headColumn = -1;
            _paperStartLatch = true;
        }
        else
        {
            _headColumn++;
        }

        _paperVersion++;
    }

    private void CommitLine()
    {
        _completedLines.Add((byte[])_currentLine.Clone());
        Array.Clear(_currentLine);
    }

    private void ResetRuntimeLocked()
    {
        _motorRunning = false;
        _slowMotor = false;
        _stylusPowered = false;
        _paperStartLatch = true;
        _nextPixelLatch = false;
        _headColumn = -1;
        _tStateAccumulator = 0;
    }
}

public sealed class ZxPrinterPaperSnapshot
{
    public ZxPrinterPaperSnapshot(int width, int height, byte[] pixels, int version)
    {
        Width = width;
        Height = height;
        Pixels = pixels;
        Version = version;
    }

    public int Width { get; }

    public int Height { get; }

    public byte[] Pixels { get; }

    public int Version { get; }
}
