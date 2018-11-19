using System;
using System.Speech.Recognition;

namespace COMINT.Retrieval.Speech
{
    class Program
    {
        static void Main(string[] args)
        {

            var recognizers = SpeechRecognitionEngine.InstalledRecognizers();

            //const string inputDocuments = @"D:\GitHub\COMINT.Retrieval\COMINT.Retrieval\Documents";
            //Transformer.TextToSpeech(inputDocuments);
            const string inputNoise = @"D:\Repos\COMINT.Retrieval\NoiseGenerator\NoisedAudioFiles";
            Transformer.SpeechToText(inputNoise);
            Console.ReadLine();
        }
    }
}
