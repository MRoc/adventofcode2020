using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Puzzle03
{
    // https://adventofcode.com/2020/day/3
    static class Program
    {
        // Puzzle 1: 203
        // Puzzle 2: 3316272960
        static void Main()
        {
            Console.WriteLine($"Puzzle 1: {Puzzle1()}");
            Console.WriteLine($"Puzzle 2: {Puzzle2()}");
        }

        private static long Puzzle1()
        {
            return Count(LoadInput(), 3, 1);
        }

        private static long Puzzle2()
        {
            var grid = LoadInput();
            return new[]
                {
                    Tuple.Create(1, 1),
                    Tuple.Create(3, 1),
                    Tuple.Create(5, 1),
                    Tuple.Create(7, 1),
                    Tuple.Create(1, 2),
                }
                .Select(s => Count(grid, s.Item1, s.Item2))
                .Aggregate(1L, (a, b) => a * b);
        }

        private static string[] LoadInput()
        {
            var assembly = Assembly.GetCallingAssembly();
            using var stream = assembly.GetManifestResourceStream("Puzzle03.input03.txt");
            using var reader = new StreamReader(stream);
            return reader
                .ReadToEnd()
                .Split("\n")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToArray();
        }

        private static int Count(string[] grid, int dX, int dY)
        {
            return Walk(grid, dX, dY).Count(c => c == '#');
        }

        private static IEnumerable<char> Walk(string[] grid, int dX, int dY)
        {
            var width = grid[0].Length;
            var height = grid.Length;

            for (int x = 0, y = 0; y < height; x = (x + dX) % width, y += dY)
            {
                yield return grid[y][x];
            }
        }
    }
}
