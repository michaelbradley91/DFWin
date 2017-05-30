using System.IO;
using System.IO.Compression;
using DFWin.Core.PInvoke;

namespace DFWin.Core.Helpers
{
    public static class ByteHelpers
    {
        public static byte[] Compress(this byte[] input, CompressionLevel compressionLevel = CompressionLevel.Fastest)
        {
            using (var inputStream = new MemoryStream(input))
            using (var compressStream = new MemoryStream())
            using (var compressor = new DeflateStream(compressStream, compressionLevel))
            {
                inputStream.CopyTo(compressor);
                compressor.Close();
                return compressStream.ToArray();
            }
        }

        /// <summary>
        /// Checks if two byte arrays are equal using a PInvoke call, which is extremely fast.
        /// Assumes neither side is null.
        /// </summary>
        public static bool FastEquals(this byte[] bytes1, byte[] bytes2)
        {
            return bytes1.Length == bytes2.Length && DllImports.MemoryCompare(bytes1, bytes2, bytes1.Length) == 0;
        }
    }
}
