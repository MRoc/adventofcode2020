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
                .Aggregate(
                    new State1(new Point(0, 0), 0),
                    (s, p) => s.NextState(p.code, p.value))
                .Position
                .ManhattanDistance();
        }

        public static int Puzzle2()
        {
            return Input
                .LoadLines("Puzzles.Input.input12.txt")
                .Select(i => (code: i[0], value: int.Parse(i[1..])))
                .Aggregate(
                    new State2(new Point(0, 0), new Point(10, 1)),
                    (s, p) => s.NextState(p.code, p.value))
                .Position
                .ManhattanDistance();
        }

        private record State1(Point Position, int Direction)
        {
            public State1 NextState(char code, int value) => code switch
            {
                'E' or 'W' or 'S' or 'N' => this with { Position = Position + Directions[code] * value },
                'L' => this with { Direction = Mod(Direction - value / 90, 4) },
                'R' => this with { Direction = Mod(Direction + value / 90, 4) },
                'F' => this with { Position = Position + Directions.ElementAt(Direction).Value * value },
                _ => throw new NotSupportedException()
            };
        }

        private record State2(Point Position, Point Waypoint)
        {
            public State2 NextState(char code, int value) => code switch
            {
                'E' or 'W' or 'S' or 'N' => this with { Waypoint = Waypoint + Directions[code] * value },
                'L' => this with { Waypoint = Waypoint.Rotate(value) },
                'R' => this with { Waypoint = Waypoint.Rotate(-value) },
                'F' => this with { Position = Position + Waypoint * value },
                _ => throw new NotSupportedException()
            };
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