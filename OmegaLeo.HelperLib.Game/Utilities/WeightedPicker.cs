using System.Collections.Generic;
using System.Linq;
using NetFlow.DocumentationHelper.Library.Attributes;
using OmegaLeo.HelperLib.Game.Models;

namespace OmegaLeo.HelperLib.Game.Utilities
{
    public static class WeightedPicker
    {
        [Documentation("WeightedPicker.Pick", "Picks a random item from a dictionary of items with associated weights.")]
        public static T Pick<T>(RandomNumberGenerator rng, Dictionary<T, float> weights)
        {
            float total = weights.Values.Sum();
            float roll = rng.Range(0f, total);

            foreach (var pair in weights)
            {
                roll -= pair.Value;
                if (roll <= 0)
                    return pair.Key;
            }

            return weights.Keys.Last(); // fallback
        }
    }
}