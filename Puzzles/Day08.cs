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
                .Select((e, i) => (Index: i, Instruction: e))
                .Where(t => t.Instruction.Op != "acc")
                .Select(t => (t.Index, (Op: t.Instruction.Op == "jmp" ? "nop" : "jmp", t.Instruction.Val)))
                .ToArray();

            var variations = variationLocations
                .Select(t => instructions.Select((e, i) => i != t.Index ? e : t.Item2).ToArray())
                .ToArray();

            return variations.Select(i => i.RunInstructions()).Where(i => i.Success).Single().Value;
        }

        private static (string Op, int Val)[] LoadInstructions()
        {
            return Input
                .LoadLines("Puzzles.Input.input08.txt")
                .Select(l => l.Split(' '))
                .Select(l => (Op: l[0], Val: int.Parse(l[1])))
                .ToArray();
        }

        private static (bool Success, int Value) RunInstructions(this (string Op, int Val)[] instructions)
        {
            var acc = 0;
            var ip = 0;

            var visitedInstructions = new HashSet<int>();
            while (!visitedInstructions.Contains(ip) && ip != instructions.Length)
            {
                visitedInstructions.Add(ip);

                var instruction = instructions[ip];

                if (instruction.Op == "acc")
                {
                    acc += instruction.Val;
                    ip++;
                }
                else if (instruction.Op == "jmp")
                {
                    ip += instruction.Val;
                }
                else if (instruction.Op == "nop")
                {
                    ip++;
                }
            }

            return (Success: ip == instructions.Length, Value: acc);
        }
    }
}
