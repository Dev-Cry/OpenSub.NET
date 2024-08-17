using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OpenSub.NET.Format
{
    public static class FileFormatExtension
    {
        private static readonly Dictionary<string, Enum.Format> ExtensionMap = new()
        {
            { ".srt", Enum.Format.SRT }
            // Placeholder for additional supported subtitle formats
        };

        public static async Task<Enum.Format> GetFormatAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException(nameof(filePath));

            if (!IsValidExtension(filePath))
                throw new NotSupportedException($"Unsupported file extension: '{Path.GetExtension(filePath)}'");

            try
            {
                var extension = Path.GetExtension(filePath).ToLower();
                var fileDetails = await GetFileDetailsAsync(filePath);

                if (ExtensionMap.TryGetValue(extension, out var format))
                {
                    return format;
                }
                else
                {
                    throw new FormatException($"Failed to identify format for extension '{extension}'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting file format: {ex.Message}");
                throw;
            }
        }

        public static bool IsValidExtension(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();
            return ExtensionMap.ContainsKey(extension);
        }

        public static async Task<(string FileName, long FileSize)> GetFileDetailsAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    throw new ArgumentException("File path is empty or null.", nameof(filePath));
                }

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("File not found.", filePath);
                }

                var fileInfo = new FileInfo(filePath);
                string fileName = fileInfo.Name; // Get file name
                long fileSize = fileInfo.Length; // Get file size in bytes

                return (fileName, fileSize);
            });
        }
    }
}
