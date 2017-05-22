using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DFWin.Models
{
    public class ScreenTools
    {
        public GraphicsDevice GraphicsDevice { get; }
        public SpriteBatch SpriteBatch { get; }

        public ScreenTools(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            GraphicsDevice = graphicsDevice;
            SpriteBatch = spriteBatch;
        }

        public int Width => ScreenBounds.Width;
        public int Height => ScreenBounds.Height;
        public Rectangle ScreenBounds => GraphicsDevice.PresentationParameters.Bounds;
    }
}
