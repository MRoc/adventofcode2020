using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Puzzle05
{
    // https://adventofcode.com/2020/day/5
    public static class Program
    {
        // Highest seat ID: 820
        // Highest seat ID: 994
        // Free seat: 741
        public static void Main()
        {
            Example();
            Puzzle1();
            Puzzle2();
        }

        private static void Example()
        {
            var seats = new []
            {
                "BFFFBBFRRR",
                "FFFBBBFRRR",
                "BBFFBBFRLL"
            };

            var highestSeatId = seats.Select(s => CalcId(s)).Max();
            Console.WriteLine($"Highest seat ID: {highestSeatId}");
        }

        private static void Puzzle1()
        {
            var highestSeatId = LoadInput().Select(CalcId).Max();
            Console.WriteLine($"Highest seat ID: {highestSeatId}");
        }

        private static void Puzzle2()
        {
            var allocatedSeats = LoadInput().Select(CalcId).ToArray();

            var minSeat = allocatedSeats.Min();
            var maxSeat = allocatedSeats.Max();

            var freeSeats = Enumerable
                .Range(minSeat, maxSeat - minSeat)
                .Except(allocatedSeats)
                .ToArray();

            foreach (var freeSeat in freeSeats)
            {
                Console.WriteLine($"Free seat: {freeSeat}");
            }

        }

        private static int CalcId(string seat)
        {
            return Convert.ToInt32(seat.Replace('B', '1').Replace('F', '0').Replace('R', '1').Replace('L', '0'), 2);
        }

        private static string[] LoadInput()
        {
            using var stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Puzzle05.input.txt");
            if (stream is null)
            {
                throw new InvalidOperationException("Could not load resource!");
            }

            using var reader = new StreamReader(stream);
            return reader
                .ReadToEnd()
                .Split('\n')
                .Where(l => !string.IsNullOrEmpty(l))
                .ToArray();
        }
    }
}
