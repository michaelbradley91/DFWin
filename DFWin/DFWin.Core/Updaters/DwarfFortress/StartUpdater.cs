using System.Linq;
using System.Threading.Tasks;
using DFWin.Core.Inputs;
using DFWin.Core.Inputs.DwarfFortress;
using DFWin.Core.Services;
using DFWin.Core.States;
using DFWin.Core.States.DwarfFortress;

namespace DFWin.Core.Updaters.DwarfFortress
{
    public class StartUpdater : DwarfFortressUpdater<StartState, IStartInput>
    {
        private readonly IDwarfFortressInputService dwarfFortressInputService;

        public StartUpdater(IDwarfFortressInputService dwarfFortressInputService)
        {
            this.dwarfFortressInputService = dwarfFortressInputService;
        }

        protected override IScreenState Update(StartState previousState, IStartInput dwarfFortressInput, UserInput userInput)
        {
            // TODO implement the start screen properly.
            var keysToSend = userInput.KeyboardInput.RecentlyPressedKeys.ToArray();
            Task.Run(() =>
            {
                dwarfFortressInputService.TrySendKeys(keysToSend);
            });

            return new StartState(dwarfFortressInput);
        }
    }
}
