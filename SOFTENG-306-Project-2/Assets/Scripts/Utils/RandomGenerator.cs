using System;

namespace Utils
{
    public class RandomGenerator
    {
        private static readonly Random random = new Random();

        /// <summary>
        /// Generates a random integer between 0 (inclusive) and the given range (exclusive)
        /// </summary>
        public static int GetNext(int range)
        {
            return random.Next(range);
        }
    }
}