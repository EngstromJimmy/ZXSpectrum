using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.JSInterop.WebAssembly;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Schema;
using Toolbelt.Blazor.Gamepad;
using ZXBox.Hardware.Input;
using ZXBox.Hardware.Input.Joystick;
using ZXBox.Hardware.Output;
using ZXBox.Snapshot;

namespace ZXBox.Blazor.Pages
{
    public partial class EmulatorComponentModel:ComponentBase
    {
        private ZXSpectrum speccy;
        System.Timers.Timer gameLoop;
        int flashcounter = 16;
        bool flash=false;
        JavaScriptKeyboard Keyboard = new JavaScriptKeyboard();
        Kempston kempston;
        Beeper<byte> beeper;
        [Inject]
        Toolbelt.Blazor.Gamepad.GamepadList GamePadList { get; set; }

        [Inject]
        protected HttpClient Http { get; set; }
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        public EmulatorComponentModel()
        {
            speccy = new ZXSpectrum(true, true,20,20,20);
            gameLoop = new System.Timers.Timer(20);
            gameLoop.Elapsed += GameLoop_Elapsed;
        }

        public async Task HandleFileSelected(IFileListEntry[] files)
        {
            var file = files.First();

            var ms = new MemoryStream();
            
            await file.Data.CopyToAsync(ms);

            var handler = FileFormatFactory.GetSnapShotHandler(file.Name);
            var bytes = ms.ToArray();
            handler.LoadSnapshot(bytes, speccy);
         }
        [Inject]
        HttpClient httpClient { get; set; }
        public string Instructions = "";
        public async Task LoadGame(string filename,string instructions)
        {
            var ms = new MemoryStream();
            var handler = FileFormatFactory.GetSnapShotHandler(filename);
            var stream = await httpClient.GetStreamAsync("/Roms/" + filename + ".json");
            await stream.CopyToAsync(ms);
            var bytes = ms.ToArray();
            handler.LoadSnapshot(bytes, speccy);
            Instructions = instructions;
        }


        protected async override Task OnInitializedAsync()
        {
            gameLoop.Start();
            speccy.InputHardware.Add(Keyboard);
            
            kempston = new Kempston();
            speccy.InputHardware.Add(kempston);

            //beeper = new Beeper<byte>(128, 255, 48000/50, 1);
            //speccy.OutputHardware.Add(beeper);

            speccy.Reset();
            await base.OnInitializedAsync();
        }

        private async void GameLoop_Elapsed(object sender, ElapsedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            
            kempston.Gamepads = await GamePadList.GetGamepadsAsync();
            Keyboard.KeyBuffer = await JSRuntime.InvokeAsync<List<string>>("getKeyStatus");

            //for (int i= 0;i < 1;i++)
            //{
            
            speccy.DoIntructions(69888);
            
            //beeper.GenerateSound();
            
            //}
            
            //await BufferSound();
            sw.Start();
            Paint();
            sw.Stop();
            Debug.WriteLine(sw.ElapsedMilliseconds);
        }

        protected async Task BufferSound()
        {
            var soundbytes = beeper.GetSoundBuffer();
            var gch = GCHandle.Alloc(soundbytes, GCHandleType.Pinned);
            var pinned = gch.AddrOfPinnedObject();
            var mono = JSRuntime as WebAssemblyJSRuntime;
            mono.InvokeUnmarshalled<IntPtr, string>("addAudioBuffer", pinned);
            gch.Free();
        }

        protected async override void OnAfterRender(bool firstRender)
        {
            await JSRuntime.InvokeAsync<bool>("InitCanvas");
            base.OnAfterRender(firstRender);
        }

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
            var gch = GCHandle.Alloc(screen, GCHandleType.Pinned);
            var pinned = gch.AddrOfPinnedObject();
            var mono = JSRuntime as WebAssemblyJSRuntime;
            mono.InvokeUnmarshalled<IntPtr,string>("PaintCanvas",pinned);
            gch.Free();
        }
    }
}