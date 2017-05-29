namespace DFWin.Core.Models
{
    public interface IWarmUpConfiguration
    {
        string ExecutablePath { get; }
        int TimeToWaitPerProcessForGoodPerformanceInMilliseconds { get; }
        int NumberOfWarmUpProcessesToSpawn { get; }
    }
}
