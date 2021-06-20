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
    }
}