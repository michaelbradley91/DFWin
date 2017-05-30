using DFWin.Core.Models;
using DFWin.Core.Services;

namespace DFWin.Core.Inputs
{
    public class DwarfFortressInput
    {
        public Tiles Tiles { get; }

        public DwarfFortressInput(Tiles tiles)
        {
            Tiles = tiles;
        }

        private DwarfFortressInput() { }

        public static readonly DwarfFortressInput None = new DwarfFortressInput();
    }
}
