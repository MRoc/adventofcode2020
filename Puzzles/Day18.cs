﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzles
{
    // Puzzle 1: 11076907812171
    // Puzzle 2: 0
    public static class Day18
    {
        public static long Puzzle1()
        {
            return Input
                .LoadLines("Puzzles.Input.input18.txt")
                .Select(l => l.Tokenize().ParseRPN().EvaluateRPN())
                .Sum();
        }

        public static long Puzzle2()
        {
            return 0;
        }

        private static string[] Tokenize(this string input)
        {
            return Regex.Split(input, @"([*()\^\/]|(?<!E)[\+\-])")
                .Where(l => !string.IsNullOrEmpty(l.Trim()))
                .Select(l => l.Trim())
                .ToArray();
        }

        private static string[] ParseRPN(this string[] tokens)
        {
            // https://en.wikipedia.org/wiki/Shunting-yard_algorithm
            
            var output = new Queue<string>();
            var operatorStack = new Stack<string>();

            foreach (var token in tokens)
            {
                if (Char.IsNumber(token[0]))
                {
                    output.Enqueue(token);
                }
                else if (token == "+" || token == "*")
                {
                    while (operatorStack.Any() && operatorStack.Peek() != "(")
                    {
                        output.Enqueue(operatorStack.Pop());
                    }

                    operatorStack.Push(token);
                }
                else if (token == "(")
                {
                    operatorStack.Push(token);
                }
                else if (token == ")")
                {
                    while (operatorStack.Any() && operatorStack.Peek() != "(")
                    {
                        output.Enqueue(operatorStack.Pop());
                    }

                    if (operatorStack.Any() && operatorStack.Peek() == "(")
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
        
        private static long EvaluateRPN(this string[] tokens)
        {
            var stack = new Stack<string>();

            foreach (var token in tokens)
            {
                if (token == "+")
                {
                    var x = long.Parse(stack.Pop());
                    var y = long.Parse(stack.Pop());
                    stack.Push((x + y).ToString());
                }
                else if (token == "*")
                {
                    var x = long.Parse(stack.Pop());
                    var y = long.Parse(stack.Pop());
                    stack.Push((x * y).ToString());
                }
                else
                {
                    stack.Push(token);
                }
            }

            return long.Parse(stack.Pop());
        }
    }
}