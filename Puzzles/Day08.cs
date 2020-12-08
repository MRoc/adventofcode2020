using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 1930
    // Puzzle 2: 1688
    public static class Day08
    {
        public static int Puzzle1()
        {
            return LoadInstructions()
                .RunInstructions()
                .Value;
        }

        public static int Puzzle2()
        {
           var instructions = LoadInstructions();

            return instructions
                .Select((e, i) => (Index: i, Instruction: e))
                .Where(t => t.Instruction.Op != "acc")
                .Select(t => (t.Index, (Op: t.Instruction.Op == "jmp" ? "nop" : "jmp", t.Instruction.Val)))
                .Select(t => instructions.Select((e, i) => i != t.Index ? e : t.Item2).ToArray())
                .Select(i => i.RunInstructions())
                .Single(i => i.Success)
                .Value;
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

            var visited = new bool[instructions.Length];
            while (ip != instructions.Length && !visited[ip])
            {
                visited[ip] = true;

                var (op, val) = instructions[ip];

                if (op == "acc")
                {
                    acc += val;
                    ip++;
                }
                else if (op == "jmp")
                {
                    ip += val;
                }
                else if (op == "nop")
                {
                    ip++;
                }
            }

            return (Success: ip == instructions.Length, Value: acc);
        }
    }
}
