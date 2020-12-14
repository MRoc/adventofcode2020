using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzles
{
    // Puzzle 1: 10885823581193
    // Puzzle 2: 3816594901962
    public static class Day14
    {
        public static long Puzzle1()
        {
            var memory = new Dictionary<int, long>();

            var maskOr = 0x0000000000000000UL;
            var maskAnd = 0xFFFFFFFFFFFFFFFFUL;

            foreach (var line in Input.LoadLines("Puzzles.Input.input14.txt"))
            {
                var match = MaskDecoder.Match(line);
                if (match.Success)
                {
                    maskOr = Convert.ToUInt64(match.Groups[1].Value.Replace('X', '0'), 2);
                    maskAnd = ~Convert.ToUInt64(match.Groups[1].Value.Replace('1', 'X').Replace('0', '1').Replace('X', '0'), 2);
                }
                else
                {
                    var result = AssignmentDecoder.Match(line);
                    var address = int.Parse(result.Groups[1].Value);
                    var value = long.Parse(result.Groups[2].Value);
                    memory[address] = value & (long) maskAnd | (long) maskOr;
                }
            } 

            return memory.Values.Sum();
        }

        public static long Puzzle2()
        {
            var memory = new Dictionary<long, long>();

            var mask = string.Empty;

            foreach (var line in Input.LoadLines("Puzzles.Input.input14.txt"))
            {
                var match = MaskDecoder.Match(line);
                if (match.Success)
                {
                    mask = match.Groups[1].Value;
                }
                else
                {
                    var result = AssignmentDecoder.Match(line);
                    var value = long.Parse(result.Groups[2].Value);
                    var addresses = PermutateAddress(long.Parse(result.Groups[1].Value), mask);

                    foreach (var address in addresses)
                    {
                        memory[address] = value;
                    }
                }
            }

            return memory.Values.Sum();
        }

        private static IEnumerable<long> PermutateAddress(long address, string bitmask)
        {
            var maskOr = Convert.ToInt64(bitmask.Replace('X', '0'), 2);

            address |= maskOr;

            var floatingBits = bitmask
                .Reverse()
                .Select((c, i) => (c: c, i: i, bitmask: bitmask))
                .Where(t => t.c == 'X')
                .ToArray();

            var addresses = PermutateAddress(address, 0, floatingBits).ToArray();
            return addresses;
        }

        private static IEnumerable<long> PermutateAddress(long address, int index, IReadOnlyList<(char c, int i, string bitmask)> items)
        {
            if (index < items.Count)
            {
                var item = items[index];

                var bit = 0x1L << item.i;

                foreach (var r in PermutateAddress(address & ~bit, index + 1, items))
                {
                    yield return r;
                }
                foreach (var r in PermutateAddress(address | bit, index + 1, items))
                {
                    yield return r;
                }
            }
            else
            {
                yield return address;
            }
        }



        private static readonly Regex MaskDecoder = new Regex(@"^mask\s=\s([0|1|X]+)\b");

        private static readonly Regex AssignmentDecoder = new Regex(@"^mem\[([0-9]+)\]\s=\s([0-9]+)\b");
    }
}