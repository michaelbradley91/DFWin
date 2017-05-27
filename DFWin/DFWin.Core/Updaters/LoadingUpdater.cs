using System;
using System.Net.Http.Headers;
using DFWin.Core.Constants;
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

        /*
         * If dwarf fortress unavailable: show "please start dwarf fortress"
         * If dwarf fortress available -> start warm up
         * Once warm up complete -> check if available again. If not - restart.
         * If yes, go to success
         */

        public LoadingUpdater(IProcessService processService, IWarmUpService warmUpService, IInputService inputService)
        {
            this.processService = processService;
            this.warmUpService = warmUpService;
            this.inputService = inputService;
        }

        protected override LoadingState Update(LoadingState previousState, GameInput input)
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
                    return !processService.IsDwarfFortressAvailable() ? LoadingState.InitialState : previousState;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
