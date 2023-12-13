using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OpenSub.NET.FileSub
{
    public static class FileFormatEncoding
    {
        /// <summary>
        /// Asynchronně detekuje kódování souboru na základě jeho Byte Order Mark (BOM).
        /// Pokud BOM není nalezen, kontroluje, zda soubor může být v ASCII.
        /// </summary>
        /// <param name="filePath">Cesta k souboru pro detekci kódování.</param>
        /// <returns>Vrátí detekované kódování souboru.</returns>
        /// <exception cref="ArgumentException">Vyvoláno, pokud je cesta k souboru prázdná nebo null.</exception>
        public static async Task<Encoding> GetEncodingAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Cesta k souboru je prázdná nebo null.", nameof(filePath));
            }

            try
            {
                using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                {
                    var bom = new byte[4];
                    await file.ReadAsync(bom, 0, 4); // Asynchronní čtení prvních čtyř bajtů pro detekci BOM

                    // Detekce BOM pro různá kódování
                    if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
                    {
                        return Encoding.UTF8; // UTF-8 s BOM
                    }
                    else if (bom[0] == 0xff && bom[1] == 0xfe)
                    {
                        return Encoding.Unicode; // UTF-16 Little Endian
                    }
                    else if (bom[0] == 0xfe && bom[1] == 0xff)
                    {
                        return Encoding.BigEndianUnicode; // UTF-16 Big Endian
                    }
                    else if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff)
                    {
                        return Encoding.UTF32; // UTF-32
                    }
                    else
                    {
                        // Pokud není nalezen BOM, kontroluje pro ASCII
                        file.Seek(0, SeekOrigin.Begin); // Vrací se na začátek souboru
                        var buffer = new byte[1024];
                        var bytesRead = file.Read(buffer, 0, buffer.Length);

                        // Kontrola, zda všechny přečtené bajty patří do ASCII rozsahu
                        for (int i = 0; i < bytesRead; i++)
                        {
                            if (buffer[i] > 0x7F) // Zkontrolovat, zda bajt leží mimo ASCII rozsah
                            {
                                return Encoding.UTF8; // Pokud najde bajt mimo ASCII, předpokládá UTF-8
                            }
                        }

                        return Encoding.ASCII; // Všechny bajty jsou v ASCII rozsahu
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba při detekci kódování: {ex.Message}");
                throw; // Propaguje výjimku dále
            }
        }
    }
}


