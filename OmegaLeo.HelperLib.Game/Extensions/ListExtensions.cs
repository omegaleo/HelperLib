using System.Collections.Generic;
using OmegaLeo.HelperLib.Game.Models;

namespace OmegaLeo.HelperLib.Game.Extensions
{
    public static class ListExtensions
    {
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