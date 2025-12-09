using System;

namespace OmegaLeo.HelperLib.Game.Models
{
    public class RandomNumberGenerator
    {
        private readonly Random _random;

        public int Seed { get; }

        public RandomNumberGenerator(int? seed = null)
        {
            Seed = seed ?? Environment.TickCount;
            _random = new Random(Seed);
        }

        public float NextFloat() =>
            (float)_random.NextDouble();

        public float Range(float min, float max) =>
            min + NextFloat() * (max - min);

        public int Range(int min, int max) =>
            _random.Next(min, max);

        public bool Chance(float probability) =>
            NextFloat() <= probability;

        public bool Percent(float percent) =>
            Chance(percent / 100f);

        public bool OneIn(int x) =>
            Range(0, x) == 0;
    }
}