using System.Diagnostics;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;

namespace ZXBox.Blazor.Perf.Tests;

internal sealed class PerfTestHost : IAsyncDisposable
{
    private readonly Process _process;
    private readonly StringBuilder _output = new();

    private PerfTestHost(Process process, string repositoryRoot, Uri baseUri)
    {
        _process = process;
        RepositoryRoot = repositoryRoot;
        BaseUri = baseUri;
    }

    public string RepositoryRoot { get; }

    public Uri BaseUri { get; }

    public string GetOutput() => _output.ToString();

    public static async Task<PerfTestHost> StartAsync(CancellationToken cancellationToken = default)
    {
        var repositoryRoot = FindRepositoryRoot();
        var port = GetOpenTcpPort();
        var baseUri = new Uri($"http://127.0.0.1:{port}/");
        var startInfo = new ProcessStartInfo(
            "dotnet",
            $@"run --project ""Platforms\ZXBox.Blazor\ZXBox.Blazor.csproj"" -c Release -- --urls {baseUri}")
        {
            WorkingDirectory = repositoryRoot,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        var process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };
        var host = new PerfTestHost(process, repositoryRoot, baseUri);
        process.OutputDataReceived += (_, args) => host.AppendOutput(args.Data);
        process.ErrorDataReceived += (_, args) => host.AppendOutput(args.Data);

        if (!process.Start())
        {
            throw new InvalidOperationException("Failed to start the Blazor perf host.");
        }

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await host.WaitUntilReadyAsync(cancellationToken);
        return host;
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (!_process.HasExited)
            {
                _process.Kill(entireProcessTree: true);
                await _process.WaitForExitAsync();
            }
        }
        finally
        {
            _process.Dispose();
        }
    }

    private void AppendOutput(string? line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return;
        }

        lock (_output)
        {
            _output.AppendLine(line);
        }
    }

    private async Task WaitUntilReadyAsync(CancellationToken cancellationToken)
    {
        using var httpClient = new HttpClient();
        var timeoutAt = DateTime.UtcNow.AddSeconds(90);

        while (DateTime.UtcNow < timeoutAt)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (_process.HasExited)
            {
                throw new InvalidOperationException($"The Blazor perf host exited early.{Environment.NewLine}{GetOutput()}");
            }

            try
            {
                using var response = await httpClient.GetAsync(BaseUri, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    return;
                }
            }
            catch (HttpRequestException)
            {
            }
            catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
            }

            await Task.Delay(500, cancellationToken);
        }

        throw new TimeoutException($"Timed out waiting for the Blazor perf host to start.{Environment.NewLine}{GetOutput()}");
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "ZXBox.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate the repository root.");
    }

    private static int GetOpenTcpPort()
    {
        using var listener = new TcpListener(System.Net.IPAddress.Loopback, 0);
        listener.Start();
        return ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;
    }
}
