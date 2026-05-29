#nullable enable

using System;

namespace ZXBox.Hardware.Speech;

public sealed class CurrahMicroSpeech
{
    public const int CurrahRomSize = 0x800;

    private const int ToggleAddress = 0x0038;
    private const int LowerRomEnd = 0x1000;
    private const int ActiveWindowEnd = 0x4000;
    private const int PlayAddressMask = 0xF000;
    private const int PlayAddressValue = 0x1000;
    private const int IntonationAddressMask = 0xF000;
    private const int IntonationAddressValue = 0x3000;

    private readonly byte[] _rom = new byte[CurrahRomSize];
    private readonly Sp0256Chip _speechChip = new();

    public bool Connected { get; private set; }

    public bool Active { get; private set; }

    public bool HasRom { get; private set; }

    public bool HasSpeechRom => _speechChip.HasRom;

    public CurrahMicroSpeechIntonation Intonation { get; private set; } = CurrahMicroSpeechIntonation.Normal;

    public void Connect()
    {
        Connected = true;
        ResetRuntime();
    }

    public void Disconnect()
    {
        Connected = false;
        ResetRuntime();
    }

    public void ResetRuntime()
    {
        Active = false;
        Intonation = CurrahMicroSpeechIntonation.Normal;
        _speechChip.ResetRuntime();
        _speechChip.SetClock(Sp0256Chip.NormalClockHz, 0);
    }

    public void LoadRom(byte[] romBytes)
    {
        ArgumentNullException.ThrowIfNull(romBytes);

        if (romBytes.Length < CurrahRomSize)
        {
            throw new ArgumentException($"Currah ROM must be at least {CurrahRomSize} bytes.", nameof(romBytes));
        }

        Array.Copy(romBytes, _rom, CurrahRomSize);
        HasRom = true;
    }

    public void LoadSpeechRom(byte[] romBytes)
    {
        _speechChip.LoadRom(romBytes);
        _speechChip.SetClock(GetClockHz(Intonation), 0);
    }

    public void ClearRom()
    {
        Array.Clear(_rom);
        HasRom = false;
    }

    public void ClearSpeechRom()
    {
        _speechChip.ClearRom();
    }

    public bool TryReadMemory(int address, int frameTState, out int value)
    {
        value = 0;

        if (!Connected)
        {
            return false;
        }

        address &= 0xffff;
        ToggleForReadAccess(address);

        if (!Active)
        {
            return false;
        }

        return TryReadMappedMemory(address, frameTState, out value);
    }

    public bool TryReadOpcodeFetch(int address, int frameTState, out int value)
    {
        value = 0;

        if (!Connected)
        {
            return false;
        }

        address &= 0xffff;
        ToggleForReadAccess(address);

        if (!Active)
        {
            return false;
        }

        return TryReadMappedMemory(address, frameTState, out value);
    }

    public bool HandleMemoryWrite(int address, int value, int frameTState)
    {
        if (!Connected)
        {
            return false;
        }

        address &= 0xffff;
        value &= 0xff;

        if (!Active)
        {
            return address < ActiveWindowEnd;
        }

        if (IsPlayAddress(address))
        {
            var allophone = (byte)(value & 0x3f);
            _speechChip.WriteAllophone(allophone, frameTState);
            return true;
        }

        if (IsIntonationAddress(address))
        {
            Intonation = GetIntonation(address);
            _speechChip.SetClock(GetClockHz(Intonation), frameTState);
            return true;
        }

        return address < ActiveWindowEnd;
    }

    public bool TryReadPort(int port, int frameTState, out int value)
    {
        value = 0xff;

        if (!Connected)
        {
            return false;
        }

        port &= 0xffff;
        if (port == ToggleAddress)
        {
            Toggle();
        }

        if (!Active)
        {
            return false;
        }

        if (IsPlayAddress(port))
        {
            value = _speechChip.ReadBusy(frameTState) ? 1 : 0;
            return true;
        }

        return false;
    }

    public bool HandlePortWrite(int port, int value, int frameTState)
    {
        if (!Connected)
        {
            return false;
        }

        port &= 0xffff;
        value &= 0xff;

        if (port == ToggleAddress)
        {
            Toggle();
            return true;
        }

        if (!Active)
        {
            return false;
        }

        if (IsPlayAddress(port))
        {
            var allophone = (byte)(value & 0x3f);
            _speechChip.WriteAllophone(allophone, frameTState);
            return true;
        }

        if (IsIntonationAddress(port))
        {
            Intonation = GetIntonation(port);
            _speechChip.SetClock(GetClockHz(Intonation), frameTState);
            return true;
        }

        return false;
    }

    public float[] RenderAudioFrame(int samplesPerFrame, int tStatesPerFrame)
    {
        var output = samplesPerFrame <= 0 ? Array.Empty<float>() : new float[samplesPerFrame];
        RenderAudioFrame(output, tStatesPerFrame);
        return output;
    }

    public void RenderAudioFrame(Span<float> destination, int tStatesPerFrame)
    {
        if (!Connected)
        {
            destination.Clear();
            return;
        }

        _speechChip.RenderFrame(destination, tStatesPerFrame);
    }

    private void Toggle()
    {
        if (Connected)
        {
            Active = !Active;
        }
    }

    private void ToggleForReadAccess(int address)
    {
        if (address == ToggleAddress)
        {
            Toggle();
        }
    }

    private bool TryReadMappedMemory(int address, int frameTState, out int value)
    {
        value = 0;

        if (!Connected || !Active)
        {
            return false;
        }

        if (IsPlayAddress(address))
        {
            value = _speechChip.ReadBusy(frameTState) ? 1 : 0;
            return true;
        }

        if (!HasRom)
        {
            return false;
        }

        if (address < LowerRomEnd)
        {
            value = _rom[address & 0x07ff];
            return true;
        }

        if (address < ActiveWindowEnd)
        {
            value = 0xff;
            return true;
        }

        return false;
    }

    private static bool IsPlayAddress(int address)
    {
        return (address & PlayAddressMask) == PlayAddressValue;
    }

    private static bool IsIntonationAddress(int address)
    {
        return (address & IntonationAddressMask) == IntonationAddressValue;
    }

    private static CurrahMicroSpeechIntonation GetIntonation(int address)
    {
        return (address & 0x01) == 0
            ? CurrahMicroSpeechIntonation.Normal
            : CurrahMicroSpeechIntonation.High;
    }

    private static int GetClockHz(CurrahMicroSpeechIntonation intonation)
    {
        return intonation == CurrahMicroSpeechIntonation.Normal
            ? Sp0256Chip.NormalClockHz
            : Sp0256Chip.HighClockHz;
    }
}

public enum CurrahMicroSpeechIntonation
{
    Normal,
    High
}
