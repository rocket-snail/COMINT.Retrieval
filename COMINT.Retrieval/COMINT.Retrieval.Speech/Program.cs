namespace COMINT.Retrieval.Speech
{
    class Program
    {
        static void Main(string[] args)
        {
            const string input = "C:\\Users\\david\\source\\repos\\COMINT.Retrieval\\COMINT.Retrieval\\Documents";

            Transformer.TextToSpeech(input, 10);
        }
    }
}
