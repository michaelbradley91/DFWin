using System.Collections.Immutable;

namespace DFWin.Core.Inputs
{
    public class UserInput
    {
        public KeyboardInput KeyboardInput { get; }
        public MouseInput MouseInput { get; }

        public UserInput(KeyboardInput keyboardInput, MouseInput mouseInput)
        {
            KeyboardInput = keyboardInput;
            MouseInput = mouseInput;
        }

        public static UserInput InitialInput => new UserInput(KeyboardInput.InitialInput, MouseInput.InitialInput);
    }
}
