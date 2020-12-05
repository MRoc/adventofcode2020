using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Puzzle02
{
    class Program
    {
        static void Main(string[] _)
        {
            var entries = LoadInput()
                .Split('\n')
                .Where(l => !string.IsNullOrEmpty(l))
                .Select(l => new Entry(l))
                .ToArray();

            var validEntries1 = entries.Count(e => e.IsValid1());
            Console.WriteLine($"There are {validEntries1} valid1 passwords!");

            var validEntries2 = entries.Count(e => e.IsValid2());
            Console.WriteLine($"There are {validEntries2} valid2 passwords!");
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

        private static string LoadInput()
        {
            var assembly = Assembly.GetCallingAssembly();
            using var stream = assembly.GetManifestResourceStream("Puzzle02.input_pw.txt");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }

    static class Extensions
    {
        public static bool Has(this string text, int index, char c)
        {
            return index < text.Length && text[index] == c;
        }
    }
}
