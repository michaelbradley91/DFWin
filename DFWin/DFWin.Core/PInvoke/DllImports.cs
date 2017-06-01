using System;
using System.Runtime.InteropServices;
using DFWin.Core.PInvoke.Structs;
using PInvoke;

namespace DFWin.Core.PInvoke
{
    public static class DllImports
    {
        public const string User32 = "user32.dll";
        public const string MsvCrt = "msvcrt.dll";

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

        [DllImport(MsvCrt, EntryPoint = "memcmp", SetLastError = true)]
        public static extern int MemoryCompare(byte[] bytes1, byte[] bytes2, long count);
    }
}
