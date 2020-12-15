using System;
using System.Collections.Generic;
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

            var store = new DictQueue();
            foreach (var (n, i) in input.Select((n, i) => (n, i)))
            {
                store.StoreTurn(n, i);
            }    

            var before = input.Last();

            for (int turn = input.Length; turn < turns; ++turn)
            {
                var (i0, i1) = store.GetTurns(before);

                if (i0 == -1)
                {
                    before = 0;
                }
                else
                {
                    before = i1 - i0;
                }
                
                store.StoreTurn(before, turn);
            }

            return before;
        }


        public static long Puzzle2()
        {
            var turns = 30000000;

            var input = new[] { 13, 16, 0, 12, 15, 1 };

            var store = new DictQueue();
            foreach (var (n, i) in input.Select((n, i) => (n, i)))
            {
                store.StoreTurn(n, i);
            }

            var before = input.Last();

            for (int turn = input.Length; turn < turns; ++turn)
            {
                var (i0, i1) = store.GetTurns(before);

                if (i0 == -1)
                {
                    before = 0;
                }
                else
                {
                    before = i1 - i0;
                }

                store.StoreTurn(before, turn);
            }

            return before;
        }

        private class DictQueue
        {
            public void StoreTurn(int number, int turn)
            {
                if (_store.ContainsKey(number))
                {
                    _store[number][0] = _store[number][1];
                    _store[number][1] = turn;
                }
                else
                {
                    _store[number] = new int[] { -1, turn };
                }
            }

            public (int i0, int i1) GetTurns(int number)
            {
                if (_store.ContainsKey(number))
                {
                    return (_store[number][0], _store[number][1]);
                }
                else
                {
                    return (-1, -1);
                }
            }

            private readonly Dictionary<int, int[]> _store = new Dictionary<int, int[]>();
        }

    }
}