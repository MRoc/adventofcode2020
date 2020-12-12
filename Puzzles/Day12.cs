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

            var state = new State1(0L, 0L, 0L);

            foreach (var instruction in instructions
               .Select(i => (code: i[0], value: long.Parse(i.Substring(1)))))
            {
                state = state.NextState(instruction);
            }

            return state.ManhattanDistance();
        }

        public record State1(long Direction, long X, long Y)
        {
            public State1 NextState((char code, long value) instruction)
            {
                var moves = new[]
                {
                    (code: 'E', x: 1, y: 0),
                    (code: 'S', x: 0, y: -1),
                    (code: 'W', x: -1, y: 0),
                    (code: 'N', x: 0, y: 1),
                };

                var move = moves.FirstOrDefault(d => d.code == instruction.code);

                if (move.code != 0)
                {
                    return new State1(Direction, X + move.x * instruction.value, Y + move.y * instruction.value);
                }

                if (instruction.code == 'L')
                {
                    return new State1(Mod(Direction - instruction.value / 90L, 4), X, Y);
                }

                if (instruction.code == 'R')
                {
                    return new State1(Mod(Direction + instruction.value / 90L, 4), X, Y);
                }

                if (instruction.code == 'F')
                {
                    move = moves[Direction];
                    return new State1(Direction, X + move.x * instruction.value, Y + move.y * instruction.value);
                }
                
                throw new NotSupportedException();
            }

            public long ManhattanDistance()
            {
                return Math.Abs(X) + Math.Abs(Y);
            }
        }

        public static long Puzzle2()
        {
            var instructions = Input.LoadLines("Puzzles.Input.input12.txt");

            var state = new State2(0L, 0L, 10L, 1L);

            foreach (var instruction in instructions
                .Select(i => (code: i[0], value: long.Parse(i.Substring(1)))))
            {
                state = state.NextState(instruction);
            }

            return state.ManhattanDistance();
        }

        public record State2(long X, long Y, long WaypointX, long WaypointY)
        {
            public State2 NextState((char code, long value) instruction)
            {
                var moves = new[]
                {
                    (code: 'E', x: 1, y: 0),
                    (code: 'S', x: 0, y: -1),
                    (code: 'W', x: -1, y: 0),
                    (code: 'N', x: 0, y: 1),
                };

                var move = moves.FirstOrDefault(d => d.code == instruction.code);

                if (move.code != 0)
                {
                    return new State2(
                        X,
                        Y,
                        WaypointX + move.x * instruction.value,
                        WaypointY + move.y * instruction.value);
                }

                if (instruction.code == 'L')
                {
                    var (wx, wy) = RotateL(WaypointX, WaypointY, instruction.value / 90L);
                    return new State2(X, Y, wx, wy);
                }

                if (instruction.code == 'R')
                {
                    var (wx, wy) = RotateR(WaypointX, WaypointY, instruction.value / 90L);
                    return new State2(X, Y, wx, wy);
                }

                if (instruction.code == 'F')
                {
                    return new State2(
                        X + WaypointX * instruction.value,
                        Y + WaypointY * instruction.value,
                        WaypointX,
                        WaypointY);
                }

                throw new NotSupportedException();
            }

            public long ManhattanDistance()
            {
                return Math.Abs(X) + Math.Abs(Y);
            }
        }

        private static long Mod(long x, long m)
        {
            return (x % m + m) % m;
        }

        private static (long, long) RotateL(long x, long y, long times)
        {
            foreach (var _ in Enumerable.Range(0, (int)times))
            {
                var nX = -y;
                var nY = x;

                x = nX;
                y = nY;
            }

            return (x, y);
        }

        private static (long, long) RotateR(long x, long y, long times)
        {
            foreach (var _ in Enumerable.Range(0, (int)times))
            {
                var nX = y;
                var nY = -x;

                x = nX;
                y = nY;
            }

            return (x, y);
        }
    }
}
