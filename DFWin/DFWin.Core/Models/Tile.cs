using DFWin.Core.Constants;

namespace DFWin.Core.Models
{
    public class Tile
    {
        public byte Value { get; set; }
        public DwarfFortressColours Foreground { get; set; }
        public DwarfFortressColours Background { get; set; }

        public byte TileSetX => (byte)(Value % 16);
        public byte TileSetY => (byte)(Value / 16);

        public Tile(byte value, DwarfFortressColours foreground, DwarfFortressColours background)
        {
            Value = value;
            Foreground = foreground;
            Background = background;
        }
    }
}