using System;

namespace DFWin.Core.Interfaces
{
    public interface IWarmUpProcessDetails
    {
        string ExecutablePath { get; }
        int TimeToWaitPerProcessForGoodPerformanceInMilliseconds { get; }
        int NumberOfWarmUpProcessesToSpawn { get; }
    }
}
