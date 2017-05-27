using System;
using Autofac;

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
                game.Run();
            }
        }
    }
#endif
}
