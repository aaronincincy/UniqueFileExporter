using System;
using System.Collections.Generic;
using System.Linq;

namespace UniqueFileExporter
{
    public static class EnumerableExtensions
    {
        public static Dictionary<TKey, TValue> ToDictionaryWithUniqueValues<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, int, TValue> valueSelector)
        {
            var uniqueItems = new HashSet<TValue>();

            return source.ToDictionary(keySelector, item =>
            {
                var count = 0;

                while (true)
                {
                    var potentialValue = valueSelector(item, count++);
                    if (!uniqueItems.Contains(potentialValue))
                    {
                        uniqueItems.Add(potentialValue);
                        return potentialValue;
                    }
                }
            });
        }
    }
}
