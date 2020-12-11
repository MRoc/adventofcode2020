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
                .FindStableState(NextState)
                .Cast<char>()
                .Count(c => c == '#');
        }

        public static long Puzzle2()
        {
            return Input
                .LoadLines("Puzzles.Input.input11.txt")
                .CreateSeats()
                .FindStableState(NextState2)
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

        private static char[,] FindStableState(this char[,] grid, Func<char[,], char[,]> transform)
        {
            var next = grid;
            var prev = default(char[,]);

            while (prev is null || !prev.AreEqual(next))
            {
                prev = next;
                next = transform(next);
            }

            return next;
        }

        private static char[,] NextState(this char[,] grid)
        {
            var nextState = new char[grid.GetLength(0), grid.GetLength(1)];

            foreach (var i in Enumerable.Range(0, grid.GetLength(0)))
            {
                foreach (var j in Enumerable.Range(0, grid.GetLength(1)))
                {
                    nextState[i, j] = grid.NextState(i, j);
                }
            }

            return nextState;
        }

        private static char[,] NextState2(this char[,] grid)
        {
            var nextState = new char[grid.GetLength(0), grid.GetLength(1)];

            foreach (var i in Enumerable.Range(0, grid.GetLength(0)))
            {
                foreach (var j in Enumerable.Range(0, grid.GetLength(1)))
                {
                    nextState[i, j] = grid.NextState2(i, j);
                }
            }

            return nextState;
        }

        private static char NextState(this char[,] grid, int i, int j)
        {
            var seat = grid.SeatAtPosition(i, j);
            var adjacentAllocatedSeats = grid.AdjacentSeats(i, j).Count(c => c == '#');

            if (seat == 'L' && adjacentAllocatedSeats == 0)
            {
                return '#';
            }

            if (seat == '#' && adjacentAllocatedSeats >= 4)
            {
                return 'L';
            }

            return seat;
        }

        private static char NextState2(this char[,] grid, int i, int j)
        {
            var seat = grid.SeatAtPosition(i, j);
            var adjacentAllocatedSeats = grid.AdjacentSeats2(i, j).Count(c => c == '#');

            if (seat == 'L' && adjacentAllocatedSeats == 0)
            {
                return '#';
            }

            if (seat == '#' && adjacentAllocatedSeats >= 5)
            {
                return 'L';
            }

            return seat;
        }

        private static IEnumerable<char> AdjacentSeats(this char[, ] seats, int row, int col)
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
            var height = seats.GetLength(0);
            var width = seats.GetLength(1);

            var c = default(int);
            var r = default(int);

            // Left
            for (r = row, c = col - 1; c >= 0; --c)
            {
                if (seats[r, c] != '.')
                {
                    yield return seats[r, c];
                    break;
                }
            }

            // Left top
            for (r = row - 1, c = col - 1; r >= 0 && c >= 0; --r, --c)
            {
                if (seats[r, c] != '.')
                {
                    yield return seats[r, c];
                    break;
                }
            }

            // Top
            for (r = row - 1, c = col; r >= 0; --r)
            {
                if (seats[r, c] != '.')
                {
                    yield return seats[r, c];
                    break;
                }
            }

            // Top right
            for (r = row - 1, c = col + 1; r >= 0 && c < width; --r, ++c)
            {
                if (seats[r, c] != '.')
                {
                    yield return seats[r, c];
                    break;
                }
            }

            // Right
            for (r = row, c = col + 1; c < width; ++c)
            {
                if (seats[r, c] != '.')
                {
                    yield return seats[r, c];
                    break;
                }
            }

            // Right bottom
            for (r = row + 1, c = col + 1; r < height && c < width; ++r, ++c)
            {
                if (seats[r, c] != '.')
                {
                    yield return seats[r, c];
                    break;
                }
            }

            // Bottom
            for (r = row + 1, c = col; r < height; ++r)
            {
                if (seats[r, c] != '.')
                {
                    yield return seats[r, c];
                    break;
                }
            }

            // Right left
            for (r = row + 1, c = col - 1; r < height && c >= 0; ++r, --c)
            {
                if (seats[r, c] != '.')
                {
                    yield return seats[r, c];
                    break;
                }
            }
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
