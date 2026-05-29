# ZXSpectrum emulator written in C#

## In this repo

In this repo, you will find:

* ZX Spectrum emulator written in C#.
* Blazor implementation of the emulator
* 48K and 128K Spectrum support
* Blazor audio output for the beeper, 128K AY sound, and Currah uSpeech
* Snapshot support for SNA and Z80
* Tape loading and playback for TAP and TZX files
* Kempston joystick support through a gamepad
* Connectable Currah uSpeech and ZX Printer peripherals

## Sponsors
Thanks you to much to my sponsors!  
![](https://raw.githubusercontent.com/EngstromJimmy/Blazm.Components/master/Display%20Ads%20Horizontal%20Leaderboard%20728x90%20TOP_RITM0148003.png)


Telerik UI for Blazor – Increase productivity and cut cost in half! Use the Telerik truly native Blazor UI components and high-performing grid to cover any app scenario. [Give it a try for free.](https://www.telerik.com/blazor-ui?utm_source=jimmyengstrom&utm_medium=cpm&utm_campaign=blazor-trial-github-blazmcomp-sponsored-message )


## Background

When I was 7 years old I got my first computer, a ZX Spectrum.  
I remember that I sat down, booted it up, and wrote:

10 PRINT "Jimmy"  
20 GOTO 10

This was MY code!, I made the computer do things. 
That was the moment I decided I wanted to become a developer.
You can find it here on Github (Firstapp.z80).

After becoming a developer I wanted to see if I could make an emulator written in C#. This has become my test project, the thing I try out all new technologies with.
If you ever see me present, chances are that I will mention the ZX Spectrum.

The naming of the project might seem strange, I originally developed the emulator for Xbox so ZXBox made sense at the time.  
I have chosen to keep the name think of is as a ZX(Spectrum) in an other box =).

I did a talk about Blazor at Microsoft Ignite 2019 where I demoed my ZX Spectrum emulator running on Blazor WebAssembly. After doing a talk on Blazor and Blutooth at Live Coders Conf, I got a lot of comments on my ZX Spectrum so I decided to publish it =)  
You can find the Blazor implementation here http://zxbox.com .  
I got a lot of amazing feedback and many wanted to see the code so I decided to share that as well.

Current emulator support includes:

* ZX Spectrum 48K and 128K models
* Beeper audio in Blazor
* 128K AY-3-8912 sound
* Currah uSpeech with bundled Currah and SP0256 ROM assets
* TAP and TZX loading with tape playback
* SNA and Z80 snapshot loading
* Kempston joystick through an Xbox-compatible gamepad
* ZX Printer emulation with a live paper preview in the Blazor UI

There are still things left to do, especially around adding more file formats and improving hardware accuracy even further.

## Building

### Blazor

1. Install the .NET 10 SDK and restore workloads with `dotnet workload restore`.
2. Run `dotnet run --project Platforms\ZXBox.Blazor\ZXBox.Blazor.csproj`.

### MonoGame

1. Install the .NET 10 SDK.
2. Run `dotnet run --project Platforms\ZXBox.Monogame\ZXBox.Monogame.csproj`.

### Benchmarks

1. Run `dotnet run -c Release --project ZXBox.Benchmarks\ZXBox.Benchmarks.csproj -- --filter * --exporters github,csv,json,html`.
2. Open the generated reports in `BenchmarkDotNet.Artifacts\results`.

The benchmark suite covers snapshot load, Manic Miner frame execution, screen rendering, AY audio render, and Currah speech render. For comparing .NET versions, local runs on the same machine are the most reliable; GitHub-hosted workflow runs are best treated as coarse trend checks.

### Blazor perf tests

1. Build the app and perf test project with `dotnet build Platforms\ZXBox.Blazor\ZXBox.Blazor.csproj -c Release` and `dotnet build ZXBox.Blazor.Perf.Tests\ZXBox.Blazor.Perf.Tests.csproj -c Release`.
2. Install Chromium for Playwright with `pwsh ZXBox.Blazor.Perf.Tests\bin\Release\net10.0\playwright.ps1 install chromium`.
3. Run `dotnet test ZXBox.Blazor.Perf.Tests\ZXBox.Blazor.Perf.Tests.csproj -c Release --no-build --logger "trx;LogFileName=blazor-perf.trx" --results-directory TestResults`.
4. Optional: set `ZXBOX_PERF_DISPLAY_FRAME_DIVISOR` and `ZXBOX_PERF_AUDIO_FRAMES_PER_BATCH` before the test run to compare reduced presentation cadence or a different maximum refill burst for the browser-owned audio loop without editing code.
5. Inspect `TestResults\blazor-perf-result.json` for the measured frame loop, paint, browser `requestAnimationFrame`, and audio queue metrics.

The Blazor perf harness opens a dedicated `/perf` route, warms up the emulator, auto-loads `ManicMiner.z80`, then records scheduler callbacks, timer fallback ticks, executed and skipped frames, paint requests, browser audio queue metrics, and browser `requestAnimationFrame` cadence. The browser audio path now uses a persistent JS-owned controller and an AudioWorklet-backed queue, while .NET keeps a long-lived reference to that controller and refills audio when the browser requests more data. The route also supports `displayFrameDivisor` and `audioFramesPerBatch` query parameters so alternate pacing modes can be tested directly from the route. The automated test validates that the harness produced sane metrics and stores the measured numbers; it deliberately does not hard-code a paint-FPS pass/fail threshold because headless browser rendering can differ sharply from a real interactive browser on local hardware. The manual `blazor-perf.yml` workflow is useful for coarse comparisons, but local runs remain the best way to compare rendering performance across SDK versions on the same machine.

## Currah uSpeech / SP0256 ROM ownership

Currah uSpeech support uses the bundled Currah ROM plus the SP0256-AL2 speech ROM image.

The SP0256-AL2 ROM image (`al2.bin`) is owned by **Microchip Technology Inc.** Microchip retains the intellectual property rights to the algorithms and data contained in that ROM image. Distribution of that ROM image is based on the permission described by Microchip's legal department in the material provided with the ROM.

## Thanks to

Jessica Engström - For loving me and letting me have a storage room filled with old computers =D  
Mikael Engström - For teaching me how to program  
Mats Sjöblom - For explaining some of the internals in the ZX Spectrum  
Rodnay Zaks - For writing the book I used to implement all the Z80 Assembler instructions.  


Projects
https://github.com/jsakamoto/Toolbelt.Blazor.Gamepad
