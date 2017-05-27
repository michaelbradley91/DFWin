using DFWin.Core.States;

namespace DFWin.Core.Models
{
    /// <summary>
    /// By convention, every IScreenState  should correspond to a screen with the same name but state removed, suffixed by screen.
    /// So, LoadingState should have a corresponding LoadingScreen.
    /// </summary>
    public interface IScreen
    {
        void Draw(ScreenTools screenTools, GameState gameState);
    }

    public abstract class Screen<TScreenState> : IScreen
        where TScreenState : IScreenState
    {
        public void Draw(ScreenTools screenTools, GameState gameState)
        {
            Draw(screenTools, (TScreenState)gameState.ScreenState);
        }

        public abstract void Draw(ScreenTools screenTools, TScreenState gameState);
    }
}
