using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Nuntium.Core
{
    public static class CollectionsExtentions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var cur in enumerable)
            {
                action(cur);
            }
        }

       
    }
}
