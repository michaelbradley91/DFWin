using DFWin.Core.Inputs;
using DFWin.Core.Resources.Models;
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
        private DwarfFortressInput dwarfFortressInput;

        private readonly object inputLock = new object();

        public GameInput GetCurrentInput()
        {
            lock (inputLock)
            {
                var userInput = new UserInput(Keyboard.GetState());
                return new GameInput(dwarfFortressInput, userInput, warmUpInput);
            }
        }

        public void SetWarmUpInput(WarmUpInput warmUpInput)
        {
            lock (inputLock)
            {
                this.warmUpInput = warmUpInput;
            }
        }

        public void SetDwarfFortressInput(DwarfFortressInput dwarfFortressInput)
        {
            lock (inputLock)
            {
                this.dwarfFortressInput = dwarfFortressInput;
            }
        }
    }
}
