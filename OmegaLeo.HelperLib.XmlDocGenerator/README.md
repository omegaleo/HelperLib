# OmegaLeo.HelperLib.XmlDocGenerator

Automatic XML documentation generator for DocumentationAttribute. This MSBuild package automatically generates XML documentation comments from your `DocumentationAttribute` declarations, making them visible in IDE IntelliSense and tooltips.

## Installation

### Automatic (Recommended)

Simply install the OmegaLeo.HelperLib.Documentation package:

```bash
dotnet add package OmegaLeo.HelperLib.Documentation
```

The XmlDocGenerator is automatically included as a dependency and will work out of the box!

### Manual

If you want to use the XmlDocGenerator standalone:

```bash
dotnet add package OmegaLeo.HelperLib.XmlDocGenerator
```

## How It Works

1. **MSBuild Integration**: The package includes MSBuild targets that are automatically imported
2. **Build-Time Generation**: Runs after CoreCompile to analyze your compiled assembly
3. **Attribute Reading**: Uses reflection to read DocumentationAttribute instances
4. **XML Generation**: Creates/augments XML documentation files
5. **IntelliSense Ready**: IDEs automatically pick up the generated XML

## Features

- ✅ **Automatic**: Zero configuration needed
- ✅ **Build-time**: No runtime overhead
- ✅ **Non-breaking**: Works alongside existing XML comments
- ✅ **Cross-platform**: Works on Windows, Linux, macOS
- ✅ **Multi-targeting**: Supports all .NET target frameworks
- ✅ **IDE Support**: Works with Visual Studio, Rider, VS Code

## Configuration

By default, XML generation is enabled. To disable it:

```xml
<PropertyGroup>
  <GenerateXmlDocFromDocumentationAttribute>false</GenerateXmlDocFromDocumentationAttribute>
</PropertyGroup>
```

## License

AGPL-3.0-only
