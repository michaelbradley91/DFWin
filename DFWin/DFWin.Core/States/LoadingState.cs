using DFWin.Core.Constants;

namespace DFWin.Core.States
{
    public class LoadingState : IScreenState
    {
        public LoadingPhase Phase { get; }
        public int LoadingPercent { get; }
        public string Message { get; }

        public LoadingState(int loadingPercent, string message, LoadingPhase phase)
        {
            LoadingPercent = loadingPercent;
            Message = message;
            Phase = phase;
        }

        public static LoadingState InitialState => new LoadingState(0, "Warming up...", LoadingPhase.WaitingForDwarfFortressToStart);
    }
}
