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
        // Puzzle 1: 838624
        // Puzzle 2: 52764180
        static void Main()
        {
            Console.WriteLine($"Puzzle 1: {Puzzle1()}");
            Console.WriteLine($"Puzzle 2: {Puzzle2()}");
        }

        private static long Puzzle1()
        {
            return LoadInput()
                .Split('\n')
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => int.Parse(l))
                .Distinct()
                .Permutation2()
                .Where(t => t.Sum() == 2020)
                .Select(t => t.Mul())
                .FirstOrDefault();
        }

        private static long Puzzle2()
        {
            return LoadInput()
                .Split('\n')
                .Where(l => !string.IsNullOrWhiteSpace(l))
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

        private static string LoadInput()
        {
            using var stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Puzzle01.input01.txt");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
