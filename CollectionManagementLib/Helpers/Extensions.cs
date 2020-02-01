using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollectionManagementLib.Helpers
{
    public static class Extensions
    {
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> filter)
        {
            return source.SelectMany(i => filter(i));
        }

    }
}
