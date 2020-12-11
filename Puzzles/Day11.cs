using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 2368
    // Puzzle 2: 0
    public static class Day11
    {
        public static long Puzzle1()
        {
            return Input
                .LoadLines("Puzzles.Input.input11.txt")
                .CreateSeats()
                .FindStableState()
                .Cast<char>()
                .Count(c => c == '#');
        }

        public static long Puzzle2()
        {
            return 0;
        }

        private static char[,] FindStableState(this char[,] grid)
        {
            var next = grid;
            var prev = default(char[,]);

            while (prev is null || !prev.AreEqual(next))
            {
                prev = next;
                next = next.NextState();
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

        private static IEnumerable<T> AdjacentSeats<T>(this T[, ] seats, int row, int col)
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
