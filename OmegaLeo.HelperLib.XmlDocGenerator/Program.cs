using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Xml;
using System.Xml.Linq;

if (args.Length < 1)
{
    Console.WriteLine("Usage: xmldocgen <assembly-path> [output-xml-path]");
    Console.WriteLine("Generates or augments XML documentation from DocumentationAttribute values.");
    return 1;
}

var assemblyPath = args[0];
var outputPath = args.Length > 1 ? args[1] : Path.ChangeExtension(assemblyPath, ".xml");

if (!File.Exists(assemblyPath))
{
    Console.Error.WriteLine($"Error: Assembly not found: {assemblyPath}");
    return 1;
}

try
{
    GenerateXmlDocumentation(assemblyPath, outputPath);
    Console.WriteLine($"XML documentation generated: {outputPath}");
    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    Console.Error.WriteLine($"Stack trace: {ex.StackTrace}");
    return 1;
}

static void GenerateXmlDocumentation(string assemblyPath, string outputPath)
{
    // Create a custom assembly load context to properly load dependencies
    var loadContext = new AssemblyLoadContext("XmlDocGen", isCollectible: true);
    
    try
    {
        // Load the assembly and its dependencies
        var assemblyDir = Path.GetDirectoryName(assemblyPath);
        if (assemblyDir != null)
        {
            loadContext.Resolving += (context, name) =>
            {
                var dllPath = Path.Combine(assemblyDir, $"{name.Name}.dll");
                if (File.Exists(dllPath))
                {
                    try
                    {
                        return context.LoadFromAssemblyPath(dllPath);
                    }
                    catch
                    {
                        return null;
                    }
                }
                return null;
            };
        }
        
        var assembly = loadContext.LoadFromAssemblyPath(Path.GetFullPath(assemblyPath));
        
        // Force load referenced assemblies BEFORE searching for the attribute
        if (assemblyDir != null)
        {
            foreach (var refAsm in assembly.GetReferencedAssemblies())
            {
                var refPath = Path.GetFullPath(Path.Combine(assemblyDir, $"{refAsm.Name}.dll"));
                if (File.Exists(refPath))
                {
                    try
                    {
                        loadContext.LoadFromAssemblyPath(refPath);
                        Console.WriteLine($"Loaded reference: {refAsm.Name}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Could not load {refAsm.Name}: {ex.Message}");
                    }
                }
            }
        }
        
        // Load existing XML documentation if it exists
        XDocument? existingDoc = null;
        if (File.Exists(outputPath))
        {
            try
            {
                existingDoc = XDocument.Load(outputPath);
            }
            catch
            {
                // If we can't load it, we'll create a new one
                existingDoc = null;
            }
        }
        
        // Create or get the root elements
        XDocument doc;
        XElement? membersElement;
        
        if (existingDoc != null)
        {
            doc = existingDoc;
            membersElement = doc.Root?.Element("members");
            if (membersElement == null)
            {
                membersElement = new XElement("members");
                doc.Root?.Add(membersElement);
            }
        }
        else
        {
            doc = new XDocument(
                new XElement("doc",
                    new XElement("assembly",
                        new XElement("name", assembly.GetName().Name)),
                    new XElement("members")
                )
            );
            membersElement = doc.Root?.Element("members");
        }
        
        if (membersElement == null)
        {
            Console.WriteLine("Error: Could not create or find members element");
            return;
        }
        
        // Get the DocumentationAttribute type from all loaded assemblies
        Type? docAttrType = null;
        foreach (var asm in loadContext.Assemblies)
        {
            try
            {
                docAttrType = asm.GetTypes()
                    .FirstOrDefault(t => t.Name == "DocumentationAttribute");
                if (docAttrType != null)
                {
                    Console.WriteLine($"Found DocumentationAttribute in {docAttrType.Assembly.GetName().Name}");
                    break;
                }
            }
            catch (ReflectionTypeLoadException)
            {
                // Skip assemblies that can't be loaded
                continue;
            }
        }
        
        if (docAttrType == null)
        {
            Console.WriteLine("Warning: DocumentationAttribute not found in assembly or its references");
            Console.WriteLine($"Loaded assemblies: {string.Join(", ", loadContext.Assemblies.Select(a => a.GetName().Name))}");
            return;
        }
        
        // Process all types in the assembly
        var processedCount = 0;
        foreach (var type in assembly.GetTypes())
        {
            if (ProcessType(type, docAttrType, membersElement))
            {
                processedCount++;
            }
        }
        
        Console.WriteLine($"Processed {processedCount} types with Documentation attributes");
        
        // Save the XML document
        var settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "    ",
            Encoding = new UTF8Encoding(false) // UTF-8 without BOM
        };
        
        using (var writer = XmlWriter.Create(outputPath, settings))
        {
            doc.Save(writer);
        }
    }
    finally
    {
        loadContext.Unload();
    }
}

