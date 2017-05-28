namespace DFWin.Core.Inputs
{
    public class UserInput
    {
        public KeyboardInput KeyboardInput { get; }

        public UserInput(KeyboardInput keyboardInput)
        {
            KeyboardInput = keyboardInput;
        }

        public static UserInput InitialInput => new UserInput(KeyboardInput.InitialInput);
    }
}
