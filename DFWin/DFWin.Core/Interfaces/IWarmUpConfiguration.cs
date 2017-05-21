using System;

namespace DFWin.Core.Interfaces
{
    public interface IWarmUpConfiguration
    {
        string ExecutablePath { get; }
        int TimeToWaitPerProcessForGoodPerformanceInMilliseconds { get; }
        int NumberOfWarmUpProcessesToSpawn { get; }
    }
}
