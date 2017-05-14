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
            WarmUp();

            var ioc = Setup.CreateIoC();

            using (var game = ioc.Resolve<DwarfFortress>()) game.Run();
        }

        private static void WarmUp()
        {
            var executableLocation = new Uri(Assembly.GetAssembly(typeof(WarmUp.Program)).CodeBase).LocalPath;

            var processes = new List<Process>();

            Debug.WriteLine("Warming up...");
            var fastEnough = false;
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

                    try
                    {
                        if (!processes[i - 1].WaitForExit(500)) processes[i - 1].Kill();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
                try
                {
                    if (!(fastEnough = processes[NumberOfWarmUpProcesses - 1].WaitForExit(500))) processes[NumberOfWarmUpProcesses - 1].Kill();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
            finally
            {
                ExceptionHelpers.TryAll(processes.Select<Process, Action>(p => (() => p.Dispose())).ToArray());
            }
            Debug.WriteLine(fastEnough ? "Warmed up successfully!" : "Unsuccessful warmup. Performance may be choppy.");
        }
    }
#endif
}
