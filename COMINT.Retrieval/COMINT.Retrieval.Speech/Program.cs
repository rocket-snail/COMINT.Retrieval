using System;

namespace COMINT.Retrieval.Speech
{
    class Program
    {
        static void Main(string[] args)
        {
            const string input = @"D:\GitHub\COMINT.Retrieval\COMINT.Retrieval\Documents";

            Transformer.TextToSpeech(input);
            Console.ReadLine();
        }
    }
}
