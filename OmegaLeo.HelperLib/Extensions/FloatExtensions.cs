using System;

namespace GameDevLibrary.Extensions
{
    public static class FloatExtensions
    {
        public static float Round(this float value, int digits = 2)
        {
            return MathF.Round(value, digits);
        }

        public static int RoundToInt(this float value)
        {
            return (int)value.Round(0);
        }
    }
}