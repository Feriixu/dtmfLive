using DtmfDetection.NAudio;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dtmfLive
{
    class Program
    {
        static void Main(string[] args)
        {
            CaptureAndAnalyzeLiveAudio();
        }

        private static void CaptureAndAnalyzeLiveAudio()
        {

            LiveAudioDtmfAnalyzer analyzer = null;
            IWaveIn audioSource = null;

            while (true)
            {
                Console.WriteLine("====================================\n"
                                  + "[O]   Capture current audio output\n"
                                  + "[M]   Capture mic in\n"
                                  + "[Esc] Stop capturing/quit");
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.O:
                        analyzer?.StopCapturing();
                        audioSource?.Dispose();

                        audioSource = new WasapiLoopbackCapture { ShareMode = AudioClientShareMode.Shared };
                        analyzer = InitLiveAudioAnalyzer(audioSource);
                        analyzer.StartCapturing();

                        break;

                    case ConsoleKey.M:
                        analyzer?.StopCapturing();
                        audioSource?.Dispose();

                        audioSource = new WaveInEvent { WaveFormat = new WaveFormat(8000, 32, 1) };
                        analyzer = InitLiveAudioAnalyzer(audioSource);
                        analyzer.StartCapturing();

                        break;

                    case ConsoleKey.Escape:
                        if (analyzer == null || !analyzer.IsCapturing)
                            return;

                        analyzer.StopCapturing();
                        audioSource.Dispose();

                        break;
                }
            }

        }
        private static LiveAudioDtmfAnalyzer InitLiveAudioAnalyzer(IWaveIn waveIn)
        {
            var analyzer = new LiveAudioDtmfAnalyzer(waveIn);
            analyzer.DtmfToneStarted += start => Console.WriteLine($"{start.DtmfTone.Key}");
            return analyzer;
        }

    }
}
