using DFWin.Core.Inputs;
using DFWin.Core.States;

namespace DFWin.Core.Updaters
{
    public class LoadingUpdater : IUpdater
    {
        public GameState Update(GameState previousState, GameInput input)
        {
            return previousState;
        }
    }
}
