using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DFWin.Core.Constants;
using DFWin.Core.Inputs;
using DFWin.Core.User32Extensions.Models;
using DFWin.Core.User32Extensions.Services;
using Microsoft.Xna.Framework.Input;
using PInvoke;

namespace DFWin.Core.Services
{
    public interface IDwarfFortressInputService : IDisposable
    {
        void StartScreenScraping();
        void TrySendKey(Keys key, bool down);
    }

    public class DwarfFortressInputService : IDwarfFortressInputService
    {
        private readonly IProcessService processService;
        private readonly IWindowService windowService;
        private readonly IGameGridService gridGameGridService;
        private readonly IInputService inputService;

        private bool hasStarted;
        private bool hasDisposed;
        private readonly CancellationTokenSource cancellationTokenSource;

        private const int MinimumDelay = 10;

        public DwarfFortressInputService(IProcessService processService, IWindowService windowService,
            IGameGridService gridGameGridService, IInputService inputService)
        {
            this.processService = processService;
            this.windowService = windowService;
            this.gridGameGridService = gridGameGridService;
            this.inputService = inputService;

            cancellationTokenSource = new CancellationTokenSource();
        }

        public void StartScreenScraping()
        {
            if (hasStarted) throw new InvalidOperationException("You should only start the screenscraper once.");

            hasStarted = true;
            RunUpdateLoop(cancellationTokenSource.Token);
        }

