using DFWin.Core.States;
using DFWin.Models;

namespace DFWin.Screens
{
    /// <summary>
    /// By convention, every IScreenState  should correspond to a screen with the same name but state removed, suffixed by screen.
    /// So, LoadingState should have a corresponding LoadingScreen.
    /// </summary>
    public interface IScreen
    {
        void Draw(ScreenTools screenTools, IScreenState screenState);
    }

    public abstract class Screen<TScreenState> : IScreen
        where TScreenState : IScreenState
    {
        public void Draw(ScreenTools screenTools, IScreenState screenState)
        {
            Draw(screenTools, (TScreenState)screenState);
        }

        public abstract void Draw(ScreenTools screenTools, TScreenState gameState);
    }
}
