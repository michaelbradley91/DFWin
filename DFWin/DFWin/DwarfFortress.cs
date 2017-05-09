using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using DFWin.Constants;
using DFWin.User32Extensions.Models;
using DFWin.User32Extensions.Service;

namespace DFWin
{
    public interface IDwarfFortress
    {
        Task Run();
    }

    public class DwarfFortress
    {
        private readonly Window dwarfFortressWindow;
        private readonly IWindowService windowService;

        public DwarfFortress([KeyFilter(DependencyKeys.Window.DwarfFortress)] Window dwarfFortressWindow, IWindowService windowService)
        {
            this.dwarfFortressWindow = dwarfFortressWindow;
            this.windowService = windowService;
        }

        public async Task Run()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (var i = 0; i < 1000; i++)
            {
                var bitmap = await windowService.TakeHiddenScreenshotOfClient(dwarfFortressWindow, Sizes.DwarfFortressPreferredClientSize);
                bitmap.Dispose();
            }
            stopWatch.Stop();

            var image = await windowService.TakeHiddenScreenshotOfClient(dwarfFortressWindow, Sizes.DwarfFortressPreferredClientSize);

            if (File.Exists("MyImage.bmp")) File.Delete("MyImage.bmp");

            image.Save("MyImage.bmp");

            Console.WriteLine("Test done. Took: " + stopWatch.Elapsed.TotalMilliseconds);
            Console.ReadLine();
        }
    }
}
