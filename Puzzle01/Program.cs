using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Puzzle01
{
    // https://adventofcode.com/2020/day/1
    static class Program
    {
        // 1436 + 584       = 2020 -> 1436 * 584       = 838624
        // 245 + 131 + 1644 = 2020 -> 245 * 131 * 1644 = 52764180
        static void Main()
        {
            var numbers = LoadInput()
                .Split('\n')
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => int.Parse(l))
                .Distinct()
                .ToArray();

            foreach (var t in numbers
                .Permutation2()
                .Where(t => t.Sum() == 2020))
            {
                Console.WriteLine($"{t[0]} + {t[1]} = 2020 -> {t[0]} * {t[1]} = {t.Mul()}");
            }

            foreach (var t in numbers
                .Permutation3()
                .Where(t => t.Sum() == 2020))
            {
                Console.WriteLine($"{t[0]} + {t[1]} + {t[2]} = 2020 -> {t[0]} * {t[1]} * {t[2]} = {t.Mul()}");
            }
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

        private static string LoadInput()
        {
            using var stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Puzzle01.input.txt");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
