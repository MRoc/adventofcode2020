using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 2238
    // Puzzle 2: 560214575859998
    public static class Day13
    {
        public static int Puzzle1()
        {
            var parts = Input
                .Load("Puzzles.Input.input13.txt")
                .Split(new char[] {'\n', ','})
                .Where(l => !string.IsNullOrEmpty(l) && l != "x")
                .Select(int.Parse)
                .ToArray();

            var start = parts
                .First();

            return parts
                .Skip(1)
                .Select(i => (id: i, wait: (start / i + 1) * i - start))
                .Select(t => (t.wait, result: t.id * t.wait))
                .MinBy(t => t.wait)
                .result;
        }

        public static long Puzzle2()
        {
            var parts = Input
                .Load("Puzzles.Input.input13.txt")
                .Split(new char[] { '\n', ',' })
                .Where(l => !string.IsNullOrEmpty(l))
                .Select(l => l == "x" ? "0" : l)
                .Skip(1)
                .Select(long.Parse)
                .Select((n, i) => (i: i, n: n))
                .Where(t => t.n != 0)
                .ToArray();

            // Disclaimer: I searched for help to come up with this solution as I am pretty bad at number theory.

            var step = parts[0].n;
            var t = 0L;

            foreach (var (i, n) in parts.Skip(1))
            {
                while ((t + i) % n != 0)
                {
                    t += step;
                }

                step *= n;
            }

            return t;
        }


        public static T0 MinBy<T0, T1>(this IEnumerable<T0> seq, Func<T0, T1> projection)
        {
            var comparer = Comparer<T1>.Default;

            var candidate = seq.First();
            var candidateMin = projection(candidate);

            foreach (var item in seq)
            {
                var m = projection(item);

                if (comparer.Compare(m, candidateMin) < 0)
                {
                    candidate = item;
                    candidateMin = m;
                }
            }

            return candidate;
        }
    }
}