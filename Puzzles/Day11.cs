using System;
using System.Collections.Generic;
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
            foreach (var i in Enumerable.Range(0, input.Count))
            {
                foreach (var j in Enumerable.Range(0, input[i].Length))
                {
                    result[i, j] = input[i][j];
                }
            }
            return result;
        }

        private static char[,] FindStableState(
            this char[,] grid,
            Func<char[,], int, int, IEnumerable<char>> adjacentSeats,
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
            Func<char[,], int, int, IEnumerable<char>> adjacentSeats,
            int numSeatsToFree)
        {
            var nextState = new char[grid.GetLength(0), grid.GetLength(1)];

            foreach (var i in Enumerable.Range(0, grid.GetLength(0)))
            {
                foreach (var j in Enumerable.Range(0, grid.GetLength(1)))
                {
                    nextState[i, j] = grid.NextState(i, j, adjacentSeats, numSeatsToFree);
                }
            }

            return nextState;
        }

        private static char NextState(
            this char[,] grid,
            int i,
            int j,
            Func<char[,], int, int, IEnumerable<char>> adjacentSeats,
            int numSeatsToFree)
        {
            var seat = grid.SeatAtPosition(i, j);
            var adjacentAllocatedSeats = adjacentSeats(grid, i, j).Count(c => c == '#');

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

        private static IEnumerable<char> AdjacentSeats1(this char[, ] seats, int row, int col)
        {
            yield return seats.SeatAtPosition(row - 1, col - 1);
            yield return seats.SeatAtPosition(row - 1, col);
            yield return seats.SeatAtPosition(row - 1, col + 1);
            yield return seats.SeatAtPosition(row, col - 1);
            yield return seats.SeatAtPosition(row, col + 1);
            yield return seats.SeatAtPosition(row + 1, col - 1);
            yield return seats.SeatAtPosition(row + 1, col);
            yield return seats.SeatAtPosition(row + 1, col + 1);
        }

        private static IEnumerable<char> AdjacentSeats2(this char[,] seats, int row, int col)
        {
            foreach (var direction in new[]
            {
                (0, -1),
                (-1, -1),
                (-1, 0),
                (-1, 1),
                (0, 1),
                (1, 1),
                (1, 0),
                (1, -1)
            })
            {
                var c = AdjacentDirection(seats, row, col, direction.Item1, direction.Item2);
                if (c != default(char))
                {
                    yield return c;
                }
            }
        }

        private static char AdjacentDirection(this char[,] seats, int row, int col, int dR, int dC)
        {
            var height = seats.GetLength(0);
            var width = seats.GetLength(1);

            var r = row + dR;
            var c = col + dC;

            for (; r >= 0 && r < height && c >= 0 && c < width; r += dR, c += dC)
            {
                if (seats[r, c] != '.')
                {
                    return seats[r, c];
                }
            }

            return default(char);
        }

        private static T SeatAtPosition<T>(this T[,] seats, int row, int col)
        {
            return row >= 0 && row < seats.GetLength(0) && col >= 0 && col < seats.GetLength(1)
                ? seats[row, col]
                : default;
        }

        private static bool AreEqual(this char[,] x, char[,] y)
        {
            return x.Cast<char>().SequenceEqual(y.Cast<char>());
        }
    }
}
