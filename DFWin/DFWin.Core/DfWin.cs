using Autofac;
using Autofac.Core;
using DFWin.Core.Services;

namespace DFWin.Core
{
    public static class DfWin
    {
        public static IContainer DependencyResolver { get; internal set; }

        public static void Error(string message)
        {
            Resolve<ILoggingService>().Error(message);
        }

        public static void Warn(string message)
        {
            Resolve<ILoggingService>().Warn(message);
        }

        public static void Trace(string message)
        {
            Resolve<ILoggingService>().Trace(message);
        }

        public static T Resolve<T>(params Parameter[] parameters)
        {
            return DependencyResolver.Resolve<T>(parameters);
        }
    }
}
