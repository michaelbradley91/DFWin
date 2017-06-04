using System;
using DFWin.Core.Inputs;
using DFWin.Core.States;
using Microsoft.Xna.Framework.Input;

namespace DFWin.Core.Middleware
{
    public class ExitMiddleware : IUpdaterMiddleware
    {
        public GameState Update(GameState previousState, Func<GameState, GameState> next)
        {
            return IsUserExiting(previousState.GameInput.UserInput.KeyboardInput)
                ? new GameState(previousState.ScreenState, previousState.GameInput, previousState.FrameRate, true)
                : next(previousState);
        }

        private static bool IsUserExiting(KeyboardInput keyboardInput)
        {
            var pressedKeys = keyboardInput.CurrentlyPressedKeys;
            var isAltDown = pressedKeys.Contains(Keys.LeftAlt) || pressedKeys.Contains(Keys.RightAlt);
            var isF4Down = pressedKeys.Contains(Keys.F4);
            return isAltDown && isF4Down;
        }
    }
}