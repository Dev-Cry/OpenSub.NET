using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OpenSub.NET.FileSub
{
    public static class FileFormatExtension
    {
        private static readonly Dictionary<string, Enum.Format> ExtensionMap = new Dictionary<string, Enum.Format>
        {
            { ".sub", Enum.Format.SUB },
            { ".srt", Enum.Format.SRT }
            // Místo pro přidání dalších podporovaných formátů titulků
        };

        public static async Task<Enum.Format> GetFormatAsync(string filePath)
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

                // Předpokládáme, že v budoucnu zde budete načítat data ze souboru
                // Například:
                // var fileHeader = await ReadFileHeaderAsync(filePath);

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

        // Metoda pro asynchronní čtení hlavičky souboru (příklad)
        // private static async Task<string> ReadFileHeaderAsync(string filePath)
        // {
        //     using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
        //     {
        //         // Logika pro čtení hlavičky souboru
        //     }
        // }
    }
}

