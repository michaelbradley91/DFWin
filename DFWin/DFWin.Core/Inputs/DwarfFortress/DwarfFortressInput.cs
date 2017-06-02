using DFWin.Core.Models;

namespace DFWin.Core.Inputs.DwarfFortress
{
    public interface IDwarfFortressInput
    {
        bool IsInitialised { get; }
        Tiles Tiles { get; }
    }

    public class DwarfFortressInput : IDwarfFortressInput
    {
        public bool IsInitialised => Tiles != null;
        public Tiles Tiles { get; set; }

        public static readonly DwarfFortressInput None = new DwarfFortressInput();
    }
}
