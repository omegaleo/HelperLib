# Omega Leo's Helper Library - Documentation
Package to help with generating and managing documentation for your C# projects.

## Features
- Attribute-based documentation: Use custom attributes to annotate your classes, methods, and properties with documentation comments.
- Documentation extraction tool: A tool to extract and format documentation into various formats (Markdown, HTML, etc.).
- Integration with build processes: Easily integrate documentation generation into your CI/CD pipelines.

## Example attribute usage:
```cs
[Documentation("GenerateDocumentation (bool generateForPackageAssembly)",
            @"Generates a List of objects of type DocumentationStructure that contain the following fields:  
**AssemblyName**: Name of the main Assembly, used to identify the root namespace  
**ClassName**: Name of the class, used to identify the upper level object  
**Title**: Title what we're generating documentation for  
**Description**: Description of what we're generating documentation for, this can contain usage examples and can use the args array to pass names(e.g.: This method uses this methodology)  
**Args**: Array of strings that describe the parameters of the method or class  
**CodeExample**: A code example of how to use the method or class  
  
*Note: If generateForPackageAssembly is set to true, this will generate documentation for the library as well.*",
            new []
            {
                "generateForPackageAssembly - Generate documentation for the DocumentationHelper library as well?"
            }, 
            @"```csharp
DocumentationHelperTool.GenerateDocumentation(true); // Generates documentation for all assemblies including the DocumentationHelper.Library package assembly
DocumentationHelperTool.GenerateDocumentation(false); // Generates documentation for all assemblies excluding the DocumentationHelper.Library package assembly
```")]
        public static IEnumerable<DocumentationStructure> GenerateDocumentation(bool generateForPackageAssembly = false)
```