using Microsoft.Xna.Framework.Input;

namespace DFWin.Core.Inputs
{
    public class UserInput
    {
        public KeyboardState Keyboard { get; }

        public UserInput(KeyboardState keyboard)
        {
            Keyboard = keyboard;
        }

        public static UserInput InitialInput => new UserInput(Microsoft.Xna.Framework.Input.Keyboard.GetState());
    }
}
