using Autofac;

namespace DFWin
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterServices(builder);

            builder.RegisterType<DwarfFortress>().AsImplementedInterfaces().SingleInstance();
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
