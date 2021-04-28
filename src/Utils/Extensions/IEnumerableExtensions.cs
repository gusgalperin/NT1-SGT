using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void Add<T>(this IEnumerable<T> source, T item)
        {
            source = source.Concat(new[] { item });
        }

        public static void Add<T>(this IEnumerable<T> source, IEnumerable<T> items)
        {
            source = source.Concat(items);
        }
    }
}
