using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 1625
    // Puzzle 2: 3100448333024
    public static class Day10
    {
        public static long Puzzle1()
        {
            return Input
                .LoadLines("Puzzles.Input.input10.txt")
                .Select(uint.Parse)
                .Append(s => new [] { 0u, s.Max() + 3u})
                .OrderBy(n => n)
                .SelectWithPrevious((a, b) => b - a)
                .GroupBy(d => d)
                .Select(g => g.Count())
                .Aggregate((a, b) => a * b);
        }

        public static ulong Puzzle2()
        {
            var input = Input
                .LoadLines("Puzzles.Input.input10.txt")
                .Select(uint.Parse)
                .Append(s => new[] { 0u, s.Max() + 3u })
                .OrderBy(n => n)
                .ToArray();

            var paths = new ulong[input.Length];
            paths[0] = 1;

            for (var i = 0; i < input.Length; ++i)
            {
                for (var j = Math.Max(0, i - 3); j < i; ++j)
                {
                    if (input[i] - input[j] <= 3)
                    {
                        paths[i] += paths[j];
                    }
                }
            }

            return paths.Last();
        }

        private static IEnumerable<T> Append<T>(this IEnumerable<T> seq, Func<IEnumerable<T>, IEnumerable<T>> value)
        {
            var arr = seq.ToArray();
            return arr.Concat(value(arr));
        }

        private static IEnumerable<T> SelectWithPrevious<T>(this IEnumerable<T> seq, Func<T, T, T> projection)
        {
            using var iterator = seq.GetEnumerator();
            
            if (iterator.MoveNext())
            {
                T previous = iterator.Current;
                while (iterator.MoveNext())
                {
                    yield return projection(previous, iterator.Current);
                    previous = iterator.Current;
                }
            }
        }
    }
}
