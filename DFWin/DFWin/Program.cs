using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using DFWin.Constants;
using DFWin.Helpers;
using DFWin.User32Extensions.Models;
using DFWin.User32Extensions.Service;

namespace DFWin
{
    public class Program
    {
        private const int NumberOfWarmUpProcesses = 5;

        /// <summary>
        /// When passed, the application only "warms up" the Dwarf Fortress window so it can take screenshots more easily later.
        /// </summary>
        private const string WarmUpFlag = "--warm-up";

        public static void Main(string[] args)
        {
            var dwarfFortressProcess = TryGetDwarfFortressProcess();
            if (dwarfFortressProcess == null) return;

            var ioc = Setup.CreateIoC(dwarfFortressProcess);

            if (args.Contains(WarmUpFlag))
            {
                WarmUpOnly(ioc).GetAwaiter().GetResult();
            }
            else
            {
                MainAsync(ioc).GetAwaiter().GetResult();
            }
        }

        private static Process TryGetDwarfFortressProcess()
        {
            var dwarfFortress = Process.GetProcesses().SingleOrDefault(p => p.ProcessName.Contains("Dwarf Fortress"));

            while (dwarfFortress == null)
            {
                Console.WriteLine("Please start Dwarf Fortress and press anything but q to continue. Press q to quit." + Environment.NewLine +
                                  "Detail: Could not find a process with name \"Dwarf Fortress\"");

                var key = Console.ReadKey();
                if (new[] { 'q', 'Q' }.Contains(key.KeyChar)) return null;

                dwarfFortress = Process.GetProcesses().SingleOrDefault(p => p.ProcessName.Contains("Dwarf Fortress"));
            }
            Console.WriteLine("Found Dwarf Fortress");

            return dwarfFortress;
        }

        private static async Task MainAsync(IComponentContext ioc)
        {
            var executableLocation = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;

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
                        Arguments = WarmUpFlag
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

            var dwarfFortress = ioc.Resolve<IDwarfFortress>();
            await dwarfFortress.Run();
        }

        private static async Task WarmUpOnly(IComponentContext container)
        {
            var windowService = container.Resolve<IWindowService>();
            var dwarfFortressWindow = container.ResolveKeyed<Window>(DependencyKeys.Window.DwarfFortress);

            await windowService.PrepareForCapture(dwarfFortressWindow, Sizes.DwarfFortressPreferredClientSize);
            while (true)
            {
                await windowService.Capture(dwarfFortressWindow, Sizes.DwarfFortressPreferredClientSize);
            }
        }
    }
}
