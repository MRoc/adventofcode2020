using System;
using System.Collections.Generic;
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

            foreach (var (n, i) in input.Select((n, i) => (n, i)))
            {
                store[n, 1] = i;
            }

            var before = input.Last();

            for (int turn = input.Length; turn < turns; ++turn)
            {
                if (store[before, 0] == -1)
                {
                    before = 0;
                }
                else
                {
                    before = store[before, 1] - store[before, 0];
                }

                store[before, 0] = store[before, 1];
                store[before, 1] = turn;
            }

            return before;
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