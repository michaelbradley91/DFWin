using DFWin.Core.Inputs;
using DFWin.Core.Inputs.DwarfFortress;
using DFWin.Core.Models;

namespace DFWin.Core.Translators
{
    public interface ITranslator
    {
        /// <summary>
        /// Try to provide the Dwarf Fortress input for the given array of tiles.
        /// If you are unable to translate the input, try to return false as quickly as possible to speed up translation.
        /// </summary>
        bool TryTranslate(Tiles tiles, out IDwarfFortressInput dwarfFortressInput);
    }

    public abstract class Translator : ITranslator
    {
        public bool TryTranslate(Tiles tiles, out IDwarfFortressInput dwarfFortressInput)
        {
            dwarfFortressInput = TranslateOrNull(tiles);
            return dwarfFortressInput != null;
        }

        public abstract IDwarfFortressInput TranslateOrNull(Tiles tiles);
    }
}
