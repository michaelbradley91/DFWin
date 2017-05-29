using System;
using DFWin.Core.Constants;
using DFWin.Core.Helpers;
using DFWin.Core.Models;
using DFWin.Core.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DFWin.Screens
{
    public class BackupScreen : Screen<BackupState>
    {
        private readonly ContentManager contentManager;

        private Texture2D WhiteRectangle => contentManager.WhiteRectangle;
        private const int TileDiameter = ContentManager.BackupTileDiameter;
        private const int WindowBorder = 20;

        public BackupScreen(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        public override void Draw(BackupState state, ScreenTools screenTools)
        {
            if (!state.HasTiles) return;

            for (var x = 0; x < state.Tiles.GetLength(0); x++)
            {
                for (var y = 0; y < state.Tiles.GetLength(1); y++)
                {
                    DrawBackupTile(screenTools, x, y, state.Tiles[x, y]);
                }
            }
        }

        private void DrawBackupTile(ScreenTools screenTools, int x, int y, Tile tile)
        {
            var sourceRectangle = new Rectangle(TileDiameter * tile.TileSetX, TileDiameter * tile.TileSetY, TileDiameter, TileDiameter);

            var tileWidth = (screenTools.Width - (WindowBorder * 2)) / Sizes.DwarfFortressPreferredGridSize.Width;
            var tileHeight = (screenTools.Height - (WindowBorder * 2)) / Sizes.DwarfFortressPreferredGridSize.Height;

            var destinationTileSize = Math.Min(tileWidth, tileHeight);

            var destinationXOffset = (screenTools.Width - ((destinationTileSize * Sizes.DwarfFortressPreferredGridSize.Width) + (WindowBorder * 2))) / 2;
            var destinationYOffset = (screenTools.Height - ((destinationTileSize * Sizes.DwarfFortressPreferredGridSize.Height) + (WindowBorder * 2))) / 2;

            var destinationRectangle = new Rectangle(destinationXOffset + (x * destinationTileSize), destinationYOffset + (y * destinationTileSize), destinationTileSize, destinationTileSize);

            screenTools.SpriteBatch.Draw(WhiteRectangle, destinationRectangle, tile.Background.ToColour());
            screenTools.SpriteBatch.Draw(contentManager.BackupTileset, destinationRectangle, sourceRectangle, tile.Foreground.ToColour());
        }
    }
}
