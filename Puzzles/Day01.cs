using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 838624
    // Puzzle 2: 52764180
    public static class Day01
    {
        public static long Puzzle1()
        {
            return Input.LoadLines("Puzzles.Input.input01.txt")
                .Select(l => int.Parse(l))
                .Distinct()
                .Permutation2()
                .Where(t => t.Sum() == 2020)
                .Select(t => t.Mul())
                .FirstOrDefault();
        }

        public static long Puzzle2()
        {
            return Input.LoadLines("Puzzles.Input.input01.txt")
                .Select(l => int.Parse(l))
                .Distinct()
                .Permutation3()
                .Where(t => t.Sum() == 2020)
                .Select(t => t.Mul())
                .FirstOrDefault();
        }

        private static IEnumerable<T[]> Permutation2<T>(this IEnumerable<T> seq)
        {
            var arr = seq.ToArray();
            return arr
                .SelectMany(e0 => arr.SkipWhile(ee => !ee.Equals(e0))
                    .Select(e1 => new[] { e0, e1 }));
        }

        private static IEnumerable<T[]> Permutation3<T>(this IEnumerable<T> seq)
        {
            var arr = seq.ToArray();
            return arr
                .SelectMany(e0 => arr.SkipWhile(ee => !ee.Equals(e0))
                    .SelectMany(e1 => arr.SkipWhile(eee => !eee.Equals(e1))
                        .Select(e2 => new[] { e0, e1, e2 })));
        }

        private static long Mul(this IEnumerable<int> seq)
        {
            return seq.Aggregate(1L, (a, b) => a * b);
        }
    }
}
