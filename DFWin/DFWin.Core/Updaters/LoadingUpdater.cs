using DFWin.Core.Inputs;
using DFWin.Core.Resources.Models;
using DFWin.Core.Services;
using DFWin.Core.States;

namespace DFWin.Core.Updaters
{
    public class LoadingUpdater : Updater<LoadingState>
    {
        private readonly IProcessService processService;
        private readonly IWarmUpService warmUpService;
        private readonly IInputService inputService;

        private WarmUpTask warmUpTask;

        public LoadingUpdater(IProcessService processService, IWarmUpService warmUpService, IInputService inputService)
        {
            this.processService = processService;
            this.warmUpService = warmUpService;
            this.inputService = inputService;
        }

        protected override LoadingState Update(LoadingState previousState, GameInput input)
        {
            if ()

            if (!input.WarmUpInput.IsRunningTask)
            {
                warmUpService.BeginWarmUp();

                return new LoadingState(0, "Warming up...");
            }

            var warmUpProgress = input.WarmUpInput.GetProgress();

            if (warmUpProgress.HasFinished)
            {
                return new LoadingState(100, warmUpProgress.Succeeded ? "Warm up successful! Starting..." : "Warm up unsuccessful. Starting...");
            }

            var progressPercentage = (100 * warmUpProgress.NumberOfProcessesCompleted) /
                                     warmUpProgress.TotalNumberOfProcesses;

            return new LoadingState(progressPercentage, "Warming up...");
        }
    }
}
