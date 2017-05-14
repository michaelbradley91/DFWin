using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Autofac;
using DFWin.Core.Constants;
using DFWin.Core.User32Extensions.Models;
using DFWin.Core.User32Extensions.Services;

namespace DFWin.WarmUp
{
    public class Program
    {
        private static readonly TimeSpan FastEnough = TimeSpan.FromMilliseconds(10);

        public static void Main(string[] args)
        {
            WarmUp().GetAwaiter().GetResult();
        }

        private static async Task WarmUp()
        {
            var container = Setup.CreateIoC();

            var windowService = container.Resolve<IWindowService>();
            var dwarfFortressWindow = container.ResolveKeyed<Window>(DependencyKeys.Window.DwarfFortress);

            await windowService.PrepareForCapture(dwarfFortressWindow, Sizes.DwarfFortressPreferredClientSize);

            var totalStopWatch = new Stopwatch();
            totalStopWatch.Start();
            var stopWatch = new Stopwatch();
            while (totalStopWatch.Elapsed < TimeSpan.FromSeconds(15))
            {
                stopWatch.Start();
                await windowService.Capture(dwarfFortressWindow, Sizes.DwarfFortressPreferredClientSize);
                stopWatch.Stop();

                Console.WriteLine($@"Capture took: {stopWatch.Elapsed.TotalMilliseconds}");

                if (stopWatch.Elapsed < FastEnough) return;
                stopWatch.Reset();
            }
        }
    }
}
