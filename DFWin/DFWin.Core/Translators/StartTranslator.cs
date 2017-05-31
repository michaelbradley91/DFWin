using System;
using System.Diagnostics;
using DFWin.Core.Inputs;
using DFWin.Core.Models;

namespace DFWin.Core.Translators
{
    public class StartTranslator : ITranslator
    {
        public bool CanTranslate(Tiles tiles)
        {
            var text = tiles.GetText();
            Debug.WriteLine(string.Join(Environment.NewLine, text));
            return false;
        }

        public DwarfFortressInput Translate(Tiles tiles)
        {
            throw new System.NotImplementedException();
        }
    }
}
