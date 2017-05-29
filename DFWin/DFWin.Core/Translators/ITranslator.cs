using DFWin.Core.Constants;
using DFWin.Core.Inputs;
using DFWin.Core.Models;
using DFWin.Core.Services;

namespace DFWin.Core.Translators
{
    public interface ITranslator
    {
        bool CanTranslate(Tile[,] tiles);
        DwarfFortressInput Translate(Tile[,] tiles);
    }

    public abstract class Translator : ITranslator
    {
        protected int Width = Sizes.DwarfFortressPreferredGridSize.Width;
        protected int Height = Sizes.DwarfFortressPreferredGridSize.Height;

        public abstract bool CanTranslate(Tile[,] tiles);
        public abstract DwarfFortressInput Translate(Tile[,] tiles);
    }
}
