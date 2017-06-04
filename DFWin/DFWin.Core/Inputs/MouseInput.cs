using System.Collections.Immutable;
using DFWin.Core.Constants;
using Microsoft.Xna.Framework.Input;

namespace DFWin.Core.Inputs
{
    public class MouseInput
    {
        /// <summary>
        /// Represents buttons that are being pressed right now. Note that this will remain true every frame of the game
        /// the button is pressed, so if the user is trying to perform a precise action with a specific number of presses,
        /// you should use RecentlyPressedButtons instead.
        /// </summary>
        public ImmutableHashSet<MouseButtons> CurrentlyPressedButtons { get; }

        /// <summary>
        /// Represents buttons that have "just" been pressed. If a button is held down, this should only indicate the button once.
        /// (Until it is released and pressed again)
        /// </summary>
        public ImmutableHashSet<MouseButtons> RecentlyPressedButtons { get; }

        public MouseInput(ImmutableHashSet<MouseButtons> currentlyPressedButtons, ImmutableHashSet<MouseButtons> recentlyPressedButtons)
        {
            CurrentlyPressedButtons = currentlyPressedButtons;
            RecentlyPressedButtons = recentlyPressedButtons;
        }

        public static MouseInput InitialInput => new MouseInput(ImmutableHashSet<MouseButtons>.Empty, ImmutableHashSet<MouseButtons>.Empty);
    }
}