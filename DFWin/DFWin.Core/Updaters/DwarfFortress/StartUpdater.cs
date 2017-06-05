using System.Linq;
using DFWin.Core.Inputs;
using DFWin.Core.Inputs.DwarfFortress;
using DFWin.Core.Screens;
using DFWin.Core.States;
using DFWin.Core.States.DwarfFortress;

namespace DFWin.Core.Updaters.DwarfFortress
{
    public class StartUpdater : DwarfFortressUpdater<StartState, IStartInput>
    {
        private readonly IStartScreen startScreen;

        public StartUpdater(IStartScreen startScreen)
        {
            this.startScreen = startScreen;
        }

        protected override IScreenState Update(StartState previousState, IStartInput dwarfFortressInput, UserInput userInput)
        {
            // TODO implement the start screen properly.
            var keysToSend = userInput.KeyboardInput.RecentlyPressedKeys.ToArray();

            DwarfFortressInputService.TrySendKeysAsync(keysToSend);

            return new StartState(dwarfFortressInput);
        }
    }
}
