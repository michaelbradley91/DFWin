using System.Drawing;

namespace DFWin.Core.Constants
{
    public static class Sizes
    {
        public static readonly Size DwarfFortressTileSize = new Size(3, 3);
        public static readonly Size DwarfFortressPreferredGridSize = new Size(80, 25);
        public static readonly Size DwarfFortressPreferredClientSize = new Size(DwarfFortressPreferredGridSize.Width * DwarfFortressTileSize.Width, DwarfFortressPreferredGridSize.Height * DwarfFortressTileSize.Height);
        public static readonly Size DwarfFortressPreferredRawTileSize = new Size(24, 24);
        public static readonly Size DwarfFortressDefaultScreenSize = new Size(1440, 810);
    }
}
