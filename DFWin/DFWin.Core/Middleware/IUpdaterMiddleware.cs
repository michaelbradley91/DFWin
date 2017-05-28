using System;
using DFWin.Core.States;

namespace DFWin.Core.Middleware
{
    public interface IUpdaterMiddleware
    {
        GameState Update(GameState previousState, Func<GameState, GameState> next);
    }
}
