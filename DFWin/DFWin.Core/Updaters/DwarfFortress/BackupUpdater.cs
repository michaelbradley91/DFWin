using System.Linq;
using System.Threading.Tasks;
using DFWin.Core.Inputs;
using DFWin.Core.Inputs.DwarfFortress;
using DFWin.Core.Services;
using DFWin.Core.States;
using DFWin.Core.States.DwarfFortress;

namespace DFWin.Core.Updaters.DwarfFortress
{
    public class BackupUpdater : DwarfFortressUpdater<BackupState, IBackupInput>
    {
        private readonly IDwarfFortressInputService dwarfFortressInputService;

        public BackupUpdater(IDwarfFortressInputService dwarfFortressInputService)
        {
            this.dwarfFortressInputService = dwarfFortressInputService;
        }    

        protected override IScreenState Update(BackupState previousState, IBackupInput dwarfFortressInput, UserInput userInput)
        {
            var keysToSend = userInput.KeyboardInput.RecentlyPressedKeys.ToArray();
            Task.Run(() =>
            {
                dwarfFortressInputService.TrySendKeys(keysToSend);
            });

            return new BackupState(dwarfFortressInput);
        }
    }
}
