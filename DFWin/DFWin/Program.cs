using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using DFWin.User32Extensions;
using PInvoke;
using ThreadState = System.Diagnostics.ThreadState;

namespace DFWin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dwarfFortress = Process.GetProcesses().SingleOrDefault(p => p.ProcessName.Contains("Dwarf Fortress"));
            
            while (dwarfFortress == null)
            {
                Console.WriteLine("Please start Dwarf Fortress and press anything but q to continue. Press q to quit." + Environment.NewLine +
                                  "Detail: Could not find a process with name \"Dwarf Fortress\"");

                var key = Console.ReadKey();
                if (new[] {'q', 'Q'}.Contains(key.KeyChar)) return;

                dwarfFortress = Process.GetProcesses().SingleOrDefault(p => p.ProcessName.Contains("Dwarf Fortress"));
            }

            var window = dwarfFortress.MainWindowHandle;

            MinimiseWindow(window);
            
            SendKeys(window, User32.VirtualKey.VK_UP);

            Console.WriteLine("Sent keys");
            Console.ReadLine();

            var image = CaptureImage(window);

            if (File.Exists("MyImage.bmp")) File.Delete("MyImage.bmp");

            image.Save("MyImage.bmp");
            
            Console.WriteLine("Test done");
            Console.ReadLine();
        }

        public static Bitmap CaptureImage(IntPtr window)
        {
            var isMinimised = User32.IsIconic(window);
            var requiresSpecialCapturing = isMinimised;

            var needToRestoreAnimation = false;
            var oldWindowLong = 0;

            if (requiresSpecialCapturing)
            {
                if (IsWindowRestorationAndMinimisationAnimated())
                {
                    SetWindowRestorationAndMinimsationAnimation(false);
                    needToRestoreAnimation = true;
                }

                oldWindowLong = SetWindowTransparency(window, true);
                RestoreWindow(window);
                EnsureWindowDrawn(window);
            }

            var image = GetWindowImage(window);

            if (requiresSpecialCapturing)
            {
                MinimiseWindow(window);
                SetWindowLong(window, (User32.SetWindowLongFlags)oldWindowLong);

                if (needToRestoreAnimation)
                {
                    SetWindowRestorationAndMinimsationAnimation(true);
                }
            }

            return image;
        }

        public static void SendKeys(IntPtr window, params User32.VirtualKey[] keys)
        {
            foreach (var key in keys)
            {
                User32.SendMessage(window, User32.WindowMessage.WM_KEYDOWN, (IntPtr)key, IntPtr.Zero);
                User32.SendMessage(window, User32.WindowMessage.WM_KEYUP, (IntPtr)key, IntPtr.Zero);
            }
        }

        private const int expectedWidth = 1280;
        private const int expectedHeight = 400;

        public static Bitmap GetWindowImage(IntPtr window)
        {
            try
            {
                EnsureWindowSizedCorrectly(window);

                var bitmap = new Bitmap(expectedWidth, expectedHeight);
                var graphics = Graphics.FromImage(bitmap);
                var deviceContext = graphics.GetHdc();

                User32.PrintWindow(window, deviceContext, User32.PrintWindowFlags.PW_CLIENTONLY);

                graphics.ReleaseHdc();
                graphics.Dispose();
                
                return bitmap;
            }
            catch (Exception e)
            {
                // TODO do something about this
                return null;
            }
        }

        private static void EnsureWindowSizedCorrectly(IntPtr window)
        {
            var clientRect = GetClientRectangle(window);
            var windowRect = GetWindowRectangle(window);
            if (clientRect.Width == expectedWidth && clientRect.Height == expectedHeight) return;

            User32.MoveWindow(window, windowRect.X, windowRect.Y, (windowRect.Width - clientRect.Width) + expectedWidth, (windowRect.Height - clientRect.Height) + expectedHeight, true);
            WaitForInputIdle(window);
            User32.SetForegroundWindow(Process.GetCurrentProcess().MainWindowHandle);
        }

        public static bool WaitForInputIdle(IntPtr window, int timeout = 0)
        {
            int pid;
            int tid = User32.GetWindowThreadProcessId(window, out pid);
            if (tid == 0) throw new ArgumentException("Window not found");
            var tick = Environment.TickCount;
            do
            {
                if (IsThreadIdle(pid, tid)) return true;
                Thread.Sleep(15);
            } while (timeout > 0 && Environment.TickCount - tick < timeout);
            return false;
        }

        private static bool IsThreadIdle(int pid, int tid)
        {
            var prc = Process.GetProcessById(pid);
            var thr = prc.Threads.Cast<ProcessThread>().First((t) => tid == t.Id);
            return thr.ThreadState == ThreadState.Wait &&
                   thr.WaitReason == ThreadWaitReason.UserRequest;
        }

        private static Rectangle GetWindowRectangle(IntPtr window)
        {
            RECT rect;
            User32.GetWindowRect(window, out rect);
            return new Rectangle(
                rect.left,
                rect.top,
                rect.right - rect.left,
                rect.bottom - rect.top);
        }

        private static Rectangle GetClientRectangle(IntPtr window)
        {
            RECT rect;
            User32.GetClientRect(window, out rect);
            return new Rectangle(
                rect.left,
                rect.top,
                rect.right - rect.left,
                rect.bottom - rect.top);
        }

        private static void EnsureWindowDrawn(IntPtr window)
        {
            User32.SendMessage(window, User32.WindowMessage.WM_PAINT, IntPtr.Zero, IntPtr.Zero);
        }

        private static void RestoreWindow(IntPtr window)
        {
            User32.ShowWindow(window, User32.WindowShowStyle.SW_SHOWNOACTIVATE);
        }

        private static void MinimiseWindow(IntPtr window)
        {
            User32.ShowWindow(window, User32.WindowShowStyle.SW_SHOWMINNOACTIVE);
        }

        private static int SetWindowTransparency(IntPtr window, bool isTransparent)
        {
            var oldWindowLong = EnsureWindowIsLayered(window);

            const int layeredWindowAlphaFlag = 0x2;

            DllImports.SetLayeredWindowAttributes(window, 0, isTransparent ? (byte)1 : (byte)0, layeredWindowAlphaFlag);

            return oldWindowLong;
        }

        /// <summary>
        /// Ensuring the window is layered means we can set the window's transparency
        /// </summary>
        private static int EnsureWindowIsLayered(IntPtr window)
        {
            var winLong = User32.GetWindowLong(window, User32.WindowLongIndexFlags.GWL_EXSTYLE);
            SetWindowLong(window, (User32.SetWindowLongFlags)winLong | User32.SetWindowLongFlags.WS_EX_LAYERED);
            return winLong;
        }

        private static void SetWindowLong(IntPtr window, User32.SetWindowLongFlags winLong)
        {
            User32.SetWindowLong(window, User32.WindowLongIndexFlags.GWL_EXSTYLE, winLong);
        }

        public static bool IsWindowRestorationAndMinimisationAnimated()
        {
            var animationInfo = new ANIMATIONINFO(false);

            DllImports.SystemParametersInfo(User32.SystemParametersInfoAction.SPI_GETANIMATION, ANIMATIONINFO.GetSize(), ref animationInfo, User32.SystemParametersInfoFlags.None);

            return animationInfo.IsWindowRestorationAndMinimisationAnimated;
        }

        public static void SetWindowRestorationAndMinimsationAnimation(bool enabled)
        {
            var animationInfo = new ANIMATIONINFO(enabled);
            DllImports.SystemParametersInfo(User32.SystemParametersInfoAction.SPI_SETANIMATION, ANIMATIONINFO.GetSize(), ref animationInfo, User32.SystemParametersInfoFlags.SPIF_SENDCHANGE);
        }
    }
}
