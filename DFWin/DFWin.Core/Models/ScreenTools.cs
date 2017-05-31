using DFWin.Core.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DFWin.Core.Models
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

        public int Width => Sizes.DwarfFortressDefaultScreenSize.Width;
        public int Height => Sizes.DwarfFortressDefaultScreenSize.Height;
        public Rectangle ScreenBounds => GraphicsDevice.PresentationParameters.Bounds;
    }
}
