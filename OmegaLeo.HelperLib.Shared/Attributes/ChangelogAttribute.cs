using System;

namespace OmegaLeo.HelperLib.Shared.Attributes
{
    [Documentation(nameof(ChangelogAttribute), "Attribute to annotate classes, methods, or properties with changelog information.", 
        new []
        {
            "Version - Changelog version",
            "Description - Description of what was changed",
            "Date - Date of the changes (Optional)"
        },
    @"```cs
[Changelog(""1.0.0"", ""Created ChangelogAttribute to be used to register changes in the code."")]
public class ChangelogAttribute : Attribute" )]
    [Changelog("1.0.0", "Created ChangelogAttribute to be used to register changes in the code.")]
    [Changelog("1.2.1", "Created documentation for ChangelogAttribute.", "January 28, 2026")]
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
