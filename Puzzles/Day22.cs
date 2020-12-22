using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzles
{
    // Puzzle 1: 31629
    // Puzzle 2: 0
    public static class Day22
    {
        public static long Puzzle1()
        {
            var match = DeckDecoder.Match(Input.Load("Puzzles.Input.input22.txt"));

            var (deck0, deck1) = Play(
                match.Groups[2].Captures.Select(c => byte.Parse(c.Value)).ToArray(),
                match.Groups[4].Captures.Select(c => byte.Parse(c.Value)).ToArray());
            
            var winningDeck = deck0.Any() ? deck0 : deck1;

            return winningDeck.AsEnumerable().Reverse().Select((n, i) => ((int)n) * (i + 1)).Sum();
        }

        public static long Puzzle2()
        {
            return 0;
        }
        
        private static (byte[], byte[]) Play(byte[] deck0, byte[] deck1)
        {
            var queue0 = new Queue<byte>(deck0);
            var queue1 = new Queue<byte>(deck1);

            var round = 0;
            
            while (queue0.Any() && queue1.Any())
            {
                var card0 = queue0.Dequeue();
                var card1 = queue1.Dequeue();

                if (card0 < card1)
                {
                    queue1.Enqueue(card1);
                    queue1.Enqueue(card0);
                }
                else if (card1 < card0)
                {
                    queue0.Enqueue(card0);
                    queue0.Enqueue(card1);
                }
                else
                {
                    throw new NotImplementedException();
                }
                
                round++;
            }

            Console.WriteLine($"Round: {round}");

            return (queue0.ToArray(), queue1.ToArray());
        }

        private static Regex DeckDecoder =
            new Regex(@"^Player\s([0-9]):\n([0-9]+\n)+\nPlayer\s([0-9]):\n([0-9]+\n)+");
    }
}
