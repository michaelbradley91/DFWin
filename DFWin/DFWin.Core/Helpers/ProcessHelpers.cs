using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DFWin.Core.Helpers
{
    public static class ProcessHelpers
    {
        public static Task<bool> WaitForExitAsync(this Process process, CancellationToken cancellationToken)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => taskCompletionSource.TrySetResult(true);
            cancellationToken.Register(() => { taskCompletionSource.TrySetCanceled(); });

            return taskCompletionSource.Task;
        }
    }
}
