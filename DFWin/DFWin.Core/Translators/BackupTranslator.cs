using DFWin.Core.Inputs;
using DFWin.Core.Inputs.DwarfFortress;
using DFWin.Core.Models;

namespace DFWin.Core.Translators
{
    public interface IBackupTranslator : ITranslator { }

    public class BackupTranslator : Translator, IBackupTranslator
    {
        public override IDwarfFortressInput TranslateOrNull(Tiles tiles)
        {
            return new BackupInput {Tiles = tiles};
        }
    }
}
