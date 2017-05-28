namespace DFWin.Core.States
{
    /// <summary>
    /// Holds information regarding the game input
    /// </summary>
    public class InputState
    {
        public KeyboardState KeyboardState { get; }

        public InputState(KeyboardState keyboardState)
        {
            KeyboardState = keyboardState;
        }

        public static readonly InputState InitialState = new InputState(KeyboardState.InitialState);
    }
}
