using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using DFWin.Constants;
using DFWin.Services;
using DFWin.User32Extensions.Models;
using DFWin.User32Extensions.Service;

namespace DFWin
{
    public interface IDwarfFortress
    {
        Task Run();
    }

    public class DwarfFortress : IDwarfFortress
    {
        private readonly Window dwarfFortressWindow;
        private readonly IWindowService windowService;
        private readonly IGameGridService gameGridService;

        public DwarfFortress(IIndex<DependencyKeys.Window, Window> windows, IWindowService windowService, IGameGridService gameGridService)
        {
            dwarfFortressWindow = windows[DependencyKeys.Window.DwarfFortress];
            this.windowService = windowService;
            this.gameGridService = gameGridService;
        }

        public async Task Run()
        {
            await windowService.PrepareForCapture(dwarfFortressWindow, Sizes.DwarfFortressPreferredClientSize);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (var i = 0; i < 1000; i++)
            {
                using (var bitmap = await windowService.Capture(dwarfFortressWindow, Sizes.DwarfFortressPreferredClientSize))
                {
                    var tiles = gameGridService.ParseScreenshot(bitmap);
                }
            }
            stopWatch.Stop();

            var image = await windowService.Capture(dwarfFortressWindow, Sizes.DwarfFortressPreferredClientSize);

            if (File.Exists("MyImage.bmp")) File.Delete("MyImage.bmp");

            image.Save("MyImage.bmp");
            image.Dispose();

            Console.WriteLine("Test done. Took: " + stopWatch.Elapsed.TotalMilliseconds);
            Console.ReadLine();
        }
    }
}
