using System;
using System.IO;
using System.Linq;
using COMINT.Retrieval.Speech.Engines;

namespace COMINT.Retrieval.Speech
{
    enum Mode {TextToSpeech, SpeechToText, Upload }
    enum Tool { Windows, Google }

    public class Program
    {
        static void Main(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory() + @"\\..\\..\\..\\..\\Workplace\\";
            var mode = Mode.SpeechToText;
            var tool = Tool.Google;
            var path = @"D:\Data\COMINT.Retrieval\Google\Noise\1";

            if (args.Any())
            {
                mode = (Mode)Enum.Parse(typeof(Mode), args[0]);
                tool = (Tool)Enum.Parse(typeof(Tool), args[1]);
                path = args[2];
            }


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
                    Transformer.TextToSpeech(engine, path);
                    break;
                case Mode.SpeechToText:
                    Transformer.SpeechToText(engine, path);
                    break;
                case Mode.Upload:
                    engine.UploadFiles(path);
                    break;
            }
            Console.WriteLine("**********************");
            Console.WriteLine("****** FINISHED ******");
            Console.WriteLine("**********************");
            Console.ReadLine();
        }
    }
}
