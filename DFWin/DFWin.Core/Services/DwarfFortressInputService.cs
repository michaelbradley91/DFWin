using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DFWin.Core.Constants;
using DFWin.Core.Inputs;
using DFWin.Core.User32Extensions;
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
                window.SendKey(key.ToVirtualKey(), down);
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
