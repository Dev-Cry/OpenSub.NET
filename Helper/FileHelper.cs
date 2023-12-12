using System;
using System.IO;
using System.Text;

namespace OpenSub.NET.Helper
{
    public static class FileHelper
    {
        /// <summary>
        /// Načte obsah titulkového souboru jako string.
        /// Nejprve detekuje kódování souboru a poté načte obsah s použitím tohoto kódování.
        /// </summary>
        /// <param name="filePath">Cesta k titulkovému souboru.</param>
        /// <returns>Textový obsah titulkového souboru.</returns>
        public static string ReadFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("Cesta k souboru je prázdná nebo null.", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Soubor nebyl nalezen.", filePath);

            var encoding = FileFormatEncoding.GetEncoding(filePath);
            return File.ReadAllText(filePath, encoding);
        }

        /// <summary>
        /// Zkontroluje, zda je soubor platným titulkovým souborem na základě jeho přípony.
        /// </summary>
        /// <param name="filePath">Cesta k titulkovému souboru.</param>
        /// <returns>True, pokud je soubor platným titulkovým souborem; jinak false.</returns>
        public static bool IsValidSubtitleFile(string filePath)
        {
            return FileFormatExtension.IsValidExtension(filePath);
        }

        // Zde můžete přidat další metody, pokud jsou potřeba, například pro načtení souboru jako pole bajtů, atd.
    }
}

