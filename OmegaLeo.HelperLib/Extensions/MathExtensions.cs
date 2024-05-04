using System.Collections.Generic;
using System.Linq;

namespace GameDevLibrary.Extensions
{
    public static class MathExtensions
    {
        /// <summary>
        /// Calculate the average value based on a list of values
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int AverageWithNullValidation(this IEnumerable<int> list)
        {
            return list.Any() ? (int)list.Average() : 0;
        }

        /// <summary>
        /// Calculate the average value based on a list of values
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static double AverageWithNullValidation(this IEnumerable<double> list)
        {
            return list.Any() ? list.Average() : 0.0;
        }

        /// <summary>
        /// Calculate the average value based on a list of values
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static float AverageWithNullValidation(this IEnumerable<float> list)
        {
            return list.Any() ? list.Average() : 0.0f;
        }
    }
}