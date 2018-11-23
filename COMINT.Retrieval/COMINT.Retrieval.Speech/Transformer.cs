using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using COMINT.Retrieval.Speech.Engines;
using Newtonsoft.Json;

namespace COMINT.Retrieval.Speech
{
    public static class Transformer
    {
        private const string OutputSpeechFolder = "Speech";
        private const string OutputTextFolder = "Text";

        public static List<FileInfo> TextToSpeech(ISpeechEngine engine, string path, int max = int.MaxValue, bool overwrite = false)
        {
            if (!Directory.Exists(path))
            {
                throw new ArgumentException($"Path does not exist: {path}");
            }
            var outputPath = Path.Combine(path, $"{OutputSpeechFolder}_{engine.Name}");
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            var existingFiles = new List<FileInfo>();
            if (!overwrite)
            {
                existingFiles = Directory.GetFiles(outputPath).Select(x => new FileInfo(x)).ToList();
            }
            var i = 0;
            var files = new List<FileInfo>();
            foreach (var item in Directory.GetFiles(path))
            {
                var file = new FileInfo(item);
                if (existingFiles.Any(x => x.Name.StartsWith(file.Name))) { continue; }
                var content = file.ReadContent();
                var outputFile = Path.Combine(outputPath, $"{file.Name}_{engine.Name}.wav");
                engine.GenerateSpeech(content, outputFile);
                Console.WriteLine($"Converted text to speech: {outputFile}");
                files.Add(new FileInfo(outputFile));
                i++;
                if (i > max)
                {
                    return files;
                }
            }
            return files;
        }

        public static List<FileInfo> SpeechToText(ISpeechEngine engine, string path, int max = int.MaxValue, bool overwrite = false)
        {
            if (!Directory.Exists(path))
            {
                throw new ArgumentException($"Path does not exist: {path}");
            }

            var outputPath = Path.Combine(path, $"{OutputTextFolder}_{engine.Name}");
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            var i = 0;
            var files = new List<FileInfo>();

            var existingFiles = new List<FileInfo>();
            if (!overwrite)
            {
                existingFiles = Directory.GetFiles(outputPath).Select(x => new FileInfo(x)).ToList();
            }

            foreach (var item in Directory.GetFiles(path))
            {
                var file = new FileInfo(item);
                if (existingFiles.Any(x => x.Name.StartsWith(file.Name))) { continue; }
                if (file.Extension.ToLowerInvariant() != ".wav") { continue; }
                var outputFile = Path.Combine(outputPath, $"{file.Name}_{engine.Name}.txt");
                engine.GenerateText(file, outputFile);
                Console.WriteLine($"Converted speech to text: {outputFile}");
                files.Add(new FileInfo(outputFile));
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
