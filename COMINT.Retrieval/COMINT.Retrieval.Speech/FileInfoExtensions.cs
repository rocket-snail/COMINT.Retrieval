using System;
using System.IO;

namespace COMINT.Retrieval.Speech
{
    public static class FileInfoExtensions
    {
        public static string ReadContent(this FileInfo file)
        {
            try
            {
                return File.ReadAllText(file.FullName);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
