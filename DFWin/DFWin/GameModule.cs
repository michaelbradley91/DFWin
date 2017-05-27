using Autofac;
using DFWin.Models;

namespace DFWin
{
    public class GameModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DwarfFortress>().AsSelf().SingleInstance();
            builder.RegisterType<WarmUpConfiguration>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ContentManager>().AsSelf().SingleInstance();

            RegisterScreens(builder);
            RegisterMiddleware(builder);
        }

        private void RegisterScreens(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Screen"))
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
