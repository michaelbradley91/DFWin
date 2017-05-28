using System;
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

        private DateTimeOffset? lastSentDown;

        protected override IScreenState Update(BackupState previousState, InputState inputState, GameInput input)
        {
            if (!inputState.KeyboardState.PressedKeys.Contains(Keys.Down))
            {
                lastSentDown = null;
            }
            else
            {
                if (!lastSentDown.HasValue)
                {
                    lastSentDown = DateTimeOffset.UtcNow;
                    Task.Run(() =>
                    {
                        dwarfFortressInputService.TrySendKey(Keys.Down, true);
                        dwarfFortressInputService.TrySendKey(Keys.Down, false);
                    });
                }
                else
                {
                    var timeDownPressedFor = DateTimeOffset.UtcNow - inputState.KeyboardState.KeyRecordings[Keys.Down].Time;
                    var timeToWait = TimeSpan.FromSeconds(1 / (Math.Max(timeDownPressedFor.TotalSeconds, 0.5) * 8f));
                    var timeSinceLastSent = DateTimeOffset.UtcNow - lastSentDown;
                    if (timeToWait < timeSinceLastSent)
                    {
                        lastSentDown = DateTimeOffset.UtcNow;
                        Task.Run(() =>
                        {
                            dwarfFortressInputService.TrySendKey(Keys.Down, true);
                            dwarfFortressInputService.TrySendKey(Keys.Down, false);
                        });
                    }
                }
            }
            

            return new BackupState(input.DwarfFortressInput.Tiles);
        }
    }
}
