namespace DFWin.Core.States
{
    /// <summary>
    /// The root of the state used to determine what should be shown on screen.
    /// The state itself should be immutable and updated during each game loop.
    /// </summary>
    public class GameState
    {
        public IScreenState ScreenState { get; }
        public InputState InputState { get; }
        public int FrameRate { get; }
        public bool ShouldExit { get; }

        public GameState(IScreenState screenState, InputState inputState, int frameRate, bool shouldExit)
        {
            ScreenState = screenState;
            InputState = inputState;
            FrameRate = frameRate;
            ShouldExit = shouldExit;
        }

        public static GameState InitialState => new GameState(LoadingState.InitialState, InputState.InitialState, 0, false);
    }
}
