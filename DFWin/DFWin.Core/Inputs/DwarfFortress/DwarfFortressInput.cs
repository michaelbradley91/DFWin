using DFWin.Core.Models;

namespace DFWin.Core.Inputs.DwarfFortress
{
    public interface IDwarfFortressInput
    {
        Tiles Tiles { get; }
    }

    public class DwarfFortressInput : IDwarfFortressInput
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
