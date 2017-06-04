using DFWin.Core.Inputs.DwarfFortress;

namespace DFWin.Core.States.DwarfFortress
{
    public class BackupState : DwarfFortressState<IBackupInput>
    {
        public BackupState(IBackupInput backupInput) : base(backupInput) { }
    }
}
