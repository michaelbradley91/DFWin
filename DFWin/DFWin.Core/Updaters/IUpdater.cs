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
}
