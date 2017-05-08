using System;
using PInvoke;

namespace DFWin.User32Extensions.Enumerations
{
    public enum WindowState
    {
        Minimised,
        Restored,
        Maximised,
    }

    public static class WindowStateExtensions
    {
        /// <summary>
        /// Returns a show style for the given state that tries not to activate the window if possible.
        /// </summary>
        public static User32.WindowShowStyle ToShowStyle(this WindowState windowState)
        {
            switch (windowState)
            {
                case WindowState.Minimised:
                    return User32.WindowShowStyle.SW_SHOWMINNOACTIVE;
                case WindowState.Restored:
                    return User32.WindowShowStyle.SW_SHOWNOACTIVATE;
                case WindowState.Maximised:
                    return User32.WindowShowStyle.SW_SHOWMAXIMIZED;
                default:
                    throw new ArgumentOutOfRangeException(nameof(windowState), windowState, null);
            }
        }
    }
}
