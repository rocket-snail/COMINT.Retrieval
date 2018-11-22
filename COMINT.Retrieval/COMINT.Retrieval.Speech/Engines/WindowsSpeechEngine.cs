using System;
using System.IO;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;

namespace COMINT.Retrieval.Speech.Engines
{
    public class WindowsSpeechEngine : ISpeechEngine
    {
        public string Name => "Windows";

        public void GenerateSpeech(string content, string file)
        {
            using (var synthesizer = new SpeechSynthesizer())
            {
                synthesizer.SetOutputToWaveFile(file);
                synthesizer.Speak(content);
            }
        }

        public void GenerateText(FileInfo input, string file)
        {
            using (var recognition = new SpeechRecognitionEngine())
            {
                Grammar grammar = new DictationGrammar();
                recognition.LoadGrammar(grammar);
                recognition.SetInputToWaveFile(input.FullName);
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
                        break;
                    }
                }
                File.WriteAllText(file, sb.ToString());
            }
        }
    }
}
