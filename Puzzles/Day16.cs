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

            return rules
                .CreatePossibleMatches(validNearby.Transpose())
                .ReducePossibleMatches()
                .Where(t => t.Key.Title.StartsWith("departure"))
                .Select(t => mine[t.Value])
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

        private static Dictionary<Rule, List<int>> CreatePossibleMatches(this IEnumerable<Rule> rules, int[][] values)
        {
            return rules
                .ToDictionary(
                    r => r,
                    r => values
                        .Select((v, vi) => (v, vi))
                        .Where(t => r.IsContained(t.v))
                        .Select(t => t.vi)
                        .ToList());
        }

        private static Dictionary<Rule, int> ReducePossibleMatches(this Dictionary<Rule, List<int>> possibleMatches)
        {
            var fixedColumn = new HashSet<int>();
            
            while (possibleMatches.Any(c => c.Value.Count > 1))
            {
                var nextCandidate = -1;
                foreach (var possibleMatch in possibleMatches
                    .Where(pm => pm.Value.Count == 1
                                 && !fixedColumn.Contains(pm.Value.First())))
                {
                    nextCandidate = possibleMatch.Value.First();
                }

                if (nextCandidate != -1)
                {
                    fixedColumn.Add(nextCandidate);
                    foreach (var t in possibleMatches.Where(t => t.Value.Count > 1))
                    {
                        t.Value.Remove(nextCandidate);
                    }
                }
            }
            
            return possibleMatches
                .ToDictionary(
                    t => t.Key,
                    t => t.Value.First());
        }

        private static int[][] Transpose(this int[][] array)
        {
            var result = new int[array[0].Length][];

            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = new int[array.Length];
            }

            for (int i = 0; i < array.Length; ++i)
            {
                for (int j = 0; j < array[i].Length; ++j)
                {
                    result[j][i] = array[i][j];
                }
            }

            return result;
        }
    }
}