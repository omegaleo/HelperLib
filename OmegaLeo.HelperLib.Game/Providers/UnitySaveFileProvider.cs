namespace OmegaLeo.HelperLib.Game.Providers
{
#if UNITY_EDITOR || UNITY_STANDALONE
    using System.IO;
    using UnityEngine;

    namespace OmegaLeo.HelperLib.SaveSystem
    {
        public class UnitySaveFileProvider : ISaveFileProvider
        {
            private readonly string _saveFolder;

            public UnitySaveFileProvider(string saveFolder = "Saves")
            {
                _saveFolder = saveFolder;
            }

            public string GetSaveDirectory()
            {
                return Path.Combine(Application.persistentDataPath, _saveFolder);
            }

            public string GetSavePath(string fileName)
            {
                if (!fileName.EndsWith(".json"))
                    fileName += ".json";
                
                return Path.Combine(GetSaveDirectory(), fileName);
            }
        }
    }
#endif
}