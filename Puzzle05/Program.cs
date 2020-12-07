using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Puzzle05
{
    // https://adventofcode.com/2020/day/5
    public static class Program
    {
        // Puzzle 1: 994
        // Puzzle 2: 741
        public static void Main()
        {
            Console.WriteLine($"Puzzle 1: {Puzzle1()}");
            Console.WriteLine($"Puzzle 2: {Puzzle2()}");
        }

        private static int Puzzle1()
        {
            return LoadInput().Select(CalcId).Max();
        }

        private static int Puzzle2()
        {
            var allocatedSeats = LoadInput().Select(CalcId).ToArray();

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

        private static string[] LoadInput()
        {
            using var stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Puzzle05.input05.txt");
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
