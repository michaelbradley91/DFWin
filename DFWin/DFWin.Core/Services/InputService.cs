using System.Collections.Generic;
using System.Collections.Immutable;
using DFWin.Core.Inputs;
using DFWin.Core.Models;
using DFWin.Core.States;
using Microsoft.Xna.Framework.Input;
using KeyboardState = DFWin.Core.States.KeyboardState;

namespace DFWin.Core.Services
{
    public interface IInputService
    {
        GameState UpdateInput(GameState previousState);

        void SetWarmUpInput(WarmUpInput warmUpInput);
        void SetDwarfFortressInput(DwarfFortressInput dwarfFortressInput);
    }

    public class InputService : IInputService
    {
        private WarmUpInput warmUpInput = WarmUpInput.None;
        private DwarfFortressInput dwarfFortressInput = DwarfFortressInput.None;

        private readonly object inputLock = new object();

        public GameState UpdateInput(GameState previousState)
        {
            lock (inputLock)
            {
                var userInput = new UserInput(Keyboard.GetState());
                var newInput = new GameInput(dwarfFortressInput, userInput, warmUpInput);
                return new GameState(previousState.ScreenState, newInput, previousState.FrameRate, previousState.ShouldExit);
            }
        }

        private KeyboardInput GetCurrentKeyboardInput()
        {
            var newKeyRecordings = previousState.InputState.KeyboardState.KeyRecordings;

            var currentlyPressedKeys = gameInput.UserInput.Keyboard.GetPressedKeys();
            var previouslyPressedKeys = previousState.InputState.KeyboardState.PressedKeys;

            var keysJustPressed = currentlyPressedKeys.Except(previouslyPressedKeys);
            var keysJustReleased = previouslyPressedKeys.Except(currentlyPressedKeys);

            newKeyRecordings = newKeyRecordings.SetItems(keysJustPressed.Select(k => new KeyValuePair<Keys, KeyRecording>(k, new KeyRecording(k, true))));
            newKeyRecordings = newKeyRecordings.SetItems(keysJustReleased.Select(k => new KeyValuePair<Keys, KeyRecording>(k, new KeyRecording(k, false))));

            var newKeyboardHistory = new KeyboardState(newKeyRecordings, ImmutableHashSet.Create(currentlyPressedKeys));
        }

        public void SetWarmUpInput(WarmUpInput updatedInput)
        {
            lock (inputLock)
            {
                warmUpInput = updatedInput;
            }
        }

        public void SetDwarfFortressInput(DwarfFortressInput updatedInput)
        {
            lock (inputLock)
            {
                dwarfFortressInput = updatedInput;
            }
        }
    }
}
