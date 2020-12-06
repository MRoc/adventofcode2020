using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Puzzle01
{
    // https://adventofcode.com/2020/day/1
    static class Program
    {
        static void Main(string[] _)
        {
            var numbers = LoadInput()
                .Split('\n')
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => int.Parse(l))
                .Distinct()
                .ToArray();

            foreach (var n0 in numbers)
            {
                foreach (var n1 in numbers)
                {
                    foreach (var n2 in numbers)
                    {
                        if (n0 + n1 + n2 == 2020)
                        {
                            Console.WriteLine($"{n0}+{n1}+{n2}=2020 -> {n0}*{n1}*{n2}={n0 * n1 * n2}");
                        }
                    }
                }
            }

            Console.ReadKey();
        }

        private static string LoadInput()
        {
            var assembly = Assembly.GetCallingAssembly();
            using var stream = assembly.GetManifestResourceStream("Puzzle01.input.txt");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
