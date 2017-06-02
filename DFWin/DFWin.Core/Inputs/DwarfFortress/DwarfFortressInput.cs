using DFWin.Core.Models;

namespace DFWin.Core.Inputs.DwarfFortress
{
    public interface IDwarfFortressInput
    {
        Tiles Tiles { get; }
    }

    public class DwarfFortressInput : IDwarfFortressInput
    {
        public Tiles Tiles { get; set; }

        public static readonly DwarfFortressInput None = new DwarfFortressInput();
    }
}
