using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 1930
    // Puzzle 2: 1688
    public static class Day08
    {
        public static int Puzzle1()
        {
            return LoadInstructions().RunInstructions().Item2;
        }

        public static int Puzzle2()
        {
           var instructions = LoadInstructions();

            var variationLocations = instructions
                .Select((e, i) => Tuple.Create(i, e))
                .Where(t => t.Item2.Item1 != "acc")
                .Select(t => Tuple.Create(t.Item1, Tuple.Create(t.Item2.Item1 == "jmp" ? "nop" : "jmp", t.Item2.Item2)))
                .ToArray();

            var variations = variationLocations
                .Select(t => instructions.Select((e, i) => i != t.Item1 ? e : t.Item2).ToArray())
                .ToArray();

            return variations.Select(i => i.RunInstructions()).Where(i => i.Item1).Single().Item2;
        }

        private static Tuple<string, int>[] LoadInstructions()
        {
            return Input
                .LoadLines("Puzzles.Input.input08.txt")
                .Select(l => l.Split(' '))
                .Select(l => Tuple.Create(l[0], int.Parse(l[1])))
                .ToArray();
        }

        private static Tuple<bool, int> RunInstructions(this Tuple<string, int>[] instructions)
        {
            var acc = 0;
            var ip = 0;

            var visitedInstructions = new HashSet<int>();
            while (!visitedInstructions.Contains(ip) && ip != instructions.Length)
            {
                visitedInstructions.Add(ip);

                var instruction = instructions[ip];

                if (instruction.Item1 == "acc")
                {
                    acc += instruction.Item2;
                    ip++;
                }
                else if (instruction.Item1 == "jmp")
                {
                    ip += instruction.Item2;
                }
                else if (instruction.Item1 == "nop")
                {
                    ip++;
                }
            }

            return Tuple.Create(ip == instructions.Length, acc);
        }
    }
}
