using System;
using System.Diagnostics;
using DFWin.Core.Inputs;
using DFWin.Core.States;

namespace DFWin.Core.Middleware
{
    public class FrameRateMiddleware : IUpdaterMiddleware
    {
        private readonly Stopwatch stopwatch;
        private int numberOfFrames;

        public FrameRateMiddleware()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public GameState Update(GameState previousState, GameInput gameInput, Func<GameState, GameInput, GameState> next)
        {
            return next(UpdateWithFrameRate(previousState), gameInput);
        }

        private GameState UpdateWithFrameRate(GameState previousState)
        {
            if (stopwatch.ElapsedMilliseconds > 1000)
            {
                stopwatch.Restart();
                var nextState = new GameState(previousState.ScreenState, previousState.GameInput, numberOfFrames, previousState.ShouldExit);
                numberOfFrames = 0;
                return nextState;
            }
            numberOfFrames++;
            return previousState;
        }
    }
}