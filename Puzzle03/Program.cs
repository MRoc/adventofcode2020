using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Puzzle03
{
    static class Program
    {
        // 1/1: Found 68 trees(1)
        // 3/1: Found 203 trees(1)
        // 5/1: Found 78 trees(1)
        // 7/1: Found 77 trees(1)
        // 1/2: Found 40 trees(1)
        // Result: 3316272960
        static void Main()
        {
            var result = MultiplyNumberOfTreesOnDifferentSlopes(SplitInput(LoadInput()));
            Console.WriteLine($"Result: {result}");
        }

        private static string LoadInput()
        {
            var assembly = Assembly.GetCallingAssembly();
            using var stream = assembly.GetManifestResourceStream("Puzzle03.input_maze.txt");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        private static string[] SplitInput(string input)
        {
            return input
                .Split("\n")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToArray();
        }

        private static long MultiplyNumberOfTreesOnDifferentSlopes(string[] grid)
        {
            return new[]
                {
                    Tuple.Create(1, 1),
                    Tuple.Create(3, 1),
                    Tuple.Create(5, 1),
                    Tuple.Create(7, 1),
                    Tuple.Create(1, 2),
                }
                .Select(s => Count(grid, s.Item1, s.Item2))
                .Aggregate(1l, (a, b) => a * b);
        }

        private static int Count(string[] grid, int dX, int dY)
        {
            var result = Walk(grid, dX, dY).Count(c => c == '#');
            Console.WriteLine($"{dX}/{dY}: Found {result} trees (1)");
            return result;
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
