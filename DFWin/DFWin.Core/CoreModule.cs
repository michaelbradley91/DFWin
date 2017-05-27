using System.Diagnostics;
using System.Linq;
using Autofac;
using DFWin.Core.Constants;
using DFWin.Core.User32Extensions.Models;

namespace DFWin.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterServices(builder);
            RegisterUpdaters(builder);
            RegisterMiddleware(builder);

            builder.RegisterType<ScreenManager>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<UpdateManager>().AsImplementedInterfaces().SingleInstance();
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterBuildCallback(c =>
            {
                DfWin.DependencyResolver = c;
            });
        }

        private void RegisterUpdaters(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Updater"))
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        private void RegisterMiddleware(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Middleware"))
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
