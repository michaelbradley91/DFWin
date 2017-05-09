using System.Drawing;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using DFWin.Constants;
using DFWin.Helpers;
using DFWin.User32Extensions.Enumerations;
using DFWin.User32Extensions.Models;

namespace DFWin.User32Extensions.Service
{
    public interface IWindowService
    {
        /// <summary>
        /// Takes a screenshot of the window, regardless of whether or not it is minimised. This avoids the current window losing focus.
        /// The window will automatically be resized to the size given.
        /// </summary>
        Task<Bitmap> TakeHiddenScreenshotOfClient(Window window, Size size);
    }

    public class WindowService : IWindowService
    {
        private readonly Window applicationWindow;

        public WindowService([KeyFilter(DependencyKeys.Window.Application)] Window applicationWindow)
        {
            this.applicationWindow = applicationWindow;
        }

        public async Task<Bitmap> TakeHiddenScreenshotOfClient(Window window, Size size)
        {
            var requiresSpecialHandling = window.IsMinimised;
            var needToRestoreAnimation = false;
            int? oldwindowDetailsAsInt = null;

            try
            {
                if (!requiresSpecialHandling) return await ResizeAndTakeScreenshot(window, size);

                if (SystemInformation.Current.AreWindowStateChangesAnimated())
                {
                    SystemInformation.Current.SetWindowStateChangeAnimation(false);
                    needToRestoreAnimation = true;
                }

                oldwindowDetailsAsInt = window.Details.WindowDetailsAsInt;
                window.EnsureLayered();
                window.SetTransparency(true);
                window.SetState(WindowState.Restored);
                window.Redraw();

                return await ResizeAndTakeScreenshot(window, size);
            }
            finally
            {
                if (requiresSpecialHandling)
                {
                    ExceptionHelpers.TryAll(
                        () => window.SetState(WindowState.Minimised),
                        () => window.SetTransparency(false),
                        () => { if (oldwindowDetailsAsInt != null) window.Details.SetDetails(oldwindowDetailsAsInt.Value); },
                        () => { if (needToRestoreAnimation) SystemInformation.Current.SetWindowStateChangeAnimation(true); }
                        );
                }
            }
        }

        private async Task<Bitmap> ResizeAndTakeScreenshot(Window window, Size size)
        {
            var wasResized = window.ResizeClientRectangle(size.Width, size.Height);

            if (!wasResized) return window.TakeScreenshotOfClient();

            // Wait a bit to give the window time to redraw.
            await Task.Delay(500);

            applicationWindow.GiveFocus();
            return window.TakeScreenshotOfClient();
        }
    }
}
