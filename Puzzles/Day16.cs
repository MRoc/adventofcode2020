using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzles
{
    // Puzzle 1: 27802
    // Puzzle 2: 279139880759
    public static class Day16
    {
        public static long Puzzle1()
        {
            var (rules, mine, nearby) = Input
                .Load("Puzzles.Input.input16.txt")
                .ParseInput();

            return nearby.SelectMany(t => t)
                .Where(n => !rules.Any(r => r.IsContained(n)))
                .Sum();
        }

        public static long Puzzle2()
        {
            var (rules, mine, nearby) = Input
                .Load("Puzzles.Input.input16.txt")
                .ParseInput();
            
            var validNearby = nearby
                .Where(n => n.All(n => rules.Any(r => r.IsContained(n))))
                .ToArray();

            var values = validNearby.Regroup();

            var possibleMatches = rules
                .Select((r, ri) =>
                    values
                        .Select((v, vi) => (v, vi))
                        .Where(t => r.IsContained(t.v))
                        .Select(t => t.vi)
                        .ToList())
                .ToArray();
            
            var fixedColumn = new HashSet<int>();
            while (possibleMatches.Any(c => c.Count > 1))
            {
                var nextCandidate = -1;
                for (int i = 0; i < possibleMatches.Length && nextCandidate == -1; ++i)
                {
                    if (possibleMatches[i].Count == 1
                        && !fixedColumn.Contains(possibleMatches[i][0]))
                    {
                        nextCandidate = possibleMatches[i][0];
                        fixedColumn.Add(nextCandidate);
                    }
                }

                if (nextCandidate != -1)
                {
                    foreach (var t in possibleMatches.Where(t => t.Count != 1))
                    {
                        t.Remove(nextCandidate);
                    }
                }
            }

            return rules
                .Select((r, ri) => (r, ri))
                .Where(t => t.r.Title.StartsWith("departure"))
                .Select(t => mine[possibleMatches[t.ri][0]])
                .Aggregate(1L, (a, b) => a * b);
        }

        private static (Rule[] rules, int[] mine, int[][] nearby) ParseInput(this string input)
        {
            var parts = input
                .Split("\n\n");
            
            var rules = parts[0]
                .Split('\n')
                .Select(Rule.Parse)
                .ToArray();

            var mine = parts[1]
                .Split('\n')
                .Skip(1)
                .First()
                .ParseCommaSeparatedInt();

            var nearby = parts[2]
                .Split('\n')
                .Where(l => !string.IsNullOrEmpty(l))
                .Skip(1)
                .Select(l => l.ParseCommaSeparatedInt())
                .ToArray();
            
            return (rules, mine, nearby);
        }

        record Rule(string Title, Range Lower, Range Upper)
        {
            public static Rule Parse(string line)
            {
                var result = RuleDecoder.Match(line);
                
                return new Rule(
                    result.Groups[1].Value,
                    Range.Parse(result.Groups[2].Value, result.Groups[3].Value),
                    Range.Parse(result.Groups[4].Value, result.Groups[5].Value));
            }

            public bool IsContained(int value) => Lower.IsContained(value) || Upper.IsContained(value);

            public bool IsContained(int[] values) => values.All(v => IsContained(v));
        }

        record Range(int Min, int Max)
        {
            public static Range Parse(string min, string max)
            {
                return new Range(int.Parse(min), int.Parse(max));
            }

            public bool IsContained(int value) => Min <= value && value <= Max;
        }

        private static readonly Regex RuleDecoder = new Regex(@"^([\w\s]*):\s([0-9]+)-([0-9]+)\sor\s([0-9]+)-([0-9]+)\b");

        private static int[] ParseCommaSeparatedInt(this string text)
        {
            return text.Split(',').Select(int.Parse).ToArray();
        }

        private static int[][] Regroup(this int[][] tickets)
        {
            var result = new int[tickets[0].Length][];
            
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = new int[tickets.Length];
            }

            for (int i = 0; i < tickets.Length; ++i)
            {
                for (int j = 0; j < tickets[i].Length; ++j)
                {
                    result[j][i] = tickets[i][j];
                }
            }

            return result;
        }
    }
}