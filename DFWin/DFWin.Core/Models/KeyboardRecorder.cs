using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace DFWin.Core.Models
{
    /// <summary>
    /// Keeps a history of keyboard state to provide you with additional information about the keyboard input.
    /// </summary>
    public interface IKeyboardRecorder
    {
        ImmutableHashSet<Keys> RecentlyPressedKeys { get; }
        void Update(ImmutableHashSet<Keys> currentlyPressedKeys);
    }

    public class KeyboardRecorder : IKeyboardRecorder
    {
        public ImmutableHashSet<Keys> RecentlyPressedKeys { get; private set; } = ImmutableHashSet<Keys>.Empty;

        private ImmutableHashSet<Keys> previouslyPressedKeys = ImmutableHashSet<Keys>.Empty;
        private ImmutableDictionary<Keys, KeyRecording> keyRecordings = ImmutableDictionary<Keys, KeyRecording>.Empty;

        private readonly Dictionary<Keys, DateTimeOffset> keysHeldByLastTimeConsideredRecentlyPressed = new Dictionary<Keys, DateTimeOffset>();
        
        public void Update(ImmutableHashSet<Keys> currentlyPressedKeys)
        {
            UpdateKeyRecordings(currentlyPressedKeys);
            RemoveReleasedKeys(currentlyPressedKeys);
            UpdateRecentlyPressedKeys(currentlyPressedKeys);
            
            previouslyPressedKeys = currentlyPressedKeys;
        }

        private void UpdateKeyRecordings(ImmutableHashSet<Keys> currentlyPressedKeys)
        {
            var justPressedKeys = currentlyPressedKeys.Except(previouslyPressedKeys);
            var justReleasedKeys = previouslyPressedKeys.Except(currentlyPressedKeys);

            keyRecordings = keyRecordings.SetItems(justPressedKeys.Select(k => new KeyValuePair<Keys, KeyRecording>(k, new KeyRecording(k, true))));
            keyRecordings = keyRecordings.SetItems(justReleasedKeys.Select(k => new KeyValuePair<Keys, KeyRecording>(k, new KeyRecording(k, false))));
        }

        private void RemoveReleasedKeys(IEnumerable<Keys> currentlyPressedKeys)
        {
            var keysReleased = keysHeldByLastTimeConsideredRecentlyPressed.Keys.Except(currentlyPressedKeys).ToList();
            foreach (var keyReleased in keysReleased)
            {
                keysHeldByLastTimeConsideredRecentlyPressed.Remove(keyReleased);
            }
        }

        private void UpdateRecentlyPressedKeys(IEnumerable<Keys> currentlyPressedKeys)
        {
            var recentlyPressedKeys = new List<Keys>();
            foreach (var currentlyPressedKey in currentlyPressedKeys)
            {
                if (!keysHeldByLastTimeConsideredRecentlyPressed.ContainsKey(currentlyPressedKey))
                {
                    recentlyPressedKeys.Add(currentlyPressedKey);
                    keysHeldByLastTimeConsideredRecentlyPressed.Add(currentlyPressedKey, DateTimeOffset.UtcNow);
                }
                else
                {
                    var timeHeldDownFor = DateTimeOffset.UtcNow - keyRecordings[currentlyPressedKey].Time;
                    var timeToWait = TimeSpan.FromSeconds(1 / (Math.Max(timeHeldDownFor.TotalSeconds, 0.5) * 6f));
                    var timeSinceLastConsideredRecentlyPressed = DateTimeOffset.UtcNow - keysHeldByLastTimeConsideredRecentlyPressed[currentlyPressedKey];

                    if (timeToWait >= timeSinceLastConsideredRecentlyPressed) continue;

                    recentlyPressedKeys.Add(currentlyPressedKey);
                    keysHeldByLastTimeConsideredRecentlyPressed[currentlyPressedKey] = DateTimeOffset.UtcNow;
                }
            }

            RecentlyPressedKeys = ImmutableHashSet.Create(recentlyPressedKeys.ToArray());
        }
    }
}