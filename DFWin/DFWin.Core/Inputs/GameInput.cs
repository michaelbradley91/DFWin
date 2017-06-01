using DFWin.Core.Inputs.DwarfFortress;

namespace DFWin.Core.Inputs
{
    /// <summary>
    /// All input that contributes to state changes. This includes user input, like the mouse and keyboard,
    /// and the Dwarf Fortress process we run against as examples.
    /// </summary>
    public class GameInput
    {
        public IDwarfFortressInput DwarfFortressInput { get; }
        public UserInput UserInput { get; }
        public WarmUpInput WarmUpInput { get; }

        public GameInput(IDwarfFortressInput dwarfFortressInput, UserInput userInput, WarmUpInput warmUpInput)
        {
            DwarfFortressInput = dwarfFortressInput;
            UserInput = userInput;
            WarmUpInput = warmUpInput;
        }

        public static GameInput InitialInput => new GameInput(DwarfFortress.DwarfFortressInput.None, UserInput.InitialInput, WarmUpInput.None);
    }
}
