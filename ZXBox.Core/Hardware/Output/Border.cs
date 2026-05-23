namespace ZXBox.Hardware.Output;

internal readonly struct Border
{
    public readonly uint ColorByte;
    public readonly int TState;

    public Border(uint ColorByte, int tState)
    {
        this.ColorByte = ColorByte;
        TState = tState;
    }
}
