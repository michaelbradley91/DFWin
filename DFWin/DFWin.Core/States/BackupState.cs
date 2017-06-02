using DFWin.Core.Inputs.DwarfFortress;
using DFWin.Core.States.DwarfFortress;

namespace DFWin.Core.States
{
    public class BackupState : DwarfFortressState<IBackupInput>, IScreenState
    {
        public BackupState(IBackupInput backupInput) : base(backupInput) { }
    }
}
