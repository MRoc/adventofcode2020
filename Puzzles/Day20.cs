using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;

namespace Puzzles
{
    // Puzzle 1: 0
    // Puzzle 2: 0
    public static class Day20
    {
        public static long Puzzle1()
        {
            var tiles = Input
                .Load("Puzzles.Input.input20.txt").Split("\n\n")
                .Select(Tile.Parse)
                .ToArray();

            foreach (var number in tiles
                .SelectMany(t => t.AsNumbers()).GroupBy(n => n))
            {
                Console.WriteLine($"{number.Key}: {number.Count()}x");
            }
            
            return 0;
        }

        public static long Puzzle2()
        {

            return 0;
        }

        public record Tile(int Id, int Top, int Right, int Bottom, int Left)
        {
            public static Tile Parse(string text)
            {
                var lines = text.Split('\n');

                var id = int.Parse(lines.First().Substring(5, 4));

                var data = lines
                    .Skip(1)
                    .Select(s => s.Replace('#', '1').Replace('.', '0').ToArray())
                    .ToArray();
                
                return new Tile(
                    id,
                    Convert.ToInt32(data.First().AsString(), 2),
                    Convert.ToInt32(data.Select(l => l.Last()).AsString(), 2),
                    Convert.ToInt32(data.Last().AsString(), 2),
                    Convert.ToInt32(data.Select(l => l.First()).AsString(), 2));
            }

            public Tile FlipVertical()
            {
                return new Tile(Id, Bottom, Right.Reverse10Bits(), Top, Left.Reverse10Bits());
            }

            public Tile FlipHorizontal()
            {
                return new Tile(Id, Top.Reverse10Bits(), Left, Bottom.Reverse10Bits(), Right);
            }

            public Tile RotateRight90()
            {
                return new Tile(Id, Left.Reverse10Bits(), Top, Right.Reverse10Bits(), Bottom);
            }

            public IEnumerable<int> AsNumbers()
            {
                return new[] {Top, Right, Bottom, Left};
            }
        }

        private static string AsString(this IEnumerable<char> chars)
        {
            var sb = new StringBuilder();
            foreach (var c in chars)
            {
                sb.Append(c);
            }

            return sb.ToString();
        }

        private static int Reverse10Bits(this int value)
        {
            return Enumerable
                .Range(0, 10)
                .Select(i => ((1 << i) & value) != 0)
                .Reverse()
                .Select((b, i) => b ? (1 << i) : 0)
                .Aggregate((a, b) => a | b);
        }
    }
}