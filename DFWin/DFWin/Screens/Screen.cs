using DFWin.Core.State;
using DFWin.Models;

namespace DFWin.Screens
{
    public interface IScreen
    {
        void Draw(ScreenTools screenTools, ScreenState screenState);
    }

    public abstract class Screen<TScreenState> : IScreen
        where TScreenState : ScreenState
    {
        public void Draw(ScreenTools screenTools, ScreenState screenState)
        {
            Draw(screenTools, (TScreenState)screenState);
        }

        public abstract void Draw(ScreenTools screenTools, TScreenState gameState);
    }
}
