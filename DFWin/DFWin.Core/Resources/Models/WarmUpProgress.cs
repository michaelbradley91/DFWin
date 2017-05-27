using DFWin.Core.Interfaces;

namespace DFWin.Core.Resources.Models
{
    public interface IWarmUpProgress
    {
        int TotalNumberOfProcesses { get; }
        int NumberOfProcessesCompleted { get; }
        bool HasFinished { get; }
        bool Succeeded { get; }
    }

    public class WarmUpProgress : IWarmUpProgress
    {
        public WarmUpProgress(IWarmUpConfiguration configuration)
        {
            TotalNumberOfProcesses = configuration.NumberOfWarmUpProcessesToSpawn;
        }

        public int TotalNumberOfProcesses { get; }
        public int NumberOfProcessesCompleted { get; set; }
        public bool HasFinished { get; set; }
        public bool Succeeded { get; set; }
    }
}
