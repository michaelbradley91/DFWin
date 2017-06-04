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

        /// <summary>
        /// Keys can fail to be sent due to the DF Window not responding or due to
        /// the backlog of keys exceeding its permitted limit. Therefore, do not assume this will succeed.
        /// </summary>
        Task TrySendKeysAsync(params Keys[] keys);

        bool IsSendingKeys { get; }
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

        private readonly object sendKeysLock = new object();
        private int numberOfWaitingSendKeys;
        private const int MaximumNumberOfWaitingSendKeys = 10;
        private Task sendKeysTask = Task.CompletedTask;
        
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
            if (keys.Length == 0) return;

            Task newTask;
            lock (sendKeysLock)
            {
                numberOfWaitingSendKeys++;

                // Exit early to protect against large numbers of backlogged key presses.
                if (numberOfWaitingSendKeys > MaximumNumberOfWaitingSendKeys)
                {
                    numberOfWaitingSendKeys--;
                    DfWin.Warn("Dropped keys due to backlog: " + string.Join(",", keys));
                    return;
                }
                try
                {
                    var copyOfSendKeysTask = sendKeysTask;
                    newTask = Task.Run(async () =>
                    {
                        await copyOfSendKeysTask;
                        if (!processService.TryGetDwarfFortressProcess(out Process process)) return;
                        var window = new Window(process.MainWindowHandle);
                        window.SendKeys(keys.Select(k => k.ToVirtualKey()).ToArray());
                    });
                    sendKeysTask = newTask;
                }
                catch
                {
                    numberOfWaitingSendKeys--;
                    throw;
                }
            }
            try
            {
                await newTask;
            }
            catch (Exception e)
            {
                DfWin.Error("Encountered error sending keys: " + e);
            }
            finally
            {
                lock (sendKeysLock)
                {
                    numberOfWaitingSendKeys--;
                }
            }
        }

        public bool IsSendingKeys
        {
            get
            {
                lock (sendKeysLock)
                {
                    return !sendKeysTask.IsCompleted;
                }
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
