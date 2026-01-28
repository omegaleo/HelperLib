using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Microsoft.Extensions.DependencyModel;
using OmegaLeo.HelperLib.Shared.Attributes;
using OmegaLeo.HelperLib.Documentation.Models;

namespace OmegaLeo.HelperLib.Documentation.Helpers
{

    [Changelog("1.2.0", "Migrated DocumentationHelper library from NetFlow.DocumentationHelper.Library to it's own package in OmegaLeo.HelperLib", "January 26, 2026")]
    public static class DocumentationHelperTool
    {
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
    {
        EnsureReferencedAssembliesLoaded();

        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .Select(a =>
                new DocumentationAssembly()
                {
                    AssemblyName = a.GetName(false)?.Name ?? nameof(a),
                    Types = GetTypesWithDocumentationAttribute(a)
                }).Where(x => x.Types.Any()).ToList();

        if (!generateForPackageAssembly)
        {
            assemblies =
                assemblies.Where(x => x.AssemblyName != typeof(DocumentationHelperTool).Assembly.GetName().Name)
                    .ToList();
        }

        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.Types)
            {
                var doc = GetDocumentation(type, assembly.AssemblyName);
                yield return doc;
            }
        }
    }

    static void EnsureReferencedAssembliesLoaded()
    {
        try
        {
            var loadedNames = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetName(false) != null)
                .Select(a => a.GetName(false).Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var deps = DependencyContext.Default?.RuntimeLibraries;
            if (deps == null) return;

            foreach (var lib in deps)
            {
                if (!loadedNames.Contains(lib.Name))
                {
                    try
                    {
                        Assembly.Load(new AssemblyName(lib.Name));
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"warning OMEGA001: Failed to load assembly '{lib.Name}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // best-effort only
            Console.Error.WriteLine($"warning OMEGA001: EnsureReferencedAssembliesLoaded failed: {ex.Message}");
        }
    }

    static IEnumerable<Type> GetTypesWithDocumentationAttribute(Assembly assembly)
    {
        Type[] types;
        try
        {
            types = assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            types = ex.Types.Where(t => t != null).ToArray();
        }

        foreach (Type type in types)
        {
            if (type == null) continue;

            var classHasDoc = type.GetCustomAttributes(typeof(DocumentationAttribute), true).Any();
            var fieldsHaveDoc = type.GetFields()
                .Any(f => f.GetCustomAttributes(typeof(DocumentationAttribute), true).Any());
            var typesHaveDoc = type.GetProperties()
                .Any(p => p.GetCustomAttributes(typeof(DocumentationAttribute), true).Any());
            var methodsHaveDoc = type.GetMethods()
                .Any(m => m.GetCustomAttributes(typeof(DocumentationAttribute), true).Any());

            if (classHasDoc || fieldsHaveDoc || typesHaveDoc || methodsHaveDoc)
            {
                yield return type;
            }
        }
    }

        [Documentation(nameof(GetDocumentation), "Generates a DocumentationStructure object for the given Type and Assembly Name",
            new []
            {
                "type - The Type to generate documentation for",
                "assemblyName - The name of the assembly the Type belongs to"
            },
            @"```csharp
 var doc = GetDocumentation(typeof(MyClass), ""MyAssembly"");
```")]
        static DocumentationStructure GetDocumentation(Type type, string assemblyName)
        {
            var classDocs = type.GetCustomAttributes(typeof(DocumentationAttribute), true)
                .Select(x => (DocumentationAttribute)x);
            
            var fieldDocs = type.GetFields().SelectMany(f =>
                f.GetCustomAttributes(typeof(DocumentationAttribute), true).Select(x => (DocumentationAttribute)x));
            var propertyDocs = type.GetProperties().SelectMany(p =>
                p.GetCustomAttributes(typeof(DocumentationAttribute), true).Select(x => (DocumentationAttribute)x));
            var methodDocs = type.GetMethods().SelectMany(m =>
                m.GetCustomAttributes(typeof(DocumentationAttribute), true).Select(x => (DocumentationAttribute)x));
            
            var docStructure = new DocumentationStructure(assemblyName, type.Name);

            foreach (var doc in classDocs)
            {
                docStructure.AddDescription(new DocumentationDescription(doc.Title, doc.Description, doc.Args, doc.CodeExample));
            }

            foreach (var doc in fieldDocs)
            {
                docStructure.AddDescription(new DocumentationDescription(doc.Title, doc.Description, doc.Args, doc.CodeExample));
            }

            foreach (var doc in propertyDocs)
            {
                docStructure.AddDescription(new DocumentationDescription(doc.Title, doc.Description, doc.Args, doc.CodeExample));
            }

            foreach (var doc in methodDocs)
            {
                docStructure.AddDescription(new DocumentationDescription(doc.Title, doc.Description, doc.Args, doc.CodeExample));
            }

            return docStructure;
        }
        
        static Dictionary<string, string> loadedXmlDocumentation =
            new Dictionary<string, string>();
        public static void LoadXmlDocumentation(string xmlDocumentation)
        {
            using (XmlReader xmlReader = XmlReader.Create(new StringReader(xmlDocumentation)))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "member")
                    {
                        string raw_name = xmlReader["name"];
                        loadedXmlDocumentation[raw_name] = xmlReader.ReadInnerXml();
                    }
                }
            }
        }
    }
}