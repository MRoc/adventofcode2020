using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Puzzles
{
    // Puzzle 1: 16192267830719
    // Puzzle 2: 0
    public static class Day20
    {
        public static long Puzzle1()
        {
            var tiles = Input
                .Load("Puzzles.Input.input20.txt").Split("\n\n")
                .Where(l => !string.IsNullOrEmpty(l))
                .Select(Tile.Parse)
                .ToArray();

            var fourers = new List<long>();

            foreach (var tile in tiles)
            {
                var numbersOfTile = tile.AsNumbers().Concat(tile.AsNumbers().Select(n => n.Reverse10Bits()));
                var allOtherNumbers = tiles.Where(t => t.Id != tile.Id).SelectMany(t =>
                    t.AsNumbers().Concat(t.AsNumbers().Select(n => n.Reverse10Bits())));

                var overlap = numbersOfTile.Where(n => allOtherNumbers.Contains(n)).Count();

                if (overlap == 4)
                {
                    fourers.Add(tile.Id);
                }
            }

            return fourers.Aggregate((a, b) => a * b);
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

        record Point(int X, int Y)
        {
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