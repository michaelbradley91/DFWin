using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DFWin.Core.Inputs;
using DFWin.Core.Inputs.DwarfFortress;
using DFWin.Core.Models;
using DFWin.Core.States;
using Microsoft.Xna.Framework.Input;

namespace DFWin.Core.Services
{
    public interface IInputService
    {
        GameState UpdateInput(GameState previousState);

        void SetWarmUpInput(WarmUpInput warmUpInput);
        void SetDwarfFortressInput(IDwarfFortressInput dwarfFortressInput);
    }

    public class InputService : IInputService
    {
        private WarmUpInput warmUpInput = WarmUpInput.None;
        private IDwarfFortressInput dwarfFortressInput = DwarfFortressInput.None;

        private readonly object inputLock = new object();

        public GameState UpdateInput(GameState previousState)
        {
            lock (inputLock)
            {
                var keyboardInput = GetCurrentKeyboardInput(previousState);
                var userInput = new UserInput(keyboardInput);
                var newInput = new GameInput(dwarfFortressInput, userInput, warmUpInput);
                return new GameState(previousState.ScreenState, newInput, previousState.FrameRate, previousState.ShouldExit);
            }
        }

        private static KeyboardInput GetCurrentKeyboardInput(GameState previousState)
        {
            var newKeyboardState = Keyboard.GetState();
            var keyRecordings = previousState.GameInput.UserInput.KeyboardInput.KeyRecordings;

            var currentlyPressedKeys = newKeyboardState.GetPressedKeys();
            var previouslyPressedKeys = previousState.GameInput.UserInput.KeyboardInput.PressedKeys;

            var keysJustPressed = currentlyPressedKeys.Except(previouslyPressedKeys);
            var keysJustReleased = previouslyPressedKeys.Except(currentlyPressedKeys);

            keyRecordings = keyRecordings.SetItems(keysJustPressed.Select(k => new KeyValuePair<Keys, KeyRecording>(k, new KeyRecording(k, true))));
            keyRecordings = keyRecordings.SetItems(keysJustReleased.Select(k => new KeyValuePair<Keys, KeyRecording>(k, new KeyRecording(k, false))));

            return new KeyboardInput(newKeyboardState, keyRecordings, ImmutableHashSet.Create(currentlyPressedKeys));
        }

        public void SetWarmUpInput(WarmUpInput updatedInput)
        {
            lock (inputLock)
            {
                warmUpInput = updatedInput;
            }
        }

        public void SetDwarfFortressInput(IDwarfFortressInput updatedInput)
        {
            lock (inputLock)
            {
                dwarfFortressInput = updatedInput;
            }
        }
    }
}
