using System.Collections.Generic;
using OmegaLeo.HelperLib.Shared.Attributes;
using OmegaLeo.HelperLib.Game.Models;

namespace OmegaLeo.HelperLib.Game.Extensions
{
    public static class ListExtensions
    {
        [Documentation(nameof(Shuffle), 
            "Shuffles the elements of the list in place using the provided RandomNumberGenerator.", 
            null, 
            @"```csharp
var list = new List<int> { 1, 2, 3, 4};
var rng = new RandomNumberGenerator();
// List before: [1, 2, 3, 4]
list.Shuffle(rng);
// List after: [3, 1, 4, 2] (example output)
```")]
        public static void Shuffle<T>(this IList<T> list, RandomNumberGenerator rng)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rng.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}