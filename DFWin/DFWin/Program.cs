using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using DFWin.Constants;
using DFWin.User32Extensions.Models;
using DFWin.User32Extensions.Service;

namespace DFWin
{
    public class Program
    {
        private const int NumberOfWarmUpProcesses = 5;

        public static void Main(string[] args)
        {
            var dwarfFortressProcess = TryGetDwarfFortressProcess();
            if (dwarfFortressProcess == null) return;

            var ioc = Setup.CreateIoC(dwarfFortressProcess);

            if (args.Length <= 0)
            {
                MainAsync(ioc).GetAwaiter().GetResult();
            }
            else
            {
                WarmUpOnly(ioc).GetAwaiter().GetResult();
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
            Console.WriteLine("Starting...");

            return dwarfFortress;
        }

        private static async Task MainAsync(IComponentContext ioc)
        {
            var processes = new List<Process>();
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            for (var i = 0; i < NumberOfWarmUpProcesses; i++)
            {
                processes.Add(Process.Start(new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    FileName = $"{assemblyName}.exe",
                    Arguments = "--warmstart"
                }));

                if (i <= 0) continue;

                await Task.Delay(500);
                processes[i - 1].Kill();
            }
            await Task.Delay(500);
            processes[NumberOfWarmUpProcesses - 1].Kill();

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
