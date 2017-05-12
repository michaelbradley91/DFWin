using System.Runtime.InteropServices;
using DFWin.Core.User32Extensions.Exceptions;
using PInvoke;

namespace DFWin.Core.User32Extensions.Models
{
    public class WindowDetails
    {
        public WindowDetails(Window window)
        {
            this.window = window;
        }

        private readonly Window window;

        private const User32.WindowLongIndexFlags Style = User32.WindowLongIndexFlags.GWL_EXSTYLE;

        /// <summary>
        /// The raw integer, whose bits represent information about the window. This uses the "Extended Style" (GWL_EXSTYLE = Get Window Long ...)
        /// </summary>
        public int WindowDetailsAsInt => User32.GetWindowLong(window.WindowPointer, Style);

        public bool IsLayered => (WindowDetailsAsInt & (int)User32.SetWindowLongFlags.WS_EX_LAYERED) != 0;

        /// <summary>
        /// Modifies the window to ensure it is layered, and so supports settings such as transparency.
        /// </summary>
        public void EnsureLayered()
        {
            var result = DllImports.SetWindowLong(window.WindowPointer, Style, (User32.SetWindowLongFlags)WindowDetailsAsInt | User32.SetWindowLongFlags.WS_EX_LAYERED);
            if (result == 0) throw new User32Exception("Unable to ensure the window was layered.", Marshal.GetLastWin32Error());
        }

        /// <summary>
        /// Set the window details precisely to the given integer representation.
        /// </summary>
        public void SetDetails(int windowDetailsAsInt)
        {
            var result = DllImports.SetWindowLong(window.WindowPointer, Style, (User32.SetWindowLongFlags)windowDetailsAsInt);
            if (result == 0) throw new User32Exception("Unable to set window long.", Marshal.GetLastWin32Error());
        }
    }
}
