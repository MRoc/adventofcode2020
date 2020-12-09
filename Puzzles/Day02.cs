using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 424
    // Puzzle 2: 747
    public static class Day02
    {
        public static int Puzzle1()
        {
            return Input
                .LoadLines("Puzzles.Input.input02.txt")
                .Select(Entry.Parse)
                .Count(e => e.IsValid1());
        }

        public static int Puzzle2()
        {
            return Input
                .LoadLines("Puzzles.Input.input02.txt")
                .Select(Entry.Parse)
                .Count(e => e.IsValid2());
        }

        public record Entry(int Min, int Max, char Character, string Password)
        {
            public static Entry Parse(string line)
            {
                var segments = line
                    .Split(' ')
                    .SelectMany(l => l.Split('-'))
                    .SelectMany(l => l.Split(':'))
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToArray();

                return new Entry(
                    int.Parse(segments[0]),
                    int.Parse(segments[1]),
                    segments[2].First(),
                    segments[3]);
            }

            public bool IsValid1()
            {
                var count = Password.Count(c => c == Character);
                return Min <= count && count <= Max;
            }

            public bool IsValid2()
            {
                return Password.Has(Min - 1, Character) ^ Password.Has(Max - 1, Character);
            }
        }

        public static bool Has(this string text, int index, char c)
        {
            return index < text.Length && text[index] == c;
        }
    }
}
