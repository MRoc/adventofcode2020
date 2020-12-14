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
            var memory = new Dictionary<long, long>();

            var maskOr = default(long);
            var maskAnd = default(long);

            foreach (var line in Input.LoadLines("Puzzles.Input.input14.txt"))
            {
                var match = MaskDecoder.Match(line);
                if (match.Success)
                {
                    var mask = match.Groups[1].Value;
                    maskOr = Convert.ToInt64(mask.Replace('X', '0'), 2);
                    maskAnd = ~Convert.ToInt64(mask.Replace('1', 'X').Replace('0', '1').Replace('X', '0'), 2);
                }
                else
                {
                    var result = AssignmentDecoder.Match(line);
                    var address = long.Parse(result.Groups[1].Value);
                    var value = long.Parse(result.Groups[2].Value);
                    memory[address] = value & maskAnd | maskOr;
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
                    var address = long.Parse(result.Groups[1].Value);
                    var value = long.Parse(result.Groups[2].Value);
                    
                    foreach (var singleAddress in PermuteAddress(
                        address | Convert.ToInt64(mask.Replace('X', '0'), 2),
                        mask
                            .Reverse()
                            .Select((c, i) => (c, i))
                            .Where(t => t.c == 'X')
                            .Select(t => 0x1L << t.i)
                            .ToArray()))
                    {
                        memory[singleAddress] = value;
                    }
                }
            }

            return memory.Values.Sum();
        }

        private static IEnumerable<long> PermuteAddress(long address, IEnumerable<long> floatingBits)
        {
            if (floatingBits.Any())
            {
                var bit = floatingBits.First();

                foreach (var r in PermuteAddress(address & ~bit, floatingBits.Skip(1)))
                {
                    yield return r;
                }
                foreach (var r in PermuteAddress(address | bit, floatingBits.Skip(1)))
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