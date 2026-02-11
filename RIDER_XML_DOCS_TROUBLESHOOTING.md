# Rider-Specific: Troubleshooting XML Documentation Display

## The Problem

You're consuming OmegaLeo.HelperLib in your project (e.g., in HomeController.cs) and when hovering over methods like `GetChangelogMarkdown` or `BenchmarkUtility.Start`, you only see:

```
[Documentation("GetChangelogMarkdown", "Generates...", null, "")]
public static string GetChangelogMarkdown(IEnumerable<Assembly> assemblies)
 in class OmegaLeo.HelperLib.Changelog.Helpers.ChangelogHelper
```

Instead of the rich XML documentation.

## Why This Happens in Rider

Rider has several caching mechanisms that can prevent it from picking up XML documentation files, especially after they've been regenerated or updated.

## Verification: Are XML Files Present?

First, verify the XML files exist and have content:

```bash
# Check if XML files exist next to the DLLs
ls -la bin/Debug/net*/OmegaLeo.HelperLib*.xml

# Check content of Changelog XML
cat bin/Debug/net*/OmegaLeo.HelperLib.Changelog.xml
```

You should see files like:
- `OmegaLeo.HelperLib.xml`
- `OmegaLeo.HelperLib.Changelog.xml`
- `OmegaLeo.HelperLib.Game.xml`

Each should contain `<member>` tags with `<summary>` elements.

## Solution Steps for Rider

### Step 1: Invalidate Caches (Most Common Fix)

1. In Rider: **File → Invalidate Caches...**
2. Check **all boxes**:
   - ✅ Clear file system cache and Local History
   - ✅ Clear VCS Log caches and indexes
   - ✅ Clear downloaded shared indexes
   - ✅ Clear workspace model
   - ✅ Clean NuGet cache
3. Click **Invalidate and Restart**

### Step 2: Clean and Rebuild

After Rider restarts:

```bash
dotnet clean
dotnet build --configuration Debug
```

Or in Rider:
1. **Build → Clean Solution**
2. **Build → Rebuild All**

### Step 3: Check XML File Location

Rider needs the XML file to be **in the same directory as the DLL**:

```
bin/Debug/net8.0/
  ├── OmegaLeo.HelperLib.Changelog.dll  ← Must be here
  └── OmegaLeo.HelperLib.Changelog.xml  ← And here
```

Verify:
```bash
# Should show both .dll and .xml
ls -1 bin/Debug/net*/OmegaLeo.HelperLib.Changelog.*
```

### Step 4: Verify XML Content

Check the XML actually has the documentation:

```bash
grep -A 5 "GetChangelogMarkdown" bin/Debug/net*/OmegaLeo.HelperLib.Changelog.xml
```

Should show:
```xml
<member name="M:OmegaLeo.HelperLib.Changelog.Helpers.ChangelogHelper.GetChangelogMarkdown...">
    <summary>Generates a markdown formatted changelog from the changelog attributes in the provided assemblies.</summary>
</member>
```

### Step 5: Force Rider to Reload References

1. Right-click on your project in Solution Explorer
2. **Properties**
3. Go to **Build → Output**
4. Note the output path
5. Check that path has both .dll and .xml files

Or try:
1. Remove the reference to OmegaLeo.HelperLib.Changelog
2. Rebuild
3. Re-add the reference
4. Rebuild

### Step 6: Check Quick Documentation Window

Instead of hover tooltip, try:
1. Place cursor on `GetChangelogMarkdown`
2. Press **Ctrl+Q** (Windows/Linux) or **F1** (macOS)
3. This opens the Quick Documentation window
4. It should show the full documentation

### Step 7: Verify Reference Type

Check how you're referencing the library:

**If using ProjectReference:**
```xml
<ProjectReference Include="..\HelperLib\OmegaLeo.HelperLib.Changelog\OmegaLeo.HelperLib.Changelog.csproj" />
```
Rider might prioritize source code over XML. Consider using a built DLL reference or PackageReference instead.