        public void TrySendKey(Keys key, bool down)
        {
            try
            {
                if (!processService.TryGetDwarfFortressProcess(out Process process)) return;
                var window = new Window(process.MainWindowHandle);
                switch (key)
                {
                    case Keys.Back:
                        window.SendKey(User32.VirtualKey.VK_BACK, down);
                        break;
                    case Keys.Tab:
                        window.SendKey(User32.VirtualKey.VK_TAB, down);
                        break;
                    case Keys.Enter:
                        window.SendKey(User32.VirtualKey.VK_RETURN, down);
                        break;
                    case Keys.CapsLock:
                        window.SendKey(User32.VirtualKey.VK_CAPITAL, down);
                        break;
                    case Keys.Escape:
                        window.SendKey(User32.VirtualKey.VK_ESCAPE, down);
                        break;
                    case Keys.Space:
                        window.SendKey(User32.VirtualKey.VK_SPACE, down);
                        break;
                    case Keys.End:
                        window.SendKey(User32.VirtualKey.VK_END, down);
                        break;
                    case Keys.Home:
                        window.SendKey(User32.VirtualKey.VK_HOME, down);
                        break;
                    case Keys.Left:
                        window.SendKey(User32.VirtualKey.VK_LEFT, down);
                        break;
                    case Keys.Up:
                        window.SendKey(User32.VirtualKey.VK_UP, down);
                        break;
                    case Keys.Right:
                        window.SendKey(User32.VirtualKey.VK_RIGHT, down);
                        break;
                    case Keys.Down:
                        window.SendKey(User32.VirtualKey.VK_DOWN, down);
                        break;
                    case Keys.Select:
                        window.SendKey(User32.VirtualKey.VK_SELECT, down);
                        break;
                    case Keys.Print:
                        window.SendKey(User32.VirtualKey.VK_PRINT, down);
                        break;
                    case Keys.Execute:
                        window.SendKey(User32.VirtualKey.VK_EXECUTE, down);
                        break;
                    case Keys.Insert:
                        window.SendKey(User32.VirtualKey.VK_INSERT, down);
                        break;
                    case Keys.Delete:
                        window.SendKey(User32.VirtualKey.VK_DELETE, down);
                        break;
                    case Keys.Help:
                        window.SendKey(User32.VirtualKey.VK_HELP, down);
                        break;
                    case Keys.D0:
                        window.SendKey(User32.VirtualKey.VK_END, down);
                        break;
                    case Keys.A:
                        window.SendKey(User32.VirtualKey.VK_A, down);
                        break;
                    case Keys.B:
                        window.SendKey(User32.VirtualKey.VK_B, down);
                        break;
                    case Keys.C:
                        window.SendKey(User32.VirtualKey.VK_C, down);
                        break;
                    case Keys.D:
                        window.SendKey(User32.VirtualKey.VK_D, down);
                        break;
                    case Keys.E:
                        window.SendKey(User32.VirtualKey.VK_E, down);
                        break;
                    case Keys.F:
                        window.SendKey(User32.VirtualKey.VK_F, down);
                        break;
                    case Keys.G:
                        window.SendKey(User32.VirtualKey.VK_G, down);
                        break;
                    case Keys.H:
                        window.SendKey(User32.VirtualKey.VK_H, down);
                        break;
                    case Keys.I:
                        window.SendKey(User32.VirtualKey.VK_I, down);
                        break;
                    case Keys.J:
                        window.SendKey(User32.VirtualKey.VK_J, down);
                        break;
                    case Keys.K:
                        window.SendKey(User32.VirtualKey.VK_K, down);
                        break;
                    case Keys.L:
                        window.SendKey(User32.VirtualKey.VK_L, down);
                        break;
                    case Keys.M:
                        window.SendKey(User32.VirtualKey.VK_M, down);
                        break;
                    case Keys.N:
                        window.SendKey(User32.VirtualKey.VK_N, down);
                        break;
                    case Keys.O:
                        window.SendKey(User32.VirtualKey.VK_O, down);
                        break;
                    case Keys.P:
                        window.SendKey(User32.VirtualKey.VK_P, down);
                        break;
                    case Keys.Q:
                        window.SendKey(User32.VirtualKey.VK_Q, down);
                        break;
                    case Keys.R:
                        window.SendKey(User32.VirtualKey.VK_R, down);
                        break;
                    case Keys.S:
                        window.SendKey(User32.VirtualKey.VK_S, down);
                        break;
                    case Keys.T:
                        window.SendKey(User32.VirtualKey.VK_T, down);
                        break;
                    case Keys.U:
                        window.SendKey(User32.VirtualKey.VK_U, down);
                        break;
                    case Keys.V:
                        window.SendKey(User32.VirtualKey.VK_V, down);
                        break;
                    case Keys.W:
                        window.SendKey(User32.VirtualKey.VK_W, down);
                        break;
                    case Keys.X:
                        window.SendKey(User32.VirtualKey.VK_X, down);
                        break;
                    case Keys.Y:
                        window.SendKey(User32.VirtualKey.VK_Y, down);
                        break;
                    case Keys.Z:
                        window.SendKey(User32.VirtualKey.VK_Z, down);
                        break;
                    case Keys.Apps:
                        window.SendKey(User32.VirtualKey.VK_APPS, down);
                        break;
                    case Keys.Sleep:
                        window.SendKey(User32.VirtualKey.VK_SLEEP, down);
                        break;
                    case Keys.NumPad0:
                        window.SendKey(User32.VirtualKey.VK_NUMPAD0, down);
                        break;
                    case Keys.NumPad1:
                        window.SendKey(User32.VirtualKey.VK_NUMPAD1, down);
                        break;
                    case Keys.NumPad2:
                        window.SendKey(User32.VirtualKey.VK_NUMPAD2, down);
                        break;
                    case Keys.NumPad3:
                        window.SendKey(User32.VirtualKey.VK_NUMPAD3, down);
                        break;
                    case Keys.NumPad4:
                        window.SendKey(User32.VirtualKey.VK_NUMPAD4, down);
                        break;
                    case Keys.NumPad5:
                        window.SendKey(User32.VirtualKey.VK_NUMPAD5, down);
                        break;
                    case Keys.NumPad6:
                        window.SendKey(User32.VirtualKey.VK_NUMPAD6, down);
                        break;
                    case Keys.NumPad7:
                        window.SendKey(User32.VirtualKey.VK_NUMPAD7, down);
                        break;
                    case Keys.NumPad8:
                        window.SendKey(User32.VirtualKey.VK_NUMPAD8, down);
                        break;
                    case Keys.NumPad9:
                        window.SendKey(User32.VirtualKey.VK_NUMPAD9, down);
                        break;
                    case Keys.Multiply:
                        window.SendKey(User32.VirtualKey.VK_MULTIPLY, down);
                        break;
                    case Keys.Add:
                        window.SendKey(User32.VirtualKey.VK_ADD, down);
                        break;
                    case Keys.Separator:
                        window.SendKey(User32.VirtualKey.VK_SEPARATOR, down);
                        break;
                    case Keys.Subtract:
                        window.SendKey(User32.VirtualKey.VK_SUBTRACT, down);
                        break;
                    case Keys.Decimal:
                        window.SendKey(User32.VirtualKey.VK_DECIMAL, down);
                        break;
                    case Keys.Divide:
                        window.SendKey(User32.VirtualKey.VK_DIVIDE, down);
                        break;
                    case Keys.F1:
                        window.SendKey(User32.VirtualKey.VK_F1, down);
                        break;
                    case Keys.F2:
                        window.SendKey(User32.VirtualKey.VK_F2, down);
                        break;
                    case Keys.F3:
                        window.SendKey(User32.VirtualKey.VK_F3, down);
                        break;
                    case Keys.F4:
                        window.SendKey(User32.VirtualKey.VK_F4, down);
                        break;
                    case Keys.F5:
                        window.SendKey(User32.VirtualKey.VK_F5, down);
                        break;
                    case Keys.F6:
                        window.SendKey(User32.VirtualKey.VK_F6, down);
                        break;
                    case Keys.F7:
                        window.SendKey(User32.VirtualKey.VK_F7, down);
                        break;
                    case Keys.F8:
                        window.SendKey(User32.VirtualKey.VK_F8, down);
                        break;
                    case Keys.F9:
                        window.SendKey(User32.VirtualKey.VK_F9, down);
                        break;
                    case Keys.F10:
                        window.SendKey(User32.VirtualKey.VK_F10, down);
                        break;
                    case Keys.F11:
                        window.SendKey(User32.VirtualKey.VK_F11, down);
                        break;
                    case Keys.F12:
                        window.SendKey(User32.VirtualKey.VK_F12, down);
                        break;
                    case Keys.F13:
                        window.SendKey(User32.VirtualKey.VK_F13, down);
                        break;
                    case Keys.F14:
                        window.SendKey(User32.VirtualKey.VK_F14, down);
                        break;
                    case Keys.F15:
                        window.SendKey(User32.VirtualKey.VK_F15, down);
                        break;
                    case Keys.F16:
                        window.SendKey(User32.VirtualKey.VK_F16, down);
                        break;
                    case Keys.F17:
                        window.SendKey(User32.VirtualKey.VK_F17, down);
                        break;
                    case Keys.F18:
                        window.SendKey(User32.VirtualKey.VK_F18, down);
                        break;
                    case Keys.F19:
                        window.SendKey(User32.VirtualKey.VK_F19, down);
                        break;
                    case Keys.F20:
                        window.SendKey(User32.VirtualKey.VK_F20, down);
                        break;
                    case Keys.F21:
                        window.SendKey(User32.VirtualKey.VK_F21, down);
                        break;
                    case Keys.F22:
                        window.SendKey(User32.VirtualKey.VK_F22, down);
                        break;
                    case Keys.F23:
                        window.SendKey(User32.VirtualKey.VK_F23, down);
                        break;
                    case Keys.F24:
                        window.SendKey(User32.VirtualKey.VK_F24, down);
                        break;
                    case Keys.NumLock:
                        window.SendKey(User32.VirtualKey.VK_NUMLOCK, down);
                        break;
                    case Keys.Scroll:
                        window.SendKey(User32.VirtualKey.VK_SCROLL, down);
                        break;
                    case Keys.LeftShift:
                        window.SendKey(User32.VirtualKey.VK_SHIFT, down);
                        break;
                    case Keys.RightShift:
                        window.SendKey(User32.VirtualKey.VK_SHIFT, down);
                        break;
                    case Keys.LeftControl:
                        window.SendKey(User32.VirtualKey.VK_CONTROL, down);
                        break;
                    case Keys.RightControl:
                        window.SendKey(User32.VirtualKey.VK_CONTROL, down);
                        break;
                    case Keys.Pause:
                        window.SendKey(User32.VirtualKey.VK_PAUSE, down);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(key), key, null);
                }
            }
            catch (Exception e)
            {
                DfWin.Error("Encountered error sending key: " + e);
            }
        }

