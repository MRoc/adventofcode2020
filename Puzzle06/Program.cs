using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Puzzle06
{
    // https://adventofcode.com/2020/day/6
    static class Program
    {
        // Puzzle 1: 6662
        // Puzzle 2: 3382
        static void Main()
        {
            Console.WriteLine($"Puzzle 1: {Puzzle1()}");
            Console.WriteLine($"Puzzle 2: {Puzzle2()}");
        }

        private static int Puzzle1()
        {
            return LoadInput()
                .Split("\n\n")
                .Select(s => s
                    .Split('\n')
                    .Where(e => !string.IsNullOrEmpty(e))
                    .Select(e => e.ToArray())
                    .Aggregate((a, b) => a.Union(b).ToArray())
                    .Length)
                .Sum();
        }

        private static int Puzzle2()
        {
            return LoadInput()
                .Split("\n\n")
                .Select(s => s
                    .Split('\n')
                    .Where(e => !string.IsNullOrEmpty(e))
                    .Select(e => e.ToArray())
                    .Aggregate((a, b) => a.Intersect(b).ToArray())
                    .Length)
                .Sum();
        }

        private static string LoadInput()
        {
            using var stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Puzzle06.input.txt");
            if (stream is null)
            {
                throw new InvalidOperationException($"Could not load resource!");
            }

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd().Replace("\r", string.Empty);
        }
    }
}
