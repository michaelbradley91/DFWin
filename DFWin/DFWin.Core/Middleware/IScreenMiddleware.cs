using System;
using DFWin.Core.Models;
using DFWin.Core.States;

namespace DFWin.Core.Middleware
{
    public interface IScreenMiddleware
    {
        void Draw(GameState gameState, ScreenTools screenTools, Action<GameState, ScreenTools> next);
    }
}
