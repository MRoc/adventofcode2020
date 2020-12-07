using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 6662
    // Puzzle 2: 3382
    public static class Day06
    {
        public static int Puzzle1()
        {
            return Input.Load("Puzzles.Input.input06.txt")
                .Split("\n\n")
                .Select(s => s
                    .Split('\n')
                    .Where(e => !string.IsNullOrEmpty(e))
                    .Select(e => e.ToArray())
                    .Aggregate((a, b) => a.Union(b).ToArray())
                    .Length)
                .Sum();
        }

        public static int Puzzle2()
        {
            return Input.Load("Puzzles.Input.input06.txt")
                .Split("\n\n")
                .Select(s => s
                    .Split('\n')
                    .Where(e => !string.IsNullOrEmpty(e))
                    .Select(e => e.ToArray())
                    .Aggregate((a, b) => a.Intersect(b).ToArray())
                    .Length)
                .Sum();
        }
    }
}