        private void RunUpdateLoop(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                var stopWatch = new Stopwatch();
                while (true)
                {
                    try
                    {
                        stopWatch.Restart();
                        cancellationToken.ThrowIfCancellationRequested();

                        await Update();

                        var timeToWait = Math.Max(0, (int) (MinimumDelay - stopWatch.ElapsedMilliseconds));
                        await Task.Delay(timeToWait, cancellationToken);
                    }
                    catch (Exception e)
                    {
                        DfWin.Error("Failed to update Dwarf Fortress state: " + e);
                        if (cancellationToken.IsCancellationRequested) return;
                    }
                }
            }, cancellationToken);
        }

        private async Task Update()
        {
            if (!processService.TryGetDwarfFortressProcess(out Process process)) return;

            var dwarfFortressWindow = new Window(process.MainWindowHandle);
            var bitmap = await windowService.Capture(dwarfFortressWindow, Sizes.DwarfFortressPreferredClientSize, true);
            var tiles = gridGameGridService.ParseScreenshot(bitmap);

            var dwarfFortressInput = new DwarfFortressInput(tiles);
            inputService.SetDwarfFortressInput(dwarfFortressInput);
        }

        public void Dispose()
        {
            if (hasDisposed) return;

            hasDisposed = true;
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }
    }
}
