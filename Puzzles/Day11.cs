using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 2368
    // Puzzle 2: 2124
    public static class Day11
    {
        public static long Puzzle1()
        {
            return Input
                .LoadLines("Puzzles.Input.input11.txt")
                .CreateSeats()
                .FindStableState(AdjacentSeats1, 4)
                .Cast<char>()
                .Count(c => c == '#');
        }

        public static long Puzzle2()
        {
            return Input
                .LoadLines("Puzzles.Input.input11.txt")
                .CreateSeats()
                .FindStableState(AdjacentSeats2, 5)
                .Cast<char>()
                .Count(c => c == '#');
        }

        private static char[,] CreateSeats(this IReadOnlyList<string> input)
        {
            var result = new char[input.Count, input[0].Length];

            foreach (var (i, j) in Enumerable.Range(0, input.Count)
                .SelectMany(ii => Enumerable.Range(0, input[ii].Length)
                    .Select(jj => (i: ii, j: jj))))
            {
                result[i, j] = input[i][j];
            }
            return result;
        }

        private static char[,] FindStableState(
            this char[,] grid,
            Func<char[,], int, int, int> adjacentSeats,
            int numSeatsToFree)
        {
            var next = grid;
            var prev = default(char[,]);

            while (prev is null || !prev.AreEqual(next))
            {
                prev = next;
                next = NextState(next, adjacentSeats, numSeatsToFree);
            }

            return next;
        }

        private static char[,] NextState(
            this char[,] grid,
            Func<char[,], int, int, int> adjacentSeats,
            int numSeatsToFree)
        {
            var height = grid.GetLength(0);
            var width = grid.GetLength(1);

            var nextState = new char[height, width];

            foreach (var (i, j) in Enumerable.Range(0, height)
                .SelectMany(ii => Enumerable.Range(0, width)
                    .Select(jj => (i: ii, j: jj))))
            {
                nextState[i, j] = grid.NextState(i, j, adjacentSeats, numSeatsToFree);
            }

            return nextState;
        }

        private static char NextState(
            this char[,] grid,
            int i,
            int j,
            Func<char[,], int, int, int> adjacentSeats,
            int numSeatsToFree)
        {
            var seat = grid[i, j];
            var adjacentAllocatedSeats = adjacentSeats(grid, i, j);

            if (seat == 'L' && adjacentAllocatedSeats == 0)
            {
                return '#';
            }

            if (seat == '#' && adjacentAllocatedSeats >= numSeatsToFree)
            {
                return 'L';
            }

            return seat;
        }

        private static int AdjacentSeats1(this char[,] seats, int row, int col)
        {
            var height = seats.GetLength(0);
            var width = seats.GetLength(1);

            var count = 0;

            foreach (var direction in Directions)
            {
                var r = row + direction.Item1;
                var c = col + direction.Item2;

                if (r >= 0 && r < height && c >= 0 && c < width && seats[r, c] == '#')
                {
                    ++count;
                }
            }

            return count;
        }

        private static int AdjacentSeats2(this char[,] seats, int row, int col)
        {
            var height = seats.GetLength(0);
            var width = seats.GetLength(1);

            var count = 0;

            foreach (var direction in Directions)
            {
                var dR = direction.Item1;
                var dC = direction.Item2;

                var r = row + dR;
                var c = col + dC;

                for (; r >= 0 && r < height && c >= 0 && c < width; r += dR, c += dC)
                {
                    var seat = seats[r, c];

                    if (seat != '.')
                    {
                        if (seat == '#')
                        {
                            ++count;
                        }

                        break;
                    }
                }
            }

            return count;
        }

        private static int AdjacentSeats1_FunctionalAlternative_Slower(this char[,] seats, int row, int col)
        {
            return Directions
                .Select(d => (r: row + d.Item1, c: col + d.Item2))
                .Where(t => t.r >= 0 && t.r < seats.GetLength(0) && t.c >= 0 && t.c < seats.GetLength(1))
                .Count(t => seats[t.r, t.c] == '#');
        }

        private static int AdjacentSeats2_FunctionalAlternative_9x_Slower(this char[,] seats, int row, int col)
        {
            return Directions
                .Select(d => Enumerable.Range(1, Math.Max(seats.GetLength(0), seats.GetLength(1)))
                    .Select(i => (r: row + i * d.Item1, c: col + i * d.Item2))
                    .TakeWhile(t => t.r >= 0 && t.r < seats.GetLength(0) && t.c >= 0 && t.c < seats.GetLength(1))
                    .Select(t => seats[t.r, t.c])
                    .FirstOrDefault(c => c != '.'))
                .Count(c => c == '#');
        }

        private static bool AreEqual(this char[,] x, char[,] y)
        {
            return x.Cast<char>().SequenceEqual(y.Cast<char>());
        }

        private static readonly IReadOnlyCollection<(int, int)> Directions = new[]
        {
            (0, -1),
            (-1, -1),
            (-1, 0),
            (-1, 1),
            (0, 1),
            (1, 1),
            (1, 0),
            (1, -1)
        };
    }
}
