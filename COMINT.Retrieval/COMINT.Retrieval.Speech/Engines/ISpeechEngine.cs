using System;
using System.IO;

namespace COMINT.Retrieval.Speech.Engines
{
    public interface ISpeechEngine
    {
        string Name { get; }

        void GenerateSpeech(string content, string file);

        void GenerateText(FileInfo input, string file);

        void UploadFiles(string path);
    }
}
