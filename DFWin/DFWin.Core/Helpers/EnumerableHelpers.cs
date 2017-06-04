using System;
using System.Collections.Generic;
using System.Linq;

namespace DFWin.Core.Helpers
{
    public static class EnumerableHelpers
    {
        public static int IndexOf<T>(this IEnumerable<T> elements, Func<T, bool> predicate)
        {
            return elements.Select((e, i) => (e, i))
                .SkipWhile(t => !predicate(t.Item1))
                .DefaultIfEmpty((default(T), -1))
                .First().Item2;
        }
    }
}
