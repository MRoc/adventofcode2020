using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzles
{
    // Puzzle 1: 11076907812171
    // Puzzle 2: 283729053022731
    public static class Day18
    {
        public static long Puzzle1()
        {
            return Input
                .LoadLines("Puzzles.Input.input18.txt")
                .Select(l => l.Tokenize().ParseRpn(false).EvaluateRpn())
                .Sum();
        }

        public static long Puzzle2()
        {
            return Input
                .LoadLines("Puzzles.Input.input18.txt")
                .Select(l => l.Tokenize().ParseRpn(true).EvaluateRpn())
                .Sum();
        }

        private static char[] Tokenize(this string input)
        {
            return Regex.Split(input, @"([*()\^\/]|(?<!E)[\+\-])")
                .Where(l => !string.IsNullOrEmpty(l.Trim()))
                .Select(l => l.Trim()[0])
                .ToArray();
        }

        private static char[] ParseRpn(this char[] tokens, bool operatorPrececence)
        {
            // https://en.wikipedia.org/wiki/Shunting-yard_algorithm
            
            var output = new Queue<char>();
            var operatorStack = new Stack<char>();

            foreach (var token in tokens)
            {
                if (char.IsNumber(token))
                {
                    output.Enqueue(token);
                }
                else if (token == '+' || token == '*')
                {
                    while (operatorStack.Any()
                           && (!operatorPrececence || token <= operatorStack.Peek())
                           && operatorStack.Peek() != '(')
                    {
                        output.Enqueue(operatorStack.Pop());
                    }

                    operatorStack.Push(token);
                }
                else if (token == '(')
                {
                    operatorStack.Push(token);
                }
                else if (token == ')')
                {
                    while (operatorStack.Any() && operatorStack.Peek() != '(')
                    {
                        output.Enqueue(operatorStack.Pop());
                    }

                    if (operatorStack.Any() && operatorStack.Peek() == '(')
                    {
                        operatorStack.Pop();
                    }
                }
            }

            while (operatorStack.Any())
            {
                output.Enqueue(operatorStack.Pop());
            }

            return output.ToArray();
        }

        private static long EvaluateRpn(this char[] tokens)
        {
            var stack = new Stack<long>();

            foreach (var token in tokens)
            {
                if (token == '+')
                {
                    var x = stack.Pop();
                    var y = stack.Pop();
                    stack.Push(x + y);
                }
                else if (token == '*')
                {
                    var x = stack.Pop();
                    var y = stack.Pop();
                    stack.Push(x * y);
                }
                else
                {
                    stack.Push(token - '0');
                }
            }

            return stack.Pop();
        }
    }
}