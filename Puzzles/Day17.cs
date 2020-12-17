using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 240
    // Puzzle 2: 0
    public static class Day17
    {
        public static long Puzzle1()
        {
            var state = LoadInput();

            foreach (var i in Enumerable.Range(0, 6))
            {
                state = state.NextState();
            }

            return state.Length;
        }

        public static long Puzzle2()
        {
            return 0;
        }

        private static Point3[] LoadInput()
        {
            return Input
                .Load("Puzzles.Input.input17.txt")
                .Split('\n')
                .SelectMany((l, cy) => l.Select((c, cx) => (x: cx, y: cy, c)))
                .Where(t => t.c == '#')
                .Select(t => new Point3(t.x, t.y, 0))
                .ToArray();
        }

        private static Point3[] NextState(this Point3[] input)
        {
            var output = new HashSet<Point3>();

            foreach (var p in input)
            {
                var activeAdjacentCells = p.AdjacentCells().Count(c => input.Contains(c));
                if (2 <= activeAdjacentCells && activeAdjacentCells <=3)
                {
                    output.Add(p);
                }
                
                foreach (var adjacentCell in p.AdjacentCells())
                {
                    var activeAdjacentCells1 = adjacentCell.AdjacentCells().Count(c => input.Contains(c));
                    if (activeAdjacentCells1 == 3)
                    {
                        output.Add(adjacentCell);
                    }
                }
            }

            return output.ToArray();
        }

        public record Point3(int X, int Y, int Z)
        {
            public IEnumerable<Point3> AdjacentCells()
            {
                for (int x = X - 1; x <= X + 1; ++x)
                {
                    for (int y = Y - 1; y <= Y + 1; ++y)
                    {
                        for (int z = Z - 1; z <= Z + 1; ++z)
                        {
                            if (x != X || y != Y || z != Z)
                            {
                                yield return new Point3(x, y, z);
                            }
                        }
                    }
                }
            }
        }
    }
}