using DFWin.Core.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.TextureAtlases;

namespace DFWin
{
    public class ContentManager
    {
        public Texture2D LoadingBackground { get; private set; }
        public Texture2D WhiteRectangle { get; private set; }
        public SpriteFont MediumFont { get; private set; }
        public Song Song { get; private set; }
        public Texture2D BackupTileset { get; private set; }

        public const int BackupTileDiameter = 16;
        public TextureRegion2D[,] BackupTiles { get; private set; }

        public void Load(GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            LoadingBackground = content.Load<Texture2D>("LoadingBackground");
            MediumFont = content.Load<SpriteFont>("Px437_IBM_BIOS_Font");
            Song = content.Load<Song>("Vindsvept - Heart of Ice");
            BackupTileset = content.Load<Texture2D>("BackupTileSet");

            WhiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            WhiteRectangle.SetData(new[] { Color.White });
        }
    }
}
