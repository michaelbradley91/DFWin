using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace DFWin
{
    public class ContentManager
    {
        public Texture2D LoadingBackground { get; private set; }
        public Texture2D WhiteRectangle { get; private set; }
        public SpriteFont MediumFont { get; private set; }
        public Song Song { get; private set; }

        public void Load(GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            LoadingBackground = content.Load<Texture2D>("LoadingBackground");
            MediumFont = content.Load<SpriteFont>("Px437_IBM_BIOS_Font");
            Song = content.Load<Song>("Vindsvept - Heart of Ice");

            WhiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            WhiteRectangle.SetData(new[] { Color.White });
        }
    }
}
