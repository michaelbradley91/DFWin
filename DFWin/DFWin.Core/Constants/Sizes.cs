using System.Drawing;

namespace DFWin.Core.Constants
{
    public static class Sizes
    {
        public const int DwarfFortressTileSize = 3;

        public const int DwarfFortressGridWidth = 80;
        public const int DwarfFortressGridHeight = 25;
        public static readonly Size DwarfFortressGridSize = new Size(DwarfFortressGridWidth, DwarfFortressGridHeight);

        // If the Dwarf Fortress window does not match this size, DFWin may not function correctly.
        public const int DwarfFortressClientWidth = DwarfFortressGridWidth * DwarfFortressTileSize;
        public const int DwarfFortressClientHeight = DwarfFortressGridHeight * DwarfFortressTileSize;
        public static readonly Size DwarfFortressClientSize = new Size(DwarfFortressClientWidth, DwarfFortressClientHeight);

        // This is the size of the screen that all parts of the game draw to. It is then scaled according to the device resolution.
        public const int DefaultTargetScreenWidth = 1366;
        public const int DefaultTargetScreenHeight = 768;
        public static readonly Size DefaultTargetScreenSize = new Size(DefaultTargetScreenWidth, DefaultTargetScreenHeight);

        public const int BackupScreenBorderWidth = 20;
        public const int BackupScreenBorderHeight = 20;
        public static readonly Size BackupScreenBorder = new Size(BackupScreenBorderWidth, BackupScreenBorderHeight);

        public const int BackupTileSize = 16;
        public const int BackupTargetScreenWidth = (DwarfFortressGridWidth * BackupTileSize) + (BackupScreenBorderWidth * 2);
        public const int BackupTargetScreenHeight = (DwarfFortressGridHeight * BackupTileSize) + (BackupScreenBorderHeight * 2);
        public static readonly Size BackupScreenSize = new Size(BackupTargetScreenWidth, BackupTargetScreenHeight);
    }
}
