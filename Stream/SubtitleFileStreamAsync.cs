using System;
using System.IO;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace OpenSub.NET.Stream
{
    public static class SubtitleFileStreamAsync
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;

        public static async Task<string> ReadFileAsStringAsync(string filePath, Encoding encoding)
        {
            string fileContent;

            var cacheKey = CreateCacheKey(filePath);
            if (Cache.Get(cacheKey) is string cachedContent)
            {
                fileContent = cachedContent;
            }
            else
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var reader = new StreamReader(stream, encoding))
                {
                    fileContent = await reader.ReadToEndAsync();
                }

                Cache.Add(cacheKey, fileContent, DateTimeOffset.Now.Add(TimeSpan.FromMinutes(5))); // Uloží do cache na 5 minut
            }

            return fileContent;
        }

        private static string CreateCacheKey(string filePath)
        {
            return $"SubtitleFile_{filePath.GetHashCode()}";
        }
    }
}