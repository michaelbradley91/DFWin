using DFWin.Core.Inputs;
using DFWin.Core.Models;

namespace DFWin.Core.Translators
{
    public interface ITranslator
    {
        /// <summary>
        /// Try to provide the Dwarf Fortress input for the given array of tiles.
        /// If you are unable to translate the input, try to return false as quickly as possible to speed up translation.
        /// </summary>
        bool TryTranslate(Tiles tiles, out DwarfFortressInput dwarfFortressInput);
    }

    public abstract class Translator : ITranslator
    {
        public bool TryTranslate(Tiles tiles, out DwarfFortressInput dwarfFortressInput)
        {
            dwarfFortressInput = TranslateOrNull(tiles);
            return dwarfFortressInput != null;
        }

        public abstract DwarfFortressInput TranslateOrNull(Tiles tiles);
    }
}
