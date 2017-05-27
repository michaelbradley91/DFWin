using System;
using System.Collections.Generic;
using System.Linq;
using DFWin.Core.Inputs;
using DFWin.Core.States;
using DFWin.Core.Updaters;

namespace DFWin.Core
{
    public interface IUpdateManager
    {
        GameState Update(GameState previousState, GameInput gameInput);
    }

    public class UpdateManager : IUpdateManager
    {
        private readonly ICollection<IUpdater> updaters;

        private readonly Dictionary<Type, IUpdater> updaterByState = new Dictionary<Type, IUpdater>();

        public UpdateManager(IEnumerable<IUpdater> updaters)
        {
            this.updaters = updaters.ToList();
        }

        public GameState Update(GameState previousState, GameInput gameInput)
        {
            return GetCurrentUpdater(previousState).Update(previousState, gameInput);
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
