using DFWin.Core.Models;

namespace DFWin.Core.Inputs.DwarfFortress
{
    public interface IDwarfFortressInput
    {
        Tiles Tiles { get; }
    }

    public abstract class DwarfFortressInput : IDwarfFortressInput
    {
        public Tiles Tiles { get; set; }

        public static readonly DwarfFortressInput None = null;
    }
}
