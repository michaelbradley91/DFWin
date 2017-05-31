using System;
using System.Collections.Generic;
using System.Linq;
using DFWin.Core.Middleware;
using DFWin.Core.Services;
using DFWin.Core.States;
using DFWin.Core.Updaters;

namespace DFWin.Core
{
    public interface IUpdateManager
    {
        GameState Update(GameState previousState);
    }

    public class UpdateManager : IUpdateManager
    {
        private readonly IInputService inputService;
        private readonly IReadOnlyList<IUpdaterMiddleware> middleware;
        private readonly ICollection<IUpdater> updaters;

        private readonly Dictionary<Type, IUpdater> updaterByState = new Dictionary<Type, IUpdater>();

        public UpdateManager(IInputService inputService, IEnumerable<IUpdaterMiddleware> middleware, IEnumerable<IUpdater> updaters)
        {
            this.inputService = inputService;
            this.middleware = middleware.ToList();
            this.updaters = updaters.ToList();
        }

        public GameState Update(GameState previousState)
        {
            return UpdateWithMiddleware(inputService.UpdateInput(previousState), middleware.GetEnumerator());
        }

        private GameState UpdateWithMiddleware(GameState previousState, IEnumerator<IUpdaterMiddleware> remainingMiddleware)
        {
            return !remainingMiddleware.MoveNext() ? GetCurrentUpdater(previousState).Update(previousState) :
                remainingMiddleware.Current.Update(previousState, g => UpdateWithMiddleware(g, remainingMiddleware));
        }

        private IUpdater GetCurrentUpdater(GameState gameState)
        {
            var success = updaterByState.TryGetValue(gameState.ScreenState.GetType(), out IUpdater updater);
            if (success) return updater;

            var updaterName = GetUpdaterName(gameState.ScreenState);
            updater = updaters.Single(s => s.GetType().Name == updaterName);

            updaterByState[gameState.ScreenState.GetType()] = updater;

            return updater;
        }

        private static string GetUpdaterName(IScreenState screenState)
        {
            var stateName = screenState.GetType().Name;
            return stateName.Substring(0, stateName.Length - "State".Length) + "Updater";
        }
    }
}
