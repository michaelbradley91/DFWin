using System;
using DFWin.Core.Inputs;
using DFWin.Core.States;
using Microsoft.Xna.Framework.Input;

namespace DFWin.Core.Middleware
{
    public class ExitMiddleware : IUpdaterMiddleware
    {
        public GameState Update(GameState previousState, GameInput gameInput, Func<GameState, GameInput, GameState> next)
        {
            return next(UpdateWithExit(previousState, gameInput), gameInput);
        }

        private static GameState UpdateWithExit(GameState previousState, GameInput gameInput)
        {
            return gameInput.UserInput.Keyboard.IsKeyDown(Keys.Escape) ?
                new GameState(previousState.ScreenState, previousState.GameInput, previousState.FrameRate, true) :
                previousState;
        }
    }
}