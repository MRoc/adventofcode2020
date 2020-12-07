using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{       
    // Puzzle 1: 115
    // Puzzle 2: 1250
    public static class Day07
    {
        public static int Puzzle1()
        {
            return Input.LoadLines("Puzzles.Input.input07.txt")
                .Select(l => new Rule(l))
                .ToArray()
                .FindParents("shiny gold")
                .Distinct()
                .Count();
        }

        private static IEnumerable<Rule> FindParents(this IReadOnlyCollection<Rule> rules, string color)
        {
            var rulesContainingColor = rules.Where(r => r.Contains.ContainsKey(color)).ToArray();
            return rulesContainingColor.Concat(rulesContainingColor.SelectMany(r => FindParents(rules, r.Color)));
        }

        public static int Puzzle2()
        {
            return Input.LoadLines("Puzzles.Input.input07.txt")
                .Select(l => new Rule(l))
                .ToArray()
                .SumChildren("shiny gold") - 1;
        }

        private static int SumChildren(this IReadOnlyCollection<Rule> rules, string color)
        {
            return 1 + rules
                .Single(r => r.Color == color)
                .Contains
                .Sum(i => i.Value * rules.SumChildren(i.Key));
        }

        private class Rule
        {
            public Rule(string text)
            {
                // "dim red bags contain 2 bright gold bags, 5 striped fuchsia bags."

                var parts = text
                    .Split(" bags contain ")
                    .SelectMany(s => s.Split(", "))
                    .Select(s => s
                        .Replace(" bags", string.Empty)
                        .Replace(" bag", string.Empty)
                        .Replace(".", string.Empty))
                    .ToArray();

                Color = parts.First();

                if (parts[1] != "no other")
                {
                    Contains = parts
                        .Skip(1)
                        .ToDictionary(
                            s => s.Substring(2),
                            s => int.Parse(s.Substring(0, 1)));
                }
            }

            public string Color { get; }

            public IDictionary<string, int> Contains { get; } = new Dictionary<string, int>();
        }
    }
}
