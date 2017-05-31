using DFWin.Core.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DFWin.Core.Models
{
    public class ScreenTools
    {
        public SpriteBatch SpriteBatch { get; }

        public ScreenTools(SpriteBatch spriteBatch)
        {
            SpriteBatch = spriteBatch;
        }

        public int Width => Sizes.DefaultTargetScreenSize.Width;
        public int Height => Sizes.DefaultTargetScreenSize.Height;
        public Rectangle Bounds => new Rectangle(0, 0, Width, Height);
    }
}
