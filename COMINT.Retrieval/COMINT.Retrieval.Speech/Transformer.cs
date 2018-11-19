using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Speech.Synthesis;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace COMINT.Retrieval.Speech
{
    public static class Transformer
    {
        private const string OutputFolder = "audio";

        public static List<FileInfo> TextToSpeech(string path, int max = int.MaxValue)
        {
            if (!Directory.Exists(path))
            {
                throw new ArgumentException($"Path does not exist: {path}");
            }
            var outputPath = Path.Combine(path, OutputFolder);
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            var i = 0;
            var files = new List<FileInfo>();
            foreach (var item in Directory.GetFiles(path))
            {
                var file = new FileInfo(item);
                using (var synth = new SpeechSynthesizer())
                {
                    var content = file.ReadContent();
                    var outputFile = Path.Combine(outputPath, $"{file.Name}.wav");
                    synth.SetOutputToWaveFile(outputFile);
                    synth.Speak(content);
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
