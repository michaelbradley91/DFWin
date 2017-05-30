using System;
using System.Drawing;
using System.Threading.Tasks;
using DFWin.Core.PInvoke.Enumerations;
using DFWin.Core.PInvoke.Models;
using DFWin.Core.Services;

namespace DFWin.Core.PInvoke.Services
{
    public interface IWindowService
    {
        Task PrepareForCapture(Window window, Size size, bool skipResize = false);

        /// <summary>
        /// Takes a screenshot of the client area of the window, regardless of whether or not it is minimised. The pixel format is 24 bpp (8 bits for RGB - no alpha)
        /// The size given should be the size of the client area. If it is not, the client area is automatically resized.
        /// </summary>
        Task<Bitmap> Capture(Window window, Size size, bool skipResize = false);
    }

    public class WindowService : IWindowService
    {
        private readonly IProcessService processService;
        private Window ApplicationWindow => new Window(processService.GetCurrentProcess().MainWindowHandle);

        private static readonly TimeSpan DelayAfterResize = TimeSpan.FromMilliseconds(500);

        public WindowService(IProcessService processService)
        {
            this.processService = processService;
        }

        public async Task PrepareForCapture(Window window, Size size, bool skipResize = false)
        {
            var needToRestoreAnimation = false;
            try
            {
                if (SystemInformation.Current.AreWindowStateChangesAnimated())
                {
                    SystemInformation.Current.SetWindowStateChangeAnimation(false);
                    needToRestoreAnimation = true;
                }

                window.EnsureLayered();
#if !DEBUG
                window.SetTransparency(true);
#endif
                window.SetState(WindowState.Restored);
                window.Redraw();

                if (skipResize) return;

                var wasResized = window.ResizeClientRectangle(size.Width, size.Height);

                if (wasResized)
                {
                    // Wait a bit to give the window time to redraw.
                    await Task.Delay(DelayAfterResize);

                    var currentProcess = processService.GetCurrentProcess();
                    var applicationWindow = new Window(currentProcess.MainWindowHandle);
                    applicationWindow.GiveFocus();
                }
            }
            finally
            {
                if (needToRestoreAnimation)
                {
                    SystemInformation.Current.SetWindowStateChangeAnimation(true);
                }
            }
        }

        public async Task<Bitmap> Capture(Window window, Size size, bool skipResize = false)
        {
            if (window.IsMinimised) await PrepareForCapture(window, size, skipResize);

            return await ResizeAndTakeScreenshot(window, size, skipResize);
        }

        private async Task<Bitmap> ResizeAndTakeScreenshot(Window window, Size size, bool skipResize)
        {
            if (skipResize) return window.TakeScreenshotOfClient(size);

            var wasResized = window.ResizeClientRectangle(size.Width, size.Height);
            if (!wasResized) return window.TakeScreenshotOfClient(size);

            // Wait a bit to give the window time to redraw.
            await Task.Delay(DelayAfterResize);

            ApplicationWindow.GiveFocus();

            return window.TakeScreenshotOfClient(size);
        }
    }
}
