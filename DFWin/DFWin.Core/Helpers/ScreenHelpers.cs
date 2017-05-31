using System;
using System.Drawing;
using DFWin.Core.Constants;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using RectangleF = MonoGame.Extended.RectangleF;

namespace DFWin.Core.Helpers
{
    public static class ScreenHelpers
    {
        public static Rectangle GetRectangleToAspectFill(Rectangle outerRectangle, Rectangle innerRectangle)
        {
            var widthRatio = outerRectangle.Width / ((float)innerRectangle.Width);
            var heightRatio = outerRectangle.Height / ((float)innerRectangle.Height);
            var minRatio = Math.Min(widthRatio, heightRatio);
            var targetWidth = minRatio * innerRectangle.Width;
            var targetHeight = minRatio * innerRectangle.Height;
            var topLeftX = (outerRectangle.Width - targetWidth) / 2;
            var topLeftY = (outerRectangle.Height - targetHeight) / 2;
            return new RectangleF(topLeftX, topLeftY, targetWidth, targetHeight).ToRectangle();
        }

        /// <summary>
        /// Creates a render target that can be used to draw to for the specified width.
        /// You should only create render targets once. Then you can reuse them.
        /// </summary>
        public static RenderTarget2D CreateRenderTarget(GraphicsDevice graphicsDevice, Size size)
        {
            return new RenderTarget2D(
                graphicsDevice,
                size.Width,
                size.Height,
                false,
                SurfaceFormat.Color, 
                DepthFormat.None,
                graphicsDevice.PresentationParameters.MultiSampleCount,
                RenderTargetUsage.DiscardContents);
        }
    }
}
