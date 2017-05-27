using System;
using DFWin.Core.Inputs;
using DFWin.Core.States;

namespace DFWin.Core.Middleware
{
    public interface IUpdaterMiddleware
    {
        GameState Update(GameState previousState, GameInput gameInput, Func<GameState, GameInput, GameState> next);
    }
}