static bool ProcessType(Type type, Type docAttrType, XElement membersElement)
{
    bool hasAnyDoc = false;
    
    // Process class/struct/interface documentation
    var typeAttrs = type.GetCustomAttributes(docAttrType, true);
    if (typeAttrs.Length > 0)
    {
        AddOrUpdateMember(membersElement, $"T:{type.FullName}", typeAttrs[0], docAttrType);
        hasAnyDoc = true;
    }
    
    // Process fields
    foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
    {
        var fieldAttrs = field.GetCustomAttributes(docAttrType, true);
        if (fieldAttrs.Length > 0)
        {
            AddOrUpdateMember(membersElement, $"F:{type.FullName}.{field.Name}", fieldAttrs[0], docAttrType);
            hasAnyDoc = true;
        }
    }
    
    // Process properties
    foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
    {
        var propAttrs = prop.GetCustomAttributes(docAttrType, true);
        if (propAttrs.Length > 0)
        {
            AddOrUpdateMember(membersElement, $"P:{type.FullName}.{prop.Name}", propAttrs[0], docAttrType);
            hasAnyDoc = true;
        }
    }
    
    // Process methods
    foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
    {
        if (method.IsSpecialName) continue; // Skip property accessors, etc.
        
        var methodAttrs = method.GetCustomAttributes(docAttrType, true);
        if (methodAttrs.Length > 0)
        {
            var memberName = GetMethodMemberName(method);
            AddOrUpdateMember(membersElement, memberName, methodAttrs[0], docAttrType);
            hasAnyDoc = true;
        }
    }
    
    return hasAnyDoc;
}

static string GetMethodMemberName(MethodInfo method)
{
    var sb = new StringBuilder();
    sb.Append($"M:{method.DeclaringType?.FullName ?? "Unknown"}.{method.Name}");
    
    var parameters = method.GetParameters();
    if (parameters.Length > 0)
    {
        sb.Append('(');
        for (int i = 0; i < parameters.Length; i++)
        {
            if (i > 0) sb.Append(',');
            sb.Append(GetTypeName(parameters[i].ParameterType));
        }
        sb.Append(')');
    }
    
    return sb.ToString();
}

static string GetTypeName(Type type)
{
    if (type.IsGenericType)
    {
        var genericType = type.GetGenericTypeDefinition();
        var genericArgs = type.GetGenericArguments();
        var name = genericType.FullName?.Substring(0, genericType.FullName.IndexOf('`')) ?? genericType.Name;
        name += "{" + string.Join(",", genericArgs.Select(GetTypeName)) + "}";
        return name;
    }
    return type.FullName ?? type.Name;
}

static void AddOrUpdateMember(XElement membersElement, string memberName, object attribute, Type docAttrType)
{
    // Extract attribute values using reflection
    var titleProp = docAttrType.GetField("Title");
    var descProp = docAttrType.GetField("Description");
    var argsProp = docAttrType.GetField("Args");
    var exampleProp = docAttrType.GetField("CodeExample");
    
    var title = titleProp?.GetValue(attribute) as string ?? "";
    var description = descProp?.GetValue(attribute) as string ?? "";
    var args = argsProp?.GetValue(attribute) as string[] ?? Array.Empty<string>();
    var codeExample = exampleProp?.GetValue(attribute) as string ?? "";
    
    // Find or create the member element
    var existingMember = membersElement.Elements("member")
        .FirstOrDefault(m => m.Attribute("name")?.Value == memberName);
    
    XElement memberElement;
    if (existingMember != null)
    {
        memberElement = existingMember;
        // Remove existing auto-generated elements (we'll recreate them)
        memberElement.Elements("summary").Remove();
        memberElement.Elements("remarks").Remove();
        memberElement.Elements("example").Remove();
    }
    else
    {
        memberElement = new XElement("member", new XAttribute("name", memberName));
        membersElement.Add(memberElement);
    }
    
    // Add summary
    if (!string.IsNullOrEmpty(description))
    {
        memberElement.Add(new XElement("summary", new XText(description)));
    }
    
    // Add parameter descriptions as remarks
    if (args != null && args.Length > 0)
    {
        var remarksContent = new StringBuilder();
        remarksContent.AppendLine();
        remarksContent.AppendLine("Parameters:");
        foreach (var arg in args)
        {
            remarksContent.AppendLine($"  - {arg}");
        }
        memberElement.Add(new XElement("remarks", new XText(remarksContent.ToString())));
    }
    
    // Add code example
    if (!string.IsNullOrEmpty(codeExample))
    {
        memberElement.Add(new XElement("example", new XCData(codeExample)));
    }
}
