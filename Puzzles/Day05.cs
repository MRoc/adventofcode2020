using System;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 994
    // Puzzle 2: 741
    public static class Day05
    {
        public static int Puzzle1()
        {
            return Input.LoadLines("Puzzles.Input.input05.txt").Select(CalcId).Max();
        }

        public static int Puzzle2()
        {
            var allocatedSeats = Input.LoadLines("Puzzles.Input.input05.txt").Select(CalcId).ToArray();

            var minSeat = allocatedSeats.Min();
            var maxSeat = allocatedSeats.Max();

            var freeSeats = Enumerable
                .Range(minSeat, maxSeat - minSeat)
                .Except(allocatedSeats)
                .ToArray();

            return freeSeats.Single();
        }

        private static int CalcId(string seat)
        {
            return Convert.ToInt32(seat.Replace('B', '1').Replace('F', '0').Replace('R', '1').Replace('L', '0'), 2);
        }
    }
}
