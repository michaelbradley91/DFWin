using DFWin.Core;
using DFWin.Core.Models;
using DFWin.Core.Screens;
using DFWin.Core.States;

namespace DFWin.Screens
{
    public abstract class Screen<TScreenState> : ScreenBase<TScreenState>
        where TScreenState : IScreenState
    {
        protected ContentManager ContentManager { get; } = DfWin.Resolve<ContentManager>();

        public abstract override void Draw(TScreenState state, ScreenTools screenTools);
    }
}
