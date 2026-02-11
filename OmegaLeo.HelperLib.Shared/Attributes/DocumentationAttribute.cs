using System;

namespace OmegaLeo.HelperLib.Shared.Attributes
{
    [System.AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    [Changelog("1.2.1", "Created documentation for DocumentationAttribute.", "January 28, 2026")]
    [Documentation("DocumentationAttribute",
        "Attribute to annotate classes, methods, or properties with documentation information.",
        new string[]
            { 
                "title: The title of the documentation.", 
                "description: A brief description.", 
                "args: An array of argument descriptions.", 
                "codeExample: An optional code example." 
            },
        @"```csharp
[Documentation(
    ""MyMethod"",
    ""This method does something important."",
    new string[] { ""param1: The first parameter."", ""param2: The second parameter."" },
    ""MyMethod(param1, param2);""
)]
public void MyMethod(string param1, int param2)
{
    // Method implementation
}
```
"
        )]
    public class DocumentationAttribute : Attribute
    {
        public string Title;
        public string Description;
        public string[] Args;
        public string CodeExample = string.Empty;

        public DocumentationAttribute(string title, string description, string[] args = null, string codeExample = "")
        {
            Title = title;
            Description = description;
            Args = args ?? Array.Empty<string>();
            CodeExample = codeExample;
        }
    }
}