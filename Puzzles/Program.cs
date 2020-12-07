using System;

namespace Puzzles
{
    public static class Program
    {
        public static void Main()
        {
            foreach (var type in new []
            {
                typeof(Day01),
                typeof(Day01),
                typeof(Day03),
                typeof(Day04),
                typeof(Day05),
                typeof(Day06),
                typeof(Day07),
            })
            {
                Console.WriteLine(type.Name);
                Console.WriteLine($"Puzzle 1: {type.GetMethod("Puzzle1")?.Invoke(null, null)}");
                Console.WriteLine($"Puzzle 2: {type.GetMethod("Puzzle2")?.Invoke(null, null)}");
            }
        }
    }
}
