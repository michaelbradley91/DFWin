using System;
using System.Runtime.InteropServices;
using DFWin.Core.User32Extensions.Structs;
using PInvoke;

namespace DFWin.Core.User32Extensions
{
    public static class DllImports
    {
        public const string User32 = "user32.dll";

        [DllImport(User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(User32.SystemParametersInfoAction uiAction, uint uiParam, ref ANIMATIONINFO pvParam, User32.SystemParametersInfoFlags fWinIni);

        [DllImport(User32, SetLastError = true)]
        public static extern int SetLayeredWindowAttributes(IntPtr hWnd, byte crKey, byte alpha, int flags);

        /// <summary>
        /// Unlike the Nuget package's, this one does set the last error.
        /// </summary>
        [DllImport(User32, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hWnd, User32.WindowLongIndexFlags nIndex, User32.SetWindowLongFlags dwNewLong);
    }
}
