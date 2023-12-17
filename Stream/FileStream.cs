using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OpenSub.NET.Stream
{
    public static class FileStream
    {
        public static async Task<string> ReadFileAsStringAsync(string filePath, Encoding encoding)
        {
            using (var stream = new System.IO.FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var reader = new StreamReader(stream, encoding))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        // Zde můžete přidat další metody pro práci s FileStream
    }
}

