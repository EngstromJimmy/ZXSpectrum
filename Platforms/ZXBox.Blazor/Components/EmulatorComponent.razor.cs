using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Microsoft.JSInterop.WebAssembly;
using SkiaSharp.Views.Blazor;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Timers;
using ZXBox.Core.Hardware.Input;
using ZXBox.Hardware.Input;
using ZXBox.Hardware.Input.Joystick;
using ZXBox.Hardware.Output;
using ZXBox.Snapshot;

namespace ZXBox.Blazor.Pages
{
    public partial class EmulatorComponentModel : ComponentBase, IAsyncDisposable
    {
        private ZXSpectrum speccy;
        public System.Timers.Timer gameLoop;
        int flashcounter = 16;
        bool flash = false;
        JavaScriptKeyboard Keyboard = new();
        Kempston kempston;
        Beeper<byte> beeper;
        public TapePlayer tapePlayer;
        public SKCanvasView _canvasView;

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
            //48000 samples per second, 50 frames per second (20ms per frame) Mono
            beeper = new Beeper<byte>(0, 127, 48000 / 50, 1);
            speccy.OutputHardware.Add(beeper);
            tapePlayer = new(beeper);
            speccy.InputHardware.Add(tapePlayer);
            mono = JSRuntime as WebAssemblyJSRuntime;
            speccy.Reset();
            gameLoop.Start();
        }

        public string TapeName { get; set; }
        public async Task HandleFileSelected(InputFileChangeEventArgs args)
        {
            if (args.File.Name.ToLower().EndsWith(".tap"))
            {
                //Load the tape
                var file = args.File;
                var ms = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(ms);
                tapePlayer.LoadTape(ms.ToArray());
                TapeName = Path.GetFileNameWithoutExtension(args.File.Name);
            }
            else
            {
                var file = args.File;
                var ms = new MemoryStream();

                await file.OpenReadStream().CopyToAsync(ms);

                var handler = FileFormatFactory.GetSnapShotHandler(file.Name);
                var bytes = ms.ToArray();
                handler.LoadSnapshot(bytes, speccy);
            }
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
            if (tapePlayer != null && tapePlayer.IsPlaying)
            {
                TapeStopped = false;
                PercentLoaded = ((Convert.ToDouble(tapePlayer.CurrentTstate) / Convert.ToDouble(tapePlayer.TotalTstates)) * 100);
                await InvokeAsync(() => StateHasChanged());
            }
            if (!TapeStopped && !tapePlayer.IsPlaying)
            {
                TapeStopped = true;
                await InvokeAsync(() => StateHasChanged());
            }
        }
        bool TapeStopped = false;
        GCHandle gchsound;
        IntPtr pinnedsound;
        WebAssemblyJSRuntime mono;
        byte[] soundbytes;
        protected async Task BufferSound()
        {
            soundbytes = beeper.GetSoundBuffer();
            gchsound = GCHandle.Alloc(soundbytes, GCHandleType.Pinned);
            pinnedsound = gchsound.AddrOfPinnedObject();
            mono.InvokeUnmarshalled<IntPtr, string>("addAudioBuffer", pinnedsound);
            gchsound.Free();
        }

        public double PercentLoaded = 0;
        protected async override void OnAfterRender(bool firstRender)
        {

            base.OnAfterRender(firstRender);
        }

        GCHandle gchscreen;
        IntPtr pinnedscreen;
      
        uint[] screen = new uint[68672]; //Height * width (256+20+20)*(192+20+20)
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

             screen = speccy.GetScreenInUint(flash);
 
            ////Allocate memory
            //gchscreen = GCHandle.Alloc(screen, GCHandleType.Pinned);
            //pinnedscreen = gchscreen.AddrOfPinnedObject();
            //mono.InvokeUnmarshalled<IntPtr, string>("PaintCanvas", pinnedscreen);
            //gchscreen.Free();

            _canvasView?.Invalidate();
        }

        SKBitmap bitmap = new SKBitmap(296, 232);

        //SKPaint paint = new SKPaint
        //{
        //    FilterQuality = SKFilterQuality.High, // High-quality filter for smoother rendering
        //    IsAntialias = true // Additional anti-aliasing if necessary
        //};
        public void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
         
            var canvas = e.Surface.Canvas;
            unsafe
            {
                var ptr = (uint*)bitmap.GetPixels().ToPointer();

                fixed (uint* srcPtr = screen)
                {
                    // Use Buffer.MemoryCopy for fast memory copying
                    Buffer.MemoryCopy(srcPtr, ptr, screen.Length * sizeof(uint), screen.Length * sizeof(uint));
                }
            }
          
            // Draw the bitmap onto the canvas
            canvas.DrawBitmap(bitmap, new SKRect(0, 0, e.Info.Width, e.Info.Height)); 
        }

        public ValueTask DisposeAsync()
        {
            gameLoop.Stop();
            gameLoop.Dispose();
            return ValueTask.CompletedTask;
        }
    }
}