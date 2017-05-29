using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DFWin.Core.Helpers;
using DFWin.Core.Inputs;
using DFWin.Core.Models;
using DFWin.Core.Services;

namespace DFWin.Core.Resources.Models
{
    public class WarmUpTask : IDisposable
    {
        public WarmUpTask(IInputService inputService, IWarmUpConfiguration configuration)
        {
            this.inputService = inputService;
            this.configuration = configuration;

            cancellationTokenSource = new CancellationTokenSource();
            progress = new WarmUpProgress(configuration);
        }

        public IWarmUpProgress Progress
        {
            get
            {
                lock (progressLock)
                {
                    return progress;
                }
            }
        }

        private bool hasStarted;
        private bool hasDisposed;
        private IWarmUpProgress progress;
        private readonly object progressLock = new object();
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly IInputService inputService;
        private readonly IWarmUpConfiguration configuration;

        /// <summary>
        /// Starts the warm up process in a background thread. This should only be called once.
        /// </summary>
        public void StartAndInitialiseWarmUpInput()
        {
            if (hasStarted) throw new InvalidOperationException("You cannot start the same warm up task multiple times");
            StartAsync(cancellationTokenSource.Token);
            inputService.SetWarmUpInput(new WarmUpInput(Progress));
            hasStarted = true;
        }

        private void StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                var numberOfProcessesCompleted = 0;
                for (var i = 0; i < configuration.NumberOfWarmUpProcessesToSpawn; i++)
                {
                    try
                    {
                        var process = StartNewWarmUpProcess();
                        var fastEnough = await WaitForProcessOrKill(process, cancellationToken);

                        numberOfProcessesCompleted++;

                        if (fastEnough)
                        {
                            UpdateProgressWithSuccess(numberOfProcessesCompleted);
                            Dispose();
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        DfWin.Error("Warm up task failed: " + e);
                        if (!hasDisposed) UpdateProgressWithFailure(numberOfProcessesCompleted);
                        Dispose();
                        return;
                    }
                        
                    UpdateProgress(numberOfProcessesCompleted);
                }
                UpdateProgressWithFailure(numberOfProcessesCompleted);
                Dispose();
            }, cancellationToken);
        }

        private Process StartNewWarmUpProcess()
        {
            return Process.Start(new ProcessStartInfo
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                FileName = configuration.ExecutablePath,
            });
        }
        
        private async Task<bool> WaitForProcessOrKill(Process process, CancellationToken cancellationToken)
        {
            try
            {
                var finished = await process.WaitForExitAsync(cancellationToken)
                    .WaitAsync(configuration.TimeToWaitPerProcessForGoodPerformanceInMilliseconds, cancellationToken);

                if (!finished && !process.HasExited) process.Kill();

                return finished;
            }
            catch (Exception waitException)
            {
                try
                {
                    DfWin.Warn($"Waiting for process failed due to exception: {waitException}");
                    if (!process.HasExited) process.Kill();
                    return false;
                }
                catch (Exception killException)
                {
                    DfWin.Warn($"Might not have successfully killed process due to exception: {killException}");
                    return false;
                }
            }
        }

        private void UpdateProgressWithSuccess(int numberOfProcessesCompleted)
        {
            UpdateProgress(new WarmUpProgress(configuration)
            {
                HasFinished = true,
                Succeeded = true,
                NumberOfProcessesCompleted = numberOfProcessesCompleted
            });
            DfWin.Trace("Warm up succeeded!");
        }

        private void UpdateProgressWithFailure(int numberOfProcessesCompleted)
        {
            UpdateProgress(new WarmUpProgress(configuration)
            {
                HasFinished = true,
                NumberOfProcessesCompleted = numberOfProcessesCompleted
            });
            DfWin.Trace("Warm up failed.");
        }

        private void UpdateProgress(int numberOfProcessesCompleted)
        {
            UpdateProgress(new WarmUpProgress(configuration)
            {
                NumberOfProcessesCompleted = numberOfProcessesCompleted
            });
        }

        private void UpdateProgress(IWarmUpProgress warmUpProgress)
        {
            lock (progressLock)
            {
                DfWin.Trace($"Progress: {warmUpProgress.NumberOfProcessesCompleted} / {warmUpProgress.TotalNumberOfProcesses}");
                progress = warmUpProgress;
                if (hasDisposed) throw new InvalidOperationException("Cannot update the warm up progress when the task has been aborted.");
                inputService.SetWarmUpInput(new WarmUpInput(warmUpProgress));
            }
        }

        /// <summary>
        /// Unlike abort, this does not dispose the task itself but still does attempt to cancel any running task.
        /// </summary>
        public void Dispose()
        {
            if (hasDisposed) return;
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            hasDisposed = true;
        }
        
        public void Abort()
        {
            Dispose();
        }
    }
}
