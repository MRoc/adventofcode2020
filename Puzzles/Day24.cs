using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 450
    // Puzzle 2: 0
    public static class Day24
    {
        public static long Puzzle1()
        {
            return Input
                .LoadLines("Puzzles.Input.input24.txt")
                .ParsePaths()
                .Select(path => path.Aggregate((x: 0, y: 0), Step))
                .GroupBy(p => p)
                .Count(g => g.Count() % 2 == 1);
        }

        public static long Puzzle2()
        {
            return 0;
        }

        private static string[][] ParsePaths(this IEnumerable<string> lines)
        {
            return lines
                .Select(l => l.ParsePath().ToArray())
                .ToArray();
        }

        private static IEnumerable<string> ParsePath(this string text)
        {
            for (int i = 0; i < text.Length;)
            {
                var direction = Directions.First(d => text[i..].StartsWith(d));
                yield return direction;
                i += direction.Length;
            }
        }

        private static string[] Directions = new[] { "se", "sw", "nw", "ne", "w", "e" };

        private static (int x, int y) Step((int x, int y) p, string direction) => direction switch
        {
            "se" => p.y % 2 == 0 ? (x: p.x + 1,  y: p.y - 1) : (x: p.x + 0, y: p.y - 1),
            "sw" => p.y % 2 == 0 ? (x: p.x + 0,  y: p.y - 1) : (x: p.x - 1, y: p.y - 1),
            "nw" => p.y % 2 == 0 ? (x: p.x + 0,  y: p.y + 1) : (x: p.x - 1, y: p.y + 1),
            "ne" => p.y % 2 == 0 ? (x: p.x + 1,  y: p.y + 1) : (x: p.x + 0, y: p.y + 1),
            "w"  => (x: p.x - 1, y: p.y + 0),
            "e"  => (x: p.x + 1,  y: p.y + 0),
            _    => throw new NotSupportedException()
        };
        
    }
}
