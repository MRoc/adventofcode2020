using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 0
    // Puzzle 2: 0
    public static class Day25
    {
        public static long Puzzle1()
        {
            var cardPublicKey = 5764801;
            var doorPublicKey = 17807724;
            return BreakEncryptionKey(cardPublicKey, doorPublicKey);
        }

        public static long Puzzle2()
        {
            return 0;
        }

        private static long BreakEncryptionKey(long cardPublicKey, long doorPublicKey)
        {
            var cardLoopSize = FindLoopSize(cardPublicKey);
            Console.WriteLine($"Card public key...............: {cardPublicKey}");
            Console.WriteLine($"Card loop size................: {cardLoopSize}");
            Console.WriteLine($"Card public key (by transform): {Transform(HandshakeSubjectNumber, cardLoopSize)}");

            var doorLoopSize = FindLoopSize(doorPublicKey);
            Console.WriteLine($"Door public key...............: {doorPublicKey}");
            Console.WriteLine($"Door loop size................: {doorLoopSize}");
            Console.WriteLine($"Door public key (by transform): {Transform(HandshakeSubjectNumber, doorLoopSize)}");

            var cardEncryptionKey = Transform(doorPublicKey, 8);
            Console.WriteLine($"Card encryption key...........: {cardEncryptionKey}");

            var doorEncryptionKey = Transform(cardPublicKey, 11);
            Console.WriteLine($"Door encryption key...........: {doorEncryptionKey}");

            return cardEncryptionKey;
        }

        private static long FindLoopSize(long key)
        {
            var count = 0L;

            var value = 1L;

            while (value != key)
            {
                value = value * HandshakeSubjectNumber % Divider;
                count++;
            }

            return count;
        }

        private static long Transform(long subjectNumber, long loopSize)
        {
            var value = 1L;
            
            for (long i = 0; i < loopSize; ++i)
            {
                value = value * subjectNumber % Divider;
            }

            return value;
        }

        private const int HandshakeSubjectNumber = 7;
        private const int Divider = 20201227;
    }
}
