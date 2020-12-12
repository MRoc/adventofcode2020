using System;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 1533
    // Puzzle 2: 0
    public static class Day12
    {
        public static long Puzzle1()
        {
            var instructions = Input.LoadLines("Puzzles.Input.input12.txt");

            var state = new State(0L, 0L, 0L);

            foreach (var instruction in instructions
               .Select(i => (code: i[0], value: long.Parse(i.Substring(1)))))
            {
                state = state.NextState(instruction);
            }

            return state.ManhattanDistance();
        }

        public record State(long Direction, long X, long Y)
        {
            public State NextState((char code, long value) instruction)
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
                    return new State(Direction, X + move.x * instruction.value, Y + move.y * instruction.value);
                }

                if (instruction.code == 'L')
                {
                    return new State(Mod(Direction - instruction.value / 90L, 4), X, Y);
                }

                if (instruction.code == 'R')
                {
                    return new State(Mod(Direction + instruction.value / 90L, 4), X, Y);
                }

                if (instruction.code == 'F')
                {
                    move = moves[Direction];
                    return new State(Direction, X + move.x * instruction.value, Y + move.y * instruction.value);
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
            return 0;
        }

        private static long Mod(long x, long m)
        {
            return (x % m + m) % m;
        }
    }
}
