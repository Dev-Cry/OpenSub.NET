using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OpenSub.NET.Format
{
    public static class FileFormatEncoding
    {
        /// <summary>
        /// Asynchronously detects the encoding of a file based on its Byte Order Mark (BOM).
        /// If BOM is not found, checks if the file can be in ASCII.
        /// </summary>
        /// <param name="filePath">Path to the file for encoding detection.</param>
        /// <returns>Returns the detected encoding of the file.</returns>
        /// <exception cref="ArgumentException">Thrown if the file path is empty or null.</exception>
        public static async Task<Encoding> GetEncodingAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path is empty or null.", nameof(filePath));
            }

            try
            {
                using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                {
                    var bom = new byte[4];
                    await file.ReadAsync(bom, 0, 4); // Asynchronous read of the first four bytes for BOM detection

                    // BOM detection for various encodings
                    if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
                    {
                        return Encoding.UTF8; // UTF-8 with BOM
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
                        // If BOM is not found, check for ASCII
                        file.Seek(0, SeekOrigin.Begin); // Return to the beginning of the file
                        var buffer = new byte[1024];
                        var bytesRead = await file.ReadAsync(buffer, 0, buffer.Length);

                        // Check if all read bytes fall within the ASCII range
                        for (int i = 0; i < bytesRead; i++)
                        {
                            if (buffer[i] > 0x7F) // Check if byte is out of ASCII range
                            {
                                return Encoding.UTF8; // If it finds a byte outside of ASCII, assume UTF-8
                            }
                        }

                        return Encoding.ASCII; // All bytes are within the ASCII range
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error detecting encoding: {ex.Message}");
                throw; // Propagate the exception
            }
        }
    }
}
