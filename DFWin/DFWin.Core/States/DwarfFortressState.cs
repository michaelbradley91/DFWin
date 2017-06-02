using DFWin.Core.Inputs.DwarfFortress;

namespace DFWin.Core.States
{
    /// <summary>
    /// All states dependent on the Dwarf Fortress screen should extend this class.
    /// In particular, all states must support a constructor taking just the input so the screen can initialise from any point.
    /// </summary>
    public class DwarfFortressState<TDwarfFortressInput>
        where TDwarfFortressInput : IDwarfFortressInput
    {
        public TDwarfFortressInput Input;

        public DwarfFortressState(TDwarfFortressInput input)
        {
            Input = input;
        }
    }
}
