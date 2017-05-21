using System.Diagnostics;
using System.Linq;
using Autofac;
using DFWin.Core.Constants;
using DFWin.Core.Services;
using DFWin.Core.User32Extensions.Models;

namespace DFWin.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterServices(builder);
            RegisterProcesses(builder);
            RegisterWindows(builder);
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterBuildCallback(c =>
            {
                DfWin.Logger = c.Resolve<ILoggingService>();
            });
        }

        private static void RegisterProcesses(ContainerBuilder containerBuilder)
        {
            // TODO figure out how to ensure Dwarf Fortress has been loaded!
            var currentProcess = Process.GetCurrentProcess();
            var dwarfFortress = Process.GetProcessesByName(Names.DwarfFortressProcess).First();

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
