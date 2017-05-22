using System;
using DFWin.Core;
using DFWin.Core.State;
using DFWin.Models;
using DFWin.Screens;

namespace DFWin
{
    public class ScreenManager
    {
        private readonly GameState gameState;

        public ScreenManager(GameState gameState)
        {
            this.gameState = gameState;
        }

        public void Draw(ScreenTools screenTools)
        {
            GetCurrentScreen().Draw(screenTools, gameState.ScreenState);
        }

        private IScreen GetCurrentScreen()
        {
            switch (gameState.CurrentScreen)
            {
                case Core.Constants.Screens.Loading:
                    return DfWin.Resolve<LoadingScreen>();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
