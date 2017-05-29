﻿using DFWin.Core.Models;
using DFWin.Core.Services;

namespace DFWin.Core.Inputs
{
    public class DwarfFortressInput
    {
        public Tile[,] Tiles { get; }

        public DwarfFortressInput(Tile[,] tiles)
        {
            Tiles = tiles;
        }

        private DwarfFortressInput() { }

        public static readonly DwarfFortressInput None = new DwarfFortressInput();
    }
}
