using DFWin.Core.Services;

namespace DFWin.Core.Inputs
{
    public class DwarfFortressInput
    {
        public Tile[,] Tiles { get; }

        public DwarfFortressInput(Tile[,] tiles)
        {
            Tiles = tiles;
        }
    }
}
