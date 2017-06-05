using System;
using System.Collections.Generic;
using System.Linq;
using DFWin.Core.Middleware;
using DFWin.Core.Models;
using DFWin.Core.Screens;
using DFWin.Core.States;

namespace DFWin.Core
{
    public interface IScreenManager
    {
        void Draw(GameState gameState, ScreenTools screenTools);
        IScreen GetCurrentScreen(GameState gameState);
        IReadOnlyCollection<IScreen> AllScreens { get; }
    }

    public class ScreenManager : IScreenManager
    {
        public IReadOnlyCollection<IScreen> AllScreens { get; }

        private readonly IEnumerable<IScreenMiddleware> middleware;
        private readonly Dictionary<Type, IScreen> screenByState = new Dictionary<Type, IScreen>();

        public ScreenManager(IEnumerable<IScreenMiddleware> middleware, IEnumerable<IScreen> screens)
        {
            this.middleware = middleware;
            AllScreens = screens.ToList();
        }

        public void Draw(GameState gameState, ScreenTools screenTools)
        {
            DrawWithMiddleware(gameState, screenTools, middleware.GetEnumerator());
        }

        private void DrawWithMiddleware(GameState gameState, ScreenTools screenTools, IEnumerator<IScreenMiddleware> middlewareRemaining)
        {
            if (middlewareRemaining.MoveNext())
            {
                middlewareRemaining.Current.Draw(gameState, screenTools, (g, t) => DrawWithMiddleware(g, t, middlewareRemaining));
            }
            else
            {
                GetCurrentScreen(gameState).Draw(gameState, screenTools);
            }
        }

        public IScreen GetCurrentScreen(GameState gameState)
        {
            var success = screenByState.TryGetValue(gameState.ScreenState.GetType(), out IScreen screen);
            if (success) return screen;

            var screenName = GetScreenName(gameState.ScreenState);
            screen = AllScreens.Single(s => s.GetType().Name == screenName);

            screenByState[gameState.ScreenState.GetType()] = screen;

            return screen;
        }

        private static string GetScreenName(IScreenState screenState)
        {
            var stateName = screenState.GetType().Name;
            return stateName.Substring(0, stateName.Length - "State".Length) + "Screen";
        }
    }
}
