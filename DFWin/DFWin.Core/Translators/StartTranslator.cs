using System;
using System.Diagnostics;
using DFWin.Core.Inputs;
using DFWin.Core.Models;

namespace DFWin.Core.Translators
{
    public class StartTranslator : Translator
    {
        public override DwarfFortressInput TranslateOrNull(Tiles tiles)
        {
            Debug.WriteLine(string.Join(Environment.NewLine, tiles.Text));
            return null;
        }
    }
}
