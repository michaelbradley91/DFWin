using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using DFWin.Core.Constants;

namespace DFWin.Core.Services
{
    public interface IGameGridService
    {
        /// <summary>
        /// Convert a screenshot of the game to its semantic representation.
        /// Assumes the bitmap is RGB (24 bits per pixel RGB - no alpha)
        /// </summary>
        Tile[,] ParseScreenshot(Bitmap screenshotOfTheGame);
    }

    public class Tile
    {
        public byte Value { get; set; }
        public Color Foreground { get; set; }
        public Color Background { get; set; }

        public byte TileSetX => (byte)(Value % 16);
        public byte TileSetY => (byte)(Value / 16);

        public Tile(byte value, Color foreground, Color background)
        {
            Value = value;
            Foreground = foreground;
            Background = background;
        }
    }

    public class GameGridService : IGameGridService
    {
        public Tile[,] ParseScreenshot(Bitmap screenshotOfTheGame)
        {
            var bytes = GetBitmapBytes(screenshotOfTheGame);
            var pixels = GetPixels(bytes);
            var pixelMatrix = ToPixelMatrix(pixels, screenshotOfTheGame.Width);
            var tiles = GetTiles(pixelMatrix);
            return tiles;
        }

        /// <summary>
        /// Initialises a dummy array of tiles. Note that this assumes the preferred grid size.
        /// </summary>
        public static Tile[,] InitialTiles
        {
            get
            {
                var tiles = new Tile[
                    Sizes.DwarfFortressPreferredGridSize.Width,
                    Sizes.DwarfFortressPreferredGridSize.Height];

                for (var x = 0; x < Sizes.DwarfFortressPreferredGridSize.Width; x++)
                {
                    for (var y = 0; y < Sizes.DwarfFortressPreferredGridSize.Height; y++)
                    {
                        tiles[x, y] = new Tile(0, Color.Black, Color.Black);
                    }
                }

                return tiles;
            }

        }

        private static Tile[,] GetTiles(Color[,] pixelMatrix)
        {
            var tiles = new Tile[pixelMatrix.GetLength(0) / 3, pixelMatrix.GetLength(1) / 3];

            // TODO this assumes the micro tile set. Do something about that...
            for (var x = 0; x < pixelMatrix.GetLength(0); x += 3)
            {
                for (var y = 0; y < pixelMatrix.GetLength(1); y += 3)
                {
                    // bottom right pixel always represents zero.
                    var zeroColour = pixelMatrix[x + 2, y + 2];
                    var bits = new BitArray(8);

                    var backgroundColor = zeroColour;

                    // If the value is eventually zero, this is a background tile so we leave the foreground
                    // equal to the background without technically knowing what it is.
                    var foregroundColor = zeroColour;

                    for (var pos = 0; pos < bits.Length; pos++)
                    {
                        var pixel = pixelMatrix[x + (pos % 3), y + (pos / 3)];
                        bits[pos] = !AreRgbEqual(zeroColour, pixel);

                        if (bits[pos]) foregroundColor = pixel;
                    }

                    var value = new byte[1];
                    bits.CopyTo(value, 0);

                    tiles[x / 3, y / 3] = new Tile(value[0], foregroundColor, backgroundColor);
                }
            }
            return tiles;
        }

        private static bool AreRgbEqual(Color left, Color right)
        {
            return left.R == right.R && left.G == right.G && left.B == right.B;
        }

        private static Color[] GetPixels(IReadOnlyList<byte> bytes)
        {
            var pixels = new Color[bytes.Count / 3];
            for (var i = 0; i < bytes.Count; i += 3)
            {
                pixels[i / 3] = Color.FromArgb(bytes[i], bytes[i + 1], bytes[i + 2]);
            }
            return pixels;
        }

        private static Color[,] ToPixelMatrix(IReadOnlyList<Color> pixels, int numberOfPixelsPerRow)
        {
            var numberOfColumns = numberOfPixelsPerRow;
            var numberOfRows = pixels.Count / numberOfPixelsPerRow;
            var pixelMatrix = new Color[numberOfColumns, numberOfRows];

            for (var row = 0; row < numberOfRows; row++)
            {
                for (var column = 0; column < numberOfColumns; column++)
                {
                    pixelMatrix[column, row] = pixels[column + (row * numberOfPixelsPerRow)];
                }
            }

            return pixelMatrix;
        }

        private static byte[] GetBitmapBytes(Bitmap bitmap)
        {
            var rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, bitmap.PixelFormat);

            // Get the address of the first line.
            var ptr = bitmapData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            var numberOfBytes = Math.Abs(bitmapData.Stride) * bitmapData.Height;
            var bytes = new byte[numberOfBytes];

            Marshal.Copy(ptr, bytes, 0, bytes.Length);

            bitmap.UnlockBits(bitmapData);

            return bytes;
        }
    }
}
