using System;
using System.Diagnostics;
using System.Linq;
using Autofac;

namespace DFWin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dwarfFortressProcess = TryGetDwarfFortressProcess();
            if (dwarfFortressProcess == null) return;
            var ioc = Setup.CreateIoC(dwarfFortressProcess);

            var dwarfFortress = ioc.Resolve<IDwarfFortress>();
            dwarfFortress.Run().GetAwaiter().GetResult();
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
    }
}
