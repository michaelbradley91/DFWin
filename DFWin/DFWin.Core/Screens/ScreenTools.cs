using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DFWin.Core.Screens
{
    public class ScreenTools
    {
        public SpriteBatch SpriteBatch { get; }

        private readonly RenderTarget2D renderTarget;

        public ScreenTools(SpriteBatch spriteBatch, RenderTarget2D renderTarget)
        {
            SpriteBatch = spriteBatch;

            this.renderTarget = renderTarget;
        }

        public int Width => renderTarget.Bounds.Width;
        public int Height => renderTarget.Bounds.Height;
        public Rectangle Bounds => new Rectangle(0, 0, Width, Height);
    }
}
