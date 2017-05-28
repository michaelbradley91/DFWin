using System.Threading.Tasks;
using DFWin.Core.Inputs;
using DFWin.Core.Services;
using DFWin.Core.States;
using Microsoft.Xna.Framework.Input;

namespace DFWin.Core.Updaters
{
    public class BackupUpdater : Updater<BackupState>
    {
        private readonly IDwarfFortressInputService dwarfFortressInputService;

        public BackupUpdater(IDwarfFortressInputService dwarfFortressInputService)
        {
            this.dwarfFortressInputService = dwarfFortressInputService;
        }

        protected override IScreenState Update(BackupState previousState, GameInput input)
        {
            Task.Run(() =>
            {
                if (input.UserInput.Keyboard.IsKeyDown(Keys.Down))
                {
                    dwarfFortressInputService.TrySendKey(Keys.Down, true);
                    dwarfFortressInputService.TrySendKey(Keys.Down, false);
                }
            });

            return new BackupState(input.DwarfFortressInput.Tiles);
        }
    }
}
