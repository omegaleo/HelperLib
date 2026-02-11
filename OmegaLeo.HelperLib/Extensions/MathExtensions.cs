using System.Collections.Generic;
using System.Linq;
using OmegaLeo.HelperLib.Shared.Attributes;

namespace OmegaLeo.HelperLib.Extensions
{
    [Documentation("MathExtensions", "Provides extension methods for mathematical operations.")]
    [Changelog("1.2.0", "Fixed root namespace to OmegaLeo.HelperLib.Extensions.", "January 28, 2026")]
    public static class MathExtensions
    {
        /// <summary>
        /// Calculate the average value based on a list of values
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Documentation(nameof(AverageWithNullValidation),
            "Calculates the average of a list of integers, returning 0 if the list is empty.",
            new[] { "list: The list of integers to calculate the average from." },
            @"```csharp
var numbers = new List<int> { 1, 2, 3, 4 };
int average = numbers.AverageWithNullValidation(); // average will be 2 (rounded down)
var emptyList = new List<int>();
int averageEmpty = emptyList.AverageWithNullValidation(); // averageEmpty will be 0
```")]
        public static int AverageWithNullValidation(this IEnumerable<int> list)
        {
            return list.Any() ? (int)list.Average() : 0;
        }

        /// <summary>
        /// Calculate the average value based on a list of values
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Documentation(nameof(AverageWithNullValidation),
            "Calculates the average of a list of doubles, returning 0.0 if the list is empty.",
            new[] { "list: The list of doubles to calculate the average from." },
            @"```csharp
var numbers = new List<double> { 1.5, 2.5, 3.5 };
double average = numbers.AverageWithNullValidation(); // average will be 2.5
var emptyList = new List<double>();
double averageEmpty = emptyList.AverageWithNullValidation(); // averageEmpty will be 0.0
```")]
        public static double AverageWithNullValidation(this IEnumerable<double> list)
        {
            return list.Any() ? list.Average() : 0.0;
        }

        /// <summary>
        /// Calculate the average value based on a list of values
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [Documentation(nameof(AverageWithNullValidation),
            "Calculates the average of a list of floats, returning 0.0f if the list is empty.",
            new[] { "list: The list of floats to calculate the average from." },
            @"```csharp
var numbers = new List<float> { 1.5f, 2.5f, 3.5f };
float average = numbers.AverageWithNullValidation(); // average will be 2.5f
var emptyList = new List<float>();
float averageEmpty = emptyList.AverageWithNullValidation(); // averageEmpty will be 0.0f
```")]
        public static float AverageWithNullValidation(this IEnumerable<float> list)
        {
            return list.Any() ? list.Average() : 0.0f;
        }
    }
}