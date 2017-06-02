using System.Collections.Immutable;
using DFWin.Core.Inputs;
using DFWin.Core.Inputs.DwarfFortress;
using DFWin.Core.Models;
using DFWin.Core.States;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;

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

        private readonly IKeyboardRecorder keyboardRecorder;

        private readonly object inputLock = new object();

        public InputService(IKeyboardRecorder keyboardRecorder)
        {
            this.keyboardRecorder = keyboardRecorder;
        }

        public GameState UpdateInput(GameState previousState)
        {
            lock (inputLock)
            {
                var keyboardInput = GetCurrentKeyboardInput();
                var userInput = new UserInput(keyboardInput);
                var newInput = new GameInput(dwarfFortressInput, userInput, warmUpInput);
                return new GameState(previousState.ScreenState, newInput, previousState.FrameRate, previousState.ShouldExit);
            }
        }

        private KeyboardInput GetCurrentKeyboardInput()
        {
            var newKeyboardState = Keyboard.GetState();
            var pressedKeys = ImmutableHashSet.Create(newKeyboardState.GetPressedKeys());

            keyboardRecorder.Update(pressedKeys);

            return new KeyboardInput(newKeyboardState, pressedKeys, keyboardRecorder.RecentlyPressedKeys);
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
