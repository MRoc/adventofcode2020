using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 151
    // Puzzle 2: 386
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
                .Select(m => Parse(m, 0, rules, 0).Any(l => l == m.Length))
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
                .Select(m => Parse(m, 0, rules, 0).Any(l => l == m.Length))
                .Count(v => v);
        }

        private static IEnumerable<int> Parse(string input, int index, Dictionary<int, Rule> rules, int ruleIndex)
        {
            var rule = rules[ruleIndex];
            if (rule.Symbol != 0)
            {
                if (index < input.Length && input[index] == rule.Symbol)
                {
                    yield return index + 1;
                }
            }
            else
            {
                foreach (var subRules in rule.Indexes)
                {
                    IEnumerable<int> startIndex = new int[] { index };
                    foreach (var tmpIndex in subRules.Aggregate(startIndex, (current, subRule) => current.SelectMany(j => Parse(input, j, rules, subRule))))
                    {
                        yield return tmpIndex;
                    }
                }
            }
        }
        
        public record Rule(int Index, char Symbol, int[][] Indexes)
        {
            public static Rule Parse(string line)
            {
                var parts = line.Split(": ");

                var index = int.Parse(parts[0]);
                var symbol = default(char);
                var indexes = default(int[][]);

                if (parts[1].StartsWith("\""))
                {
                    symbol = parts[1][1];
                }
                else
                {
                    indexes = parts[1].Split(" | ")
                        .Select(p => p.Split(" ").Select(int.Parse).ToArray())
                        .ToArray();
                }

                return new Rule(index, symbol, indexes);
            }
        }
    }
}