using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Puzzle06
{
    // https://adventofcode.com/2020/day/6
    static class Program
    {
        static void Main()
        {
            // Example 1: 11
            // Puzzle 1.: 6662
            // Example 2: 6
            // Puzzle 2.: 3382

            Console.WriteLine($"Example 1: {CountAny("Puzzle06.example.txt")}");
            Console.WriteLine($"Puzzle 1.: {CountAny("Puzzle06.input.txt")}");
            Console.WriteLine($"Example 2: {CountAll("Puzzle06.example.txt")}");
            Console.WriteLine($"Puzzle 2.: {CountAll("Puzzle06.input.txt")}");
        }

        private static int CountAny(string resourceName)
        {
            return LoadInput(resourceName)
                .Split("\n\n")
                .Select(s => s
                    .Split('\n')
                    .Where(e => !string.IsNullOrEmpty(e))
                    .Select(e => e.ToArray())
                    .Aggregate((a, b) => a.Union(b).ToArray())
                    .Length)
                .Sum();
        }

        private static int CountAll(string resourceName)
        {
            return LoadInput(resourceName)
                .Split("\n\n")
                .Select(s => s
                    .Split('\n')
                    .Where(e => !string.IsNullOrEmpty(e))
                    .Select(e => e.ToArray())
                    .Aggregate((a, b) => a.Intersect(b).ToArray())
                    .Length)
                .Sum();
        }

        private static string LoadInput(string name)
        {
            using var stream = Assembly.GetCallingAssembly().GetManifestResourceStream(name);
            if (stream is null)
            {
                throw new InvalidOperationException($"Could not load resource {name}!");
            }

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd().Replace("\r", string.Empty);
        }
    }
}
