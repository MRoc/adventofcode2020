using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace Puzzles
{
    // Puzzle 1: 151
    // Puzzle 2: 0
    public static class Day19
    {
        public static long Puzzle1()
        {
            var input = Input.Load("Puzzles.Input.input19.txt").Split("\n\n");

            var rules = input[0]
                .Split("\n")
                .Select(l => Rule.Parse(l))
                .OrderBy(r => r.Index)
                .ToArray();

            return input[1]
                .Split("\n")
                .Select(m => Parse(m, 0, rules, 0) == m.Length)
                .Count(v => v);
        }

        private static int Parse(string input, int inputIndex, Rule[] rules, int ruleIndex)
        {
            var rule = rules[ruleIndex];
            if (rule.Symbol != 0)
            {
                return inputIndex < input.Length && input[inputIndex] == rule.Symbol
                    ? inputIndex + 1
                    : -1;
            }
            else
            {
                var tmpIndex = inputIndex;
                for (int i = 0; i < rule.Index0.Length && tmpIndex != -1; ++i)
                {
                    var ri = rule.Index0[i];
                    tmpIndex = Parse(input, tmpIndex, rules, ri);
                }

                if (tmpIndex > 0)
                {
                    return tmpIndex;
                }
                    
                if (rule.Index1 is { })
                {
                    tmpIndex = inputIndex;
                    for (int i = 0; i < rule.Index1.Length && tmpIndex != -1; ++i)
                    {
                        var ri = rule.Index1[i];
                        tmpIndex = Parse(input, tmpIndex, rules, ri);
                    }

                    if (tmpIndex > 0)
                    {
                        return tmpIndex;
                    }
                }
            }

            return -1;
        }

        public static long Puzzle2()
        {
            return 0;
        }

        public record Rule(int Index, char Symbol, int[] Index0, int[] Index1)
        {
            public static Rule Parse(string line)
            {
                var parts = line.Split(": ");

                var index = int.Parse(parts[0]);
                var symbol = default(char);
                var index0 = default(int[]);
                var index1 = default(int[]);

                if (parts[1].StartsWith("\""))
                {
                    symbol = parts[1][1];
                }
                else
                {
                    if (parts[1].Contains("|"))
                    {
                        var subParts = parts[1].Split(" | ");
                        index0 = subParts[0].Split(" ").Select(int.Parse).ToArray();
                        index1 = subParts[1].Split(" ").Select(int.Parse).ToArray();
                    }
                    else
                    {
                        index0 = parts[1].Split(" ").Select(int.Parse).ToArray();
                    }
                }

                return new Rule(index, symbol, index0, index1);
            }
        }
    }
}