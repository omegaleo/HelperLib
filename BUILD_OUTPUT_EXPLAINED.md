# Understanding Build Output: Why Referenced Project Files Appear

## The Question

*"The OmegaLeo.HelperLib/bin/Debug/netstandard2.1/ folder shows files for OmegaLeo.HelperLib.xml and the other libs as well. Is that normal?"*

## Short Answer

**YES, this is completely normal and expected behavior!** ✅

## What You're Seeing

When you build `OmegaLeo.HelperLib`, the output directory contains:

```
OmegaLeo.HelperLib/bin/Debug/netstandard2.1/
├── OmegaLeo.HelperLib.dll                    ← Main library
├── OmegaLeo.HelperLib.xml                    ← Main library's XML docs
├── OmegaLeo.HelperLib.Changelog.dll          ← Referenced project
├── OmegaLeo.HelperLib.Changelog.xml          ← Referenced project's XML docs
├── OmegaLeo.HelperLib.Documentation.dll      ← Referenced project
├── OmegaLeo.HelperLib.Documentation.xml      ← Referenced project's XML docs
├── OmegaLeo.HelperLib.Shared.dll             ← Referenced project
├── OmegaLeo.HelperLib.Shared.xml             ← Referenced project's XML docs
└── ... (PDB files, deps.json, etc.)
```

## Why This Happens

### Build-Time Behavior

When `OmegaLeo.HelperLib.csproj` has these `<ProjectReference>` entries:

```xml
<ItemGroup>
  <ProjectReference Include="..\OmegaLeo.HelperLib.Changelog\..." />
  <ProjectReference Include="..\OmegaLeo.HelperLib.Documentation\..." />
  <ProjectReference Include="..\OmegaLeo.HelperLib.Shared\..." />
</ItemGroup>
```

MSBuild automatically:

1. **Builds the referenced projects** (if needed)
2. **Copies their output DLLs** to the main project's output directory
3. **Copies associated files** (XML documentation, PDB debug symbols)
4. **Does this transitively** - includes dependencies of dependencies

### Why?

This is necessary because:

- **Runtime Requirements**: The main assembly needs these DLLs to run
- **Development Experience**: Provides complete documentation for IntelliSense
- **Testing**: Allows running/debugging with all dependencies present
- **Deployment**: Ensures all required files are in one place

## What About NuGet Packages?

**Don't worry - NuGet packaging handles this correctly!**

### Package Contents

When you run `dotnet pack OmegaLeo.HelperLib.csproj`, the resulting `.nupkg` contains:

```
lib/netstandard2.1/
├── OmegaLeo.HelperLib.dll    ← Only the main library
└── OmegaLeo.HelperLib.xml    ← Only the main library's docs
```

**The referenced projects are NOT bundled inside!**

### Package Dependencies

Instead, the package declares **dependencies** in its `.nuspec`:

```xml
<dependencies>
  <group targetFramework=".NETStandard2.1">
    <dependency id="OmegaLeo.HelperLib.Changelog" version="1.2.1.1" />
    <dependency id="OmegaLeo.HelperLib.Documentation" version="1.2.1.1" />
    <dependency id="OmegaLeo.HelperLib.Shared" version="1.2.1.1" />
  </group>
</dependencies>
```

When someone installs your package:
- NuGet downloads `OmegaLeo.HelperLib` package
- NuGet sees the dependencies
- NuGet downloads the dependency packages separately
- Each library is a separate, proper NuGet package

## Comparison: Build vs. Package

### Build Output (bin folder)
```
✅ Contains all DLLs and XML files (main + referenced)
✅ Ready to run/test immediately
✅ All dependencies in one place
```

### NuGet Package (nupkg file)
```
✅ Contains only main library's DLL and XML
✅ Lists dependencies separately
✅ Proper package architecture
✅ Allows version management per dependency
```

## This is Standard .NET Behavior

All .NET projects work this way:

- **Microsoft's Libraries**: System.Text.Json references System.Memory, etc.
- **Popular Libraries**: Newtonsoft.Json, Entity Framework, etc.
- **Your Projects**: Same behavior for all project references

## When Would This Be a Problem?

This would only be unusual if:

- ❌ The NuGet package bundled all DLLs together (it doesn't!)
- ❌ Referenced projects weren't in the output (they should be!)
- ❌ You're deploying and missing DLLs (build output has them all!)

## Summary

| Location | Behavior | Correct? |
|----------|----------|----------|
| `bin/Debug/` folder | Contains main + referenced DLLs & XML | ✅ YES - Normal |
| `.nupkg` package | Contains only main DLL & XML | ✅ YES - Correct |
| Package dependencies | Lists referenced projects | ✅ YES - Proper |

## Bottom Line

**Everything is working correctly!** The build output correctly includes all dependencies, and NuGet packaging correctly separates them into individual packages with proper dependency declarations.

This is exactly how .NET project references and NuGet packaging are supposed to work. No changes needed! 🎉
