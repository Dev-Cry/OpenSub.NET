using OpenSub.NET.Format;

namespace OpenSub.NET.Stream
{
    public static class ReadFileAsync
    {
        public static async Task<string> ReadAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("Cesta k souboru je prázdná nebo null.", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Soubor nebyl nalezen.", filePath);

            var encoding = await FileFormatEncoding.GetEncodingAsync(filePath);
            return await FileStream.ReadFileAsStringAsync(filePath, encoding);
        }

        // <summary>
        /// Zkontroluje, zda je soubor platným titulkovým souborem na základě jeho přípony.
        /// </summary>
        /// <param name="filePath">Cesta k titulkovému souboru.</param>
        /// <returns>True, pokud je soubor platným titulkovým souborem; jinak false.</returns>
        public static bool IsValidFile(string filePath)
        {
            return FileFormatExtension.IsValidExtension(filePath);
        }
    }
}
