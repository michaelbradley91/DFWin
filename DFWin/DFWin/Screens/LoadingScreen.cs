using System;
using DFWin.Core.Constants;
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
        private Texture2D Background => ContentManager.LoadingBackground;
        private Texture2D WhiteRectangle => ContentManager.WhiteRectangle;
        private SpriteFont MediumFont => ContentManager.MediumFont;

        public override void Draw(LoadingState state, ScreenTools screenTools)
        {
            var widthMultiplier = screenTools.Width / ((float)Background.Width);
            var heightMultiplier = screenTools.Height / ((float)Background.Height);

            var multiplier = Math.Min(widthMultiplier, heightMultiplier);

            var targetWidth = multiplier * screenTools.Width;
            var targetHeight = multiplier * screenTools.Height;

            screenTools.SpriteBatch.Draw(Background, new RectangleF((screenTools.Width - targetWidth) / 2f, (screenTools.Height - targetHeight) / 2f, targetWidth, targetHeight).ToRectangle(), Color.White);

            screenTools.SpriteBatch.Draw(WhiteRectangle, new RectangleF(screenTools.Width * 0.1f, screenTools.Height * 0.9f, screenTools.Width * 0.8f, screenTools.Height * 0.06f).ToRectangle(), Colours.LoadingBarBackground);
            screenTools.SpriteBatch.Draw(WhiteRectangle, new RectangleF(screenTools.Width * 0.1f, screenTools.Height * 0.9f, screenTools.Width * 0.8f * (state.LoadingPercent / 100f), screenTools.Height * 0.06f).ToRectangle(), Colours.LoadingBar);

            CentreString(screenTools, GetMessage(state), new Vector2(screenTools.Width / 2f, screenTools.Height * 0.4f), Color.Red);
        }

        private static string GetMessage(LoadingState loadingState)
        {
            switch (loadingState.Phase)
            {
                case LoadingPhase.WaitingForDwarfFortressToStart:
                    return "Please start Dwarf Fortress";
                case LoadingPhase.WaitingForWarmUpToFinish:
                    return "Warming up...";
                case LoadingPhase.WarmUpSuccessful:
                    return "Warm up successful! Starting...";
                case LoadingPhase.WarmUpUnsuccessful:
                    return "Warm up unsuccessful. Starting...";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CentreString(ScreenTools tools, string text, Vector2 position, Color colour)
        {
            var size = MediumFont.MeasureString(text);

            tools.SpriteBatch.DrawString(MediumFont, text, new Vector2(position.X - (size.X / 2f), position.Y - (size.Y / 2f)), colour);
        }
    }
}
