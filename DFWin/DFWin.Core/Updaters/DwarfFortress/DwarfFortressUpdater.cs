using DFWin.Core.Helpers;
using DFWin.Core.Inputs;
using DFWin.Core.Inputs.DwarfFortress;
using DFWin.Core.Services;
using DFWin.Core.States;

namespace DFWin.Core.Updaters.DwarfFortress
{
    /// <summary>
    /// Any updater reliant on the Dwarf Fortress screen should extend this updater.
    /// This updater will automatically return the correct state for the input if the screen changes.
    /// </summary>
    public abstract class DwarfFortressUpdater<TScreenState, TDwarfFortressInput> : Updater<TScreenState>
        where TScreenState : IScreenState
        where TDwarfFortressInput : IDwarfFortressInput
    {
        private readonly string name;

        protected IDwarfFortressInputService DwarfFortressInputService { get; } = DfWin.Resolve<IDwarfFortressInputService>();

        protected DwarfFortressUpdater()
        {
            name = GetType().Name;
        }

        protected override IScreenState Update(TScreenState previousState, GameInput gameInput)
        {
            var inputName = gameInput.DwarfFortressInput.GetType().Name;
            var targetUpdaterName = GetNameOfDwarfFortressUpdater(inputName);

            return targetUpdaterName == name
                ? Update(previousState, (TDwarfFortressInput)gameInput.DwarfFortressInput, gameInput.UserInput) 
                : StateHelpers.CreateInitialScreenState(gameInput.DwarfFortressInput);
        }

        private static string GetNameOfDwarfFortressUpdater(string inputName)
        {
            return inputName.Substring(0, inputName.Length - "Input".Length) + "Updater";
        }

        protected abstract IScreenState Update(TScreenState previousState, TDwarfFortressInput dwarfFortressInput, UserInput userInput);
    }
}
