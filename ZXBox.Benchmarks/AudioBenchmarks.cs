using System;
using BenchmarkDotNet.Attributes;
using ZXBox.Hardware.Sound;
using ZXBox.Hardware.Speech;

namespace ZXBox.Benchmarks;

public class AudioBenchmarks : BenchmarkBase
{
    private const int AudioSamplesPerFrame = 48000 / 50;
    private const int AudioFramesPerBatch = 20;

    private Ay38912Chip _ayChip = null!;
    private float[] _ayFrameBuffer = Array.Empty<float>();

    private ZXSpectrum _speechSpectrum = null!;
    private float[] _speechFrameBuffer = Array.Empty<float>();

    [IterationSetup(Target = nameof(RenderAyFrame))]
    public void SetupAy()
    {
        _ayChip = new Ay38912Chip();
        _ayFrameBuffer = new float[AudioSamplesPerFrame];

        ProgramAyRegisterSet(_ayChip);
    }

    [IterationSetup(Target = nameof(QueueAndRenderCurrahSpeechFrame))]
    public void SetupSpeech()
    {
        _speechSpectrum = new ZXSpectrum();
        _speechSpectrum.ConnectCurrahMicroSpeech();
        _speechSpectrum.CurrahMicroSpeech.LoadSpeechRom(CreateSyntheticSpeechRom());
        _speechSpectrum.ReadByteFromMemory(0x0038);
        _speechFrameBuffer = new float[AudioSamplesPerFrame];
    }

    [Benchmark(Description = "Render AY audio frame", OperationsPerInvoke = AudioFramesPerBatch)]
    public void RenderAyFrame()
    {
        for (var i = 0; i < AudioFramesPerBatch; i++)
        {
            _ayChip.RenderAudioFrame(_ayFrameBuffer, Ay38912Chip.FrameTStates128k);
        }
    }

    [Benchmark(Description = "Queue and render Currah speech frame", OperationsPerInvoke = AudioFramesPerBatch)]
    public void QueueAndRenderCurrahSpeechFrame()
    {
        for (var i = 0; i < AudioFramesPerBatch; i++)
        {
            _speechSpectrum.WriteByteToMemory(0x1000, 0x00);
            _speechSpectrum.CurrahMicroSpeech.RenderAudioFrame(_speechFrameBuffer, _speechSpectrum.FrameTStates);
        }
    }

    private static void ProgramAyRegisterSet(Ay38912Chip chip)
    {
        WriteAyRegister(chip, 0, 0x40);
        WriteAyRegister(chip, 1, 0x01);
        WriteAyRegister(chip, 2, 0x80);
        WriteAyRegister(chip, 3, 0x00);
        WriteAyRegister(chip, 4, 0x20);
        WriteAyRegister(chip, 5, 0x02);
        WriteAyRegister(chip, 6, 0x0F);
        WriteAyRegister(chip, 7, 0x00);
        WriteAyRegister(chip, 8, 0x10);
        WriteAyRegister(chip, 9, 0x0D);
        WriteAyRegister(chip, 10, 0x09);
        WriteAyRegister(chip, 11, 0x40);
        WriteAyRegister(chip, 12, 0x02);
        WriteAyRegister(chip, 13, 0x0B);
    }

    private static void WriteAyRegister(Ay38912Chip chip, int register, int value, int frameTState = 0)
    {
        chip.HandlePortWrite(0xFFFD, register, frameTState);
        chip.HandlePortWrite(0xBFFD, value, frameTState);
    }

    private static byte[] CreateSyntheticSpeechRom()
    {
        var rom = new byte[Sp0256Chip.Sp0256RomSize];
        rom[0] = 0x87;
        rom[1] = 0x1F;
        rom[2] = 0x20;
        rom[3] = 0x00;
        return rom;
    }
}
