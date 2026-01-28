using System;

namespace OmegaLeo.HelperLib.Shared.Attributes
{
    [Changelog("1.0.0", "Created ChangelogAttribute to be used to register changes in the code.")]
    [Documentation(nameof(ChangelogAttribute), "Attribute to annotate classes, methods, or properties with changelog information.")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class ChangelogAttribute : Attribute
    {
        public string Version { get; }
        public string Description { get; }
        
        public string Date { get; set; }

        public ChangelogAttribute(string version, string description, string date = "")
        {
            Version = version;
            Description = description;
            Date = date;
        }
    }
}
