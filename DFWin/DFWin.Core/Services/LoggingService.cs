using System.Diagnostics;

namespace DFWin.Core.Services
{
    public interface ILoggingService
    {
        void Error(string message);
        void Warn(string message);
        void Trace(string message);
    }

    public class LoggingService : ILoggingService
    {
        public void Error(string message)
        {
            Debug.WriteLine("Error: " + message);
        }

        public void Warn(string message)
        {
            Debug.WriteLine("Warn: " + message);
        }

        public void Trace(string message)
        {
            Debug.WriteLine("Trace: " + message);
        }
    }
}
