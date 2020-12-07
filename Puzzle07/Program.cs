using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Puzzle07
{
    static class Program
    {
        // Puzzle 1: 115
        // Puzzle 2: 1250

        static void Main(string[] args)
        {
            Console.WriteLine($"Puzzle 1: {Puzzle1()}");
            Console.WriteLine($"Puzzle 2: {Puzzle2()}");
        }

        private static int Puzzle1()
        {
            return LoadInput()
                .Split('\n')
                .Where(l => !string.IsNullOrEmpty(l))
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

        private static int Puzzle2()
        {
            return LoadInput()
                .Split('\n')
                .Where(l => !string.IsNullOrEmpty(l))
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

        public class Rule
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


        private static string LoadInput()
        {
            using var stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Puzzle07.input07.txt");
            if (stream is null)
            {
                throw new InvalidOperationException($"Could not load resource!");
            }

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd().Replace("\r", string.Empty);
        }
    }
}
