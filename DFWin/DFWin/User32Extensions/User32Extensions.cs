using System;
using System.Drawing;
using System.Runtime.InteropServices;
using DFWin.User32Extensions.Exceptions;
using DFWin.User32Extensions.Structs;
using PInvoke;

namespace DFWin.User32Extensions
{
    public static class User32Extensions
    {
        public static Rectangle GetWindowRectangle(IntPtr window)
        {
            RECT rect;

            var succeeded = User32.GetWindowRect(window, out rect);
            if (!succeeded) throw new User32Exception("Could not get the window rectangle", Marshal.GetLastWin32Error());
            
            return new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
        }

        public static Rectangle GetClientRectangle(IntPtr window)
        {
            RECT rect;

            var succeeded = User32.GetClientRect(window, out rect);
            if (!succeeded) throw new User32Exception("Could not get the client rectangle", Marshal.GetLastWin32Error());

            return new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
        }

        public static ANIMATIONINFO GetSystemAnimationInfo()
        {
            var animationInfo = new ANIMATIONINFO(false);

            var succeeded = DllImports.SystemParametersInfo(User32.SystemParametersInfoAction.SPI_GETANIMATION, ANIMATIONINFO.GetSize(), ref animationInfo, User32.SystemParametersInfoFlags.None);
            if (!succeeded) throw new User32Exception("Could not get the system animation information", Marshal.GetLastWin32Error());

            return animationInfo;
        }

        public static void SetSystemAnimationInfo(ANIMATIONINFO animationInfo)
        {
            var succeeded = DllImports.SystemParametersInfo(User32.SystemParametersInfoAction.SPI_SETANIMATION, ANIMATIONINFO.GetSize(), ref animationInfo, User32.SystemParametersInfoFlags.SPIF_SENDCHANGE);
            if (!succeeded) throw new User32Exception("Could not set the system animation information", Marshal.GetLastWin32Error());
        }
    }
}
