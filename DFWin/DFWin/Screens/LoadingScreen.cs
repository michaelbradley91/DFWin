using System;
using DFWin.Core.State;
using DFWin.Models;
using DFWin.Styles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace DFWin.Screens
{
    public class LoadingScreen : Screen<LoadingState>
    {
        private readonly ContentManager contentManager;

        private Texture2D Background => contentManager.LoadingBackground;
        private Texture2D WhiteRectangle => contentManager.WhiteRectangle;

        public LoadingScreen(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        public override void Draw(ScreenTools screenTools, LoadingState loadingState)
        {
            var widthMultiplier = screenTools.Width / ((float)Background.Width);
            var heightMultiplier = screenTools.Height / ((float)Background.Height);

            var multiplier = Math.Min(widthMultiplier, heightMultiplier);

            var targetWidth = multiplier * screenTools.Width;
            var targetHeight = multiplier * screenTools.Height;

            screenTools.SpriteBatch.Draw(Background, new RectangleF((screenTools.Width - targetWidth) / 2f, (screenTools.Height - targetHeight) / 2f, targetWidth, targetHeight).ToRectangle(), Color.White);

            screenTools.SpriteBatch.Draw(WhiteRectangle, new RectangleF(screenTools.Width * 0.1f, screenTools.Height * 0.89f, screenTools.Width * 0.8f, screenTools.Height * 0.08f).ToRectangle(), Colours.LoadingBarBackground);
            screenTools.SpriteBatch.Draw(WhiteRectangle, new RectangleF(screenTools.Width * 0.1f, screenTools.Height * 0.89f, screenTools.Width * 0.8f * (loadingState.LoadingPercent / 100f), screenTools.Height * 0.08f).ToRectangle(), Colours.LoadingBar);
        }
    }
}
