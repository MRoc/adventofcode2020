using System.Collections.Generic;
using System.Linq;

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
                .ToDictionary(r => r.Index, r => r);

            return input[1]
                .Split("\n")
                .Where(l => !string.IsNullOrEmpty(l))
                .Select(m => Parse(m, 0, rules, 0) == m.Length)
                .Count(v => v);
        }

        public static long Puzzle2()
        {
            var input = Input.Load("Puzzles.Input.input19.txt").Split("\n\n");

            var rules = input[0]
                .Split("\n")
                .Select(l => Rule.Parse(l))
                .ToDictionary(r => r.Index, r => r);

            rules[8] = Rule.Parse("8: 42 | 42 8");
            rules[11] = Rule.Parse("11: 42 31 | 42 11 31");

            // 211 is the wrong answer

            return input[1]
                .Split("\n")
                .Where(l => !string.IsNullOrEmpty(l))
                .Select(m => Parse(m, 0, rules, 0) >= m.Length)
                .Count(v => v);
        }
        
        private static int Parse(string input, int index, Dictionary<int, Rule> rules, int ruleIndex)
        {
            var rule = rules[ruleIndex];
            if (rule.Symbol != 0)
            {
                if (index < input.Length && input[index] == rule.Symbol)
                {
                    return index + 1;
                }
            }
            else
            {
                var tmpIndex0 = index;
                for (int i = 0; i < rule.Index0.Length && tmpIndex0 != -1; ++i)
                {
                    tmpIndex0 = Parse(input, tmpIndex0, rules, rule.Index0[i]);
                }

                if (tmpIndex0 > 0)
                {
                    return tmpIndex0;
                }

                if (rule.Index1 is { })
                {
                    var tmpIndex1 = index;
                    for (int i = 0; i < rule.Index1.Length && tmpIndex1 != -1; ++i)
                    {
                        tmpIndex1 = Parse(input, tmpIndex1, rules, rule.Index1[i]);
                    }

                    if (tmpIndex1 > 0)
                    {
                        return tmpIndex1;
                    }
                }
            }

            return -1;
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