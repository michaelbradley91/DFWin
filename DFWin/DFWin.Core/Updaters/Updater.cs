using DFWin.Core.Inputs;
using DFWin.Core.Inputs.DwarfFortress;
using DFWin.Core.Services;
using DFWin.Core.States;

namespace DFWin.Core.Updaters
{
    /// <summary>
    /// By convention, there should be an updater for every screen state. So a LoadingState requires a LoadingUpdater
    /// An updater will be called whenever its corresponding screen state is the current state.
    /// </summary>
    public interface IUpdater
    {
        GameState Update(GameState previousState);
    }

    public abstract class Updater<TScreenState> : IUpdater
        where TScreenState : IScreenState
    {
        public GameState Update(GameState previousState)
        {
            var nextScreenState = Update((TScreenState) previousState.ScreenState, previousState.GameInput);
            return new GameState(nextScreenState, previousState.GameInput, previousState.FrameRate, previousState.ShouldExit);
        }

        protected abstract IScreenState Update(TScreenState previousState, GameInput gameInput);
    }
}
