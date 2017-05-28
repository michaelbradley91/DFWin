using System;
using DFWin.Core.Inputs;
using DFWin.Core.Services;
using DFWin.Core.States;

namespace DFWin.Core.Middleware
{
    public class DwarfFortressProcessMiddleware : IUpdaterMiddleware
    {
        private readonly IProcessService processService;

        public DwarfFortressProcessMiddleware(IProcessService processService)
        {
            this.processService = processService;
        }

        public GameState Update(GameState previousState, GameInput gameInput, Func<GameState, GameInput, GameState> next)
        {
            return !processService.IsDwarfFortressAvailable() ? 
                next(new GameState(LoadingState.InitialState, previousState.InputState, previousState.FrameRate, previousState.ShouldExit), gameInput) 
                : next(previousState, gameInput);
        }
    }
}