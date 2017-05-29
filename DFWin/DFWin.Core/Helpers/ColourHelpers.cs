using System;
using System.Collections.Generic;
using System.Linq;
using DFWin.Core.Constants;
using Microsoft.Xna.Framework;

namespace DFWin.Core.Helpers
{
    public static class ColourHelpers
    {
        private static readonly IDictionary<int, DwarfFortressColours> DwarfFortressColourByRgb;
        private static readonly IDictionary<DwarfFortressColours, Color> MonoGameColourByDwarfFortressColour;

        static ColourHelpers()
        {
            DwarfFortressColourByRgb = new Dictionary<int, DwarfFortressColours>
            {
                [RgbToIntRepresentation(0, 0, 0)] = DwarfFortressColours.Black,
                [RgbToIntRepresentation(0, 0, 128)] = DwarfFortressColours.Blue,
                [RgbToIntRepresentation(0, 128, 0)] = DwarfFortressColours.Green,
                [RgbToIntRepresentation(0, 128, 128)] = DwarfFortressColours.Cyan,
                [RgbToIntRepresentation(128, 0, 0)] = DwarfFortressColours.Red,
                [RgbToIntRepresentation(128, 0, 128)] = DwarfFortressColours.Magenta,
                [RgbToIntRepresentation(128, 128, 0)] = DwarfFortressColours.Brown,
                [RgbToIntRepresentation(192, 192, 192)] = DwarfFortressColours.LightGray,
                [RgbToIntRepresentation(128, 128, 128)] = DwarfFortressColours.DarkGray,
                [RgbToIntRepresentation(0, 0, 255)] = DwarfFortressColours.LightBlue,
                [RgbToIntRepresentation(0, 255, 0)] = DwarfFortressColours.LightGreen,
                [RgbToIntRepresentation(0, 255, 255)] = DwarfFortressColours.LightCyan,
                [RgbToIntRepresentation(255, 0, 0)] = DwarfFortressColours.LightRed,
                [RgbToIntRepresentation(255, 0, 255)] = DwarfFortressColours.LightMagenta,
                [RgbToIntRepresentation(255, 255, 0)] = DwarfFortressColours.Yellow,
                [RgbToIntRepresentation(255, 255, 255)] = DwarfFortressColours.White
            };

            MonoGameColourByDwarfFortressColour = DwarfFortressColourByRgb.ToDictionary(kvp => kvp.Value, kvp =>
            {
                var rgb = IntRepresentationToRgb(kvp.Key);
                return new Color(rgb.Item1, rgb.Item2, rgb.Item3);
            });
        }

        private static int RgbToIntRepresentation(int r, int g, int b)
        {
            return r + (256 * (g + (256 * b)));
        }

        private static Tuple<int, int, int> IntRepresentationToRgb(int representation)
        {
            return new Tuple<int, int, int>(representation % 256, (representation / 256) % 256, ((representation / 256) / 256) % 256);
        }

        public static DwarfFortressColours GetDwarfFortressColour(int r, int g, int b)
        {
            var hasKey = DwarfFortressColourByRgb.TryGetValue(RgbToIntRepresentation(r, g, b), out DwarfFortressColours colour);
            return hasKey ? colour : DwarfFortressColours.Black;
        }

        /// <summary>
        /// Returns the RGB colour represented by the Dwarf Fortress colour in Dwarf Fortress itself.
        /// </summary>
        public static Color ToColour(this DwarfFortressColours colour)
        {
            return MonoGameColourByDwarfFortressColour[colour];
        }
    }
}
