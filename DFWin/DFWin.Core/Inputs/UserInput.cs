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
    }
}
