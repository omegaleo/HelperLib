# Documentation Attribute IDE Tooltip Example

## Recommended: Use Markdown for Code Examples

For the best IDE experience, we **strongly recommend using markdown code fences** in your `codeExample` parameter:

```csharp
[Documentation(
    "MyMethod",
    "Does something cool",
    new[] { "param: Description" },
    @"```csharp
// Use markdown code fences like this!
MyMethod(""example"");
```"
)]
```

This provides:
- ✅ Proper syntax highlighting in IDEs
- ✅ Better formatting in IntelliSense tooltips
- ✅ Consistent appearance across Visual Studio, Rider, and VS Code
- ✅ Markdown rendering in documentation generators

## Before (Without XML Doc Generator)

When using the `DocumentationAttribute` without the XML Doc Generator, IDEs would not show the documentation:

```csharp
[Documentation(
    "BenchmarkUtility", 
    "Utility class for benchmarking code execution time.", 
    null, 
    @"```csharp
BenchmarkUtility.Start(""MyBenchmark"");
// Code to benchmark
BenchmarkUtility.Stop(""MyBenchmark"");
var results = BenchmarkUtility.GetResults(""MyBenchmark"");
```")]
public class BenchmarkUtility
{
    // ...
}
```

**IDE Hover Tooltip Shows:** No documentation (just the class signature)

---

## After (With XML Doc Generator)

With the XML Doc Generator running as part of the build, the same code automatically generates XML documentation that appears in IDE tooltips:

### Generated XML Documentation:
```xml
<member name="T:OmegaLeo.HelperLib.Helpers.BenchmarkUtility">
    <summary>Utility class for benchmarking code execution time.</summary>
    <example><![CDATA[```csharp
BenchmarkUtility.Start("MyBenchmark");
// Code to benchmark
BenchmarkUtility.Stop("MyBenchmark");
var results = BenchmarkUtility.GetResults("MyBenchmark");
```]]></example>
</member>
```

**IDE Hover Tooltip Now Shows:**
- **Summary:** "Utility class for benchmarking code execution time."
- **Example:** Full code example with syntax highlighting

---

## Real-World Example: Method with Parameters

```csharp
[Documentation(
    "AverageWithNullValidation",
    "Calculates the average of a list of integers, returning 0 if the list is empty.",
    new string[] { "list: The list of integers to calculate the average from." },
    @"```csharp
var numbers = new List<int> { 1, 2, 3, 4 };
int average = numbers.AverageWithNullValidation(); // average will be 2
var emptyList = new List<int>();
int averageEmpty = emptyList.AverageWithNullValidation(); // averageEmpty will be 0
```")]
public static int AverageWithNullValidation(this IEnumerable<int> list)
{
    return list.Any() ? (int)list.Average() : 0;
}
```

### Generated XML:
```xml
<member name="M:OmegaLeo.HelperLib.Extensions.MathExtensions.AverageWithNullValidation(System.Collections.Generic.IEnumerable{System.Int32})">
    <summary>Calculates the average of a list of integers, returning 0 if the list is empty.</summary>
    <remarks>
Parameters:
  - list: The list of integers to calculate the average from.
</remarks>
    <example><![CDATA[```csharp
var numbers = new List<int> { 1, 2, 3, 4 };
int average = numbers.AverageWithNullValidation(); // average will be 2
var emptyList = new List<int>();
int averageEmpty = emptyList.AverageWithNullValidation(); // averageEmpty will be 0
```]]></example>
</member>
```

**IDE Hover Tooltip Now Shows:**
- **Summary:** "Calculates the average of a list of integers, returning 0 if the list is empty."
- **Remarks:** Parameter documentation
- **Example:** Complete usage example with expected behavior

---

## Benefits

1. **Rich Documentation in IDE:** See full descriptions, parameters, and examples without leaving your code
2. **Automatic Generation:** No manual XML comment writing - it's all generated from the attribute
3. **DRY Principle:** Define documentation once in the attribute, use it for both runtime and design-time
4. **Better IntelliSense:** Enhanced code completion with examples and detailed descriptions
5. **Consistent Format:** All documentation follows .NET XML documentation standards

---

## How to View in Your IDE

### Visual Studio
- **Hover** over any class/method/property with DocumentationAttribute
- **Quick Info** (Ctrl+K, Ctrl+I) for detailed view

### JetBrains Rider
- **Hover** over the symbol
- **Quick Documentation** (Ctrl+Q or F1) for full documentation panel

### VS Code (with C# extension)
- **Hover** over the symbol for IntelliSense popup
- Shows summary and examples inline
