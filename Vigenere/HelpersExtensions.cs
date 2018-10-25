using System;
using System.Collections.Generic;
using System.Text;

namespace Vigenere
{
    using System.Linq;

    public static class HelpersExtensions
    {
        private static readonly Random Random = new Random();

        public static T GetRandomElement<T>(this IList<T> list)
        {
            var idx = Random.Next() % list.Count;

            return list[idx];
        }

        public static IEnumerable<T> GetRandomElements<T>(this IList<T> list, int count)
        {
            return Enumerable.Range(0, count).Select(x => GetRandomElement(list));
        }

        public static string Sum(this IEnumerable<char> chars)
        {
            return new string(chars.ToArray());
        }

        public static T Mode<T>(this IEnumerable<T> sequence)
        {
            return sequence
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .First();
        }
    }
}
