using DFWin.Core.Inputs;
using Microsoft.Xna.Framework.Input;

namespace DFWin.Core.Services
{
    public interface IInputService
    {
        GameInput GetCurrentInput();

        void SetWarmUpInput(WarmUpInput warmUpInput);
        void SetDwarfFortressInput(DwarfFortressInput dwarfFortressInput);
    }

    public class InputService : IInputService
    {
        private WarmUpInput warmUpInput = WarmUpInput.None;
        private DwarfFortressInput dwarfFortressInput = DwarfFortressInput.None;

        private readonly object inputLock = new object();

        public GameInput GetCurrentInput()
        {
            lock (inputLock)
            {
                var userInput = new UserInput(Keyboard.GetState());
                return new GameInput(dwarfFortressInput, userInput, warmUpInput);
            }
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
