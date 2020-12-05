using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Puzzle03
{
    class Program
    {
        static void Main(string[] _)
        {
            var grid = SplitMaze(LoadInput());

            var result = MultiplyNumberOfTreesOnDifferentSlopes(grid);

            Console.WriteLine($"Result: {result}");
        }

        private static string LoadInput()
        {
            var assembly = Assembly.GetCallingAssembly();
            using var stream = assembly.GetManifestResourceStream("Puzzle03.input_maze.txt");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        private static char[][] SplitMaze(string input)
        {
            return input
                .Split("\n")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => l.ToArray())
                .ToArray();
        }

        private static long MultiplyNumberOfTreesOnDifferentSlopes(char[][] grid)
        {
            long result = 1;

            foreach (var s in _slopes)
            {
                var trees = CountTrees(grid, s.Item1, s.Item2);
                Console.WriteLine($"{s.Item1}/{s.Item2}: Found {trees} trees (1)");

                result *= trees;
            }

            return result;
        }

        private static int CountTrees(char[][] grid, int dX, int dY)
        {
            var width = grid[0].Length;
            var height = grid.Length;

            var count = 0;

            for (int x = 0, y = 0;  y < height; x = (x + dX) % width, y += dY)
            {
                if (grid[y][x] == '#')
                {
                    count++;
                }
            }

            return count;
        }

        private static readonly Tuple<int, int>[] _slopes = new Tuple<int, int>[]
        {
            Tuple.Create(1, 1),
            Tuple.Create(3, 1),
            Tuple.Create(5, 1),
            Tuple.Create(7, 1),
            Tuple.Create(1, 2),
        };
    }
}
