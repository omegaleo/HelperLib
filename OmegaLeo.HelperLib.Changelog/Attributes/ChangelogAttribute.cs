using System;

namespace OmegaLeo.HelperLib.Changelog.Attributes
{
    [Changelog("v1.0.0", "Created ChangelogAttribute to be used to register changes in the code.")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class ChangelogAttribute : Attribute
    {
        public string Version { get; }
        public string Description { get; }

        public ChangelogAttribute(string version, string description)
        {
            Version = version;
            Description = description;
        }
    }
}
