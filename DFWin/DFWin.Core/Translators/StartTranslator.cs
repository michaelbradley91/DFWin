using System;
using System.Collections.Generic;
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

            var startInput = new StartInput
            {
                Tiles = tiles,
                Title = tiles.Text[3].Trim(),
                MenuOptions = GetMenuOptions(tiles).ToArray(),
                DesignedBy = GetDesignedBy(tiles),
                ProgrammedBy = GetProgrammedBy(tiles),

            };

            return startInput;
        }

        private static IEnumerable<string> GetMenuOptions(Tiles tiles)
        {
            var currentLine = 5;
            while (!string.IsNullOrWhiteSpace(tiles.Text[currentLine]))
            {
                yield return tiles.Text[currentLine].Trim();
                currentLine++;
            }
        }

        private static string GetDesignedBy(Tiles tiles)
        {
            var line = tiles.Text.Single(t => t.Contains("Designed by"));
            var start = line.IndexOf("Designed by", StringComparison.InvariantCultureIgnoreCase) + "Designed by".Length;
            var end = line.IndexOf("  ", start, StringComparison.InvariantCultureIgnoreCase);
            return line.Substring(start, end - start);
        }

        private static string GetProgrammedBy(Tiles tiles)
        {
            var line = tiles.Text.Single(t => t.Contains("Programmed by"));
            var start = line.IndexOf("Programmed by", StringComparison.InvariantCultureIgnoreCase) + "Programmed by".Length;
            var end = line.IndexOf("  ", start, StringComparison.InvariantCultureIgnoreCase);
            return line.Substring(start, end - start);
        }

        private static bool IsStartScreen(Tiles tiles)
        {
            return tiles.Text.Any(t => t.Contains("Start Playing"));
        }
    }
}
