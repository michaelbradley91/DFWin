using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DFWin.Core.Constants;
using DFWin.Core.User32Extensions;
using DFWin.Core.User32Extensions.Models;
using DFWin.Core.User32Extensions.Services;
using Microsoft.Xna.Framework.Input;

namespace DFWin.Core.Services
{
    public interface IDwarfFortressService : IDisposable
    {
        void StartScreenScraping();
        void TrySendKeys(params Keys[] keys);
    }

    public class DwarfFortressService : IDwarfFortressService
    {
        private readonly IProcessService processService;
        private readonly IWindowService windowService;
        private readonly IGameGridService gridGameGridService;
        private readonly ITranslatorManager translatorManager;

        private bool hasStarted;
        private bool hasDisposed;
        private readonly CancellationTokenSource cancellationTokenSource;

        private const int MinimumDelay = 10;
        
        public DwarfFortressService(IProcessService processService, IWindowService windowService,
            IGameGridService gridGameGridService, ITranslatorManager translatorManager)
        {
            this.processService = processService;
            this.windowService = windowService;
            this.gridGameGridService = gridGameGridService;
            this.translatorManager = translatorManager;

            cancellationTokenSource = new CancellationTokenSource();
        }

        public void StartScreenScraping()
        {
            if (hasStarted) throw new InvalidOperationException("You should only start the screenscraper once.");

            hasStarted = true;
            RunUpdateLoop(cancellationTokenSource.Token);
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

            translatorManager.TranslateInBackgroundAndUpdateGameInput(tiles);
        }

        public void TrySendKeys(params Keys[] keys)
        {
            try
            {
                if (!processService.TryGetDwarfFortressProcess(out Process process)) return;
                var window = new Window(process.MainWindowHandle);
                window.SendKeys(keys.Select(k => k.ToVirtualKey()).ToArray());
            }
            catch (Exception e)
            {
                DfWin.Error("Encountered error sending keys: " + e);
            }
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
