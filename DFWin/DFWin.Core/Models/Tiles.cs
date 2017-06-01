using System;
using System.Collections.Generic;
using DFWin.Core.Helpers;

namespace DFWin.Core.Models
{
    public class Tiles
    {
        public int Width => tiles.GetLength(0);
        public int Height => tiles.GetLength(1);
        public IReadOnlyList<string> Text => text.Value;

        private readonly Tile[,] tiles;
        private readonly Lazy<IReadOnlyList<string>> text;

        private readonly int hashCode;
        private readonly byte[] bytes;

        public Tiles(Tile[,] tiles)
        {
            this.tiles = tiles;
            hashCode = ComputeHashCode(tiles);
            bytes = ComputeBytes(tiles);
            text = new Lazy<IReadOnlyList<string>>(GetText);
        }

        public Tile this[int x, int y]
        {
            get
            {
                try
                {
                    return tiles[x, y];
                }
                catch
                {
                    // It is faster to try and cope with failure than to check upfront.
                    return Tile.BackgroundTile;
                }
            }
        }

        /// <summary>
        /// Returns a string per line representing the text on that line.
        /// </summary>
        private IReadOnlyList<string> GetText()
        {
            var strs = new string[Height];
            var chars = new char[Width];
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    chars[x] = this[x, y].GetCharacter();
                }
                strs[y] = new string(chars);
            }
            return strs;
        }
        
        private static int ComputeHashCode(Tile[,] tiles)
        {
            var hashCode = 0;
            foreach (var tile in tiles)
            {
                hashCode *= 5;
                hashCode += tile.GetHashCode();
            }
            return hashCode;
        }

        private static byte[] ComputeBytes(Tile[,] tiles)
        {
            var bytes = new byte[tiles.Length * 2];
            var position = 0;
            foreach (var tile in tiles)
            {
                var tileBytes = tile.GetBytes();
                bytes[position] = tileBytes.Item1;
                bytes[position + 1] = tileBytes.Item2;
                position += 2;
            }
            return bytes.Compress();
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var otherTiles = obj as Tiles;
            return otherTiles != null && Equals(otherTiles);
        }

        public bool Equals(Tiles otherTiles)
        {
            return otherTiles.GetHashCode() == GetHashCode() && bytes.FastEquals(otherTiles.bytes);
        }
    }
}
