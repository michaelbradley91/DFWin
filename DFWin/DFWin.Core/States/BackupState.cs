using DFWin.Core.Models;

namespace DFWin.Core.States
{
    public class BackupState : IScreenState
    {
        public bool HasTiles => Tiles != null;
        public Tiles Tiles { get; }

        public BackupState(Tiles tiles)
        {
            Tiles = tiles;
        }
    }
}
