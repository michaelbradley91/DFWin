using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DFWin.Core.Inputs;
using DFWin.Core.Models;
using DFWin.Core.States;
using Microsoft.Xna.Framework.Input;
using KeyboardState = DFWin.Core.States.KeyboardState;

namespace DFWin.Core.Middleware
{
    public class KeyboardMiddleware : IUpdaterMiddleware
    {
        public GameState Update(GameState previousState, GameInput gameInput, Func<GameState, GameInput, GameState> next)
        {
            return next(UpdateWithKeyHistory(previousState, gameInput), gameInput);
        }

        private static GameState UpdateWithKeyHistory(GameState previousState, GameInput gameInput)
        {
            var newKeyRecordings = previousState.InputState.KeyboardState.KeyRecordings;

            var currentlyPressedKeys = gameInput.UserInput.Keyboard.GetPressedKeys();
            var previouslyPressedKeys = previousState.InputState.KeyboardState.PressedKeys;

            var keysJustPressed = currentlyPressedKeys.Except(previouslyPressedKeys);
            var keysJustReleased = previouslyPressedKeys.Except(currentlyPressedKeys);

            newKeyRecordings = newKeyRecordings.SetItems(keysJustPressed.Select(k => new KeyValuePair<Keys, KeyRecording>(k, new KeyRecording(k, true))));
            newKeyRecordings = newKeyRecordings.SetItems(keysJustReleased.Select(k => new KeyValuePair<Keys, KeyRecording>(k, new KeyRecording(k, false))));
            
            var newKeyboardHistory = new KeyboardState(newKeyRecordings, ImmutableHashSet.Create(currentlyPressedKeys));

            return new GameState(previousState.ScreenState, new InputState(newKeyboardHistory), previousState.FrameRate, previousState.ShouldExit);
        }
    }
}
