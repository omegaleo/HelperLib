using System;
using System.IO;
using NetFlow.DocumentationHelper.Library.Attributes;
using Newtonsoft.Json;
using OmegaLeo.HelperLib.Game.Interfaces;

namespace OmegaLeo.HelperLib.Game.Services
{
    public class SaveManager
    {
        private readonly ISaveFileProvider _fileProvider;
        private readonly JsonSerializerSettings _jsonOptions;

        public SaveManager(ISaveFileProvider fileProvider)
        {
            _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
            _jsonOptions = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };
        }

        [Documentation(nameof(Save), "Saves data to a JSON file.")]
        public void Save<T>(string fileName, T data)
        {
            string json = JsonConvert.SerializeObject(data, _jsonOptions);
            string path = _fileProvider.GetSavePath(fileName);
            string directory = Path.GetDirectoryName(path);
            
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            
            File.WriteAllText(path, json);
        }

        [Documentation(nameof(Load), "Loads data from a JSON file.")]
        public T Load<T>(string fileName)
        {
            string path = _fileProvider.GetSavePath(fileName);
            
            if (!File.Exists(path))
                throw new FileNotFoundException($"Save file not found: {fileName}");
            
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json, _jsonOptions);
        }

        [Documentation(nameof(TryLoad), "Attempts to load data from a JSON file, returning a success flag.")]
        public bool TryLoad<T>(string fileName, out T data)
        {
            try
            {
                data = Load<T>(fileName);
                return true;
            }
            catch
            {
                data = default;
                return false;
            }
        }

        [Documentation(nameof(SaveExists), "Checks if a save file exists.")]
        public bool SaveExists(string fileName)
        {
            return File.Exists(_fileProvider.GetSavePath(fileName));
        }

        [Documentation(nameof(DeleteSave), "Deletes a save file.")]
        public void DeleteSave(string fileName)
        {
            string path = _fileProvider.GetSavePath(fileName);
            if (File.Exists(path))
                File.Delete(path);
        }

        [Documentation(nameof(GetAllSaveFiles), "Retrieves all save files in the save directory.")]
        public string[] GetAllSaveFiles()
        {
            string savePath = _fileProvider.GetSaveDirectory();
            if (!Directory.Exists(savePath))
                return Array.Empty<string>();

            return Directory.GetFiles(savePath, "*.json");
        }
    }
}