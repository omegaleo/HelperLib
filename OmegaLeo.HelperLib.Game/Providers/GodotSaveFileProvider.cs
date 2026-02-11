using System.IO;
using OmegaLeo.HelperLib.Game.Interfaces;

namespace OmegaLeo.HelperLib.Game.Providers
{
#if GODOT
    public class GodotSaveFileProvider : ISaveFileProvider
    {
        private readonly string _saveFolder;

        public GodotSaveFileProvider(string saveFolder = "Saves")
        {
            _saveFolder = saveFolder;
        }

        public string GetSaveDirectory()
        {
            return Path.Combine("user://", _saveFolder);
        }

        public string GetSavePath(string fileName)
        {
            if (!fileName.EndsWith(".json"))
                fileName += ".json";
            
            return Path.Combine(GetSaveDirectory(), fileName);
        }
    }
#endif
}