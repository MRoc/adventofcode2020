using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 203
    // Puzzle 2: 3316272960
    public static class Day03
    {
        public static long Puzzle1()
        {
            return Count(Input.LoadLines("Puzzles.Input.input03.txt"), 3, 1);
        }

        public static long Puzzle2()
        {
            var grid = Input.LoadLines("Puzzles.Input.input03.txt");
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
