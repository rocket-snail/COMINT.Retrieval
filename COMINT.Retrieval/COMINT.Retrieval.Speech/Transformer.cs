﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using COMINT.Retrieval.Speech.Engines;
using COMINT.Retrieval.Speech.Helpers;

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

            var files = new List<FileInfo>();


            var items = Directory.GetFiles(path).Select(x => new FileInfo(x)).ToList()
                .Where(y => !existingFiles.Any(x => x.Name.StartsWith(y.Name)));

            Parallel.ForEach(items, (item) =>
            {
                var content = item.ReadContent();
                var outputFile = Path.Combine(outputPath, $"{item.Name}_{engine.Name}.wav");
                engine.GenerateSpeech(content, outputFile);
                Console.WriteLine($"Converted text to speech: {outputFile}");
                files.Add(new FileInfo(outputFile));
            });
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

            var files = new List<FileInfo>();

            var existingFiles = new List<FileInfo>();
            if (!overwrite)
            {
                existingFiles = Directory.GetFiles(outputPath).Select(x => new FileInfo(x)).ToList();
            }

            var items = Directory.GetFiles(path).Select(x => new FileInfo(x)).ToList()
                .Where(y => !existingFiles.Any(x => x.Name.StartsWith(y.Name)));

            Parallel.ForEach(items, new ParallelOptions { MaxDegreeOfParallelism = 20 }, (item) =>
            {
                var outputFile = Path.Combine(outputPath, $"{item.Name}_{engine.Name}.txt");
                engine.GenerateText(item, outputFile);
                Console.WriteLine($"Converted speech to text: {outputFile}");
                files.Add(new FileInfo(outputFile));
            });

            return files;
        }
    }
}
