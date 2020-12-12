using System;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 1533
    // Puzzle 2: 25235
    public static class Day12
    {
        public static long Puzzle1()
        {
            var instructions = Input.LoadLines("Puzzles.Input.input12.txt");

            var state = new State1(0L, new Point(0L, 0L));

            foreach (var instruction in instructions
               .Select(i => (code: i[0], value: long.Parse(i.Substring(1)))))
            {
                state = state.NextState(instruction);
            }

            return state.Position.ManhattanDistance();
        }

        public class State1
        {
            public State1(long direction, Point position)
            {
                Direction = direction;
                Position = position;
            }

            public long Direction { get; }

            public Point Position { get; }

            public State1 NextState((char code, long value) instruction)
            {
                var moves = new[]
                {
                    (code: 'E', direction: new Point(1, 0)),
                    (code: 'S', direction: new Point(0, -1)),
                    (code: 'W', direction: new Point(-1, 0)),
                    (code: 'N', direction: new Point(0, 1))
                };

                var (code, direction) = moves.FirstOrDefault(d => d.code == instruction.code);
                if (code != 0)
                {
                    return new State1(Direction, Position + direction * instruction.value);
                }

                if (instruction.code == 'L')
                {
                    return new State1(Mod(Direction - instruction.value / 90L, 4), Position);
                }

                if (instruction.code == 'R')
                {
                    return new State1(Mod(Direction + instruction.value / 90L, 4), Position);
                }

                if (instruction.code == 'F')
                {
                    return new State1(Direction, Position + moves[Direction].direction * instruction.value);
                }

                throw new NotSupportedException();
            }
        }

        public static long Puzzle2()
        {
            var instructions = Input.LoadLines("Puzzles.Input.input12.txt");

            var state = new State2(new Point(0L, 0L), new Point(10L, 1L));

            foreach (var instruction in instructions
                .Select(i => (code: i[0], value: long.Parse(i.Substring(1)))))
            {
                state = state.NextState(instruction);
            }

            return state.Position.ManhattanDistance();
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

            public State2 NextState((char code, long value) instruction)
            {
                var moves = new[]
                {
                    (code: 'E', direction: new Point(1, 0)),
                    (code: 'S', direction: new Point(0, -1)),
                    (code: 'W', direction: new Point(-1, 0)),
                    (code: 'N', direction: new Point(0, 1))
                };

                var (code, direction) = moves.FirstOrDefault(d => d.code == instruction.code);
                if (code != 0)
                {
                    return new State2(Position, Waypoint + direction * instruction.value);
                }

                if (instruction.code == 'L')
                {
                    return new State2(Position, Waypoint.RotateL(instruction.value / 90L));
                }

                if (instruction.code == 'R')
                {
                    return new State2(Position, Waypoint.RotateR(instruction.value / 90L));
                }

                if (instruction.code == 'F')
                {
                    return new State2(Position + Waypoint * instruction.value, Waypoint);
                }

                throw new NotSupportedException();
            }
        }

        private static long Mod(long x, long m)
        {
            return (x % m + m) % m;
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

            public static Point operator -(Point p0, Point p1)
            {
                return new Point(p0.X - p1.X, p0.Y - p1.Y);
            }

            public static Point operator /(Point p0, Point p1)
            {
                return new Point(p0.X / p1.X, p0.Y / p1.Y);
            }

            public static Point operator *(Point p0, Point p1)
            {
                return new Point(p0.X * p1.X, p0.Y * p1.Y);
            }

            public static Point operator +(Point p0, long value)
            {
                return new Point(p0.X + value, p0.Y + value);
            }

            public static Point operator -(Point p0, long value)
            {
                return new Point(p0.X - value, p0.Y - value);
            }

            public static Point operator /(Point p0, long value)
            {
                return new Point(p0.X / value, p0.Y / value);
            }

            public static Point operator *(Point p0, long value)
            {
                return new Point(p0.X * value, p0.Y * value);
            }

            public Point RotateL(long times)
            {
                var x = X;
                var y = Y;

                foreach (var _ in Enumerable.Range(0, (int)times))
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

                foreach (var _ in Enumerable.Range(0, (int)times))
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
