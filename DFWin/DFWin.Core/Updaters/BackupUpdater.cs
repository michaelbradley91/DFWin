using System.Linq;
using System.Threading.Tasks;
using DFWin.Core.Inputs;
using DFWin.Core.Inputs.DwarfFortress;
using DFWin.Core.Models;
using DFWin.Core.Services;
using DFWin.Core.States;

namespace DFWin.Core.Updaters
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
