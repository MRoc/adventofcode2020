using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 450
    // Puzzle 2: 4059
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
            return Enumerable.Range(0, 100)
                .Aggregate(
                    Input
                        .LoadLines("Puzzles.Input.input24.txt")
                        .ParsePaths()
                        .Select(path => path.Aggregate((x: 0, y: 0), Step))
                        .GroupBy(p => p)
                        .Where(g => g.Count() % 2 == 1)
                        .Select(g => g.Key)
                        .ToHashSet(),
                    (a, _) => a.NextState().ToHashSet())
                .Count;
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

        private static readonly string[] Directions = new[] { "se", "sw", "nw", "ne", "w", "e" };

        private static (int x, int y) Step((int x, int y) p, string direction) => direction switch
        {
            "se" => p.y % 2 == 0 ? (x: p.x + 1, y: p.y - 1) : (x: p.x + 0, y: p.y - 1),
            "sw" => p.y % 2 == 0 ? (x: p.x + 0, y: p.y - 1) : (x: p.x - 1, y: p.y - 1),
            "nw" => p.y % 2 == 0 ? (x: p.x + 0, y: p.y + 1) : (x: p.x - 1, y: p.y + 1),
            "ne" => p.y % 2 == 0 ? (x: p.x + 1, y: p.y + 1) : (x: p.x + 0, y: p.y + 1),
            "w" => (x: p.x - 1, y: p.y + 0),
            "e" => (x: p.x + 1, y: p.y + 0),
            _ => throw new NotSupportedException()
        };

        private static IEnumerable<(int x, int y)> AdjacentTiles(this (int x, int y) tile)
        {
            return Directions.Select(d => Step(tile, d));
        }

        private static IEnumerable<(int x, int y)> NextState(this HashSet<(int x, int y)> state)
        {
            return state
                .Where(t => t.AdjacentTiles().Count(state.Contains).IsInRange(1, 2))
                .Concat(state
                    .SelectMany(s => s.AdjacentTiles().Where(t => !state.Contains(t)))
                    .Where(t => t.AdjacentTiles().Count(state.Contains) == 2));
        }
        
        private static bool IsInRange(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }
    }
}
