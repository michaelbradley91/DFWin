using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using DFWin.Core.Constants;
using DFWin.Core.Helpers;
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
