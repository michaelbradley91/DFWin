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
    }
}
