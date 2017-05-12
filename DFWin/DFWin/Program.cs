using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using DFWin.Core.Constants;
using DFWin.Core.Helpers;

namespace DFWin
{
#if WINDOWS
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        private const int NumberOfWarmUpProcesses = 10;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            WarmUpAsync().GetAwaiter().GetResult();

            var ioc = Setup.CreateIoC();

            using (var game = ioc.Resolve<DwarfFortress>()) game.Run();
        }

        private static async Task WarmUpAsync()
        {
            var executableLocation = new Uri(Assembly.GetAssembly(typeof(WarmUp.Program)).CodeBase).LocalPath;

            var processes = new List<Process>();

            Console.WriteLine("Warming up...");
            try
            {
                for (var i = 0; i < NumberOfWarmUpProcesses; i++)
                {
                    processes.Add(Process.Start(new ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        FileName = executableLocation,
                    }));

                    if (i <= 0) continue;

                    await Task.Delay(500);
                    processes[i - 1].Kill();
                }
                await Task.Delay(500);
                processes[NumberOfWarmUpProcesses - 1].Kill();
            }
            finally
            {
                ExceptionHelpers.TryAll(processes.Select<Process, Action>(p => (() => p.Dispose())).ToArray());
            }
            Console.WriteLine("Warm up complete! Starting...");
        }
    }
#endif
}
