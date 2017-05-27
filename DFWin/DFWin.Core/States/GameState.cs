using DFWin.Core.Constants;

namespace DFWin.Core.States
{
    /// <summary>
    /// The root of the state used to determine what should be shown on screen.
    /// The state itself should be immutable and updated during each game loop.
    /// </summary>
    public class GameState
    {
        public IScreenState ScreenState { get; }

        public GameState(IScreenState screenState)
        {
            ScreenState = screenState;
        }

        public static GameState InitialState => new GameState(LoadingState.InitialState);
    }
}
