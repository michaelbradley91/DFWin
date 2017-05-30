using System;
using DFWin.Core.Helpers;

namespace DFWin.Core.Models
{
    public class Tiles
    {
        public int Width => tiles.GetLength(0);
        public int Height => tiles.GetLength(1);

        private readonly Tile[,] tiles;

        private readonly int hashCode;
        private readonly byte[] bytes;

        public Tiles(Tile[,] tiles)
        {
            this.tiles = tiles;
            hashCode = ComputeHashCode(tiles);
            bytes = ComputeBytes(tiles);
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
