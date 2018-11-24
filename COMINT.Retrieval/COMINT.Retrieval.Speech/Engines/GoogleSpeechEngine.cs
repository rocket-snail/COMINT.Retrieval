

using System;
using System.IO;
using System.Linq;
using System.Text;
using Google.Cloud.Speech.V1;
using Google.Cloud.Storage.V1;
using Google.Cloud.TextToSpeech.V1;

namespace COMINT.Retrieval.Speech.Engines
{
    class GoogleSpeechEngine : ISpeechEngine
    {
        public string Name => "Google";

        private const string Bucket = "comint-retrieval-data";

        public void GenerateSpeech(string content, string file)
        {
            var client = TextToSpeechClient.Create();
            var input = new SynthesisInput
            {
                Text = content
            };

            // Build the voice request.
            var voiceSelection = new VoiceSelectionParams
            {
                LanguageCode = "en-US",
                SsmlGender = SsmlVoiceGender.Female
            };

            // Specify the type of audio file.
            var audioConfig = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Linear16
            };

            // Perform the text-to-speech request.
            var response = client.SynthesizeSpeech(input, voiceSelection, audioConfig);

            // Write the response to the output file.
            using (var output = File.Create(file))
            {
                output.Write(response.AudioContent, 0, response.AudioContent.Length);
            }
        }

        public void GenerateText(FileInfo input, string file)
        {
            var speech = SpeechClient.Create();

            var operation = speech.LongRunningRecognize(new RecognitionConfig()
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                SampleRateHertz = 24000,
                LanguageCode = "en",
            }, RecognitionAudio.FromStorageUri($"gs://{Bucket}/{input.Name}"));
            operation = operation.PollUntilCompleted();
            var response = operation.Result;

            var sb = new StringBuilder();
            foreach (var result in response.Results)
            {
                var content = result.Alternatives.OrderByDescending(a => a.Confidence).First();
                sb.AppendLine(content.Transcript);
            }
            File.WriteAllText(file, sb.ToString());
        }

        public void UploadFiles(string path)
        {
            var storage = StorageClient.Create();
            foreach (var file in Directory.GetFiles(path))
            {
                using (var stream = File.OpenRead(file))
                {
                    var name = Path.GetFileName(file);
                    storage.UploadObject(Bucket, name, null, stream);
                    Console.WriteLine($"Uploaded to {Bucket}: {name}");
                }
            }
        }
    }
}
