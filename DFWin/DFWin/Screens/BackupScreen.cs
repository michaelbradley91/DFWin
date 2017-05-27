using DFWin.Core.Models;
using DFWin.Core.Services;
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
            var destinationRectangle = new Rectangle(20 + (x * TileDiameter), 20 + (y * TileDiameter), TileDiameter, TileDiameter);

            screenTools.SpriteBatch.Draw(WhiteRectangle, destinationRectangle, tile.Background);
            screenTools.SpriteBatch.Draw(contentManager.BackupTileset, destinationRectangle, sourceRectangle, tile.Foreground);
        }
    }
}
