using System;
using DFWin.Core.States;
using Microsoft.Xna.Framework.Input;

namespace DFWin.Core.Middleware
{
    public class ExitMiddleware : IUpdaterMiddleware
    {
        public GameState Update(GameState previousState, Func<GameState, GameState> next)
        {
            return next(UpdateWithExit(previousState));
        }

        private static GameState UpdateWithExit(GameState previousState)
        {
            return previousState.GameInput.UserInput.KeyboardInput.KeyboardState.IsKeyDown(Keys.Escape) ?
                new GameState(previousState.ScreenState, previousState.GameInput, previousState.FrameRate, true) :
                previousState;
        }
    }
}