using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using DFWin.User32Extensions;
using DFWin.User32Extensions.Enumerations;
using DFWin.User32Extensions.Models;
using DFWin.User32Extensions.Service;
using DFWin.User32Extensions.Structs;
using PInvoke;
using ThreadState = System.Diagnostics.ThreadState;

namespace DFWin
{
    public class Program
    {
        private const int expectedWidth = 1280;
        private const int expectedHeight = 400;

        public static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        public static async Task MainAsync()
        {
            var dwarfFortress = Process.GetProcesses().SingleOrDefault(p => p.ProcessName.Contains("Dwarf Fortress"));

            while (dwarfFortress == null)
            {
                Console.WriteLine("Please start Dwarf Fortress and press anything but q to continue. Press q to quit." + Environment.NewLine +
                                  "Detail: Could not find a process with name \"Dwarf Fortress\"");

                var key = Console.ReadKey();
                if (new[] { 'q', 'Q' }.Contains(key.KeyChar)) return;

                dwarfFortress = Process.GetProcesses().SingleOrDefault(p => p.ProcessName.Contains("Dwarf Fortress"));
            }

            var dwarfFortressWindow = new Window(dwarfFortress.MainWindowHandle);

            dwarfFortressWindow.SetState(WindowState.Minimised);
            dwarfFortressWindow.SendKeys(User32.VirtualKey.VK_UP);

            Console.WriteLine("Sent keys");
            Console.ReadLine();

            var image = await new WindowService().TakeHiddenScreenshotOfClient(dwarfFortressWindow, expectedWidth, expectedHeight);

            if (File.Exists("MyImage.bmp")) File.Delete("MyImage.bmp");

            image.Save("MyImage.bmp");

            Console.WriteLine("Test done");
            Console.ReadLine();
        }
    }
}
