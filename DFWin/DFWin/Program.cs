using System;
using Autofac;
using DFWin.Core.Services;

namespace DFWin
{
#if WINDOWS
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            var ioc = Setup.CreateIoC();

            using (var game = ioc.Resolve<DwarfFortress>())
            {
                var warmUpService = ioc.Resolve<IWarmUpService>();
                warmUpService.BeginWarmUp();
                game.Run();
            }
        }
    }
#endif
}
