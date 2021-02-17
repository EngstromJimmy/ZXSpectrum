using System;
using ZXBox.Snapshot;
using System.IO;

namespace ZXBox.Snapshot
{
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
                    default:
                        return null;
                }
            }
            return null;
        }
    }
}