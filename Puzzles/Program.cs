using System;
using System.Linq;
using System.Reflection;

namespace Puzzles
{
    public static class Program
    {
        public static void Main()
        {
            foreach (var type in Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Name.StartsWith("Day")))
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
