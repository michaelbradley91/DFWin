using System;
using DFWin.Core.Constants;
using DFWin.Core.Helpers;
using DFWin.Core.Inputs;
using DFWin.Core.Services;
using DFWin.Core.States;

namespace DFWin.Core.Updaters
{
    public class LoadingUpdater : Updater<LoadingState>
    {
        private readonly IProcessService processService;
        private readonly IWarmUpService warmUpService;
        
        public LoadingUpdater(IProcessService processService, IWarmUpService warmUpService)
        {
            this.processService = processService;
            this.warmUpService = warmUpService;
        }

        protected override IScreenState Update(LoadingState previousState, GameInput input)
        {
            switch (previousState.Phase)
            {
                case LoadingPhase.WaitingForDwarfFortressToStart:
                    if (!processService.IsDwarfFortressAvailable())
                    {
                        return LoadingState.InitialState;
                    }
                    warmUpService.BeginWarmUp();
                    return new LoadingState(0, LoadingPhase.WaitingForWarmUpToFinish);
                case LoadingPhase.WaitingForWarmUpToFinish:
                    var progress = input.WarmUpInput.GetProgress();
                    if (progress.HasFinished)
                    {
                        return new LoadingState(100, progress.Succeeded ? LoadingPhase.WarmUpSuccessful : LoadingPhase.WarmUpUnsuccessful);
                    }
                    else
                    {
                        var progressPercentage = (100 * progress.NumberOfProcessesCompleted) / progress.TotalNumberOfProcesses;
                        return new LoadingState(progressPercentage, LoadingPhase.WaitingForWarmUpToFinish);
                    }
                case LoadingPhase.WarmUpSuccessful:
                case LoadingPhase.WarmUpUnsuccessful:
                    return input.DwarfFortressInput.IsInitialised
                        ? StateHelpers.CreateInitialScreenState(input.DwarfFortressInput) 
                        : previousState;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
