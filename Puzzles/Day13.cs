using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;

namespace Puzzles
{
    // Puzzle 1: 2238
    // Puzzle 2: 0
    public static class Day13
    {
        public static int Puzzle1()
        {
            var parts = Input
                .Load("Puzzles.Input.input13.txt")
                .Split('\n', ',')
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
//            var parts = Input
//                .Load("Puzzles.Input.input13.txt")
//                .Split('\n', ',')
//                .Where(l => !string.IsNullOrEmpty(l))
//                .Select(l => l == "x" ? "0" : l)
//                .Skip(1)
//                .Select(long.Parse)
//                .Select((n, i) => (number: n, mod: n - i))
//                .Where(t => t.number != 0)
//                .ToArray();

//            var numbers = parts.Select(p => p.number).ToArray();
//            var modulos = parts.Select(p => p.mod).ToArray();

//            var step = numbers[0];

//            var t = 0L;

//            while (true)
//            {
//                for (int i = 1; i < parts.Length; ++i)
//                {
//                    if (t % numbers[i] != modulos[i])
//                    {
//                        goto Label;
//                    }
//                }

//                return t;

//Label:

//                t += step;

//            }

            return 0;
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