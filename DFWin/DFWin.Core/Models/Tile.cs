using System;
using DFWin.Core.Constants;
using DFWin.Core.Helpers;

namespace DFWin.Core.Models
{
    public class Tile
    {
        public byte Value { get; set; }
        public DwarfFortressColours Foreground { get; set; }
        public DwarfFortressColours Background { get; set; }

        public byte TileSetX => (byte)(Value % 16);
        public byte TileSetY => (byte)(Value / 16);

        private readonly int identifier;

        public Tile(byte value, DwarfFortressColours foreground, DwarfFortressColours background)
        {
            Value = value;
            Foreground = foreground;
            Background = background;

            identifier = Value + (256 * ((int)Foreground + (256 * (int)Background)));
        }

        public static readonly Tile BackgroundTile = new Tile(0, DwarfFortressColours.Black, DwarfFortressColours.Black);

        /// <summary>
        /// Returns the character represented by this tile. If the tile is not a character, this
        /// returns a space.
        /// </summary>
        public char GetCharacter()
        {
            return TileHelpers.CharacterMap[Value];
        }

        public override int GetHashCode()
        {
            return identifier;
        }

        public Tuple<byte, byte> GetBytes()
        {
            return Tuple.Create(Value, (byte)((byte)Foreground + (16 * (byte)Background)));
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var tile = obj as Tile;
            if (tile == null) return false;
            return Equals(tile);
        }

        public bool Equals(Tile tile)
        {
            return tile.identifier == identifier;
        }
    }
}