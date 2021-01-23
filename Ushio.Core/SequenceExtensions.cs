using System;
using System.Collections.Generic;
using System.Linq;

namespace Ushio.Core
{
    public static class SequenceExtensions
    {
        public static T Random<T>(this IEnumerable<T> items)
        {
            return NumberGeneration<T>.Random(items);
        }
    }

    public static class NumberGeneration<T>
    {
        private static Random rnd;

        static NumberGeneration()
        {
            rnd = new Random();
        }

        public static T Random(IEnumerable<T> items)
        {
            return items.ElementAt(rnd.Next(items.Count()));
        }
    }
}
