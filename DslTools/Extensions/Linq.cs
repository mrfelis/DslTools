using System;
using System.Collections.Generic;
using System.Linq;

namespace DslTools.Extensions
{
    public static class Linq
    {
        /// <summary>
        /// Wraps the elements of an enumerable within a <see cref="KeyValuePair{TKey, TValue}"/>
        /// where the <see cref="KeyValuePair{TKey, TValue}.Key"/> is the 0 based index of the element
        /// within the enumerable (the starting index cna be overridden) and the <see cref="KeyValuePair{TKey, TValue}.Value"/> is the
        /// actual element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The elements to be wrapped</param>
        /// <param name="start">Optional starting index for Keys of the returned enumerable</param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<int, T>> WithIndex<T>(this IEnumerable<T> items, int start = 0)
        {
            return items.Select(i => new KeyValuePair<int, T>(start++, i));
        }

        /// <summary>
        /// Zips two enumerables together using a selector function to combine the elements of the two
        /// enumerables. All elements of both enumerables will be returned. If one enumerable contains
        /// less elements than the other, the default value of that type will be provided to the 
        /// selector function.
        /// </summary>
        /// <typeparam name="TFirst">the type of the first enumerable</typeparam>
        /// <typeparam name="TSecond">the type of the second enumerable </typeparam>
        /// <typeparam name="TResult">the type returned by the selector function</typeparam>
        /// <param name="first">an enumerable</param>
        /// <param name="second">another enumerable</param>
        /// <param name="resultSelector"></param>
        /// <returns>Returns an enumerable whose first element is a combination 
        /// of the first elements of the both en umerables. The second is a 
        /// combination of the second elements. Etc.</returns>
        public static IEnumerable<TResult> ZipAll<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            var a = first.GetEnumerator();
            var b = second.GetEnumerator();

            var a1 = false;
            var b1 = false;

            bool done()
            {
                a1 = a.MoveNext();
                b1 = b.MoveNext();
                return a1 || b1;
            }

            while (done())
            {
                var r = resultSelector(a1 ? a.Current : default(TFirst), b1 ? b.Current : default(TSecond));
                yield return r;
            }
        }
    }
}