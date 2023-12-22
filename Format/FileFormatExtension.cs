using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OpenSub.NET.Format
{
    public static class FileFormatExtension
    {
        private static readonly Dictionary<string, Enum.Format> ExtensionMap = new Dictionary<string, Enum.Format>
        {
            { ".sub", Enum.Format.VTT },
            { ".srt", Enum.Format.SRT }
            // Místo pro přidání dalších podporovaných formátů titulků
        };

        public static Enum.Format GetFormat(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Cesta k souboru je prázdná nebo null.", nameof(filePath));
            }

            if (!IsValidExtension(filePath))
            {
                throw new NotSupportedException($"Formát souboru s příponou '{Path.GetExtension(filePath)}' není podporován.");
            }

            try
            {
                var extension = Path.GetExtension(filePath).ToLower();

                var fileDetails = GetFileDetailsAsync(filePath).Result;

                if (ExtensionMap.TryGetValue(extension, out var format))
                {
                    return format;
                }
                else
                {
                    throw new FormatException($"Nepodařilo se identifikovat formát pro příponu '{extension}'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba při získávání formátu souboru: {ex.Message}");
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
                    throw new ArgumentException("Cesta k souboru je prázdná nebo null.", nameof(filePath));
                }

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("Soubor nebyl nalezen.", filePath);
                }

                var fileInfo = new FileInfo(filePath);
                string fileName = fileInfo.Name; // Získá název souboru
                long fileSize = fileInfo.Length; // Získá velikost souboru v bajtech

                return (fileName, fileSize);
            });
        }
    }
}

