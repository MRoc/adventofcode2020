using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 319
    // Puzzle 2: 2424
    public static class Day15
    {
        public static long Puzzle1()
        {
            return Calculate(2020);
        }

        public static long Puzzle2()
        {
            return Calculate(30000000);
        }

        private static int Calculate(int turns)
        {
            var input = new[] { 13, 16, 0, 12, 15, 1 };

            var memory = new int[turns];
            for (int turn = 0; turn < input.Length - 1; ++turn)
            {
                memory[input[turn]] = turn + 1;
            }

            var value = input.Last();
            var lastValue = value;
            var lastTurn = input.Length;

            for (int turn = input.Length + 1; turn <= turns; ++turn)
            {
                if (memory[value] == 0)
                {
                    value = 0;
                }
                else
                {
                    value = lastTurn - memory[value];
                }

                memory[lastValue] = lastTurn;
                lastValue = value;
                lastTurn = turn;
            }

            return value;
        }
    }
}