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
            var state = Input
                .LoadLines("Puzzles.Input.input24.txt")
                .ParsePaths()
                .Select(path => path.Aggregate((x: 0, y: 0), Step))
                .GroupBy(p => p)
                .Where(g => g.Count() % 2 == 1)
                .Select(g => g.Key)
                .ToArray();
            
            foreach (var _ in Enumerable.Range(0, 100))
            {
                state = state.NextState().Distinct().ToArray();
            }    
            
            return state.Length;
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

        private static IEnumerable<(int x, int y)> AdjacentTiles(this (int x, int y) tile)
        {
            return Directions.Select(d => Step(tile, d));
        }
        
        private static IEnumerable<(int x, int y)> NextState(this (int x, int y)[] state)
        {
            foreach (var tile in state)
            {
                var countAdjacentTiles = tile.AdjacentTiles().Count(p => state.Contains(p));
                if (countAdjacentTiles > 0 && countAdjacentTiles <= 2)
                {
                    yield return tile;
                }
                
                foreach (var adjacentTile in tile.AdjacentTiles().Where(t => !state.Contains(t))) 
                {
                    var c = adjacentTile.AdjacentTiles().Count(p => state.Contains(p));
                    if (c == 2)
                    {
                        yield return adjacentTile;
                    }
                }
            }
        }
    }
}
