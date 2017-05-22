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
            builder.RegisterType<ScreenManager>().AsSelf().SingleInstance();

            RegisterScreens(builder);
        }

        private void RegisterScreens(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Screen"))
                .AsSelf()
                .SingleInstance();
        }
    }
}
