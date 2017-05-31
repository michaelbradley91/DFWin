using System;
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

        public GameState Update(GameState previousState, Func<GameState, GameState> next)
        {
            return !processService.IsDwarfFortressAvailable() ?
                next(new GameState(LoadingState.InitialState, previousState.GameInput, previousState.FrameRate, previousState.ShouldExit)) 
                : next(previousState);
        }
    }
}