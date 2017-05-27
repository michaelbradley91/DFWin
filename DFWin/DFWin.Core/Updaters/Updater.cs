using DFWin.Core.Inputs;
using DFWin.Core.States;

namespace DFWin.Core.Updaters
{
    /// <summary>
    /// By convention, there should be an updater for every screen state. So a LoadingState requires a LoadingUpdater
    /// An updater will be called whenever its corresponding screen state is the current state.
    /// </summary>
    public interface IUpdater
    {
        GameState Update(GameState previousState, GameInput input);
    }

    public abstract class Updater<TScreenState> : IUpdater
        where TScreenState : IScreenState
    {
        public GameState Update(GameState previousState, GameInput input)
        {
            var nextScreenState = Update((TScreenState) previousState.ScreenState, input);
            return new GameState(nextScreenState);
        }

        protected abstract TScreenState Update(TScreenState previousState, GameInput input);
    }
}
