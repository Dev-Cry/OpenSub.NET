using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OpenSub.NET.Format;
using OpenSub.NET.Parser;

namespace OpenSub.NET.Stream
{
    public static class ReadSubtitleFileAsync
    {
        public static async Task<string> ReadAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path is empty or null.", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found.", filePath);

            var encoding = await FileFormatEncoding.GetEncodingAsync(filePath);
            return await SubtitleFileStreamAsync.ReadFileAsStringAsync(filePath, encoding);
        }

        /// <summary>
        /// Checks if the file is a valid subtitle file based on its extension.
        /// </summary>
        /// <param name="filePath">Path to the subtitle file.</param>
        /// <returns>True if the file is a valid subtitle file; otherwise false.</returns>
        public static bool IsValidFile(string filePath)
        {
            return FileFormatExtension.IsValidExtension(filePath);
        }

        public static async Task<List<dynamic>> ParseSubtitlesAsync(string filePath)
        {
            var fileContent = await ReadAsync(filePath);
            var format = await FileFormatExtension.GetFormatAsync(filePath);

            switch (format)
            {
                case Enum.Format.SRT:
                    return SubRipParser.Parse(fileContent).Cast<dynamic>().ToList();
                default:
                    throw new NotSupportedException($"Format '{format}' is not supported.");
            }
        }
    }
}
