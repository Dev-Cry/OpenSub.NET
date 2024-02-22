using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OpenSub.NET.Stream
{
    public static class SubtitleFileStreamAsync
    {
        public static async Task<string> ReadFileAsStringAsync(string filePath, Encoding encoding)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new StreamReader(stream, encoding))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}