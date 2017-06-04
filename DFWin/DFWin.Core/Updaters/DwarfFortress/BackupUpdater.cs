using System.Linq;
using DFWin.Core.Inputs;
using DFWin.Core.Inputs.DwarfFortress;
using DFWin.Core.States;
using DFWin.Core.States.DwarfFortress;

namespace DFWin.Core.Updaters.DwarfFortress
{
    public class BackupUpdater : DwarfFortressUpdater<BackupState, IBackupInput>
    {
        protected override IScreenState Update(BackupState previousState, IBackupInput dwarfFortressInput, UserInput userInput)
        {
            var keysToSend = userInput.KeyboardInput.RecentlyPressedKeys.ToArray();

            DwarfFortressInputService.TrySendKeysAsync(keysToSend);

            return new BackupState(dwarfFortressInput);
        }
    }
}
