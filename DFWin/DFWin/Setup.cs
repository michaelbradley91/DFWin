using Autofac;
using DFWin.Core;

namespace DFWin
{
    public static class Setup
    {
        public static IContainer CreateIoC()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule<GameModule>();
            containerBuilder.RegisterModule<CoreModule>();

            return containerBuilder.Build();
        }
    }
}
