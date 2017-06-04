using System.Collections.Immutable;
using DFWin.Core.Models;
using Microsoft.Xna.Framework.Input;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;

namespace DFWin.Core.Inputs
{
    public class KeyboardInput
    {
        /// <summary>
        /// Represents keys that are being pressed right now. Note that this will remain true every frame of the game
        /// the key is pressed, so if the user is trying to perform a precise action with a specific number of presses,
        /// you should use RecentlyPressedKeys instead.
        /// </summary>
        public ImmutableHashSet<Keys> CurrentlyPressedKeys { get; }

        /// <summary>
        /// Represents keys that have "just" been pressed. If a key is held down, this should only indicate the key once
        /// until after a short period of time after which it starts to repeat at an increasing rate.
        /// </summary>
        public ImmutableHashSet<Keys> RecentlyPressedKeys { get; }

        public KeyboardInput(ImmutableHashSet<Keys> currentlyPressedKeys, ImmutableHashSet<Keys> recentlyPressedKeys)
        {
            CurrentlyPressedKeys = currentlyPressedKeys;
            RecentlyPressedKeys = recentlyPressedKeys;
        }

        public static KeyboardInput InitialInput => new KeyboardInput(ImmutableHashSet<Keys>.Empty, ImmutableHashSet<Keys>.Empty);
    }
}
