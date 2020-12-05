using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Puzzle05
{
    public static class Program
    {
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

            foreach (var seat in seats)
            {
                var (row, column, id) = CalcRowColumnId(seat);
                Console.WriteLine($"{seat}: row {row}, column {column}, seat ID {id}");
            }

            var highestSeatId = seats.Select(s => CalcRowColumnId(s).Item3).Max();
            Console.WriteLine($"Highest seat ID: {highestSeatId}");
        }

        private static void Puzzle1()
        {
            var highestSeatId = LoadInput()
                .Select(s => CalcRowColumnId(s).Item3)
                .Max();
            Console.WriteLine($"Highest seat ID: {highestSeatId}");
        }

        private static void Puzzle2()
        {
            var allocatedSeats = LoadInput()
                .Select(s => CalcRowColumnId(s).Item3)
                .ToArray();

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

        private static Tuple<int, int, int> CalcRowColumnId(string seat)
        {
            var row = Find(seat.Take(7).Select(c => c == 'B' ? +1 : -1), 0, 127);
            var col = Find(seat.Skip(7).Select(c => c == 'R' ? +1 : -1), 0, 7);
            var id = row * 8 + col;
            return Tuple.Create(row, col, id);
        }

        private static int Find(IEnumerable<int> pattern, int min, int max)
        {
            var l = min;
            var r = max;

            foreach (var c in pattern)
            {
                int m = (int)Math.Ceiling((r - l) * 0.5);
                if (c > 0)
                {
                    l += m;
                }
                else if (c < 0)
                {
                    r -= m;
                }
            }

            if (l != r)
            {
                throw new InvalidOperationException($"l={l} != r={r}");
            }

            return l;
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
