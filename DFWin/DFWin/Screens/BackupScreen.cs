using DFWin.Attributes;
using DFWin.Core.Constants;
using DFWin.Core.Helpers;
using DFWin.Core.Models;
using DFWin.Core.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DFWin.Screens
{
    [TargetScreenSize(Sizes.BackupTargetScreenWidth, Sizes.BackupTargetScreenHeight)]
    public class BackupScreen : Screen<BackupState>
    {
        private readonly ContentManager contentManager;

        private Texture2D WhiteRectangle => contentManager.WhiteRectangle;

        public BackupScreen(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        public override void Draw(BackupState state, ScreenTools screenTools)
        {
            if (!state.HasTiles) return;

            for (var x = 0; x < state.Tiles.Width; x++)
            {
                for (var y = 0; y < state.Tiles.Height; y++)
                {
                    DrawBackupTile(screenTools, x, y, state.Tiles[x, y]);
                }
            }
        }

        private void DrawBackupTile(ScreenTools screenTools, int x, int y, Tile tile)
        {
            var sourceRectangle = new Rectangle(
                Sizes.BackupTileSize * tile.TileSetX, 
                Sizes.BackupTileSize * tile.TileSetY, 
                Sizes.BackupTileSize, 
                Sizes.BackupTileSize);

            var destinationRectangle = new Rectangle(
                Sizes.BackupScreenBorder.Width + (x * Sizes.BackupTileSize),
                Sizes.BackupScreenBorder.Height + (y * Sizes.BackupTileSize),
                Sizes.BackupTileSize,
                Sizes.BackupTileSize);

            screenTools.SpriteBatch.Draw(WhiteRectangle, destinationRectangle, tile.Background.ToColour());
            screenTools.SpriteBatch.Draw(contentManager.BackupTileset, destinationRectangle, sourceRectangle, tile.Foreground.ToColour());
        }
    }
}
