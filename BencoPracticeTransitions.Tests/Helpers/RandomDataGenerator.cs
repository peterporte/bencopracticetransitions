using System;

namespace BencoPracticeTransitions.Tests.Helpers
{
    public static class RandomDataGenerator
    {
        private static readonly Random Random;

        static RandomDataGenerator()
        {
            Random = new Random(DateTime.Now.Millisecond);
        }

        public static string RandomPhoneNumber()
        {
            return $"{Random.Next(200, 999)}-{Random.Next(100, 999)}-{Random.Next(1000, 9999)}";
        }


        public static decimal RandomCurrency(decimal minValue, decimal maxValue)
        { // TODO investigate this algorithm. It may only generate min <= x < max instead of min <= x <=max
            return (((long)(Convert.ToDecimal(Random.NextDouble()) * (maxValue - minValue) * 100)) / 100.00M) + minValue;
        }

        public static long RandomLong(long minValue, long maxValue)
        {
            // TODO investigate this algorithm. It may only generate min <= x < max instead of min <= x <=max
            return ((long)(Random.NextDouble() * (maxValue - minValue))) + minValue; 
        }

        public static long RandomInt(int minValue, int maxValue)
        {
            return Random.Next(minValue, maxValue);
        }

    }
}
