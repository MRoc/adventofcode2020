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
                typeof(Day02),
                typeof(Day03),
                typeof(Day04),
                typeof(Day05),
                typeof(Day06),
                typeof(Day07),
                typeof(Day08),
                typeof(Day09),
                typeof(Day10),
                typeof(Day11),
            })
            {
                Console.WriteLine(type.Name);

                foreach (var puzzle in new[]
                {
                    "Puzzle1",
                    "Puzzle2"
                })
                {
                    var (result, duration) = Invoke(type, puzzle);
                    Console.WriteLine($"{puzzle}: {result} ({(int)duration.TotalMilliseconds}ms)");
                }   
            }
        }

        private static (object result, TimeSpan duration) Invoke(Type t, string name)
        {
            var start = DateTime.UtcNow;
            var result = t.GetMethod(name)?.Invoke(null, null);
            var duration = DateTime.UtcNow.Subtract(start);
            return (result, duration);
        }
    }
}
