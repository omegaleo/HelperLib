namespace OmegaLeo.HelperLib.Game.Models
{
    [System.Serializable]
    public class Loot<T>
    {
        public T Item { get; set; }
        public float Weight { get; set; }

        public Loot(T item, float quantity)
        {
            Item = item;
            Weight = quantity;
        }
    }
}