using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace COMINT.Retrieval.Speech
{
    public static class Transformer
    {
        private const string OutputSpeechFolder = "audio";
        private const string OutputTextFolder = "text";

        public static List<FileInfo> TextToSpeech(string path, int max = int.MaxValue)
        {
            if (!Directory.Exists(path))
            {
                throw new ArgumentException($"Path does not exist: {path}");
            }
            var outputPath = Path.Combine(path, OutputSpeechFolder);
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            var i = 0;
            var files = new List<FileInfo>();
            foreach (var item in Directory.GetFiles(path))
            {
                var file = new FileInfo(item);
                using (var synthesizer = new SpeechSynthesizer())
                {
                    var content = file.ReadContent();
                    var outputFile = Path.Combine(outputPath, $"{file.Name}.wav");
                    synthesizer.SetOutputToWaveFile(outputFile);
                    synthesizer.Speak(content);
                    Console.WriteLine($"Converted text to speech: {outputFile}");
                }
                i++;
                if (i > max)
                {
                    return files;
                }
            }
            return files;
        }

        public static List<FileInfo> SpeechToText(string path, int max = int.MaxValue)
        {
            if (!Directory.Exists(path))
            {
                throw new ArgumentException($"Path does not exist: {path}");
            }

            var outputPath = Path.Combine(path, OutputTextFolder);
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            var i = 0;
            var files = new List<FileInfo>();

            foreach (var item in Directory.GetFiles(path))
            {
                var file = new FileInfo(item);
                if(file.Extension.ToLowerInvariant() != ".wav") { continue; }
                using (var recognition = new SpeechRecognitionEngine())
                {
                    Grammar grammar = new DictationGrammar();
                    recognition.LoadGrammar(grammar);
                    recognition.SetInputToWaveFile(item);
                    recognition.BabbleTimeout = new TimeSpan(int.MaxValue);
                    recognition.InitialSilenceTimeout = new TimeSpan(int.MaxValue);
                    recognition.EndSilenceTimeout = new TimeSpan(100000000);
                    recognition.EndSilenceTimeoutAmbiguous = new TimeSpan(100000000);
                    var sb = new StringBuilder();
                    while (true)
                    {
                        try
                        {
                            var content = recognition.Recognize();
                            if (content == null)
                            {
                                break;
                            }
                            sb.Append(content.Text);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            break;
                        }
                    }
                    var outputFile = Path.Combine(outputPath, $"{file.Name}.txt");
                    File.WriteAllText(outputFile, sb.ToString());
                    Console.WriteLine($"Converted speech to text: {outputFile}");
                    files.Add(new FileInfo(outputFile));
                }

                i++;
                if (i > max)
                {
                    return files;
                }
            }

            return files;
        }

        public static void TrecToDocuments(string trecFile)
        {
            const string outputPath = "C:\\Users\\david\\source\\repos\\COMINT.Retrieval\\COMINT.Retrieval\\Documents";

            var doc = XDocument.Load(trecFile);
            var jsonText = JsonConvert.SerializeXNode(doc);
            dynamic dyn = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
            foreach (var item in dyn.TREC)
            {
                foreach (var document in item.Value)
                {
                    var outputFile = Path.Combine(outputPath, $"{document.recordId}.txt");
                    File.WriteAllText(outputFile, document.text);
                    Console.WriteLine($"Write file: {outputFile}");
                }
            }
        }
    }
}
