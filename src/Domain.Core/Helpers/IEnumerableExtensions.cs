using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Core
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, bool when, Func<TSource, bool> predicate)
        {
            if (when)
                return source.Where(predicate);

            return source;
        }

        public static IEnumerable<TSource> Add<TSource>(this IEnumerable<TSource> source, TSource item)
        {
            return source.Concat(new[] { item });
        }
    }
}