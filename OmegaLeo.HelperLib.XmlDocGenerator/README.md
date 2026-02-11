# OmegaLeo.HelperLib.XmlDocGenerator

A tool that automatically generates XML documentation from `DocumentationAttribute` custom attributes, making them visible in IDE tooltips.

## What Problem Does This Solve?

The `DocumentationAttribute` is a custom attribute that allows you to document classes, methods, and properties with rich information including descriptions, parameters, and code examples. However, custom attributes don't automatically appear in IDE hover tooltips.

This tool solves that problem by:
1. Reading the compiled assemblies after build
2. Extracting information from `DocumentationAttribute` instances
3. Generating/augmenting the XML documentation file with this information
4. Making the documentation visible in IDEs like Visual Studio and JetBrains Rider

## How It Works

The tool is automatically run as a post-build step in projects that include the MSBuild target. It:

1. **Loads the assembly** with proper dependency resolution
2. **Finds all types** with `DocumentationAttribute`
3. **Extracts documentation** information (Title, Description, Args, CodeExample)
4. **Generates XML** in the standard .NET XML documentation format:
   - `<summary>` - from Description
   - `<remarks>` - from Args (parameters)
   - `<example>` - from CodeExample

## Usage

### In Your Code

Use the `DocumentationAttribute` as usual:

```csharp
[Documentation(
    "MyMethod",
    "This method does something important.",
    new string[] { "param1: The first parameter.", "param2: The second parameter." },
    @"```csharp
MyMethod(param1, param2);
```"
)]
public void MyMethod(string param1, int param2)
{
    // Method implementation
}
```

### Build Integration

The tool is automatically integrated into the build process for projects that include the MSBuild target. No manual action is required.

When you build your project, you'll see a message like:
```
[DocumentationAttribute] Augmenting XML documentation for YourProject from DocumentationAttribute
```

### Viewing in IDE

Once built, hover over any method, class, or property with a `DocumentationAttribute` in your IDE:

- **Visual Studio**: Hover tooltip will show the documentation
- **JetBrains Rider**: Quick Documentation (Ctrl+Q / F1) will show the documentation
- **VS Code**: IntelliSense will show the documentation

## Configuration

The tool runs automatically after the Build target with these settings:

```xml
<Target Name="GenerateXmlDocFromAttributes" AfterTargets="Build">
    <PropertyGroup>
        <XmlDocGenExe>$(MSBuildThisFileDirectory)../OmegaLeo.HelperLib.XmlDocGenerator/bin/$(Configuration)/net8.0/OmegaLeo.HelperLib.XmlDocGenerator.dll</XmlDocGenExe>
        <TargetAssembly>$(TargetDir)$(TargetFileName)</TargetAssembly>
        <TargetXmlDoc>$(TargetDir)$(TargetName).xml</TargetXmlDoc>
    </PropertyGroup>
    
    <Exec Command="dotnet &quot;$(XmlDocGenExe)&quot; &quot;$(TargetAssembly)&quot; &quot;$(TargetXmlDoc)&quot;"
          Condition="Exists('$(XmlDocGenExe)') And Exists('$(TargetAssembly)')"
          IgnoreExitCode="true"
          ContinueOnError="true" />
</Target>
```

## Requirements

- .NET 8.0 SDK (for building the tool)
- Projects must have `GenerateDocumentationFile` enabled (optional but recommended for best results)

## Troubleshooting

### Tool not running
- Ensure the XmlDocGenerator project is built first: `dotnet build OmegaLeo.HelperLib.XmlDocGenerator/OmegaLeo.HelperLib.XmlDocGenerator.csproj`
- Check that the path in the MSBuild target is correct relative to your project

### Documentation not showing in IDE
- Ensure the XML file is generated in the output directory
- For consumed packages, ensure the XML file is included in the package
- Restart your IDE if it doesn't pick up changes

### Assembly loading errors
- The tool uses `AssemblyLoadContext` to load assemblies and their dependencies
- Dependencies must be present in the same directory as the target assembly

## Examples

### Before (Custom Attribute Only)
```csharp
[Documentation("Calculate", "Calculates something", new[] { "x: First number", "y: Second number" })]
public int Calculate(int x, int y) => x + y;
```

**IDE shows**: No documentation (custom attributes don't appear in tooltips)

### After (With XML Doc Generator)
The same code now generates:
```xml
<member name="M:MyNamespace.MyClass.Calculate(System.Int32,System.Int32)">
    <summary>Calculates something</summary>
    <remarks>
Parameters:
  - x: First number
  - y: Second number
</remarks>
</member>
```

**IDE shows**: Full documentation with description and parameter information!

## Technical Details

- **Language**: C# (.NET 8.0)
- **Dependencies**: `System.Reflection.Metadata`
- **XML Format**: Standard .NET XML documentation format
- **Assembly Loading**: Uses `AssemblyLoadContext` for isolated loading
- **Performance**: Runs only during build, no runtime overhead

## Contributing

To modify the XML generation logic, see `Program.cs` in the XmlDocGenerator project.
