using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzles
{
    // Puzzle 1: 10885823581193
    // Puzzle 2: 0
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
                    var mask = match
                        .Groups[1]
                        .Value
                        .AsEnumerable()
                        .Reverse()
                        .Select((b, i) => (b: b, i: i))
                        .Where(t => t.b != 'X')
                        .ToArray();

                    maskOr = mask
                        .Where(t => t.b == '1')
                        .Select(t => 0x1UL << t.i)
                        .Aggregate((a, b) => a | b);

                    maskAnd = ~mask
                        .Where(t => t.b == '0')
                        .Select(t => 0x1UL << t.i)
                        .Aggregate((a, b) => a | b);
                }
                else
                {
                    var result = AssignmentDecoder.Match(line);
                    var address = int.Parse(result.Groups[1].Value);
                    var value = long.Parse(result.Groups[2].Value);

                    memory[address] = value & (long) maskAnd | (long) maskOr;
                }
            } 

            return memory.Select(i => i.Value).Sum();
        }

        private static readonly Regex MaskDecoder = new Regex(@"^mask\s=\s([0|1|X]+)\b");

        private static readonly Regex AssignmentDecoder = new Regex(@"^mem\[([0-9]+)\]\s=\s([0-9]+)\b");

        public static long Puzzle2()
        {
            return 0;
        }
    }
}