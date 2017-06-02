using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFWin.Core.Inputs;
using DFWin.Core.Services;
using DFWin.Core.States;
using Microsoft.Xna.Framework.Input;

namespace DFWin.Core.Updaters
{
    public class KeyThrottler
    {
        public IReadOnlyCollection<Keys> PressedKeysToProcess { get; private set; } = new List<Keys>();

        private readonly Dictionary<Keys, DateTimeOffset> lastTimeKeySentBeforeRelease = new Dictionary<Keys, DateTimeOffset>();

        public void Update(KeyboardInput keyboardInput)
        {
            var keysToProcess = new List<Keys>();
            var keysReleased = lastTimeKeySentBeforeRelease.Keys.Except(keyboardInput.PressedKeys).ToList();
            foreach (var keyReleased in keysReleased)
            {
                lastTimeKeySentBeforeRelease.Remove(keyReleased);
            }
            foreach (var pressedKey in keyboardInput.PressedKeys)
            {
                if (!lastTimeKeySentBeforeRelease.ContainsKey(pressedKey))
                {
                    keysToProcess.Add(pressedKey);
                    lastTimeKeySentBeforeRelease.Add(pressedKey, DateTimeOffset.UtcNow);
                }
                else
                {
                    var timeDownPressedFor = DateTimeOffset.UtcNow - keyboardInput.KeyRecordings[pressedKey].Time;
                    var timeToWait = TimeSpan.FromSeconds(1 / (Math.Max(timeDownPressedFor.TotalSeconds, 0.5) * 6f));
                    var timeSinceLastSent = DateTimeOffset.UtcNow - lastTimeKeySentBeforeRelease[pressedKey];

                    if (timeToWait >= timeSinceLastSent) continue;

                    keysToProcess.Add(pressedKey);
                    lastTimeKeySentBeforeRelease[pressedKey] = DateTimeOffset.UtcNow;
                }
            }
            PressedKeysToProcess = keysToProcess;
        }
    }

    public class BackupUpdater : Updater<BackupState>
    {
        private readonly IDwarfFortressInputService dwarfFortressInputService;
        private readonly KeyThrottler keyThrottler;

        public BackupUpdater(IDwarfFortressInputService dwarfFortressInputService)
        {
            this.dwarfFortressInputService = dwarfFortressInputService;

            keyThrottler = new KeyThrottler();
        }

        protected override IScreenState Update(BackupState previousState, GameInput input)
        {
            keyThrottler.Update(input.UserInput.KeyboardInput);

            var keysToSend = keyThrottler.PressedKeysToProcess.ToArray();
            Task.Run(() =>
            {
                dwarfFortressInputService.TrySendKeys(keysToSend);
            });

            return new BackupState(input.DwarfFortressInput.Tiles);
        }
    }
}
