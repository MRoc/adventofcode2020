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

            var store = new int[turns, 2];

            for (int turn = 0; turn < input.Length; ++turn)
            {
                store[input[turn], 1] = turn + 1;
            }

            var value = input.Last();

            for (int turn = input.Length + 1; turn <= turns; ++turn)
            {
                if (store[value, 0] == 0)
                {
                    value = 0;
                }
                else
                {
                    value = store[value, 1] - store[value, 0];
                }

                store[value, 0] = store[value, 1];
                store[value, 1] = turn;
            }

            return value;
        }
    }
}