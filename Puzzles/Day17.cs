using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 240
    // Puzzle 2: 1180
    public static class Day17
    {
        public static long Puzzle1()
        {
            var state = LoadInput3();

            foreach (var i in Enumerable.Range(0, 6))
            {
                state = state.NextState();
            }

            return state.Length;
        }

        public static long Puzzle2()
        {
            var state = LoadInput4();
            
            foreach (var i in Enumerable.Range(0, 6))
            {
                state = state.NextState();
            }

            return state.Length;
        }

        private static IAdjacents[] LoadInput3()
        {
            return Input
                .Load("Puzzles.Input.input17.txt")
                .Split('\n')
                .SelectMany((l, cy) => l.Select((c, cx) => (x: cx, y: cy, c)))
                .Where(t => t.c == '#')
                .Select(t => new Point3(t.x, t.y, 0))
                .ToArray();
        }

        private static IAdjacents[] LoadInput4()
        {
            return Input
                .Load("Puzzles.Input.input17.txt")
                .Split('\n')
                .SelectMany((l, cy) => l.Select((c, cx) => (x: cx, y: cy, c)))
                .Where(t => t.c == '#')
                .Select(t => new Point4(t.x, t.y, 0, 0))
                .ToArray();
        }

        private static IAdjacents[] NextState(this IAdjacents[] input)
        {
            return input
                .SelectMany(i => i.AdjacentCells())
                .GroupBy(c => c)
                .ToDictionary(
                    i => i.Key,
                    i => i.Count())
                .Where(t => t.Value == 3
                            || t.Value == 2 && input.Contains(t.Key))
                .Select(t => t.Key)
                .ToArray();
        }

        public interface IAdjacents
        {
            IEnumerable<IAdjacents> AdjacentCells();
        }

        public record Point3(int X, int Y, int Z) : IAdjacents
        {
            public IEnumerable<IAdjacents> AdjacentCells()
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

        public record Point4(int X, int Y, int Z, int W) : IAdjacents
        {
            public IEnumerable<IAdjacents> AdjacentCells()
            {
                for (int x = X - 1; x <= X + 1; ++x)
                {
                    for (int y = Y - 1; y <= Y + 1; ++y)
                    {
                        for (int z = Z - 1; z <= Z + 1; ++z)
                        {
                            for (int w = W - 1; w <= W + 1; ++w)
                            {
                                if (x != X || y != Y || z != Z || w != W)
                                {
                                    yield return new Point4(x, y, z, w);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}