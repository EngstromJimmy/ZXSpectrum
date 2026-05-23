namespace ZXBox.Core.Tape;

public static class TapeFormatFactory
{
    public static TapeImage ReadTape(byte[] data, string fileName = null)
    {
        if (TzxFormat.IsTzx(data, fileName))
        {
            return new TzxFormat().ReadFile(data);
        }

        return new TapFormat().ReadFile(data);
    }

    public static bool IsSupportedTapeFile(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return false;
        }

        var extension = System.IO.Path.GetExtension(fileName).ToLowerInvariant();
        return extension is ".tap" or ".tzx";
    }
}
