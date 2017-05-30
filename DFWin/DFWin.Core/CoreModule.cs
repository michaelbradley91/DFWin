using Autofac;

namespace DFWin.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterServices(builder);
            RegisterUpdaters(builder);
            RegisterMiddleware(builder);
            RegisterTranslators(builder);
            RegisterCaches(builder);

            builder.RegisterType<ScreenManager>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<UpdateManager>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<TranslatorManager>().AsImplementedInterfaces().SingleInstance();
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

        private void RegisterTranslators(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Translator"))
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        private void RegisterCaches(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Cache"))
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
