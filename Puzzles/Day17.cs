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
            var state = LoadInput();

            foreach (var i in Enumerable.Range(0, 6))
            {
                state = state.NextState3();
            }

            return state.Length;
        }

        public static long Puzzle2()
        {
            var state = LoadInput().Select(p => new Point4(p.X, p.Y, 0, 0)).ToArray();
            foreach (var i in Enumerable.Range(0, 6))
            {
                state = state.NextState4();
            }

            return state.Length;
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

        private static Point3[] NextState3(this Point3[] input)
        {
            var output = new HashSet<Point3>();

            foreach (var activeCells in input
                .Where(c => c.AdjacentCells().OptimizedCountInRange(input.Contains, 2, 3)))
            {
                output.Add(activeCells);
            }

            foreach (var adjacentCell in input
                .SelectMany(p => p.AdjacentCells())
                .Distinct()
                .Where(c0 => c0.AdjacentCells().OptimizedCountEquals(input.Contains, 3)))
            {
                output.Add(adjacentCell);
            }

            return output.ToArray();
        }

        private static Point4[] NextState4(this Point4[] input)
        {
            var output = new HashSet<Point4>();

            foreach (var activeCells in input
                .Where(c => c.AdjacentCells().OptimizedCountInRange(input.Contains, 2, 3)))
            {
                output.Add(activeCells);
            }

            foreach (var adjacentCell in input
                .SelectMany(p => p.AdjacentCells())
                .Distinct()
                .Where(c0 => c0.AdjacentCells().OptimizedCountEquals(input.Contains, 3)))
            {
                output.Add(adjacentCell);
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

        public record Point4(int X, int Y, int Z, int W)
        {
            public IEnumerable<Point4> AdjacentCells()
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

        public static bool OptimizedCountEquals<T>(this IEnumerable<T> seq, Func<T, bool> predicate, int expected)
        {
            using var iterator = seq.GetEnumerator();

            var count = 0;
            while (iterator.MoveNext())
            {
                if (predicate(iterator.Current))
                {
                    count++;

                    if (count > expected)
                    {
                        return false;
                    }
                }
            }

            return count == expected;
        }

        public static bool OptimizedCountInRange<T>(this IEnumerable<T> seq, Func<T, bool> predicate, int min, int max)
        {
            using var iterator = seq.GetEnumerator();

            var count = 0;
            while (iterator.MoveNext())
            {
                if (predicate(iterator.Current))
                {
                    count++;

                    if (count > max)
                    {
                        return false;
                    }
                }
            }

            return min <= count && count <= max;
        }
    }
}