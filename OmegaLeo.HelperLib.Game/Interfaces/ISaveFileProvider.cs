namespace OmegaLeo.HelperLib.Game.Interfaces
{
    public interface ISaveFileProvider
    {
        string GetSavePath(string fileName);
        string GetSaveDirectory();
    }
}