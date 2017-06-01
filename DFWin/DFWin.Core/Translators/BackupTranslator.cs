using DFWin.Core.Inputs;
using DFWin.Core.Models;

namespace DFWin.Core.Translators
{
    public interface IBackupTranslator : ITranslator { }

    public class BackupTranslator : Translator, IBackupTranslator
    {
        public override DwarfFortressInput TranslateOrNull(Tiles tiles)
        {
            return new DwarfFortressInput(tiles);
        }
    }
}
