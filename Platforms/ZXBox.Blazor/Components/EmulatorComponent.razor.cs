using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Microsoft.JSInterop.WebAssembly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Timers;
using ZXBox.Hardware.Input;
using ZXBox.Hardware.Input.Joystick;
using ZXBox.Hardware.Output;
using ZXBox.Snapshot;

namespace ZXBox.Blazor.Pages
{
    public partial class EmulatorComponentModel : ComponentBase
    {
        private ZXSpectrum speccy;
        public System.Timers.Timer gameLoop;
        int flashcounter = 16;
        bool flash = false;
        JavaScriptKeyboard Keyboard = new JavaScriptKeyboard();
        Kempston kempston;
        Beeper<byte> beeper;

        [Inject]
        Toolbelt.Blazor.Gamepad.GamepadList GamePadList { get; set; }

        [Inject]
        protected HttpClient Http { get; set; }
        [Inject]
        protected IJSInProcessRuntime JSRuntime { get; set; }
        public EmulatorComponentModel()
        {
            gameLoop = new System.Timers.Timer(20);
            gameLoop.Elapsed += GameLoop_Elapsed;
        }

        public ZXSpectrum GetZXSpectrum(RomEnum rom)
        {
            return new ZXSpectrum(true, true, 20, 20, 20, rom);
        }

        public void StartZXSpectrum(RomEnum rom)
        {
            speccy = GetZXSpectrum(rom);
            speccy.InputHardware.Add(Keyboard);

            kempston = new Kempston();
            speccy.InputHardware.Add(kempston);
            //48000 samples per second, 50 frames per second (20ms per frame)
            beeper = new Beeper<byte>(0, 127, 48000 / 50, 1);
            speccy.OutputHardware.Add(beeper);
            mono = JSRuntime as WebAssemblyJSRuntime;
            speccy.Reset();
            gameLoop.Start();
        }

        public async Task HandleFileSelected(InputFileChangeEventArgs args)
        {
            var file = args.File;

            var ms = new MemoryStream();

            await file.OpenReadStream().CopyToAsync(ms);

            var handler = FileFormatFactory.GetSnapShotHandler(file.Name);
            var bytes = ms.ToArray();
            handler.LoadSnapshot(bytes, speccy);
        }
        [Inject]
        HttpClient httpClient { get; set; }
        public string Instructions = "";
        public async Task LoadGame(string filename, string instructions)
        {
            var ms = new MemoryStream();
            var handler = FileFormatFactory.GetSnapShotHandler(filename);
            var stream = await httpClient.GetStreamAsync("Roms/" + filename + ".json");
            await stream.CopyToAsync(ms);
            var bytes = ms.ToArray();
            handler.LoadSnapshot(bytes, speccy);
            Instructions = instructions;
        }

        private async void GameLoop_Elapsed(object sender, ElapsedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();

            //Get gamepads
            kempston.Gamepads = await GamePadList.GetGamepadsAsync();

            //Run JavaScriptInterop to find the currently pressed buttons
            Keyboard.KeyBuffer = await JSRuntime.InvokeAsync<List<string>>("getKeyStatus");
            sw.Start();
            speccy.DoIntructions(69888);

            beeper.GenerateSound();
            await BufferSound();

            Paint();
            sw.Stop();
            if (sw.ElapsedMilliseconds > 20)
            {
                Console.WriteLine(sw.ElapsedMilliseconds + "ms");
            }
        }

        GCHandle gchsound;
        IntPtr pinnedsound;
        WebAssemblyJSRuntime mono;
        byte[] soundbytes;
        protected async Task BufferSound()
        {
            soundbytes = beeper.GetSoundBuffer();
            Console.Write(string.Join(',', soundbytes.Select(s => s.ToString())));
            gchsound = GCHandle.Alloc(soundbytes, GCHandleType.Pinned);
            pinnedsound = gchsound.AddrOfPinnedObject();
            mono.InvokeUnmarshalled<IntPtr, string>("addAudioBuffer", pinnedsound);
            gchsound.Free();
        }

        protected async override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeAsync<bool>("InitCanvas");
            }
            base.OnAfterRender(firstRender);
        }

        GCHandle gchscreen;
        IntPtr pinnedscreen;
        //uint[] screen = new uint[68672]; //Height * width (256+20+20)*(192+20+20)
        public async void Paint()
        {
            if (flashcounter == 0)
            {
                flashcounter = 16;
                flash = !flash;
            }
            else
            {
                flashcounter--;
            }

            var screen = speccy.GetScreenInUint(flash);

            //Allocate memory
            gchscreen = GCHandle.Alloc(screen, GCHandleType.Pinned);
            pinnedscreen = gchscreen.AddrOfPinnedObject();
            mono.InvokeUnmarshalled<IntPtr, string>("PaintCanvas", pinnedscreen);
            gchscreen.Free();
        }
    }
}