using System.Collections.Generic;
using System.Linq;

namespace OmegaLeo.HelperLib.Game.Models
{
    public class LootTable<T>
    {
        public List<Loot<T>> Loot = new List<Loot<T>>();

        public void Add(T item, float weight)
        {
            Loot.Add(new Loot<T>(item, weight));
        }

        public T Roll(RandomNumberGenerator rng)
        {
            float total = Loot.Sum(e => e.Weight);
            float roll = rng.Range(0f, total);

            foreach (var e in Loot)
            {
                roll -= e.Weight;
                if (roll <= 0)
                    return e.Item;
            }

            return Loot.Last().Item;
        }
    }
}