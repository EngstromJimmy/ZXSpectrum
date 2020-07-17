using System;
using System.Collections.Generic;
using System.Drawing;
using ZXBox.Hardware.Interfaces;
namespace ZXBox.Hardware.Output
{
    public class Screen : 
#if NETFX_CORE
    ZXBox_Core.IOutput   
#else
    IOutput
#endif
    {
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

                //colours = new uint[16]
                //{
                //    0xFF000000, 0xFF0000cd, 0xFFcd0000, 0xFFcd00cd,
                //    0xFF00cd00, 0xFF00cdcd, 0xFFcdcd00, 0xFFcdcdcd,
                //    0xFF000000, 0xFF0000ff, 0xFFff0000, 0xFFff00ff,
                //    0xFF00ff00, 0xFF00ffff, 0xFFffff00, 0xFFffffff
                //};
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


        public Screen(Zilog.Z80 cpu,bool renderBorder,bool switchColors,int borderTop=48,int borderBottom=56,int borderSide=64)
        {
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

            this.cpu = cpu;
        }

        uint[] colours;


        public Color GetBackgroundColor()
        {
            return Color.FromArgb(((int)colours[LastBorderColor] & 0xFF), ((int)colours[LastBorderColor]>>8 & 0xFF), ((int)colours[LastBorderColor]>>16 & 0xFF));
        }
        int bordercounter;
        private uint GetBorderColor(double tState)
        {
            if (RenderBorder)
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
                if (bordercounter > border.Count - 1 )
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
            return colours[0];
        }

        
#if NETFX_CORE
        private ZXBox_Core.ZXSpectrum48 cpu;
#else
        private Zilog.Z80 cpu;
#endif
        private int bordertop = 20;
        private int borderbottom = 20;
        private int bordersides = 20;

        
        public int Height = 0;
        private bool RenderBorder;
        public int Width =0;

        public double tstatesperpixel =  0.58; 

        private uint[] screen = null;
        //private int border;
        private List<Border> border = new List<Border>();
        public uint LastBorderColor;

        public void Output(int Port, int ByteValue, int tState)
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
            
        public uint[] drawScreen(bool flash)
        {
            /* RGB palette entries for Spectrums 8 colours in normal and bright mode */
            tstate = 0;
            pixmapIndex = 0;
            bordercounter = 0;
            foreground = colours[0];
            background = colours[15];

            /* Fill in top part of screen with border colour */
            for (x = Width * bordertop; x-- > 0; )
                screen[pixmapIndex++] = GetBorderColor(tstate += tstatesperpixel);

            for (attributeIndex = screenIndex + 24 * 32 * 8, pixelIndex = screenIndex, y = 0; y < 192; )
            {
                /* Fill in left part of border with border colour */

                for (x = bordersides; x-- > 0; pixmapIndex++)
                    screen[pixmapIndex] = GetBorderColor(tstate += tstatesperpixel);


                /* Calculate start address of pixel row, the Spectrums video ram is
                 * connected to the screen handler with a few wires crossed, bit order
                 * is 7621 0543 instead of 7654 3210. */

                pixelIndex = screenIndex + ((((y & 0x7) << 3) | ((y & 0x38) >> 3) | (y & 0xc0)) << 5);

                /* Loop through the 32 bytes in a pixel row */

                for (x = 32; x-- > 0; )
                {
                    /* Set background and foreground colours for this byte */

                    if (!flash || (cpu.ReadByteFromMemory(attributeIndex) & 0x80) == 0)
                    {
                        /* Three bit foreground colour is encoded in bits 0..2.
                         * The "bright" attribute is in bit 6. */

                        foreground = colours[(cpu.ReadByteFromMemory(attributeIndex) & 0x7) + (((cpu.ReadByteFromMemory(attributeIndex) & 0x40) != 0) ? 8 : 0)];

                        /* Three bit background colour is encoded in bits 3..5.
                         * "Bright" attribute still in bit 6. */

                        background = colours[((cpu.ReadByteFromMemory(attributeIndex) >> 3) & 0x7) + (((cpu.ReadByteFromMemory(attributeIndex) & 0x40) != 0) ? 8 : 0)];
                    }
                    else
                    {
                        /* Three bit foreground colour is encoded in bits 0..2.
                         * However, we are in the inverse flash phase... */

                        background = colours[(cpu.ReadByteFromMemory(attributeIndex) & 0x7) + (((cpu.ReadByteFromMemory(attributeIndex) & 0x40) != 0) ? 8 : 0)];

                        /* Three bit background colour is encoded in bits 3..5.
                         * The inverse flash phase makes this the foreground. */

                        foreground = colours[((cpu.ReadByteFromMemory(attributeIndex) >> 3) & 0x7) + (((cpu.ReadByteFromMemory(attributeIndex) & 0x40) != 0) ? 8 : 0)];
                    }

                    /* We are done with this attribute, advance to next byte */

                    attributeIndex++;

                    /* Loop through the pixels in the current pixel byte and add them to the
                     * output pixmap*/

                    for (pixelMask = 0x80, pixelByte = cpu.ReadByteFromMemory(pixelIndex++); pixelMask != 0; pixelMask >>= 1)
                    {
                        screen[pixmapIndex++] = ((pixelMask & pixelByte) != 0) ? foreground : background;
                        //pixmapIndex += 2;
                        tstate += tstatesperpixel;
                    }
                }

                /* Add 32 pixels of border colour to both pixel rows */

                for (x = bordersides; x-- > 0; pixmapIndex++)
                    screen[pixmapIndex] = GetBorderColor(tstate += tstatesperpixel);

                /* Skip next pixel row as we have already filled it */

                //pixmapIndex += width;

                /* Unless the next row is a multiple of 8, we need to rewind the attribute
                 * pointer to the beginning of the pixel row. */

                if ((++y & 0x7) != 0)
                    attributeIndex -= 32;
            }

            /* Fill in bottom part of screen with border colour */

            for (x = Width * borderbottom; x-- > 0; )
                screen[pixmapIndex++] = GetBorderColor(tstate += tstatesperpixel);

            border.Clear();
            border.Add(new Border(LastBorderColor,0));
            return screen;

        }



        int pixelIndex, attributeIndex;
        int x, pixelByte, pixelMask;
        uint foreground, background;
        int y;
        double tstate = 0;
        int pixmapIndex = 0;
        int screenIndex = 16 * 1024;

        //public void drawScreen(Span<int> screen,bool flash)
        //{
        //    tstate = 0;
        //    pixmapIndex = 0;
        //    /* RGB palette entries for Spectrums 8 colours in normal and bright mode */
        //    bordercounter = 0;

        //    foreground = colours[0];
        //    background = colours[15];

        //    /* Fill in top part of screen with border colour */
        //    for (x = Width * bordertop; x-- > 0; )
        //        screen[pixmapIndex++] = (int)GetBorderColor(tstate += tstatesperpixel);

        //    for (attributeIndex = screenIndex + 24 * 32 * 8, pixelIndex = screenIndex, y = 0; y < 192; )
        //    {
        //        /* Fill in left part of border with border colour */

        //        for (x = bordersides; x-- > 0; pixmapIndex++)
        //            screen[pixmapIndex] = (int)GetBorderColor(tstate += tstatesperpixel);


        //        /* Calculate start address of pixel row, the Spectrums video ram is
        //         * connected to the screen handler with a few wires crossed, bit order
        //         * is 7621 0543 instead of 7654 3210. */

        //        pixelIndex = screenIndex + ((((y & 0x7) << 3) | ((y & 0x38) >> 3) | (y & 0xc0)) << 5);

        //        /* Loop through the 32 bytes in a pixel row */

        //        for (x = 32; x-- > 0; )
        //        {
        //            /* Set background and foreground colours for this byte */

        //            if (!flash || (cpu.ReadByteFromMemory(attributeIndex) & 0x80) == 0)
        //            {
        //                /* Three bit foreground colour is encoded in bits 0..2.
        //                 * The "bright" attribute is in bit 6. */

        //                foreground = colours[(cpu.ReadByteFromMemory(attributeIndex) & 0x7) + (((cpu.ReadByteFromMemory(attributeIndex) & 0x40) != 0) ? 8 : 0)];

        //                /* Three bit background colour is encoded in bits 3..5.
        //                 * "Bright" attribute still in bit 6. */

        //                background = colours[((cpu.ReadByteFromMemory(attributeIndex) >> 3) & 0x7) + (((cpu.ReadByteFromMemory(attributeIndex) & 0x40) != 0) ? 8 : 0)];
        //            }
        //            else
        //            {
        //                /* Three bit foreground colour is encoded in bits 0..2.
        //                 * However, we are in the inverse flash phase... */

        //                background = colours[(cpu.ReadByteFromMemory(attributeIndex) & 0x7) + (((cpu.ReadByteFromMemory(attributeIndex) & 0x40) != 0) ? 8 : 0)];

        //                /* Three bit background colour is encoded in bits 3..5.
        //                 * The inverse flash phase makes this the foreground. */

        //                foreground = colours[((cpu.ReadByteFromMemory(attributeIndex) >> 3) & 0x7) + (((cpu.ReadByteFromMemory(attributeIndex) & 0x40) != 0) ? 8 : 0)];
        //            }

        //            /* We are done with this attribute, advance to next byte */

        //            attributeIndex++;

        //            /* Loop through the pixels in the current pixel byte and add them to the
        //             * output pixmap*/

        //            for (pixelMask = 0x80, pixelByte = cpu.ReadByteFromMemory(pixelIndex++); pixelMask != 0; pixelMask >>= 1)
        //            {
        //                screen[pixmapIndex++] = (int)(((pixelMask & pixelByte) != 0) ? foreground : background);
        //                //pixmapIndex += 2;
        //                tstate += tstatesperpixel;
        //            }
        //        }

        //        /* Add 32 pixels of border colour to both pixel rows */

        //        for (x = bordersides; x-- > 0; pixmapIndex++)
        //            screen[pixmapIndex] = (int)GetBorderColor(tstate += tstatesperpixel);

        //        /* Skip next pixel row as we have already filled it */

        //        //pixmapIndex += width;

        //        /* Unless the next row is a multiple of 8, we need to rewind the attribute
        //         * pointer to the beginning of the pixel row. */

        //        if ((++y & 0x7) != 0)
        //            attributeIndex -= 32;
        //    }

        //    /* Fill in bottom part of screen with border colour */

        //    for (x = Width * borderbottom; x-- > 0; )
        //        screen[pixmapIndex++] = (int)GetBorderColor(tstate += tstatesperpixel);

        //    border.Clear();
        //    border.Add(new Border(LastBorderColor, 0));
        //}
    }
}