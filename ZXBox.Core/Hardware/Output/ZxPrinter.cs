using System;
using System.Collections.Generic;

namespace ZXBox.Hardware.Output;

public sealed class ZxPrinter
{
    public const int PaperWidth = 256;
    public const int FastPixelStepTStates = 220;
    public const int SlowPixelStepTStates = FastPixelStepTStates * 2;

    private const int IdleStatus = 0x3E;
    private const int ReadyStatus = 0xBE;
    private const int LineLeadInPixels = 64;
    private const int LineTotalPixels = 384;

    private readonly object _syncRoot = new();
    private readonly List<byte[]> _completedLines = new();
    private readonly byte[] _currentLine = new byte[PaperWidth];

    private bool _connected;
    private bool _motorRunning;
    private bool _slowMotor;
    private bool _stylusPowered;
    private bool _pendingSlowMotor;
    private bool _hasPendingSlowMotorChange;
    private bool _currentLineStarted;
    private int _lastWritePixel = -1;
    private long _lineTStateAccumulator;
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
                return _completedLines.Count + (_currentLineStarted ? 1 : 0);
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
            _currentLineStarted = false;
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

            value = BuildStatusByteLocked();
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

            if (!_motorRunning)
            {
                if ((value & 0x04) == 0)
                {
                    _motorRunning = true;
                    _slowMotor = (value & 0x02) != 0;
                    _stylusPowered = (value & 0x80) != 0;
                    _pendingSlowMotor = _slowMotor;
                    _hasPendingSlowMotorChange = false;
                    _lastWritePixel = -1;
                    _lineTStateAccumulator = 0;
                }

                return true;
            }

            ProcessElapsedLocked();

            if ((value & 0x04) != 0)
            {
                StopMotorLocked();
                return true;
            }

            var currentPixel = GetCurrentPixelPositionLocked();
            _lastWritePixel = currentPixel;
            _stylusPowered = (value & 0x80) != 0;

            var nextSlowMotor = (value & 0x02) != 0;
            if (currentPixel < 0)
            {
                _slowMotor = nextSlowMotor;
                _pendingSlowMotor = nextSlowMotor;
                _hasPendingSlowMotorChange = false;
            }
            else
            {
                _pendingSlowMotor = nextSlowMotor;
                _hasPendingSlowMotorChange = _pendingSlowMotor != _slowMotor;
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

            _lineTStateAccumulator += diff;
            ProcessElapsedLocked();
        }
    }

    public ZxPrinterPaperSnapshot GetPaperSnapshot()
    {
        lock (_syncRoot)
        {
            var height = _completedLines.Count + (_currentLineStarted ? 1 : 0);
            if (height == 0)
            {
                return new ZxPrinterPaperSnapshot(PaperWidth, 0, Array.Empty<byte>(), _paperVersion);
            }

            var pixels = new byte[PaperWidth * height];

            for (var lineIndex = 0; lineIndex < _completedLines.Count; lineIndex++)
            {
                Buffer.BlockCopy(_completedLines[lineIndex], 0, pixels, lineIndex * PaperWidth, PaperWidth);
            }

            if (_currentLineStarted)
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

    private int BuildStatusByteLocked()
    {
        if (!_motorRunning)
        {
            return IdleStatus;
        }

        var currentPixel = GetCurrentPixelPositionLocked();
        var status = (_stylusPowered || (currentPixel > -10 && currentPixel < 0))
            ? ReadyStatus
            : IdleStatus;

        if (currentPixel > _lastWritePixel)
        {
            status |= 0x01;
        }

        return status;
    }

    private int GetCurrentPixelPositionLocked()
    {
        var stepSize = _slowMotor ? SlowPixelStepTStates : FastPixelStepTStates;
        return (int)(_lineTStateAccumulator / stepSize) - LineLeadInPixels;
    }

    private void ProcessElapsedLocked()
    {
        if (!_motorRunning)
        {
            return;
        }

        while (true)
        {
            var currentPixel = GetCurrentPixelPositionLocked();
            FillCurrentLineUntilLocked(currentPixel);

            if (currentPixel < PaperWidth)
            {
                return;
            }

            if (_lastWritePixel < PaperWidth)
            {
                CommitCurrentLineLocked();
            }

            if (currentPixel < LineTotalPixels - LineLeadInPixels)
            {
                return;
            }

            AdvanceToNextLineLocked();
        }
    }

    private void FillCurrentLineUntilLocked(int currentPixel)
    {
        if (currentPixel <= _lastWritePixel)
        {
            return;
        }

        var upperBound = Math.Min(currentPixel, PaperWidth);
        var wrotePrintablePixels = false;

        for (var pixel = _lastWritePixel; pixel < upperBound; pixel++)
        {
            if (pixel < 0)
            {
                continue;
            }

            _currentLine[pixel] = _stylusPowered ? (byte)1 : (byte)0;
            wrotePrintablePixels = true;
        }

        if (wrotePrintablePixels)
        {
            _currentLineStarted = true;
            _paperVersion++;
        }
    }

    private void CommitCurrentLineLocked()
    {
        _completedLines.Add((byte[])_currentLine.Clone());
        Array.Clear(_currentLine);
        _currentLineStarted = false;
        _paperVersion++;
    }

    private void AdvanceToNextLineLocked()
    {
        var stepSize = _slowMotor ? SlowPixelStepTStates : FastPixelStepTStates;
        _lineTStateAccumulator -= (long)stepSize * LineTotalPixels;
        _lastWritePixel = -1;

        if (_hasPendingSlowMotorChange)
        {
            _slowMotor = _pendingSlowMotor;
            _hasPendingSlowMotorChange = false;
        }
    }

    private void StopMotorLocked()
    {
        var currentPixel = GetCurrentPixelPositionLocked();
        if (currentPixel >= 0 && currentPixel < PaperWidth)
        {
            FillCurrentLineUntilLocked(PaperWidth);
            CommitCurrentLineLocked();
        }

        _motorRunning = false;
        _slowMotor = false;
        _stylusPowered = false;
        _pendingSlowMotor = false;
        _hasPendingSlowMotorChange = false;
        _currentLineStarted = false;
        _lastWritePixel = -1;
        _lineTStateAccumulator = 0;
    }

    private void ResetRuntimeLocked()
    {
        _motorRunning = false;
        _slowMotor = false;
        _stylusPowered = false;
        _pendingSlowMotor = false;
        _hasPendingSlowMotorChange = false;
        _currentLineStarted = false;
        _lastWritePixel = -1;
        _lineTStateAccumulator = 0;
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
