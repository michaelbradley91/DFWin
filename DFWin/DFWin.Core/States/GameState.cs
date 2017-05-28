using DFWin.Core.Inputs;

namespace DFWin.Core.States
{
    /// <summary>
    /// The root of the state used to determine what should be shown on screen.
    /// The state itself should be immutable and updated during each game loop.
    /// </summary>
    public class GameState
    {
        public IScreenState ScreenState { get; }
        public GameInput GameInput { get; }
        public int FrameRate { get; }
        public bool ShouldExit { get; }

        public GameState(IScreenState screenState, GameInput gameInput, int frameRate, bool shouldExit)
        {
            ScreenState = screenState;
            GameInput = gameInput;
            FrameRate = frameRate;
            ShouldExit = shouldExit;
        }

        public static GameState InitialState => new GameState(LoadingState.InitialState, GameInput.InitialInput, 0, false);
    }
}
