using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DFWin.Core.Constants;
using DFWin.Core.PInvoke;
using DFWin.Core.PInvoke.Models;
using DFWin.Core.PInvoke.Services;
using Microsoft.Xna.Framework.Input;

namespace DFWin.Core.Services
{
    public interface IDwarfFortressInputService : IDisposable
    {
        void StartScreenScraping();
        
        Task TrySendKeysAsync(params Keys[] keys);
    }

    public class DwarfFortressInputService : IDwarfFortressInputService
    {
        private readonly IProcessService processService;
        private readonly IWindowService windowService;
        private readonly ITilesService tilesService;
        private readonly ITranslatorManager translatorManager;

        private bool hasStarted;
        private bool hasDisposed;
        private readonly CancellationTokenSource cancellationTokenSource;

        private const int MinimumDelay = 10;
        
        public DwarfFortressInputService(IProcessService processService, IWindowService windowService,
            ITilesService tilesService, ITranslatorManager translatorManager)
        {
            this.processService = processService;
            this.windowService = windowService;
            this.tilesService = tilesService;
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
            var bitmap = await windowService.Capture(dwarfFortressWindow, Sizes.DwarfFortressClientSize, true);
            var tiles = tilesService.ParseScreenshot(bitmap);

            translatorManager.TranslateInBackgroundAndUpdateGameInput(tiles);
        }

        public async Task TrySendKeysAsync(params Keys[] keys)
        {
            try
            {
                await Task.Run(() =>
                {
                    if (!processService.TryGetDwarfFortressProcess(out Process process)) return;
                    var window = new Window(process.MainWindowHandle);
                    window.SendKeys(keys.Select(k => k.ToVirtualKey()).ToArray());
                });
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
