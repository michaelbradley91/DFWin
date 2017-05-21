using DFWin.Core.Services;

namespace DFWin.Core
{
    public static class DfWin
    {
        public static ILoggingService Logger { get; internal set; }
        
        public static void Error(string message)
        {
            Logger.Error(message);
        }

        public static void Warn(string message)
        {
            Logger.Warn(message);
        }

        public static void Trace(string message)
        {
            Logger.Trace(message);
        }
    }
}
