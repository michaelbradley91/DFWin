using DFWin.Core.Services;

namespace DFWin.Core.States
{
    public class BackupState : IScreenState
    {
        public bool HasTiles => Tiles != null;
        public Tile[,] Tiles { get; }

        public BackupState(Tile[,] tiles)
        {
            Tiles = tiles;
        }
    }
}
