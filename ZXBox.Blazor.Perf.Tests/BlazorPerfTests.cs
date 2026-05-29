using System.Text.Json;
using Microsoft.Playwright;

namespace ZXBox.Blazor.Perf.Tests;

[TestClass]
public sealed class BlazorPerfTests
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public TestContext TestContext { get; set; } = null!;

    [TestMethod]
    public async Task Launcher_click_starts_the_emulator_view()
    {
        await using var host = await PerfTestHost.StartAsync();

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
            Args =
            [
                "--autoplay-policy=no-user-gesture-required",
                "--disable-background-timer-throttling",
                "--disable-backgrounding-occluded-windows",
                "--disable-renderer-backgrounding"
            ]
        });

        var page = await browser.NewPageAsync(new BrowserNewPageOptions
        {
            ViewportSize = new ViewportSize { Width = 1280, Height = 900 }
        });
        page.Console += (_, message) => TestContext.WriteLine($"[console:{message.Type}] {message.Text}");
        page.PageError += (_, message) => TestContext.WriteLine($"[pageerror] {message}");

        await page.GotoAsync(host.BaseUri.ToString(), new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle,
            Timeout = 120000
        });

        try
        {
            await page.Locator(".launcher-option").Nth(0).ClickAsync();
            await page.WaitForSelectorAsync("#emulatorCanvas", new PageWaitForSelectorOptions
            {
                Timeout = 120000
            });
        }
        catch
        {
            var bodyText = await page.EvaluateAsync<string>("() => document.body?.innerText ?? ''");
            TestContext.WriteLine($"Body text: {bodyText}");
            TestContext.WriteLine(host.GetOutput());
            throw;
        }

        Assert.AreEqual(1, await page.Locator("#emulatorCanvas").CountAsync(), $"The emulator canvas never appeared after launching 48K.{Environment.NewLine}{host.GetOutput()}");
        Assert.AreEqual(0, await page.Locator(".launcher-panel").CountAsync(), "The launcher stayed visible after selecting the 48K model.");
    }

    [TestMethod]
    public async Task Launcher_click_resumes_the_audio_context()
    {
        await using var host = await PerfTestHost.StartAsync();

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
            Args =
            [
                "--autoplay-policy=user-gesture-required",
                "--disable-background-timer-throttling",
                "--disable-backgrounding-occluded-windows",
                "--disable-renderer-backgrounding"
            ]
        });

        var page = await browser.NewPageAsync(new BrowserNewPageOptions
        {
            ViewportSize = new ViewportSize { Width = 1280, Height = 900 }
        });
        page.Console += (_, message) => TestContext.WriteLine($"[console:{message.Type}] {message.Text}");
        page.PageError += (_, message) => TestContext.WriteLine($"[pageerror] {message}");

        await page.GotoAsync(host.BaseUri.ToString(), new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle,
            Timeout = 120000
        });

        try
        {
            await page.Locator(".launcher-option").Nth(0).ClickAsync();
            await page.WaitForSelectorAsync("#emulatorCanvas", new PageWaitForSelectorOptions
            {
                Timeout = 120000
            });
            await page.WaitForFunctionAsync(
                "() => window.__zxboxLastAudioController?.context?.state === 'running'",
                null,
                new PageWaitForFunctionOptions
                {
                    Timeout = 120000
                });
        }
        catch
        {
            var audioState = await page.EvaluateAsync<string>("() => window.__zxboxLastAudioController?.context?.state ?? 'missing'");
            var bodyText = await page.EvaluateAsync<string>("() => document.body?.innerText ?? ''");
            TestContext.WriteLine($"Audio state: {audioState}");
            TestContext.WriteLine($"Body text: {bodyText}");
            TestContext.WriteLine(host.GetOutput());
            throw;
        }

        Assert.AreEqual("running", await page.EvaluateAsync<string>("() => window.__zxboxLastAudioController?.context?.state ?? ''"), $"The audio context did not resume after the launcher click.{Environment.NewLine}{host.GetOutput()}");
    }

    [TestMethod]
    public async Task Connecting_currah_applies_the_peripheral_change()
    {
        await using var host = await PerfTestHost.StartAsync();

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
            Args =
            [
                "--autoplay-policy=user-gesture-required",
                "--disable-background-timer-throttling",
                "--disable-backgrounding-occluded-windows",
                "--disable-renderer-backgrounding"
            ]
        });

        var page = await browser.NewPageAsync(new BrowserNewPageOptions
        {
            ViewportSize = new ViewportSize { Width = 1280, Height = 900 }
        });
        page.Console += (_, message) => TestContext.WriteLine($"[console:{message.Type}] {message.Text}");
        page.PageError += (_, message) => TestContext.WriteLine($"[pageerror] {message}");

        await page.GotoAsync(host.BaseUri.ToString(), new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle,
            Timeout = 120000
        });

        try
        {
            await page.Locator(".launcher-option").Nth(0).ClickAsync();
            await page.WaitForSelectorAsync("#emulatorCanvas", new PageWaitForSelectorOptions
            {
                Timeout = 120000
            });
            await page.Locator("button").Filter(new() { HasTextString = "Connect Currah uSpeech" }).EvaluateAsync("button => button.click()");
            await page.WaitForFunctionAsync(
                "() => document.body?.innerText?.includes('Currah connected.') === true",
                null,
                new PageWaitForFunctionOptions
                {
                    Timeout = 120000
                });
            await page.WaitForFunctionAsync(
                "() => window.__zxboxLastAudioController?.context?.state === 'running'",
                null,
                new PageWaitForFunctionOptions
                {
                    Timeout = 120000
                });
        }
        catch
        {
            var audioState = await page.EvaluateAsync<string>("() => window.__zxboxLastAudioController?.context?.state ?? 'missing'");
            var bodyText = await page.EvaluateAsync<string>("() => document.body?.innerText ?? ''");
            TestContext.WriteLine($"Audio state: {audioState}");
            TestContext.WriteLine($"Body text: {bodyText}");
            TestContext.WriteLine(host.GetOutput());
            throw;
        }

        Assert.IsTrue(await page.EvaluateAsync<bool>("() => document.body?.innerText?.includes('Currah connected.') === true"), $"The Currah connection status was not shown.{Environment.NewLine}{host.GetOutput()}");
        Assert.AreEqual("running", await page.EvaluateAsync<string>("() => window.__zxboxLastAudioController?.context?.state ?? ''"), $"The audio context did not resume after connecting Currah.{Environment.NewLine}{host.GetOutput()}");
    }

    [TestMethod]
    public async Task Connecting_printer_and_running_immediate_lprint_updates_the_virtual_paper()
    {
        await using var host = await PerfTestHost.StartAsync();

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
            Args =
            [
                "--autoplay-policy=no-user-gesture-required",
                "--disable-background-timer-throttling",
                "--disable-backgrounding-occluded-windows",
                "--disable-renderer-backgrounding"
            ]
        });

        var page = await browser.NewPageAsync(new BrowserNewPageOptions
        {
            ViewportSize = new ViewportSize { Width = 1280, Height = 900 }
        });
        page.Console += (_, message) => TestContext.WriteLine($"[console:{message.Type}] {message.Text}");
        page.PageError += (_, message) => TestContext.WriteLine($"[pageerror] {message}");

        await page.GotoAsync(host.BaseUri.ToString(), new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle,
            Timeout = 120000
        });

        try
        {
            await page.Locator(".launcher-option").Nth(0).ClickAsync();
            await page.WaitForSelectorAsync("#emulatorCanvas", new PageWaitForSelectorOptions
            {
                Timeout = 120000
            });
            await page.Locator("button").Filter(new() { HasTextString = "Connect ZX Printer" }).EvaluateAsync("button => button.click()");
            await page.WaitForSelectorAsync("#printerCanvas", new PageWaitForSelectorOptions
            {
                Timeout = 120000
            });
            await page.EvaluateAsync("() => DotNet.invokeMethodAsync('ZXBox.Blazor', 'DebugRunImmediateLprint', 'TEST')");
            await page.WaitForFunctionAsync(
                @"() => {
                    const canvas = document.getElementById('printerCanvas');
                    if (!(canvas instanceof HTMLCanvasElement)) {
                        return false;
                    }

                    const context = canvas.getContext('2d');
                    if (!context) {
                        return false;
                    }

                    const image = context.getImageData(0, 0, canvas.width, canvas.height).data;
                    let visibleRows = 0;

                    for (let y = 0; y < canvas.height; y++) {
                        let rowHasInk = false;

                        for (let x = 0; x < canvas.width; x++) {
                            const index = (y * canvas.width + x) * 4;
                            if (image[index] !== 184 || image[index + 1] !== 188 || image[index + 2] !== 192 || image[index + 3] !== 255) {
                                rowHasInk = true;
                                break;
                            }
                        }

                        if (rowHasInk) {
                            visibleRows++;
                        }
                    }

                    return visibleRows >= 24;
                }",
                null,
                new PageWaitForFunctionOptions
                {
                    Timeout = 120000
                });
        }
        catch
        {
            var visibleRows = await page.EvaluateAsync<int>(
                @"() => {
                    const canvas = document.getElementById('printerCanvas');
                    if (!(canvas instanceof HTMLCanvasElement)) {
                        return -1;
                    }

                    const context = canvas.getContext('2d');
                    if (!context) {
                        return -1;
                    }

                    const image = context.getImageData(0, 0, canvas.width, canvas.height).data;
                    let visibleRows = 0;

                    for (let y = 0; y < canvas.height; y++) {
                        let rowHasInk = false;

                        for (let x = 0; x < canvas.width; x++) {
                            const index = (y * canvas.width + x) * 4;
                            if (image[index] !== 184 || image[index + 1] !== 188 || image[index + 2] !== 192 || image[index + 3] !== 255) {
                                rowHasInk = true;
                                break;
                            }
                        }

                        if (rowHasInk) {
                            visibleRows++;
                        }
                    }

                    return visibleRows;
                }");
            var bodyText = await page.EvaluateAsync<string>("() => document.body?.innerText ?? ''");
            TestContext.WriteLine($"Visible printer rows: {visibleRows}");
            TestContext.WriteLine($"Body text: {bodyText}");
            TestContext.WriteLine(host.GetOutput());
            throw;
        }

        Assert.IsTrue(await page.EvaluateAsync<bool>(
            @"() => {
                const canvas = document.getElementById('printerCanvas');
                if (!(canvas instanceof HTMLCanvasElement)) {
                    return false;
                }

                const context = canvas.getContext('2d');
                if (!context) {
                    return false;
                }

                const image = context.getImageData(0, 0, canvas.width, canvas.height).data;
                let visibleRows = 0;

                for (let y = 0; y < canvas.height; y++) {
                    let rowHasInk = false;

                    for (let x = 0; x < canvas.width; x++) {
                        const index = (y * canvas.width + x) * 4;
                        if (image[index] !== 184 || image[index + 1] !== 188 || image[index + 2] !== 192 || image[index + 3] !== 255) {
                            rowHasInk = true;
                            break;
                        }
                    }

                    if (rowHasInk) {
                        visibleRows++;
                    }
                }

                return visibleRows >= 24;
            }"), $"The printer preview stayed too small to see after an immediate LPRINT.{Environment.NewLine}{host.GetOutput()}");
    }

    [TestMethod]
    public async Task Perf_route_reports_frame_audio_and_paint_metrics()
    {
        await using var host = await PerfTestHost.StartAsync();

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
            Args =
            [
                "--autoplay-policy=no-user-gesture-required",
                "--disable-background-timer-throttling",
                "--disable-backgrounding-occluded-windows",
                "--disable-renderer-backgrounding"
            ]
        });

        var page = await browser.NewPageAsync(new BrowserNewPageOptions
        {
            ViewportSize = new ViewportSize { Width = 1280, Height = 900 }
        });
        page.Console += (_, message) => TestContext.WriteLine($"[console:{message.Type}] {message.Text}");
        page.PageError += (_, message) => TestContext.WriteLine($"[pageerror] {message}");

        var perfUrl = BuildPerfUri(host.BaseUri);
        await page.GotoAsync(perfUrl.ToString(), new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle,
            Timeout = 120000
        });

        try
        {
            await page.WaitForFunctionAsync(
                "() => window.__zxboxPerfDone === true",
                null,
                new PageWaitForFunctionOptions
                {
                    Timeout = 120000
                });
        }
        catch
        {
            var perfStatus = await page.EvaluateAsync<string>("() => document.getElementById('perf-status')?.innerText ?? ''");
            var perfResultPreview = await page.EvaluateAsync<string>("() => document.getElementById('perf-results')?.innerText ?? ''");
            var bodyText = await page.EvaluateAsync<string>("() => document.body?.innerText ?? ''");
            TestContext.WriteLine($"Perf status: {perfStatus}");
            TestContext.WriteLine($"Perf result preview: {perfResultPreview}");
            TestContext.WriteLine($"Body text: {bodyText}");
            TestContext.WriteLine(host.GetOutput());
            throw;
        }

        var resultJson = await page.EvaluateAsync<string>("() => window.__zxboxPerfResultJson");
        Assert.IsFalse(string.IsNullOrWhiteSpace(resultJson), $"The perf route did not publish a JSON result.{Environment.NewLine}{host.GetOutput()}");

        var result = JsonSerializer.Deserialize<BlazorPerfResult>(resultJson, JsonOptions);
        Assert.IsNotNull(result, "The perf result JSON could not be deserialized.");

        PersistResult(host.RepositoryRoot, resultJson);
        TestContext.WriteLine($"Loop FPS: {result.LoopFps:F2}");
        TestContext.WriteLine($"Presented paint FPS: {result.PresentedPaintFps:F2}");
        TestContext.WriteLine($"requestAnimationFrame FPS: {result.Raf.Fps:F2}");
        TestContext.WriteLine($"Audio underruns: {result.Audio.UnderrunCount}");

        Assert.AreEqual("ManicMiner.z80", result.Game);
        Assert.AreEqual("48k", result.Model);
        Assert.IsTrue(result.MeasureSeconds >= 7.5, $"Expected roughly 8 seconds of measurement but saw {result.MeasureSeconds:F2}s.");
        Assert.IsTrue(result.TimerTicksObserved > 0 || result.SchedulerCallbacksObserved > 0, "Neither the timer fallback nor the audio scheduler requested any work.");
        Assert.IsTrue(result.FramesExecuted > 0, "No emulator frames were executed during the perf run.");
        Assert.IsTrue(result.LoopFps > 0d, "The measured loop FPS was zero.");
        Assert.IsTrue(result.PaintInvalidations > 0, "No canvas invalidations were requested.");
        Assert.IsTrue(result.Raf.FrameCount > 0, "requestAnimationFrame sampling did not observe any browser frames.");
        Assert.IsTrue(string.Equals(result.Audio.ContextState, "running", StringComparison.OrdinalIgnoreCase), $"AudioContext never reached the running state. State: {result.Audio.ContextState}");
        Assert.IsTrue(result.Audio.EnqueueCount > 0, "No audio buffers were queued.");
        Assert.IsTrue(result.Audio.MaxQueuedAheadSeconds > 0, "Audio never built any queued-ahead time.");
        Assert.IsTrue(result.Audio.SampleRate > 0, "The browser audio sample rate was not reported.");
        Assert.IsTrue(result.Audio.SamplesPushed > 0, "No audio samples were pushed to the browser audio path.");
        Assert.IsTrue(result.Audio.GeneratedSeconds > 0, "No measurable amount of audio was generated.");
    }

    [TestMethod]
    public async Task Perf_route_keeps_running_when_audio_autoplay_is_blocked()
    {
        await using var host = await PerfTestHost.StartAsync();

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
            Args =
            [
                "--autoplay-policy=user-gesture-required",
                "--disable-background-timer-throttling",
                "--disable-backgrounding-occluded-windows",
                "--disable-renderer-backgrounding"
            ]
        });

        var page = await browser.NewPageAsync(new BrowserNewPageOptions
        {
            ViewportSize = new ViewportSize { Width = 1280, Height = 900 }
        });
        page.Console += (_, message) => TestContext.WriteLine($"[console:{message.Type}] {message.Text}");
        page.PageError += (_, message) => TestContext.WriteLine($"[pageerror] {message}");

        var perfUrl = BuildPerfUri(host.BaseUri);
        await page.GotoAsync(perfUrl.ToString(), new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle,
            Timeout = 120000
        });

        try
        {
            await page.WaitForFunctionAsync(
                "() => window.__zxboxPerfDone === true",
                null,
                new PageWaitForFunctionOptions
                {
                    Timeout = 120000
                });
        }
        catch
        {
            var perfStatus = await page.EvaluateAsync<string>("() => document.getElementById('perf-status')?.innerText ?? ''");
            var perfResultPreview = await page.EvaluateAsync<string>("() => document.getElementById('perf-results')?.innerText ?? ''");
            var bodyText = await page.EvaluateAsync<string>("() => document.body?.innerText ?? ''");
            TestContext.WriteLine($"Perf status: {perfStatus}");
            TestContext.WriteLine($"Perf result preview: {perfResultPreview}");
            TestContext.WriteLine($"Body text: {bodyText}");
            TestContext.WriteLine(host.GetOutput());
            throw;
        }

        var resultJson = await page.EvaluateAsync<string>("() => window.__zxboxPerfResultJson");
        Assert.IsFalse(string.IsNullOrWhiteSpace(resultJson), $"The perf route did not publish a JSON result when autoplay was blocked.{Environment.NewLine}{host.GetOutput()}");

        var result = JsonSerializer.Deserialize<BlazorPerfResult>(resultJson, JsonOptions);
        Assert.IsNotNull(result, "The perf result JSON could not be deserialized when autoplay was blocked.");

        TestContext.WriteLine($"Blocked autoplay Loop FPS: {result.LoopFps:F2}");
        TestContext.WriteLine($"Blocked autoplay Frames: {result.FramesExecuted}");
        TestContext.WriteLine($"Blocked autoplay Scheduler callbacks: {result.SchedulerCallbacksObserved}");
        TestContext.WriteLine($"Blocked autoplay Timer ticks: {result.TimerTicksObserved}");

        Assert.IsTrue(result.FramesExecuted >= 100, "The emulator did not keep running long enough when browser audio autoplay was blocked.");
        Assert.IsTrue(result.LoopFps >= 10d, "The emulator effectively stalled when browser audio autoplay was blocked.");
    }

    private static Uri BuildPerfUri(Uri baseUri)
    {
        var query = new Dictionary<string, string?>
        {
            ["warmupSeconds"] = "2",
            ["measureSeconds"] = "8",
            ["game"] = "ManicMiner.z80",
            ["model"] = "48k",
            ["displayFrameDivisor"] = Environment.GetEnvironmentVariable("ZXBOX_PERF_DISPLAY_FRAME_DIVISOR") ?? "1",
            ["audioFramesPerBatch"] = Environment.GetEnvironmentVariable("ZXBOX_PERF_AUDIO_FRAMES_PER_BATCH") ?? "2"
        };

        var queryString = string.Join(
            "&",
            query.Select(pair => $"{Uri.EscapeDataString(pair.Key)}={Uri.EscapeDataString(pair.Value ?? string.Empty)}"));

        return new Uri($"{new Uri(baseUri, "perf")}?{queryString}");
    }

    private void PersistResult(string repositoryRoot, string resultJson)
    {
        var resultsDirectory = Path.Combine(repositoryRoot, "TestResults");
        Directory.CreateDirectory(resultsDirectory);

        var outputPath = Path.Combine(resultsDirectory, "blazor-perf-result.json");
        File.WriteAllText(outputPath, resultJson);
        TestContext.WriteLine($"Saved perf result to {outputPath}");
    }
}
