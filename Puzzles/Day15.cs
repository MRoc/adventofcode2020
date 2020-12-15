using System;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 319
    // Puzzle 2: 0
    public static class Day15
    {
        public static long Puzzle1()
        {
            var turns = 2020;

            var input = new[] { 13, 16, 0, 12, 15, 1 };

            var output = new int[turns];
            Array.Copy(input, output, input.Length);

            var before = input.Last();

            for (int turn = input.Length; turn < turns; ++turn)
            {
                var (i0, i1) = LastIndexesOf(output, before, turn);

                if (i0 == -1)
                {
                    output[turn] = before = 0;
                }
                else
                {
                    output[turn] = before = i1 - i0;
                }
            }

            return output.Last();
        }

        private static (int i0, int i1) LastIndexesOf(this int[] data, int number, int start)
        {
            var index1 = -1;
            var index0 = -1;

            for (int i = start - 1; i >= 0 && index0 == -1; --i)
            {
                if (data[i] == number)
                {
                    if (index1 == -1)
                    {
                        index1 = i;
                    }
                    else
                    {
                        index0 = i;
                    }
                }
            }

            return (index0, index1);
        }

        public static long Puzzle2()
        {
            return 0;
        }
    }
}