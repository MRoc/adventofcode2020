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
        // 245+131+1644=2020 -> 245*131*1644=52764180
        // 245+1644+131=2020 -> 245*1644*131=52764180
        // 131+245+1644=2020 -> 131*245*1644=52764180
        // 131+1644+245=2020 -> 131*1644*245=52764180
        // 1644+245+131=2020 -> 1644*245*131=52764180
        // 1644+131+245=2020 -> 1644*131*245=52764180
        static void Main()
        {
            foreach (var t in LoadInput()
                .Split('\n')
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => int.Parse(l))
                .Distinct()
                .Permutate()
                .Where(t => t.Item1 + t.Item2 + t.Item3 == 2020))
            {
                Console.WriteLine($"{t.Item1}+{t.Item2}+{t.Item3}=2020 -> {t.Item1}*{t.Item2}*{t.Item3}={t.Item1 * t.Item2 * t.Item3}");
            }
        }

        private static IEnumerable<Tuple<T, T, T>> Permutate<T>(this IEnumerable<T> seq)
        {
            return seq.SelectMany(e0 => seq.SelectMany(e1 => seq.Select(e2 => Tuple.Create(e0, e1, e2))));
        }

        private static string LoadInput()
        {
            using var stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Puzzle01.input.txt");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
