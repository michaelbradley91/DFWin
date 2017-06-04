using System.Collections.Immutable;
using DFWin.Core.Constants;

namespace DFWin.Core.Models
{
    public interface IMouseRecorder
    {
        ImmutableHashSet<MouseButtons> RecentlyPressedButtons { get; }
        void Update(ImmutableHashSet<MouseButtons> currentlyPressedButtons);
    }

    public class MouseRecorder : IMouseRecorder
    {
        public ImmutableHashSet<MouseButtons> RecentlyPressedButtons { get; private set; } = ImmutableHashSet<MouseButtons>.Empty;

        private ImmutableHashSet<MouseButtons> heldButtons = ImmutableHashSet<MouseButtons>.Empty;

        public void Update(ImmutableHashSet<MouseButtons> currentlyPressedButtons)
        {
            RecentlyPressedButtons = currentlyPressedButtons.Except(heldButtons).ToImmutableHashSet();
            heldButtons = currentlyPressedButtons;
        }
    }
}
