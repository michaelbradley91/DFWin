using System.Drawing;

namespace DFWin.Core.Constants
{
    public static class Sizes
    {
        public static readonly Size DwarfFortressTileSize = new Size(3, 3);
        public static readonly Size DwarfFortressGridSize = new Size(80, 25);

        // If the Dwarf Fortress window does not match this size, DFWin may not function correctly.
        public static readonly Size DwarfFortressClientSize = new Size(DwarfFortressGridSize.Width * DwarfFortressTileSize.Width, DwarfFortressGridSize.Height * DwarfFortressTileSize.Height);

        // This is the size of the screen that all parts of the game draw to. It is then scaled according to the device resolution.
        public static readonly Size DwarfFortressTargetScreenSize = new Size(1366, 768);

        public static readonly Size BackupScreenBorder = new Size(20, 20);
        public static readonly Size BackupScreenSize = new Size((DwarfFortressGridSize.Width * 16) + (BackupScreenBorder.Width * 2), (DwarfFortressGridSize.Height * 16) + (BackupScreenBorder.Height * 2));
    }
}
