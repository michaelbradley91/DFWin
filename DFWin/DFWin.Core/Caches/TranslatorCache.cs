using System.Collections.Generic;
using DFWin.Core.Inputs;
using DFWin.Core.Models;

namespace DFWin.Core.Caches
{
    public interface ITranslatorCache
    {
        void Cache(Tiles tiles, DwarfFortressInput input);
        bool TryGet(Tiles tiles, out DwarfFortressInput input);
    }

    public class TranslatorCache : ITranslatorCache
    {
        private const int MaximumNumberToCache = 1000;
        private readonly object cacheLock = new object();
        private readonly IDictionary<Tiles, DwarfFortressInput> cache = new Dictionary<Tiles, DwarfFortressInput>();
        private readonly LinkedList<Tiles> cacheHistory = new LinkedList<Tiles>();

        public void Cache(Tiles tiles, DwarfFortressInput input)
        {
            lock (cacheLock)
            {
                cache[tiles] = input;
                cacheHistory.AddFirst(tiles);

                if (cacheHistory.Count <= MaximumNumberToCache) return;

                var tilesToRemove = cacheHistory.Last.Value;
                cacheHistory.RemoveLast();
                cache.Remove(tilesToRemove);
            }
        }

        public bool TryGet(Tiles tiles, out DwarfFortressInput input)
        {
            lock (cacheLock)
            {
                return cache.TryGetValue(tiles, out input);
            }
        }
    }
}
