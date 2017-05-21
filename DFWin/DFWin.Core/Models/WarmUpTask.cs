using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DFWin.Core.Helpers;
using DFWin.Core.Interfaces;

namespace DFWin.Core.Models
{
    public class WarmUpTask : IDisposable
    {
        public WarmUpTask(IWarmUpConfiguration configuration)
        {
            this.configuration = configuration;

            cancellationTokenSource = new CancellationTokenSource();
            progress = new WarmUpProgress(configuration);
        }

        public WarmUpProgress Progress
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
        private bool hasAborted;
        private Task task;
        private WarmUpProgress progress;
        private readonly object progressLock = new object();
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly IWarmUpConfiguration configuration;

        /// <summary>
        /// Starts the warm up process in a background thread. This should only be called once.
        /// </summary>
        public void Start()
        {
            if (hasStarted) throw new InvalidOperationException("You cannot start the same warm up task multiple times");
            task = StartAsync(cancellationTokenSource.Token);
            hasStarted = true;
        }

        private Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                var numberOfProcessesCompleted = 0;
                for (var i = 0; i < configuration.NumberOfWarmUpProcessesToSpawn; i++)
                {
                    var process = StartNewWarmUpProcess();
                    var fastEnough = await WaitForProcessOrKill(process, cancellationToken);

                    numberOfProcessesCompleted++;

                    if (fastEnough)
                    {
                        UpdateProgressWithSuccess(numberOfProcessesCompleted);
                        return;
                    }

                    UpdateProgress(numberOfProcessesCompleted);
                }
                UpdateProgressWithFailure(numberOfProcessesCompleted);
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

        private void UpdateProgress(WarmUpProgress warmUpProgress)
        {
            lock (progressLock)
            {
                DfWin.Trace($"Progress: {warmUpProgress.NumberOfProcessesCompleted} / {warmUpProgress.TotalNumberOfProcesses}");
                progress = warmUpProgress;
            }
        }

        public void Dispose()
        {
            if (hasAborted) return;
            Abort();
        }

        /// <summary>
        /// Disposes of this task and attempts to terminate all processes it spawned that are still running.
        /// </summary>
        public void Abort()
        {
            if (hasAborted) return;
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            task.Dispose();
            hasAborted = true;
        }
    }
}
