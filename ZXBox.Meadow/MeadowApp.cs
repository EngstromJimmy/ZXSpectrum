using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Novell.Directory.Ldap.Extensions;
using System;
using System.Collections.Generic;
using System.ServiceModel.Discovery.VersionApril2005;
using System.Threading;

namespace ZXBox.MeadowApp
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbPwmLed onboardLed;
        ZXBox.MeadowApp.Display.Ssd1351 display;
        GraphicsLibrary graphics;

        ZXSpectrum speccy;

        public MeadowApp()
        {
            Initialize();
            RunEmulator();   
        }
    

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);

            var spiBus = Device.CreateSpiBus();

            display = new ZXBox.MeadowApp.Display.Ssd1351(
              device: Device,
              spiBus: spiBus,
              chipSelectPin: Device.Pins.D00,
              dcPin: Device.Pins.D02,
              resetPin: Device.Pins.D01,
              width: 128, height: 127);

            
            //graphics.Clear();
            
            //graphics.Show();
            speccy = new ZXSpectrum(true, true,32,32,0);
            speccy.Reset();
            graphics = new GraphicsLibrary(display);
            display.Clear(true);
            display.ClearScreen(1);
            display.InvertDisplay(false);
            graphics.DrawRectangle(0, 0, 127, 127, Color.FromHex("#00cdcd"), true);
            graphics.Show();

        }

        List<byte> bitmap = new List<byte>();
        void RunEmulator()
        {
            Console.WriteLine("Starting emulator");
            while (true)
            {
                try
                {
                    speccy.DoIntructions(69888*50);
                    Console.WriteLine("OnCycle");
                    var screen = speccy.GetScreenInUint(true);
                    Console.WriteLine("Got screen");
                    
                    //graphics.Clear();
                    for (int y = 0; y < 128; y++)
                    {
                        for (int x = 0; x < 128; x++)
                        {
                            graphics.DrawPixel(x, y, Color.FromUint(screen[(y*256*2)+x*2]));                            
                        }
                    }
                    graphics.Show();
                    Console.WriteLine("Showing screen");
                    
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

        }

        void CycleColors(int duration)
        {
            Console.WriteLine("Cycle colors...");

            while (true)
            {
                ShowColorPulse(Color.Blue, duration);
                ShowColorPulse(Color.Cyan, duration);
                ShowColorPulse(Color.Green, duration);
                ShowColorPulse(Color.GreenYellow, duration);
                ShowColorPulse(Color.Yellow, duration);
                ShowColorPulse(Color.Orange, duration);
                ShowColorPulse(Color.OrangeRed, duration);
                ShowColorPulse(Color.Red, duration);
                ShowColorPulse(Color.MediumVioletRed, duration);
                ShowColorPulse(Color.Purple, duration);
                ShowColorPulse(Color.Magenta, duration);
                ShowColorPulse(Color.Pink, duration);
            }
        }

        void ShowColorPulse(Color color, int duration = 1000)
        {
            onboardLed.StartPulse(color, (duration / 2));
            Thread.Sleep(duration);
            onboardLed.Stop();
        }

        void ShowColor(Color color, int duration = 1000)
        {
            Console.WriteLine($"Color: {color}");
            onboardLed.SetColor(color);
            Thread.Sleep(duration);
            onboardLed.Stop();
        }
    }
}
