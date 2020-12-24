using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Puzzles
{
    // Puzzle 1: 0
    // Puzzle 2: 0
    public static class Day24
    {
        public static long Puzzle1()
        {
            var paths = Input
                .LoadLines("Puzzles.Input.input24.txt")
                .ParsePaths();

            var tiles = new HashSet<(int x, int y)>();
            
            foreach (var path in paths)
            {
                var p = (x: 0, y: 0);
                foreach (var direction in path)
                {
                    p = Step(p, direction);
                }

                if (tiles.Contains(p))
                {
                    tiles.Remove(p);
                }
                else
                {
                    tiles.Add(p);
                }
            }
            
            return tiles.Count;
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

        private static string[] Directions = new[]
        {
            "se", "sw", "nw", "ne", "w", "e"
        };

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
