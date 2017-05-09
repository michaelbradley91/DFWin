using System.Drawing;

namespace DFWin.Constants
{
    public static class Sizes
    {
        public static readonly Size DwarfFortressTileSize = new Size(16, 16);
        public static readonly Size DwarfFortressPreferredGridSize = new Size(80, 25);
        public static readonly Size DwarfFortressPreferredClientSize = new Size(DwarfFortressPreferredGridSize.Width * DwarfFortressTileSize.Width, DwarfFortressPreferredGridSize.Height * DwarfFortressTileSize.Height);
    }
}
