using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 133015568
    // Puzzle 2: 16107959
    public static class Day09
    {
        public static long Puzzle1()
        {
            var numbers = Input
                .LoadLines("Puzzles.Input.input09.txt")
                .Select(long.Parse)
                .ToArray();

            const int window = 25;

            return numbers
                .Select((n, i) => (n, i))
                .Skip(window)
                .Single(t => numbers
                    .Skip(t.i - window)
                    .Take(window)
                    .Permutation2()
                    .All(r => r.Sum() != numbers[t.i])).n;
        }

        public static long Puzzle2()
        {
            var numbers = Input
                .LoadLines("Puzzles.Input.input09.txt")
                .Select(long.Parse)
                .ToArray();

            return Enumerable.Range(0, numbers.Length)
                .Select(i => numbers.Skip(i + 1).ToArray())
                .SelectMany(r => r
                    .Select((rr, i) => r.Take(i + 1).ToArray())
                    .Select(rr => (s: rr.Sum(), v: rr.Min() + rr.Max()))
                    .TakeWhile(t => t.s <= 133015568))
                .Single(t => t.s == 133015568 && t.v != 2 * 133015568).v;
        }

        private static IEnumerable<IReadOnlyCollection<T>> Permutation2<T>(this IEnumerable<T> seq)
        {
            var arr = seq.ToArray();
            return arr
                .SelectMany(e0 => arr
                    .SkipWhile(ee => !ee.Equals(e0))
                    .Select(e1 => new[] { e0, e1 })
                    .Where(e2 => !e2[0].Equals(e2[1])));
        }
    }
}
