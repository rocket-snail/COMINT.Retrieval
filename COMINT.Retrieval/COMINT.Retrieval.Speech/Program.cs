using System;
using System.IO;
using System.Runtime.InteropServices;
using COMINT.Retrieval.Speech.Engines;

namespace COMINT.Retrieval.Speech
{
    enum Mode {TextToSpeech, SpeechToText }
    enum Tool { Windows, Google }

    public class Program
    {
        static void Main(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory() + @"\\..\\..\\..\\..\\Workplace\\";
            var mode = Mode.TextToSpeech;
            var tool = Tool.Google;

            ISpeechEngine engine;
            switch (tool)
            {
                case Tool.Windows:
                    engine = new WindowsSpeechEngine();
                    break;
                case Tool.Google:
                    engine = new GoogleSpeechEngine();
                    break;
                default:
                    throw new ArgumentException();
            }

            switch (mode)
            {
                case Mode.TextToSpeech:
                    Transformer.TextToSpeech(engine, basePath + "Documents");
                    break;
                case Mode.SpeechToText:
                    Transformer.SpeechToText(engine, basePath + "Noise");
                    break;
            }
            Console.ReadLine();
        }
    }
}
