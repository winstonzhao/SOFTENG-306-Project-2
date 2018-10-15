using System;

namespace Utils
{
    public class RandomGenerator
    {
        private static readonly Random random = new Random();

        public static int GetNext(int range)
        {
            return random.Next(range);
        }
    }
}