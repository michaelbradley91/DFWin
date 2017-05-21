using System.Threading;
using System.Threading.Tasks;

namespace DFWin.Core.Helpers
{
    public static class TaskHelpers
    {
        public static Task<bool> WaitAsync(this Task task, int milliseconds, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var finished = task.Wait(milliseconds, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return finished;
            }, cancellationToken);
        }
    }
}
