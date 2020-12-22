using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzles
{
    // Puzzle 1: 31629
    // Puzzle 2: 35196
    public static class Day22
    {
        public static long Puzzle1()
        {
            return Puzzle(recursive: false);
        }

        public static long Puzzle2()
        {
            return Puzzle(recursive: true);
        }

        private static long Puzzle(bool recursive)
        {
            var match = DeckDecoder.Match(Input.Load("Puzzles.Input.input22.txt"));
            
            var (deck0, deck1) = Play(
                match.Groups[2].Captures.Select(c => byte.Parse(c.Value)).ToArray(),
                match.Groups[4].Captures.Select(c => byte.Parse(c.Value)).ToArray(),
                recursive);

            var winningDeck = deck0.Any() ? deck0 : deck1;

            return winningDeck.AsEnumerable().Reverse().Select((n, i) => ((int)n) * (i + 1)).Sum();
        }
        
        private static (byte[], byte[]) Play(byte[] deck0, byte[] deck1, bool recursive)
        {
            var previousRounds = new HashSet<Deck>();

            var queue0 = new Queue<byte>(deck0);
            var queue1 = new Queue<byte>(deck1);

            while (queue0.Any() && queue1.Any())
            {
                var currentDeck = new Deck(queue0.ToArray(), queue1.ToArray());
                if (previousRounds.Contains(currentDeck))
                {
                    return (deck0.Take(1).ToArray(), deck1.Take(0).ToArray());
                }
                previousRounds.Add(currentDeck);

                var card0 = queue0.Dequeue();
                var card1 = queue1.Dequeue();

                var winner = -1;
                if (recursive && queue0.Count >= card0 && queue1.Count >= card1)
                {
                    var (subQueue0, subQueue1) = Play(
                        queue0.Take(card0).ToArray(),
                        queue1.Take(card1).ToArray(),
                        true);
                    winner = subQueue0.Any() ? 0 : 1;
                }
                else if (card0 < card1)
                {
                    winner = 1;
                }
                else if (card1 < card0)
                {
                    winner = 0;
                }

                if (winner == 0)
                {
                    queue0.Enqueue(card0);
                    queue0.Enqueue(card1);
                }
                else if (winner == 1)
                {
                    queue1.Enqueue(card1);
                    queue1.Enqueue(card0);
                }
            }

            return (queue0.ToArray(), queue1.ToArray());
        }

        class Deck : IEquatable<Deck>
        {
            public Deck(byte[] a, byte[] b)
            {
                _a = a;
                _b = b;
                _hashCode = _a.CalculateHashCode() ^ _b.CalculateHashCode();
            }

            private readonly byte[] _a;

            private readonly byte[] _b;

            private readonly int _hashCode;

            public bool Equals(Deck other)
            {
                return other is { }
                       && Enumerable.SequenceEqual(_a, other._a)
                       && Enumerable.SequenceEqual(_b, other._b);
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Deck);
            }

            public override int GetHashCode()
            {
                return _hashCode;
            }
        }

        public static int CalculateHashCode(this byte[] array)
        {
            unchecked
            {
                var result = 0;
                foreach (byte b in array)
                {
                    result = (result * 31) ^ b;
                }
                return result;
            }
        }

        private static Regex DeckDecoder =
            new Regex(@"^Player\s([0-9]):\n([0-9]+\n)+\nPlayer\s([0-9]):\n([0-9]+\n)+");
    }
}
