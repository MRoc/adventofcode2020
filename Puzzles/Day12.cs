using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 1533
    // Puzzle 2: 25235
    public static class Day12
    {
        public static int Puzzle1()
        {
            return Input
                .LoadLines("Puzzles.Input.input12.txt")
                .Select(i => (code: i[0], value: int.Parse(i[1..])))
                .Process(
                    new State1(0, new Point(0, 0)),
                    (i, p) => p.NextState(i.code, i.value))
                .Last()
                .Position
                .ManhattanDistance();
        }

        public static int Puzzle2()
        {
            return Input
                .LoadLines("Puzzles.Input.input12.txt")
                .Select(i => (code: i[0], value: int.Parse(i[1..])))
                .Process(
                    new State2(new Point(0, 0), new Point(10, 1)),
                    (i, p) => p.NextState(i.code, i.value))
                .Last()
                .Position
                .ManhattanDistance();
        }

        private static IEnumerable<T2> Process<T1, T2>(this IEnumerable<T1> seq, T2 seed, Func<T1, T2, T2> projection)
        {
            foreach (var item in seq)
            {
                seed = projection(item, seed);

                yield return seed;
            }
        }

        private record State1(int Direction, Point Position)
        {
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

        private record State2(Point Position, Point Waypoint)
        {
            public State2 NextState(char code, int value)
            {
                switch (code)
                {
                    case 'E':
                    case 'W':
                    case 'S':
                    case 'N':
                        return new State2(Position, Waypoint + Directions[code] * value);
                    case 'L':
                        return new State2(Position, Waypoint.Rotate(value));
                    case 'R':
                        return new State2(Position, Waypoint.Rotate(-value));
                    case 'F':
                        return new State2(Position + Waypoint * value, Waypoint);
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        private record Point(int X, int Y)
        {
            public static Point operator +(Point p0, Point p1)
            {
                return new Point(p0.X + p1.X, p0.Y + p1.Y);
            }

            public static Point operator *(Point p0, int value)
            {
                return new Point(p0.X * value, p0.Y * value);
            }

            public static Point operator *(Point p0, Point p1)
            {
                return new Point(p0.X * p1.X - p0.Y * p1.Y, p0.X * p1.Y + p0.Y * p1.X);
            }

            public Point Rotate(int degrees)
            {
                return this * new Point(Cos(degrees), Sin(degrees));
            }

            public int ManhattanDistance()
            {
                return Math.Abs(X) + Math.Abs(Y);
            }
        }

        private static int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }

        private static int Cos(int degree)
        {
            return new[] { 1, 0, -1, 0 }[Mod(degree / 90, 4)];
        }

        private static int Sin(int degree)
        {
            return new[] { 0, 1, 0, -1 }[Mod(degree / 90, 4)];
        }

        private static readonly IDictionary<char, Point> Directions = new Dictionary<char, Point>
        {
            {'E', new Point(1, 0)},
            {'S', new Point(0, -1)},
            {'W', new Point(-1, 0)},
            {'N', new Point(0, 1)}
        };
    }
}