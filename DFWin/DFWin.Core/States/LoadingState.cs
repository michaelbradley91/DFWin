using DFWin.Core.Constants;

namespace DFWin.Core.States
{
    public class LoadingState : IScreenState
    {
        public LoadingPhase Phase { get; }
        public int LoadingPercent { get; }

        public LoadingState(int loadingPercent, LoadingPhase phase)
        {
            LoadingPercent = loadingPercent;
            Phase = phase;
        }

        public static LoadingState InitialState => new LoadingState(0, LoadingPhase.WaitingForDwarfFortressToStart);
    }
}
