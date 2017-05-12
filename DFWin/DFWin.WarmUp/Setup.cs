using Autofac;
using DFWin.Core;

namespace DFWin.WarmUp
{
    public static class Setup
    {
        public static IContainer CreateIoC()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule<CoreModule>();

            return containerBuilder.Build();
        }
    }
}
