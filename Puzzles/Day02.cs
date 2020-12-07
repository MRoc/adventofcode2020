using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 424
    // Puzzle 2: 747
    public static class Day02
    {
        public static int Puzzle1()
        {
            return Input.LoadLines("Puzzles.Input.input02.txt")
                .Select(l => new Entry(l))
                .Count(e => e.IsValid1());
        }

        public static int Puzzle2()
        {
            return Input.LoadLines("Puzzles.Input.input02.txt")
                .Select(l => new Entry(l))
                .Count(e => e.IsValid2());
        }

        class Entry
        {
            public Entry(string line)
            {
                var segments = line
                    .Split(' ')
                    .SelectMany(l => l.Split('-'))
                    .SelectMany(l => l.Split(':'))
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToArray();

                Min = int.Parse(segments[0]);
                Max = int.Parse(segments[1]);
                Character = segments[2].First();
                Password = segments[3];
            }

            private int Min { get; }

            private int Max { get; }

            private char Character { get; }

            private string Password { get; }

            public bool IsValid1()
            {
                var count = Password.Count(c => c == Character);
                return Min <= count && count <= Max;
            }

            public bool IsValid2()
            {
                return Password.Has(Min - 1, Character) ^ Password.Has(Max - 1, Character);
            }

            public override string ToString()
            {
                return $"{Min}-{Max} {Character}: {Password}";
            }
        }

        public static bool Has(this string text, int index, char c)
        {
            return index < text.Length && text[index] == c;
        }
    }
}
