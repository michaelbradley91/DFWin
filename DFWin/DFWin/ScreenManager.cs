using System;
using System.Collections.Generic;
using System.Linq;
using DFWin.Core.States;
using DFWin.Models;
using DFWin.Screens;

namespace DFWin
{
    public class ScreenManager
    {
        private readonly GameState gameState;
        private readonly ICollection<IScreen> screens;

        private readonly Dictionary<Type, IScreen> screenByState = new Dictionary<Type, IScreen>();

        public ScreenManager(GameState gameState, IEnumerable<IScreen> screens)
        {
            this.gameState = gameState;
            this.screens = screens.ToList();
        }

        public void Draw(ScreenTools screenTools)
        {
            GetCurrentScreen().Draw(screenTools, gameState.ScreenState);
        }

        private IScreen GetCurrentScreen()
        {
            var success = screenByState.TryGetValue(gameState.ScreenState.GetType(), out IScreen screen);
            if (success) return screen;

            var screenName = GetScreenName(gameState.ScreenState);
            screen = screens.Single(s => s.GetType().Name == screenName);

            screenByState[gameState.ScreenState.GetType()] = screen;

            return screen;
        }

        private string GetScreenName(IScreenState screenState)
        {
            var stateName = screenState.GetType().Name;
            return stateName.Substring(0, stateName.Length - "State".Length) + "Screen";
        }
    }
}
