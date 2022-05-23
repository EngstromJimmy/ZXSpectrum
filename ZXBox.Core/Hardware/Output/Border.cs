namespace ZXBox.Hardware.Output;

internal class Border
{
    public uint ColorByte;
    public int tState;
    public Border(uint ColorByte, int tState)
    {
        this.ColorByte = ColorByte;
        this.tState = tState;
    }
}
