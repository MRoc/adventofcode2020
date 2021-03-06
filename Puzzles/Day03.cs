﻿using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 203
    // Puzzle 2: 3316272960
    public static class Day03
    {
        public static long Puzzle1()
        {
            return Input
                .LoadLines("Puzzles.Input.input03.txt")
                .Count(3, 1);
        }

        public static long Puzzle2()
        {
            return new[]
                {
                    (1, 1),
                    (3, 1),
                    (5, 1),
                    (7, 1),
                    (1, 2)
                }
                .Select(s => Input
                    .LoadLines("Puzzles.Input.input03.txt")
                    .Count(s.Item1, s.Item2))
                .Aggregate(1L, (a, b) => a * b);
        }

        private static int Count(this IReadOnlyList<string> grid, int dX, int dY)
        {
            return grid.Walk(dX, dY).Count(c => c == '#');
        }

        private static IEnumerable<char> Walk(this IReadOnlyList<string> grid, int dX, int dY)
        {
            var width = grid[0].Length;
            var height = grid.Count;

            for (int x = 0, y = 0; y < height; x = (x + dX) % width, y += dY)
            {
                yield return grid[y][x];
            }
        }
    }
}
