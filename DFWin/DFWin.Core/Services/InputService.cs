using System.Collections.Immutable;
using DFWin.Core.Constants;
using DFWin.Core.Helpers;
using DFWin.Core.Inputs;
using DFWin.Core.Inputs.DwarfFortress;
using DFWin.Core.Models;
using DFWin.Core.States;
using Microsoft.Xna.Framework.Input;
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
        private readonly IMouseRecorder mouseRecorder;

        private readonly object inputLock = new object();

        public InputService(IKeyboardRecorder keyboardRecorder, IMouseRecorder mouseRecorder)
        {
            this.keyboardRecorder = keyboardRecorder;
            this.mouseRecorder = mouseRecorder;
        }

        public GameState UpdateInput(GameState previousState)
        {
            lock (inputLock)
            {
                var keyboardInput = GetCurrentKeyboardInput();
                var mouseInput = GetCurrentMouseInput();
                var userInput = new UserInput(keyboardInput, mouseInput);
                var newInput = new GameInput(dwarfFortressInput, userInput, warmUpInput);
                return new GameState(previousState.ScreenState, newInput, previousState.FrameRate, previousState.ShouldExit);
            }
        }

        private KeyboardInput GetCurrentKeyboardInput()
        {
            var newKeyboardState = Keyboard.GetState();
            var pressedKeys = ImmutableHashSet.Create(newKeyboardState.GetPressedKeys());

            keyboardRecorder.Update(pressedKeys);

            return new KeyboardInput(pressedKeys, keyboardRecorder.RecentlyPressedKeys);
        }

        private MouseInput GetCurrentMouseInput()
        {
            var newMouseState = Mouse.GetState();

            var pressedButtons = newMouseState.GetPressedButtons();

            mouseRecorder.Update(pressedButtons);

            return new MouseInput(pressedButtons, mouseRecorder.RecentlyPressedButtons);
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
