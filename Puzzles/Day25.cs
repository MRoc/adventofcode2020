
namespace Puzzles
{
    // Puzzle 1: 3015200
    // Puzzle 2: 0
    public static class Day25
    {
        public static long Puzzle1()
        {
            var cardPublicKey = 12090988;
            var doorPublicKey = 240583;

            var cardLoopSize = FindLoopSize(cardPublicKey);
            var cardEncryptionKey = Transform(doorPublicKey, cardLoopSize);
            return cardEncryptionKey;
        }

        public static long Puzzle2()
        {
            return 0;
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
