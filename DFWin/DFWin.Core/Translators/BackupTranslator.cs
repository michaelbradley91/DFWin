using DFWin.Core.Inputs;
using DFWin.Core.Models;
using DFWin.Core.Services;

namespace DFWin.Core.Translators
{
    public interface IBackupTranslator : ITranslator { }

    public class BackupTranslator : Translator, IBackupTranslator
    {
        public override bool CanTranslate(Tile[,] tiles)
        {
            return true;
        }

        public override DwarfFortressInput Translate(Tile[,] tiles)
        {
            return new DwarfFortressInput(tiles);
        }
    }
}
