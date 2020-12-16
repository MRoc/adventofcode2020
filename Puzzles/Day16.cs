using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzles
{
    // Puzzle 1: 27802
    // Puzzle 2: 0
    public static class Day16
    {
        public static long Puzzle1()
        {
            var parts = Input.Load("Puzzles.Input.input16.txt").Split("\n\n");

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

            return nearby.SelectMany(t => t)
                .Where(n => !rules.Any(r => r.IsContained(n)))
                .Sum();
        }

        public static long Puzzle2()
        {
            return 0;
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
    }
}