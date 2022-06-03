using System;
using System.IO;
namespace ZXBox;

/// <summary>
/// Summary description for binary.
/// </summary>
public class Binary
{
    private BinaryReader br;
    private FileStream fs;
    private string filename;
    public byte[] bytes;

    private int _byteposition = 0;
    public int BytePosition
    {
        get { return _byteposition; }
    }

    public Binary(string infilename)
    {
        filename = infilename;
    }

    public void Open()
    {
        fs = new FileStream(filename, FileMode.Open);
        br = new BinaryReader(fs);
        bytes = new byte[br.BaseStream.Length];
        br.Read(bytes, 0, Convert.ToInt32(br.BaseStream.Length));
    }
    public void Close()
    {
        fs.Close();
        br.Close();
    }

    public byte[] Readbytes(int position, int length)
    {
        SetPosition(position);
        //move position
        return ReadNextbytes(length);

        //br.BaseStream.Position=position;
        //return br.ReadBytes(length);
    }

    public void SetPosition(int position)
    {
        _byteposition = position;
        //br.BaseStream.Position=position;
    }

    public byte[] ReadNextbytes(int length)
    {
        byte[] b = new byte[length];
        for (int a = 0; a < length; a++)
            b[a] = bytes[_byteposition++];
        return b;

        //return br.ReadBytes(length);
    }

    public char[] ReadChars(int position, int length)
    {
        _byteposition = position;
        char[] c = new char[length];
        for (int a = 0; a < length; a++)
            c[a] = (char)bytes[_byteposition++];
        return c;
        //br.BaseStream.Position=position;
        //return br.ReadChars(length);
    }

    public long Lenght()
    {
        return bytes.Length;
        //return br.BaseStream.Length;
    }
    /// <summary>
    /// 2 bytes
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    int GetIntelWord()
    {
        _byteposition += 2;
        return bytes[_byteposition - 2] | (bytes[_byteposition - 1] << 8);
    }

    /// <summary>
    /// 4 bytes
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    int GetIntelDWord()
    {
        _byteposition += 4;
        return bytes[_byteposition - 4] | (bytes[_byteposition - 3] << 8) | (bytes[_byteposition - 2] << 16) | (bytes[_byteposition - 1] << 24);
    }

}
