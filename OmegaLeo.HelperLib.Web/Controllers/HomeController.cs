// File: `OmegaLeo.HelperLib.Web/Controllers/HomeController.cs`

using System.Diagnostics;
using System.Reflection;
using System.Runtime.ExceptionServices;
using GameDevLibrary.Helpers;
using Microsoft.AspNetCore.Mvc;
using NetFlow.DocumentationHelper.Library.Helpers;
using NetFlow.DocumentationHelper.Library.Models;
using OmegaLeo.HelperLib.Changelog.Tools;
using OmegaLeo.HelperLib.Web.Models;

namespace OmegaLeo.HelperLib.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private static Dictionary<string, LibraryDocumentationViewModel> _libraryCache;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        InitializeLibraries();
    }

    private void InitializeLibraries()
    {
        if (_libraryCache != null) return;

        var root = GetSolutionRoot();
        _logger.LogInformation("Initializing libraries from base directory: {Root}", root);

        _libraryCache = new Dictionary<string, LibraryDocumentationViewModel>
        {
            ["core"] = GenerateLibraryDocs(
                typeof(BenchmarkUtility).Assembly,
                Path.Combine(root, "changelogs", "OmegaLeo.HelperLib.CHANGELOG.md"),
                "Core Library",
                "Core utilities and helper functions for general C# development",
                "OmegaLeo.HelperLib"
            ),
            ["changelog"] = GenerateLibraryDocs(
                typeof(Changelog.Models.Changelog).Assembly,
                Path.Combine(root, "changelogs", "OmegaLeo.HelperLib.Changelog.CHANGELOG.md"),
                "Changelog Library",
                "Tools for managing and generating changelogs",
                "OmegaLeo.HelperLib.Changelog"
            ),
            ["game"] = GenerateLibraryDocs(
                typeof(OmegaLeo.HelperLib.Game.Models.ConsoleCommand).Assembly,
                Path.Combine(root, "changelogs", "OmegaLeo.HelperLib.Game.CHANGELOG.md"),
                "Game Development Library",
                "Game development utilities for Unity and Godot",
                "OmegaLeo.HelperLib.Game"
            )
        };

        _logger.LogInformation("Initialized {Count} libraries", _libraryCache.Count);
    }

    public IActionResult Index()
    {
        return View(_libraryCache);
    }

    public IActionResult Library(string id)
    {
        if (string.IsNullOrEmpty(id) || !_libraryCache.ContainsKey(id))
            return NotFound();

        var cache = _libraryCache[id];

        if (cache.Documentation.Any())
        {
            _logger.LogInformation($"Found cache for {id}: {cache.Documentation.Count} entries");
        }
        else
        {
            _logger.LogError($"Cache for {id} is empty!");
        }

        return View(cache);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private void LogDocumentationHelperInfo()
    {
        try
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var target = assemblies.FirstOrDefault(a =>
                a.GetName().Name.Equals("NetFlow.DocumentationHelper.Library", StringComparison.OrdinalIgnoreCase));

            if (target == null)
            {
                _logger.LogWarning("NetFlow.DocumentationHelper.Library not loaded. Loaded assemblies ({Count}):",
                    assemblies.Length);
                foreach (var a in assemblies.OrderBy(a => a.GetName().Name))
                {
                    _logger.LogInformation("  {Name} {Version} => {Location}", a.GetName().Name, a.GetName().Version,
                        SafeLocation(a));
                }

                return;
            }

            _logger.LogInformation("Found {Name} {Version} at {Location}", target.GetName().Name,
                target.GetName().Version, SafeLocation(target));

            Type attrType = null;
            try
            {
                attrType = target.GetType("NetFlow.DocumentationHelper.Library.Attributes.DocumentationAttribute",
                    false, true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to GetType for DocumentationAttribute in {Assembly}",
                    target.GetName().Name);
            }

            if (attrType == null)
            {
                _logger.LogWarning("Type 'DocumentationAttribute' not found in {Assembly}.", target.GetName().Name);
            }
            else
            {
                var ctors = attrType.GetConstructors(BindingFlags.Public | BindingFlags.Instance |
                                                     BindingFlags.NonPublic);
                if (!ctors.Any())
                    _logger.LogWarning("No constructors found on DocumentationAttribute in {Assembly}.",
                        target.GetName().Name);
                foreach (var ctor in ctors)
                {
                    var sig = string.Join(", ", ctor.GetParameters().Select(p => p.ParameterType.Name + " " + p.Name));
                    _logger.LogInformation("DocumentationAttribute ctor: ({Signature})", sig);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to log DocumentationHelper assembly info.");
        }

        static string SafeLocation(Assembly a)
        {
            try
            {
                return string.IsNullOrEmpty(a.Location) ? "<in-memory>" : a.Location;
            }
            catch
            {
                return "<unknown>";
            }
        }
    }

    private void InspectDocumentationHelperMethod()
    {
        try
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type helperType = null;
            foreach (var asm in assemblies)
            {
                try
                {
                    var t = asm.GetTypes().FirstOrDefault(tt =>
                        tt.Name.Equals("DocumentationHelperTool", StringComparison.OrdinalIgnoreCase)
                        || tt.FullName?.EndsWith(".DocumentationHelperTool") == true);
                    if (t != null)
                    {
                        helperType = t;
                        break;
                    }
                }
                catch (ReflectionTypeLoadException rtlex)
                {
                    _logger.LogDebug(rtlex, "Could not enumerate types in {Assembly}", asm.GetName().Name);
                    foreach (var loaderEx in rtlex.LoaderExceptions ?? Array.Empty<Exception>())
                        _logger.LogDebug(loaderEx, "Loader exception for {Assembly}", asm.GetName().Name);
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex, "Failed enumerating types in {Assembly}", asm.GetName().Name);
                }
            }

            if (helperType == null)
            {
                _logger.LogWarning("DocumentationHelperTool type not found in loaded assemblies.");
                return;
            }

            _logger.LogInformation("DocumentationHelperTool found: {Type} in {Assembly} ({Location})",
                helperType.FullName, helperType.Assembly.GetName().Name, SafeLocation(helperType.Assembly));

            var method = helperType.GetMethod("GenerateDocumentation",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            if (method == null)
            {
                _logger.LogWarning("GenerateDocumentation method not found on {Type}.", helperType.FullName);
            }
            else
            {
                var parameters = string.Join(", ",
                    method.GetParameters().Select(p => p.ParameterType.Name + " " + p.Name));
                _logger.LogInformation("Method: {Name} ({Parameters}) -> {ReturnType}", method.Name, parameters,
                    method.ReturnType.FullName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to inspect DocumentationHelperTool type/method.");
        }

        static string SafeLocation(Assembly a)
        {
            try
            {
                return string.IsNullOrEmpty(a.Location) ? "<in-memory>" : a.Location;
            }
            catch
            {
                return "<unknown>";
            }
        }
    }

    private LibraryDocumentationViewModel GenerateLibraryDocs(Assembly assembly, string changelogPath,
        string displayName, string description, string nugetPackageId)
    {
        _logger.LogInformation("=== Starting GenerateLibraryDocs for {Assembly} ===", assembly.GetName().Name);
        _logger.LogInformation("Assembly location: {Location}", assembly.Location ?? "<in-memory>");
        _logger.LogInformation("Changelog path: {Path}", changelogPath);
        _logger.LogInformation("Changelog exists: {Exists}", System.IO.File.Exists(changelogPath));

        // Log types in assembly and their attributes
        try
        {
            var types = assembly.GetTypes();
            _logger.LogInformation("Assembly {Name} contains {Count} types", assembly.GetName().Name, types.Length);

            var typesWithDocAttr = types.Where(t =>
                    t.GetCustomAttributes().Any(a =>
                        a.GetType().Name.Contains("Documentation", StringComparison.OrdinalIgnoreCase)))
                .ToList();

            _logger.LogInformation("Found {Count} types with DocumentationAttribute-like attributes",
                typesWithDocAttr.Count);

            if (typesWithDocAttr.Any())
            {
                foreach (var type in typesWithDocAttr.Take(5))
                {
                    _logger.LogInformation("  Type with doc attr: {TypeName}", type.FullName);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to inspect assembly types");
        }

        LogDocumentationHelperInfo();

        IEnumerable<DocumentationStructure> allDocumentation;
        try
        {
            // Pass the specific assembly to only scan that assembly
            var result = DocumentationHelperTool.GenerateDocumentation();
            allDocumentation = result?
                .Where(d => d.AssemblyName?.Equals(assembly.GetName().Name ,StringComparison.OrdinalIgnoreCase) == true)
                .ToList() ?? Enumerable.Empty<DocumentationStructure>();

            _logger.LogInformation("GenerateDocumentation returned {Count} items for {Assembly}",
                allDocumentation.Count(), assembly.GetName().Name);

            if (!allDocumentation.Any())
            {
                _logger.LogWarning(
                    "No documentation items generated for {Assembly}. Check if types have [Documentation] attribute applied.",
                    assembly.GetName().Name);
            }
        }
        catch (MissingMethodException mmex)
        {
            _logger.LogError(mmex,
                "Missing method when calling DocumentationHelperTool.GenerateDocumentation(). Possible assembly mismatch.");
            InspectDocumentationHelperMethod();
            ExceptionDispatchInfo.Capture(mmex).Throw();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DocumentationHelperTool.GenerateDocumentation threw an exception.");
            InspectDocumentationHelperMethod();
            throw;
        }

        Dictionary<string, List<Changelog.Models.Changelog>> changelog =
            new Dictionary<string, List<Changelog.Models.Changelog>>();
        var changelogMarkdown = string.Empty;
        if (System.IO.File.Exists(changelogPath))
        {
            try
            {
                changelog = ChangelogTool.ExtractChangelog(assembly);
                changelogMarkdown = ChangelogTool.GetMarkdown(assembly);
                _logger.LogInformation("Parsed changelog with {Count} versions", changelog.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse changelog at {Path}", changelogPath);
            }
        }
        else
        {
            _logger.LogWarning("Changelog file not found at {Path}", changelogPath);
        }

        var viewModel = new LibraryDocumentationViewModel
        {
            LibraryName = displayName,
            Description = description,
            NuGetPackageId = nugetPackageId,
            ChangelogMarkdown = changelogMarkdown,
            Documentation = allDocumentation.ToList()
        };

        _logger.LogInformation(
            "=== Completed GenerateLibraryDocs for {Assembly}. Total documentation items: {Count} ===",
            assembly.GetName().Name, viewModel.Documentation.Count);

        return viewModel;
    }


    static string GetSolutionRoot()
    {
        return AppContext.BaseDirectory;
    }
}
