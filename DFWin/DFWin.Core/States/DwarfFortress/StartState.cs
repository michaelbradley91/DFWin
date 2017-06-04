using DFWin.Core.Inputs.DwarfFortress;

namespace DFWin.Core.States.DwarfFortress
{
    public class StartState : DwarfFortressState<IStartInput>
    {
        public StartState(IStartInput input) : base(input) { }
    }
}
