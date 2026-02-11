# Fresh Start: Approaches to Override /// Comments with DocumentationAttribute

## Goal
Make `[Documentation]` attribute the **single source of truth** that overrides any existing `///` XML comments, working automatically for both library developers and NuGet package consumers.

---

## Option 1: Roslyn Source Generator (RECOMMENDED) ⭐

### How It Works
```csharp
// Original code (what user writes):
namespace MyNamespace 
{
    [Documentation("MyClass", "A cool class")]
    public class MyClass 
    {
        [Documentation("DoWork", "Does the work")]
        public void DoWork() { }
    }
}

// Generated code (MyClass.Documentation.g.cs):
namespace MyNamespace 
{
    /// <summary>A cool class</summary>
    public partial class MyClass 
    {
        /// <summary>Does the work</summary>
        public partial void DoWork();
    }
}
```

### Pros
✅ **Standard .NET approach** - Uses official Roslyn APIs
✅ **Zero setup for consumers** - Packaged as analyzer, auto-runs
✅ **IDE friendly** - Full IntelliSense support
✅ **Non-invasive** - Doesn't modify source files
✅ **Incremental** - Fast compilation with IIncrementalGenerator
✅ **Easy debugging** - Generated files visible in IDE
✅ **NuGet ready** - Package as analyzer, include in Documentation package

### Cons
⚠️ **Requires partial classes** - Classes must be marked `partial`
⚠️ **Learning curve** - Source generators are complex
⚠️ **Build-time only** - Not visible until after build

### Implementation
1. Create `OmegaLeo.HelperLib.SourceGenerator` project
2. Implement `IIncrementalGenerator`
3. Find syntax nodes with `DocumentationAttribute`
4. Generate partial classes with XML doc comments
5. Package as analyzer with `<PackageReference Include="...SourceGenerator" OutputItemType="Analyzer">`
6. Include in Documentation package

### Package Structure
```
OmegaLeo.HelperLib.SourceGenerator.nupkg
├── analyzers/dotnet/cs/
│   └── OmegaLeo.HelperLib.SourceGenerator.dll
└── build/
    └── OmegaLeo.HelperLib.SourceGenerator.props
```

---

## Option 2: Pre-Build File Rewriter

### How It Works
```csharp
// Before build:
[Documentation("MyMethod", "Does work")]
/// <summary>Old comment</summary>  ← Will be replaced
public void MyMethod() { }

// After CommentGenerator runs (BEFORE compile):
[Documentation("MyMethod", "Does work")]
/// <summary>Does work</summary>  ← New comment from attribute
public void MyMethod() { }
```

### Pros
✅ **Direct control** - Actually modifies source files
✅ **Works with existing code** - No partial classes needed
✅ **Visible immediately** - Changes are in source
✅ **Simple to understand** - Just file modification

### Cons
❌ **Modifies source files** - Can conflict with version control
❌ **Timing issues** - Must run before compile, after restore
❌ **File locks** - Can cause issues with IDEs
❌ **Consumer complexity** - Hard to package for NuGet users
❌ **Merge conflicts** - Generated changes in git

---

## Option 3: Hybrid - Source Generator + XML Post-Processor

### How It Works
1. **Source Generator**: Generates partial classes with /// comments
2. **XmlDocGenerator**: Post-processes XML files to add examples from attributes

```csharp
// User writes:
[Documentation("DoWork", "Does work", null, "// example code")]
public void DoWork() { }

// Source Generator adds:
/// <summary>Does work</summary>

// XmlDocGenerator augments XML with:
<example><![CDATA[// example code]]></example>
```

### Pros
✅ **Best of both worlds** - IDE sees comments, XML has examples
✅ **Clean source** - No manual /// needed
✅ **Rich documentation** - Full XML in packages

### Cons
⚠️ **Dual approach** - Two tools to maintain
⚠️ **Complexity** - More moving parts

---

## Option 4: Roslyn Analyzer + Code Fix

### How It Works
- Analyzer detects `[Documentation]` without matching `///`
- Shows warning/info diagnostic
- Code fix automatically adds `///` from attribute
- User can apply fix manually or in batch

### Pros
✅ **IDE integration** - Shows as lightbulb/suggestion
✅ **User control** - User decides when to apply
✅ **Standard pattern** - How many .NET tools work

### Cons
❌ **Manual step** - Not fully automatic
❌ **User must trigger** - Doesn't help consumers much

---

## Recommendation: Option 1 (Source Generator)

### Why?
1. **Industry standard** - How modern .NET tools work
2. **Automatic for consumers** - Zero setup
3. **IDE support** - Full IntelliSense
4. **Non-invasive** - No source file modification
5. **Maintainable** - Clear separation of concerns

### Migration Path
1. Mark classes as `partial` (one-time change)
2. Install source generator (packaged with Documentation)
3. Build project
4. IDE shows documentation immediately

### For Consumers
```bash
# Install package
dotnet add package OmegaLeo.HelperLib.Documentation

# Write code
[Documentation("MyMethod", "Cool stuff")]
public partial class MyClass 
{
    public void MyMethod() { }
}

# Build - source generator runs automatically
dotnet build

# IDE shows: "Cool stuff" in IntelliSense!
```

---

## What Needs to Change

### Current State
- Classes/methods have `[Documentation]` attribute
- Some have manual `///` comments (duplication)
- XmlDocGenerator augments compiled XML

### With Source Generator
- Classes/methods marked `partial`
- Only `[Documentation]` attribute needed
- Source generator creates `///` at compile time
- XmlDocGenerator still augments compiled XML with examples

### File Changes Required
```csharp
// Before:
public class BenchmarkUtility { }

// After:
public partial class BenchmarkUtility { }  // Add 'partial'
```

---

## Next Steps

1. ✅ Get user confirmation on approach
2. Create source generator project
3. Implement IIncrementalGenerator
4. Test locally
5. Package as analyzer
6. Test with consumer project
7. Update all classes to partial
8. Document the change

---

## Questions for User

1. **Are you okay with marking classes as `partial`?**
   - This is required for source generators
   - Minimal code change
   - Standard .NET pattern

2. **Should we keep XmlDocGenerator for code examples?**
   - Source generator handles /// comments
   - XmlDocGenerator can add <example> tags to XML
   - Complementary tools

3. **Any concerns about source generators?**
   - Build-time dependency
   - Requires rebuild to see changes
   - Standard in .NET ecosystem
