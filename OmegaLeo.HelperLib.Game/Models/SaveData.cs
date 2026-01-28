using System;

namespace OmegaLeo.HelperLib.Game.Models
{
    [Serializable]
    public class SaveData
    {
        public Guid Id;

        public SaveData()
        {
            Id = Guid.NewGuid();
        }
    }
}