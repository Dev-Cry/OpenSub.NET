using OpenSub.NET.Enum;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenSub.NET.Helper
{
    /// <summary>
    /// Poskytuje funkcionality pro identifikaci formátu titulkových souborů na základě jejich přípon.
    /// </summary>
    public static class FileFormatExtension
    {
        // Mapování přípon souborů na odpovídající formáty titulků.
        private static readonly Dictionary<string, Format> ExtensionMap = new Dictionary<string, Format>
        {
            { ".sub", Format.SUB },
            { ".srt", Format.SRT }
            // Místo pro přidání dalších podporovaných formátů titulků
        };

        /// <summary>
        /// Ověřuje, zda je přípona souboru platná a podporovaná.
        /// </summary>
        /// <param name="filePath">Cesta k titulkovému souboru.</param>
        /// <returns>Vrací true, pokud přípona souboru je podporovaná, jinak false.</returns>
        public static bool IsValidExtension(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();
            return ExtensionMap.ContainsKey(extension);
        }

        /// <summary>
        /// Získá formát titulkového souboru na základě jeho přípony.
        /// </summary>
        /// <param name="filePath">Cesta k titulkovému souboru.</param>
        /// <returns>Vrací formát titulkového souboru.</returns>
        /// <exception cref="ArgumentException">Vyvoláno, pokud je cesta k souboru prázdná nebo null.</exception>
        /// <exception cref="NotSupportedException">Vyvoláno, pokud přípona souboru není podporována.</exception>
        /// <exception cref="FormatException">Vyvoláno, pokud se nepodaří identifikovat formát.</exception>
        public static Format GetFormat(string filePath)
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

                if (ExtensionMap.TryGetValue(extension, out var format))
                {
                    return format;
                }
                else
                {
                    throw new FormatException($"Nepodařilo se identifikovat formát pro příponu '{extension}'.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Chyba argumentu: {ex.Message}");
                throw;
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine($"Nepodporovaný formát: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Chyba formátu: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neznámá chyba: {ex.Message}");
                throw;
            }
        }
    }
}

