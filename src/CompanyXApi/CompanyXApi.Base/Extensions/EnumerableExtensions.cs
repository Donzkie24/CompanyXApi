using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyX.Base.Extensions
{
    /// <summary>
    /// Extension methods for IEnumerable
    /// </summary>
    public static class EnumerableExtensions
    {
        public static bool SafeAny<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }

        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            if (list != null)
            {
                foreach (T x in list)
                {
                    action(x);
                }
            }
        }
        public static async Task ForEachAsync<T>(this IEnumerable<T> list, Func<T, Task> func)
        {
            foreach (var value in list)
            {
                await func(value);
            }
        }
        public static int CountIntersect<T>(this IEnumerable<T> collectionA, IEnumerable<T> collectionB)
        {
            HashSet<T> tempA = new HashSet<T>(collectionA);
            int Result = 0;
            foreach (var itemB in collectionB)
            {
                if (tempA.Remove(itemB))
                    Result++;
            }
            return Result;
        }
    }
}
