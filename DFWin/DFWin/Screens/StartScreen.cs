using DFWin.Attributes;
using DFWin.Core.Constants;
using DFWin.Core.Helpers;
using DFWin.Core.Models;
using DFWin.Core.States.DwarfFortress;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DFWin.Screens
{
    // TODO draw the start screen properly
    [TargetScreenSize(Sizes.BackupTargetScreenWidth, Sizes.BackupTargetScreenHeight)]
    public class StartScreen : Screen<StartState>
    {
        private Texture2D WhiteRectangle => ContentManager.WhiteRectangle;
        private Texture2D BackupTileSet => ContentManager.BackupTileSet;

        public override void Draw(StartState state, ScreenTools screenTools)
        {
            for (var x = 0; x < state.Input.Tiles.Width; x++)
            {
                for (var y = 0; y < state.Input.Tiles.Height; y++)
                {
                    DrawBackupTile(screenTools, x, y, state.Input.Tiles[x, y]);
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
            screenTools.SpriteBatch.Draw(BackupTileSet, destinationRectangle, sourceRectangle, tile.Foreground.ToColour());
        }
    }
}
