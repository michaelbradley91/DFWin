namespace DFWin.Core.States
{
    public class LoadingState : IScreenState
    {
        public int LoadingPercent { get; }
        public string Message { get; }

        public LoadingState(int loadingPercent, string message)
        {
            LoadingPercent = loadingPercent;
            Message = message;
        }
        
        public static LoadingState InitialState => new LoadingState(0, "Loading...");
    }
}
