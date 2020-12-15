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
            store.InitStore();

            for (int turn = 0; turn < input.Length; ++turn)
            {
                store[input[turn], 1] = turn;
            }

            var value = input.Last();

            for (int turn = input.Length; turn < turns; ++turn)
            {
                if (store[value, 0] == -1)
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

        private static void InitStore(this int[,] arr)
        {
            var len = arr.GetLength(0);
            for (int i = 0; i < len; ++i)
            {
                arr[i, 0] = -1;
                arr[i, 1] = -1;
            }
        }
    }
}