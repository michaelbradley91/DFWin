using DFWin.Core.Constants;

namespace DFWin.Core.State
{
    /// <summary>
    /// The root of all application state.
    /// </summary>
    public class GameState
    {
        public Screens CurrentScreen;
        public ScreenState ScreenState;

        public static GameState InitialState => new GameState
        {
            CurrentScreen = Screens.Loading,
            ScreenState = new LoadingState { LoadingPercent = 0, Message = "Loading..." }
        };
    }
}
