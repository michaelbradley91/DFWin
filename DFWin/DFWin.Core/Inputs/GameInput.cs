namespace DFWin.Core.Inputs
{
    /// <summary>
    /// All input that contributes to state changes. This includes user input, like the mouse and keyboard,
    /// and the Dwarf Fortress process we run against as examples.
    /// </summary>
    public class GameInput
    {
        public DwarfFortressInput DwarfFortressInput { get; }
        public UserInput UserInput { get; }

        public GameInput(DwarfFortressInput dwarfFortressInput, UserInput userInput)
        {
            DwarfFortressInput = dwarfFortressInput;
            UserInput = userInput;
        }
    }
}
