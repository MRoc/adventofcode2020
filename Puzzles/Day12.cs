using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 1533
    // Puzzle 2: 25235
    public static class Day12
    {
        private static readonly IDictionary<char, Point> Directions = new Dictionary<char, Point>
        {
            {'E', new Point(1, 0)},
            {'S', new Point(0, -1)},
            {'W', new Point(-1, 0)},
            {'N', new Point(0, 1)}
        };

        public static long Puzzle1()
        {
            return Input
                .LoadLines("Puzzles.Input.input12.txt")
                .Select(i => (code: i[0], value: int.Parse(i.Substring(1))))
                .Process(
                    new State1(0, new Point(0L, 0L)),
                    (i, p) => p.NextState(i.code, i.value))
                .Last()
                .Position
                .ManhattanDistance();
        }

        public static long Puzzle2()
        {
            return Input
                .LoadLines("Puzzles.Input.input12.txt")
                .Select(i => (code: i[0], value: int.Parse(i.Substring(1))))
                .Process(
                    new State2(new Point(0L, 0L), new Point(10L, 1L)),
                    (i, p) => p.NextState(i.code, i.value))
                .Last()
                .Position
                .ManhattanDistance();
        }

        private static int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }

        private static IEnumerable<T2> Process<T1, T2>(this IEnumerable<T1> seq, T2 seed, Func<T1, T2, T2> projection)
        {
            yield return seed;

            foreach (var item in seq)
            {
                seed = projection(item, seed);

                yield return seed;
            }
        }

        public class State1
        {
            public State1(int direction, Point position)
            {
                Direction = direction;
                Position = position;
            }

            public int Direction { get; }

            public Point Position { get; }

            public State1 NextState(char code, int value)
            {
                switch (code)
                {
                    case 'E':
                    case 'W':
                    case 'S':
                    case 'N':
                        return new State1(Direction, Position + Directions[code] * value);
                    case 'L':
                        return new State1(Mod(Direction - value / 90, 4), Position);
                    case 'R':
                        return new State1(Mod(Direction + value / 90, 4), Position);
                    case 'F':
                        return new State1(Direction, Position + Directions.ElementAt(Direction).Value * value);
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public class State2
        {
            public State2(Point position, Point waypoint)
            {
                Position = position;
                Waypoint = waypoint;
            }

            public Point Position { get; }

            public Point Waypoint { get; }

            public State2 NextState(char code, long value)
            {
                switch (code)
                {
                    case 'E':
                    case 'W':
                    case 'S':
                    case 'N':
                        return new State2(Position, Waypoint + Directions[code] * value);
                    case 'L':
                        return new State2(Position, Waypoint.RotateL(value / 90L));
                    case 'R':
                        return new State2(Position, Waypoint.RotateR(value / 90L));
                    case 'F':
                        return new State2(Position + Waypoint * value, Waypoint);
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public class Point
        {
            public Point(long x, long y)
            {
                X = x;
                Y = y;
            }

            public long X { get; }

            public long Y { get; }

            public static Point operator +(Point p0, Point p1)
            {
                return new Point(p0.X + p1.X, p0.Y + p1.Y);
            }

            public static Point operator *(Point p0, long value)
            {
                return new Point(p0.X * value, p0.Y * value);
            }

            public Point RotateL(long times)
            {
                var x = X;
                var y = Y;

                foreach (var _ in Enumerable.Range(0, (int) times))
                {
                    var nX = -y;
                    var nY = x;

                    x = nX;
                    y = nY;
                }

                return new Point(x, y);
            }

            public Point RotateR(long times)
            {
                var x = X;
                var y = Y;

                foreach (var _ in Enumerable.Range(0, (int) times))
                {
                    var nX = y;
                    var nY = -x;

                    x = nX;
                    y = nY;
                }

                return new Point(x, y);
            }

            public long ManhattanDistance()
            {
                return Math.Abs(X) + Math.Abs(Y);
            }
        }
    }
}