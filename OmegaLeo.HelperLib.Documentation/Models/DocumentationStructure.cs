using System;
using System.Collections.Generic;
using OmegaLeo.HelperLib.Shared.Attributes;

namespace OmegaLeo.HelperLib.Documentation.Models
{

    public class DocumentationStructure
    {
        public string AssemblyName;
        public string ClassName;
        public List<DocumentationDescription> Descriptions;

        public DocumentationStructure(string assemblyName, string className)
        {
            AssemblyName = assemblyName;
            ClassName = className;
            Descriptions = new List<DocumentationDescription>();
        }

        public void AddDescription(DocumentationDescription description)
        {
            Descriptions.Add(description);
        }
    }

    public class DocumentationDescription
    {
        public string Title;
        public string Description;
        public string[] Parameters;
        public string[] MethodParameters;
        public string ReturnType;
        public string CodeExample = string.Empty;

        [Documentation(nameof(DocumentationDescription), "Description of the current Documented object", new []
        {
            "title - Name of the object",
            "description - Description of what the object does",
            "parameters - The parameters for the object (in case of a method with parameters)",
            "methodParameters - Reflected method parameter list when the documented member is a method",
            "returnType - Reflected return type when the documented member is a method",
            "codeExample - Code example of how to use the object"
        })]
        public DocumentationDescription(string title, string description, string[]? parameters = null, string codeExample = "", string[]? methodParameters = null, string returnType = "")
        {
            Title = title;
            Description = description;
            Parameters = parameters ?? Array.Empty<string>();
            MethodParameters = methodParameters ?? Array.Empty<string>();
            ReturnType = returnType ?? string.Empty;
            CodeExample = codeExample;
        }
    }
}