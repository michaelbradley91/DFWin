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
                var down = input.UserInput.Keyboard.IsKeyDown(Keys.Down);
                var up = input.UserInput.Keyboard.IsKeyDown(Keys.Up);
                var left = input.UserInput.Keyboard.IsKeyDown(Keys.Left);
                var right = input.UserInput.Keyboard.IsKeyDown(Keys.Right);
                var enter = input.UserInput.Keyboard.IsKeyDown(Keys.Enter);

                dwarfFortressInputService.TrySendKey(Keys.Down, down);
                dwarfFortressInputService.TrySendKey(Keys.Up, up);
                dwarfFortressInputService.TrySendKey(Keys.Right, right);
                dwarfFortressInputService.TrySendKey(Keys.Left, left);
                dwarfFortressInputService.TrySendKey(Keys.Enter, enter);
            });

            return new BackupState(input.DwarfFortressInput.Tiles);
        }
    }
}
