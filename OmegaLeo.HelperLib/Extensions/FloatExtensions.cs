using System;
using OmegaLeo.HelperLib.Shared.Attributes;

namespace OmegaLeo.HelperLib.Extensions
{
    [Documentation(nameof(FloatExtensions), "Provides extension methods for the float data type.", null, 
        "")]
    [Changelog("1.2.0", "Fixed root namespace to OmegaLeo.HelperLib.Extensions.", "January 28, 2026")]
    public static class FloatExtensions
    {
        [Documentation(nameof(Round), "Rounds a float value to the specified number of decimal places.",
            new[]
            {
                "value: The float value to round.",
                "digits: The number of decimal places to round to. Default is 2."
            }, 
            @"```cs
var originalValue = 3.14159f;
var roundedValue = originalValue.Round(2); // roundedValue will be 3.14
```
")]
        public static float Round(this float value, int digits = 2)
        {
            return MathF.Round(value, digits);
        }

        [Documentation(nameof(RoundToInt), "Rounds a float value to the nearest integer and returns it as an int.",
            new[]
            {
                "value: The float value to round.",
            }, 
            @"```cs
var originalValue = 3.14159f;
var roundedValue = originalValue.RoundToInt(); // roundedValue will be 3
```
")]
        public static int RoundToInt(this float value)
        {
            return (int)value.Round(0);
        }
    }
}