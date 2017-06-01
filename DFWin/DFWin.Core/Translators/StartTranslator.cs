using System.Linq;
using DFWin.Core.Inputs.DwarfFortress;
using DFWin.Core.Models;

namespace DFWin.Core.Translators
{
    public class StartTranslator : Translator
    {
        public override IDwarfFortressInput TranslateOrNull(Tiles tiles)
        {
            if (!IsStartScreen(tiles)) return null;            
            
            return null;
        }

        private bool IsStartScreen(Tiles tiles)
        {
            return tiles.Text.Any(t => t.Contains("Start Playing"));
        }
    }
}
