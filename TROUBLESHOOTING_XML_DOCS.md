# Troubleshooting: XML Documentation Not Showing in IDE

## The Problem

You've built the project and XML documentation files are generated, but when you hover over classes like `BenchmarkUtility` or methods like `GetStopwatch` in your IDE (Rider, Visual Studio), you don't see the rich documentation from `DocumentationAttribute`.

## Why This Happens

### Key Insight: Source Projects vs. Consuming Projects

When you have the **source code open** in your IDE, the IDE prioritizes showing information directly from the source code (the actual C# files) rather than from XML documentation files. This is by design - IDEs assume if you have the source, you don't need the XML summary.

**XML documentation is primarily for library consumers** who only have the compiled DLL, not the source code.

## The Solution

To see your XML documentation in action, you need to:

1. **Build your library** (the XML is generated correctly)
2. **Create a separate test/consumer project** that references the built DLL
3. **Use the library in that project** - now you'll see the XML documentation!

### Step-by-Step Example

#### 1. Build the Library

```bash
dotnet build OmegaLeo.HelperLib/OmegaLeo.HelperLib.csproj --configuration Debug
```

This creates:
- `bin/Debug/netstandard2.1/OmegaLeo.HelperLib.dll`
- `bin/Debug/netstandard2.1/OmegaLeo.HelperLib.xml` ← The documentation!

#### 2. Create a Test Project

```bash
cd /path/to/your/workspace
dotnet new console -n TestHelperLib
cd TestHelperLib
dotnet add reference ../HelperLib/OmegaLeo.HelperLib/OmegaLeo.HelperLib.csproj
```

#### 3. Use the Library

In `Program.cs`:

```csharp
using OmegaLeo.HelperLib.Helpers;

// Now hover over BenchmarkUtility in your IDE
BenchmarkUtility.Start("test");
```

**Now you'll see the documentation!** 🎉

## Alternative: Testing with NuGet Package

If you want to test the NuGet package experience:

```bash
# Pack the library
dotnet pack OmegaLeo.HelperLib/OmegaLeo.HelperLib.csproj

# Create test project
dotnet new console -n TestNuGet
cd TestNuGet

# Add local package source
dotnet nuget add source /path/to/HelperLib/bin/Debug -n local

# Install package
dotnet add package OmegaLeo.HelperLib
```

## IDE-Specific Troubleshooting

### JetBrains Rider

If you're consuming the library and still not seeing docs:

1. **Invalidate Caches**: `File → Invalidate Caches / Restart`
2. **Rebuild Solution**: `Build → Rebuild All`
3. **Check XML Location**: Ensure `.xml` file is next to `.dll`

To verify XML is loaded:
- Navigate to a type: `Ctrl+N` → type "BenchmarkUtility"
- Check Quick Documentation: `Ctrl+Q`

### Visual Studio

1. **Clean Solution**: `Build → Clean Solution`
2. **Rebuild**: `Build → Rebuild Solution`
3. **Clear Component Cache**: Close VS, delete `.vs` folder

### VS Code

1. **Reload Window**: `Ctrl+Shift+P` → "Developer: Reload Window"
2. **Restart OmniSharp**: `Ctrl+Shift+P` → "OmniSharp: Restart OmniSharp"

## Verifying XML Documentation Exists

Check that documentation was generated:

```bash
# List XML files
ls -la bin/Debug/netstandard2.1/*.xml

# Check BenchmarkUtility documentation
grep -A 10 "BenchmarkUtility" bin/Debug/netstandard2.1/OmegaLeo.HelperLib.xml
```

You should see:
```xml
<member name="T:OmegaLeo.HelperLib.Helpers.BenchmarkUtility">
    <summary>Utility class for benchmarking code execution time.</summary>
    <example>...</example>
</member>
```

## For NuGet Package Authors

To ensure consumers get XML documentation:

1. **Verify `GenerateDocumentationFile` is enabled** in `.csproj`:
   ```xml
   <PropertyGroup>
       <GenerateDocumentationFile>true</GenerateDocumentationFile>
   </PropertyGroup>
   ```

2. **Ensure XML is included in package**:
   ```bash
   # Pack the project
   dotnet pack
   
   # Verify XML is in the package
   unzip -l bin/Debug/YourPackage.nupkg | grep .xml
   ```

3. **Test the package** by consuming it in another project

## Summary

✅ **XML documentation IS being generated** from DocumentationAttribute
✅ **It's in the correct location** (bin folder next to DLL)
✅ **IDEs use source code when available** (not XML)
✅ **To see XML docs in action**: Reference the library from another project
✅ **For package consumers**: Everything works automatically!

The documentation system is working correctly - it's designed to provide IntelliSense for **users** of your library, not while you're developing it!