**If using PackageReference (NuGet):**
```xml
<PackageReference Include="OmegaLeo.HelperLib.Changelog" Version="1.2.1.1" />
```
Ensure the package includes the .xml files in the lib/ folder.

**If using DLL Reference:**
```xml
<Reference Include="OmegaLeo.HelperLib.Changelog">
  <HintPath>path\to\OmegaLeo.HelperLib.Changelog.dll</HintPath>
</Reference>
```
Ensure the .xml file is in the same directory as the .dll.

## Advanced: Rider's External Annotations Cache

If the above doesn't work, Rider might have cached old annotations:

### Windows:
```
%LOCALAPPDATA%\JetBrains\Rider<version>\resharper-host\local\Transient\ReSharperHost\
```

### macOS:
```
~/Library/Caches/JetBrains/Rider<version>/resharper-host/local/Transient/ReSharperHost/
```

### Linux:
```
~/.cache/JetBrains/Rider<version>/resharper-host/local/Transient/ReSharperHost/
```

Try deleting these caches while Rider is closed.

## Testing the XML Documentation

Create a simple test:

```csharp
using OmegaLeo.HelperLib.Changelog.Helpers;
using System.Reflection;

class Program
{
    static void Main()
    {
        // Hover over GetChangelogMarkdown - should show docs
        var markdown = ChangelogHelper.GetChangelogMarkdown(
            new[] { Assembly.GetExecutingAssembly() }
        );
    }
}
```

Build and open in Rider. If hover still doesn't work:
1. Try Ctrl+Q (Quick Documentation)
2. Check Find Usages → see if it shows documentation

## Still Not Working?

### Check Rider Settings

1. **File → Settings → Editor → General → Code Completion**
2. Ensure **Show the documentation popup in (ms)**: is not 0
3. Try increasing to 500-1000ms

### Check ReSharper Settings

1. **File → Settings → Tools → ReSharper**
2. **External Sources → Enable XML Documentation comments**
3. Ensure it's enabled

### Last Resort: Complete Reset

1. Close Rider
2. Delete `.idea` folder in your solution directory
3. Delete Rider cache:
   - Windows: `%LOCALAPPDATA%\JetBrains\Rider<version>\`
   - macOS: `~/Library/Caches/JetBrains/Rider<version>/`
   - Linux: `~/.cache/JetBrains/Rider<version>/`
4. Reopen solution in Rider
5. Let it reindex everything

## Expected Result

After following these steps, hovering over `GetChangelogMarkdown` should show:

```
GetChangelogMarkdown(IEnumerable<Assembly> assemblies): string

Generates a markdown formatted changelog from the changelog 
attributes in the provided assemblies.

Returns: The generated markdown string
```

## Comparison: What You See vs. What You Should See

### What You're Seeing (Wrong):
```
[Documentation("GetChangelogMarkdown", "Generates...", null, "")]
public static string GetChangelogMarkdown(IEnumerable<Assembly> assemblies)
```

### What You Should See (Correct):
```
GetChangelogMarkdown(IEnumerable<Assembly> assemblies): string

Generates a markdown formatted changelog from the changelog 
attributes in the provided assemblies.
```

## If All Else Fails

The issue might be with how the library is being referenced. Try:

1. **Create a fresh test project:**
   ```bash
   dotnet new console -n TestRider
   cd TestRider
   dotnet add reference path/to/OmegaLeo.HelperLib.Changelog.csproj
   dotnet build
   ```

2. **Open ONLY this test project in Rider**

3. **Test if documentation appears**

If documentation works in the test project but not in your main project, the issue is with your main project's configuration or Rider's cache for that specific solution.

## Summary

The XML documentation IS being generated correctly. Rider-specific caching and reference resolution can prevent it from being displayed. The most common fix is:

1. **Invalidate Caches and Restart**
2. **Clean and Rebuild**
3. **Ensure .xml files are next to .dll files**

These steps should resolve the issue in 99% of cases!
