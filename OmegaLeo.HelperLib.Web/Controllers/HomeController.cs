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

        return View(_libraryCache[id]);
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
        _logger.LogInformation("Generating documentation for {Assembly}", assembly.GetName().Name);
        _logger.LogInformation("Assembly location: {Location}", assembly.Location ?? "<in-memory>");
        _logger.LogInformation("Changelog path: {Path}", changelogPath);
        _logger.LogInformation("Changelog exists: {Exists}", System.IO.File.Exists(changelogPath));
    
        LogDocumentationHelperInfo();

        IEnumerable<DocumentationStructure> allDocumentation = Enumerable.Empty<DocumentationStructure>();
        try
        {
            // Pass the specific assembly to GenerateDocumentation
            var result = DocumentationHelperTool.GenerateDocumentation();
            allDocumentation = result?.ToList() ?? Enumerable.Empty<DocumentationStructure>();
        
            _logger.LogInformation("GenerateDocumentation returned {Count} items for {Assembly}", 
                allDocumentation.Count(), assembly.GetName().Name);
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

        try
        {
            var assemblyName = assembly.GetName().Name;

            List<DocumentationStructure> filteredDocumentation = allDocumentation
                .Where<DocumentationStructure>(d =>
                {
                    try
                    {
                        var prop = d.GetType().GetProperty("AssemblyName");
                        return prop != null && string.Equals(prop.GetValue(d)?.ToString(), assemblyName,
                            StringComparison.OrdinalIgnoreCase);
                    }
                    catch
                    {
                        return false;
                    }
                })
                .OrderBy<DocumentationStructure, string>(d =>
                {
                    try
                    {
                        return d.GetType().GetProperty("ClassName")?.GetValue(d)?.ToString() ?? "";
                    }
                    catch
                    {
                        return "";
                    }
                }).ToList<DocumentationStructure>();

            string changelogMarkdown = "";
            if (System.IO.File.Exists(changelogPath))
            {
                changelogMarkdown = ChangelogTool.GetMarkdown(assembly);
            }

            string version = assembly.GetCustomAttribute<System.Reflection.AssemblyInformationalVersionAttribute>()
                                 ?.InformationalVersion
                             ?? assembly.GetName().Version?.ToString()
                             ?? "1.0.0";

            if (version.Contains('+'))
            {
                version = version.Substring(0, version.IndexOf('+'));
            }

            return new LibraryDocumentationViewModel
            {
                LibraryName = displayName,
                Description = description,
                Version = version,
                NuGetPackageId = nugetPackageId,
                Documentation = filteredDocumentation,
                ChangelogMarkdown = changelogMarkdown
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to process documentation for {Assembly}", assembly.FullName);
            return new LibraryDocumentationViewModel();
        }
    }


    static string GetSolutionRoot()
    {
        return AppContext.BaseDirectory;
    }
}