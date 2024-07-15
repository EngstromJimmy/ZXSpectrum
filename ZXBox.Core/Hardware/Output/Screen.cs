using System.Collections.Generic;
using System.Drawing;
using ZXBox.Hardware.Interfaces;
namespace ZXBox.Hardware.Output;

public class ScreenAttribute
{
    public bool Flash { get; set; }
    public bool Bright { get; set; }
    public int Ink { get; set; } = 0;
    public int Paper { get; set; } = 7;
}

public class Screen : IOutput
{
    //32*24
    public ScreenAttribute[] ScreenAttributes { get; set; } = new ScreenAttribute[768];

    public void SwitchColors(bool switchColors)
    {
        if (switchColors)
        {

            colours = new uint[16]
            {
                0x000000FF, 0x0000cdFF, 0xcd0000FF, 0xcd00cdFF,
                0x00cd00FF, 0x00cdcdFF, 0xcdcd00FF, 0xcdcdcdFF,
                0x000000FF, 0x0000ffFF, 0xff0000FF, 0xff00ffFF,
                0x00ff00FF, 0x00ffffFF, 0xffff00FF, 0xffffffFF
            };
        }
        else
        {
            colours = new uint[16]
            {
                0xFF000000, 0xFFcd0000, 0xFF0000cd, 0xFFcd00cd,
                0xFF00cd00, 0xFFcdcd00, 0xFF00cdcd, 0xFFcdcdcd,
                0xFF000000, 0xFFff0000, 0xFF0000ff, 0xFFff00ff,
                0xFF00ff00, 0xFFffff00, 0xFF00ffff, 0xFFffffff
            };
        }
    }

    public void SetAttribute(int pos, bool flash, bool bright, int ink, int paper)
    {

        pos = pos - 0x5800;

        if (ScreenAttributes[pos].Bright != bright || ScreenAttributes[pos].Flash != flash || ScreenAttributes[pos].Ink != ink || ScreenAttributes[pos].Paper != paper)
        {
            ScreenAttributes[pos].Bright = bright;
            ScreenAttributes[pos].Flash = flash;
            ScreenAttributes[pos].Ink = ink;
            ScreenAttributes[pos].Paper = paper;

            y = (pos / 32) * 8;
            x = (pos * 8) - (y * 32);

            for (int yi = y; yi < y + 8; yi++)
            {
                for (int xi = x; xi < x + 8; xi++)
                {
                    UpdatePixel(xi, yi);
                }
            }
        }
    }

    public void SetPixels(int xpos, int ypos, byte b)
    {
        for (int i = 0; i < 8; i++)
        {
            pixels[(xpos + (ypos * 256)) + i] = (b >> 7 - i & 0x01) != 0 ? true : false;
            UpdatePixel(xpos + i, ypos);
        }
    }

    private void UpdatePixel(int xpos, int ypos)
    {
        int x = xpos + bordersides;
        int y = (ypos + bordertop) * (256 + bordersides + bordersides);

        var attr = ScreenAttributes[(xpos / 8) + ((ypos / 8) * 32)];

        var ink = colours[attr.Ink + (attr.Bright ? 8 : 0)];
        var paper = colours[attr.Paper + (attr.Bright ? 8 : 0)];

        screen[x + y] = pixels[xpos + (ypos * 256)] ? ink : paper;
        if (attr.Flash)
        {
            screenflash[x + y] = pixels[xpos + (ypos * 256)] ? paper : ink;
        }
        else
        {
            screenflash[x + y] = pixels[xpos + (ypos * 256)] ? ink : paper;
        }
    }

    public Screen(Zilog.Z80 cpu, bool renderBorder, bool switchColors, int borderTop = 48, int borderBottom = 56, int borderSide = 64)
    {
        for (int a = 0; a < ScreenAttributes.Length; a++)
        {
            ScreenAttributes[a] = new ScreenAttribute();
        }

        SwitchColors(switchColors);
        RenderBorder = renderBorder;

        if (renderBorder)
        {

            this.bordertop = borderTop;
            this.borderbottom = borderBottom;
            this.bordersides = borderSide;

        }
        else
        {
            bordertop = 0;
            borderbottom = 0;
            bordersides = 0;
        }

        Height = 192 + (bordertop + borderbottom);
        Width = 256 + (bordersides * 2);
        screen = new uint[Height * Width];
        screenflash = new uint[Height * Width];
        pixels = new bool[Height * Width];
        this.cpu = cpu;
    }

    uint[] colours;

    public Color GetBackgroundColor()
    {
        return Color.FromArgb(((int)colours[LastBorderColor] & 0xFF), ((int)colours[LastBorderColor] >> 8 & 0xFF), ((int)colours[LastBorderColor] >> 16 & 0xFF));
    }
    int bordercounter;
    private uint GetBorderColor(double tState)
    {
        if (tState == 0)
        {
            bordercounter = 0;
        }
        if ((border.Count - 1) > bordercounter)
        {
            if (border[bordercounter + 1].tState <= tState)
                bordercounter++;
        }
        if (bordercounter > border.Count - 1)
            bordercounter = border.Count - 1;
        if (bordercounter > 0)
        {
            LastBorderColor = border[bordercounter].ColorByte;
        }
        if (tState == 69887)
            border.Clear();
        if (border.Count == 0)
            return LastBorderColor;
        return colours[LastBorderColor];
    }

    private Zilog.Z80 cpu;
    private int bordertop = 20;
    private int borderbottom = 20;
    private int bordersides = 20;

    public int Height = 0;
    private bool RenderBorder;
    public int Width = 0;

    public double tstatesperpixel = 0.58;

    private uint[] screen = null;
    private uint[] screenflash = null;
    private bool[] pixels = null;
    private List<Border> border = new List<Border>();
    public uint LastBorderColor;

    public void Output(ushort Port, byte ByteValue, int tState)
    {
        if ((Port & 0x0001) == 0)
        {
            border.Add(new Border(((uint)ByteValue & 0x07), tState));
        }
    }

    /* drawScreen takes four parametres:
   *   screen - bitmap of the rendered screen
   *   flash - flash state, 0 for normal, non-zero for inverted.
  */

    int p = 0;
    public uint[] drawScreen(bool flash)
    {
        tstatesperpixel = (Height * Width) / 69888d;
        if (RenderBorder)
        {
            //Update border top
            for (p = 0; p < (bordertop * Width); p++)
            {
                screen[p] = GetBorderColor(p * tstatesperpixel);
                screenflash[p] = GetBorderColor(p * tstatesperpixel);
            }

            for (int py = bordertop; py < Height - borderbottom; py++)
            {
                for (int px = 0; px < bordersides; px++)
                {
                    var pixel = ((py * Width) + px);
                    screen[pixel] = GetBorderColor(pixel * tstatesperpixel);
                    screenflash[pixel] = GetBorderColor(pixel * tstatesperpixel);
                }

                for (int px = Width - bordersides; px < Width; px++)
                {
                    var pixel = ((py * Width) + px);
                    screen[pixel] = GetBorderColor(pixel * tstatesperpixel);
                    screenflash[pixel] = GetBorderColor(pixel * tstatesperpixel);
                }

            }

            for (p = (Width * (Height - borderbottom)); p < (Height * Width); p++)
            {
                screen[p] = GetBorderColor(p * tstatesperpixel);
                screenflash[p] = GetBorderColor(p * tstatesperpixel);
            }
        }
        border.Clear();
        border.Add(new Border(LastBorderColor, 0));

        if (flash)
        {
            return screenflash;
        }
        else
        {
            return screen;
        }
    }

    int x;
    int y;
}
