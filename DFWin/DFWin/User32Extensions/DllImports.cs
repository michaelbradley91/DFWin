using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PInvoke;

namespace DFWin.User32Extensions
{
    public static class DllImports
    {
        public const string User32 = "user32.dll";

        [DllImport(User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(User32.SystemParametersInfoAction uiAction, uint uiParam, ref ANIMATIONINFO pvParam, User32.SystemParametersInfoFlags fWinIni);

        [DllImport("user32")]
        public static extern int SetLayeredWindowAttributes(IntPtr hWnd, byte crKey, byte alpha, int flags);
    }
}
