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

            var deck0 = new Queue<int>(match.Groups[2].Captures.Select(c => int.Parse(c.Value)));
            var deck1 = new Queue<int>(match.Groups[4].Captures.Select(c => int.Parse(c.Value)));

            while (deck0.Any() && deck1.Any())
            {
                var card0 = deck0.Dequeue();
                var card1 = deck1.Dequeue();

                if (card0 < card1)
                {
                    deck1.Enqueue(card1);
                    deck1.Enqueue(card0);
                }
                else if (card1 < card0)
                {
                    deck0.Enqueue(card0);
                    deck0.Enqueue(card1);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            var winningDeck = deck0.Any() ? deck0 : deck1;

            return winningDeck.AsEnumerable().Reverse().Select((n, i) => n * (i + 1)).Sum();
        }

        public static long Puzzle2()
        {
            return 0;
        }

        private static Regex DeckDecoder =
            new Regex(@"^Player\s([0-9]):\n([0-9]+\n)+\nPlayer\s([0-9]):\n([0-9]+\n)+");
    }
}
