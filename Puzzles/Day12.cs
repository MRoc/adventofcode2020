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


            var facing = 0L;
            var x = 0L;
            var y = 0L;

            var moves = new[]
            {
                (code: 'E', x: 1, y: 0),
                (code: 'S', x: 0, y: -1),
                (code: 'W', x: -1, y: 0),
                (code: 'N', x: 0, y: 1),
            };

             foreach (var instruction in instructions
                .Select(i => (code: i[0], value: long.Parse(i.Substring(1)))))
            {
                var move = moves.FirstOrDefault(d => d.code == instruction.code);
                if (move.code != 0)
                {
                    x += move.x * instruction.value;
                    y += move.y * instruction.value;
                }
                else if (instruction.code == 'L')
                {
                    facing = Mod(facing - instruction.value / 90L, 4);
                }
                else if (instruction.code == 'R')
                {
                    facing = Mod(facing + instruction.value / 90L, 4);
                }
                else if (instruction.code == 'F')
                {
                    move = moves[facing];
                    x += move.x * instruction.value;
                    y += move.y * instruction.value;
                }
                else
                {
                    throw new NotSupportedException();
                }
            }

            return Math.Abs(x) + Math.Abs(y);
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
