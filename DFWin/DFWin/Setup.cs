using System.Diagnostics;
using Autofac;
using DFWin.Constants;
using DFWin.User32Extensions.Models;

namespace DFWin
{
    public static class Setup
    {
        public static IContainer CreateIoC(Process dwarfFortress)
        {
            var containerBuilder = new ContainerBuilder();

            RegisterProcesses(containerBuilder, dwarfFortress);
            RegisterWindows(containerBuilder);

            containerBuilder.RegisterModule<CoreModule>();

            return containerBuilder.Build();
        }

        private static void RegisterProcesses(ContainerBuilder containerBuilder, Process dwarfFortress)
        {
            var currentProcess = Process.GetCurrentProcess();

            containerBuilder.Register(c => dwarfFortress).Keyed<Process>(DependencyKeys.Process.DwarfFortress).SingleInstance();
            containerBuilder.Register(c => currentProcess).Keyed<Process>(DependencyKeys.Process.Application).SingleInstance();
        }

        private static void RegisterWindows(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(
                c => new Window(c.ResolveKeyed<Process>(DependencyKeys.Process.DwarfFortress).MainWindowHandle))
                .Keyed<Window>(DependencyKeys.Window.DwarfFortress).SingleInstance();
            containerBuilder.Register(
                c => new Window(c.ResolveKeyed<Process>(DependencyKeys.Process.Application).MainWindowHandle))
                .Keyed<Window>(DependencyKeys.Window.Application).SingleInstance();
        }
    }
}
