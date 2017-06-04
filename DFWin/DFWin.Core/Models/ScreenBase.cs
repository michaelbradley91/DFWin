using DFWin.Core.States;

namespace DFWin.Core.Models
{
    /// <summary>
    /// By convention, every IScreenState  should correspond to a screen with the same name but state removed, suffixed by screen.
    /// So, LoadingState should have a corresponding LoadingScreen.
    /// </summary>
    public interface IScreen
    {
        void Draw(GameState gameState, ScreenTools screenTools);
    }

    public abstract class ScreenBase<TScreenState> : IScreen
        where TScreenState : IScreenState
    {
        public void Draw(GameState gameState, ScreenTools screenTools)
        {
            Draw((TScreenState)gameState.ScreenState, screenTools);
        }

        public abstract void Draw(TScreenState state, ScreenTools screenTools);
    }
}
