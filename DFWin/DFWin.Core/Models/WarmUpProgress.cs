using DFWin.Core.Interfaces;

namespace DFWin.Core.Models
{
    public class WarmUpProgress
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
