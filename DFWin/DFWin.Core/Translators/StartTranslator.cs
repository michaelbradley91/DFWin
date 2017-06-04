using System;
using System.Collections.Generic;
using System.Linq;
using DFWin.Core.Constants;
using DFWin.Core.Helpers;
using DFWin.Core.Inputs.DwarfFortress;
using DFWin.Core.Models;

namespace DFWin.Core.Translators
{
    public class StartTranslator : Translator
    {
        private const string StartPlayingMenuOptionText = "Start Playing";

        public override IDwarfFortressInput TranslateOrNull(Tiles tiles)
        {
            if (!IsStartScreen(tiles)) return null;

            var menuOptionLineNumbers = GetMenuOptionLineNumbers(tiles).ToArray();

            var startInput = new StartInput
            {
                Tiles = tiles,
                Title = GetTitle(tiles),
                MenuOptions = GetMenuOptions(tiles, menuOptionLineNumbers).ToArray(),
                SelectedOption = GetSelectedMenuOption(tiles, menuOptionLineNumbers),
                DesignedBy = GetDesignedBy(tiles),
                ProgrammedBy = GetProgrammedBy(tiles),
                Version = GetVersion(tiles),
                VisitText = GetVisitText(tiles),
                VisitUrl = GetVisitUrl(tiles),
            };

            return startInput;
        }

        private static bool IsStartScreen(Tiles tiles)
        {
            return tiles.Text.Any(t => t.Contains(StartPlayingMenuOptionText));
        }

        private static string GetTitle(Tiles tiles)
        {
            return tiles.Text[3].Trim();
        }

        private static IEnumerable<int> GetMenuOptionLineNumbers(Tiles tiles)
        {
            var currentLine = tiles.Text.IndexOf(t => t.Contains(StartPlayingMenuOptionText));
            while (!string.IsNullOrWhiteSpace(tiles.Text[currentLine]))
            {
                yield return currentLine;
                currentLine++;
            }
        }

        private static IEnumerable<string> GetMenuOptions(Tiles tiles, IEnumerable<int> menuOptionLineNumbers)
        {
            return menuOptionLineNumbers.Select(l => tiles.Text[l].Trim());
        }

        private static int GetSelectedMenuOption(Tiles tiles, IEnumerable<int> menuOptionLineNumbers)
        {
            foreach (var (lineNumber, index) in menuOptionLineNumbers.Select((l, i) => (l, i)))
            {
                var textIndex = tiles.Text[lineNumber].AsEnumerable().IndexOf(c => c != ' ');
                if (tiles[textIndex, lineNumber].Foreground == DwarfFortressColours.White)
                {
                    return index;
                }
            }
            throw new InvalidOperationException("No menu option appeared selected");
        }

        private static string GetDesignedBy(Tiles tiles)
        {
            var line = tiles.Text.Single(t => t.Contains("Designed by"));
            var start = line.IndexOf("Designed by", StringComparison.InvariantCultureIgnoreCase) + "Designed by".Length;
            var end = line.IndexOf("  ", start, StringComparison.InvariantCultureIgnoreCase);
            return line.Substring(start, end - start).Trim();
        }

        private static string GetProgrammedBy(Tiles tiles)
        {
            var line = tiles.Text.Single(t => t.Contains("Programmed by"));
            var start = line.IndexOf("Programmed by", StringComparison.InvariantCultureIgnoreCase) + "Programmed by".Length;
            var end = line.IndexOf("  ", start, StringComparison.InvariantCultureIgnoreCase);
            return line.Substring(start, end - start).Trim();
        }

        private static string GetVersion(Tiles tiles)
        {
            var lastLine = tiles.Text.Last();
            // The 2 is to strip the "v" at the start of the version.
            var startOfVersion = lastLine.LastIndexOf(" ", StringComparison.InvariantCultureIgnoreCase) + 2;
            return lastLine.Substring(startOfVersion).Trim();
        }

        private static string GetVisitText(Tiles tiles)
        {
            var visitTextLine = tiles.Text[tiles.Text.Count - 3];
            var sections = visitTextLine.SplitByString("  ");
            return sections[1].Trim();
        }

        private static string GetVisitUrl(Tiles tiles)
        {
            var visitTextLine = tiles.Text[tiles.Text.Count - 2];
            var sections = visitTextLine.SplitByString("  ");
            return sections[1].Trim();
        }
    }
}
