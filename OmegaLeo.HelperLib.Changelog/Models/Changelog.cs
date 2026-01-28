namespace OmegaLeo.HelperLib.Changelog.Models
{
    public class Changelog
    {
        public string Target;
        public string Description;

        public Changelog(string target, string description)
        {
            Target = target;
            Description = description;
        }
    }
}