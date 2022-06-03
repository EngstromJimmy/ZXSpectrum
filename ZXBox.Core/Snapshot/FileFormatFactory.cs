using System.IO;

namespace ZXBox.Snapshot;

public class FileFormatFactory
{
    public static ISnapshot GetSnapShotHandler(string filename)
    {
        if (filename != null)
        {
            switch (Path.GetExtension(filename).ToLower())
            {
                case ".z80":
                    return new Z80FileFormat();
                case ".sna":
                    return new SNAFileFormat();
                case ".gb":
                    return new GBFileFormat();
                default:
                    return null;
            }
        }
        return null;
    }
}