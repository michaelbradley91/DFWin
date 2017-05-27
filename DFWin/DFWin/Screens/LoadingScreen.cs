using System;
using DFWin.Core.Models;
using DFWin.Core.States;
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

            screenTools.SpriteBatch.Draw(WhiteRectangle, new RectangleF(screenTools.Width * 0.1f, screenTools.Height * 0.9f, screenTools.Width * 0.8f, screenTools.Height * 0.06f).ToRectangle(), Colours.LoadingBarBackground);
            screenTools.SpriteBatch.Draw(WhiteRectangle, new RectangleF(screenTools.Width * 0.1f, screenTools.Height * 0.9f, screenTools.Width * 0.8f * (loadingState.LoadingPercent / 100f), screenTools.Height * 0.06f).ToRectangle(), Colours.LoadingBar);

            CentreString(screenTools, loadingState.Message, new Vector2(screenTools.Width / 2f, screenTools.Height * 0.4f), Color.Red);
        }

        private void CentreString(ScreenTools tools, string text, Vector2 position, Color colour)
        {
            var size = contentManager.MediumFont.MeasureString(text);

            tools.SpriteBatch.DrawString(contentManager.MediumFont, text, new Vector2(position.X - (size.X / 2f), position.Y - (size.Y / 2f)), colour);
        }
    }
}
