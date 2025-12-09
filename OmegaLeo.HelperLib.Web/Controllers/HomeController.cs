// File: `OmegaLeo.HelperLib.Web/Controllers/HomeController.cs`
using System.Diagnostics;
using System.Reflection;
using GameDevLibrary.Helpers;
using Microsoft.AspNetCore.Mvc;
using NetFlow.DocumentationHelper.Library.Helpers;
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

        _libraryCache = new Dictionary<string, LibraryDocumentationViewModel>
        {
            ["core"] = GenerateLibraryDocs(
                typeof(BenchmarkUtility).Assembly,
                Path.Join(root, "OmegaLeo.HelperLib", "CHANGELOG.md"),
                "Core Library",
                "Core utilities and helper functions for general C# development",
                "OmegaLeo.HelperLib"
            ),
            ["changelog"] = GenerateLibraryDocs(
                typeof(Changelog.Models.Changelog).Assembly,
                Path.Join(root, "OmegaLeo.HelperLib.Changelog", "CHANGELOG.md"),
                "Changelog Library",
                "Tools for managing and generating changelogs",
                "OmegaLeo.HelperLib.Changelog"
            ),
            ["game"] = GenerateLibraryDocs(
                typeof(OmegaLeo.HelperLib.Game.Models.ConsoleCommand).Assembly,
                Path.Join(root, "OmegaLeo.HelperLib.Game", "CHANGELOG.md"),
                "Game Development Library",
                "Game development utilities for Unity and Godot",
                "OmegaLeo.HelperLib.Game"
            )
        };
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

    private LibraryDocumentationViewModel GenerateLibraryDocs(Assembly assembly, string changelogPath, string displayName, string description, string nugetPackageId)
    {
        var allDocumentation = DocumentationHelperTool.GenerateDocumentation();
        var assemblyName = assembly.GetName().Name;

        var filteredDocumentation = allDocumentation
            .Where(d => d.AssemblyName.Equals(assemblyName, StringComparison.OrdinalIgnoreCase))
            .OrderBy(d => d.ClassName)
            .ToList();

        string changelogMarkdown = "";
        if (System.IO.File.Exists(changelogPath))
        {
            changelogMarkdown = ChangelogTool.GetMarkdown(assembly);
        }

        string version = assembly.GetCustomAttribute<System.Reflection.AssemblyInformationalVersionAttribute>()?.InformationalVersion
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


    static string GetSolutionRoot()
    {
        string dir = AppContext.BaseDirectory;

        while (!string.IsNullOrEmpty(dir))
        {
            if (Directory.GetFiles(dir, "*.sln").Length > 0)
                return dir;

            dir = Directory.GetParent(dir)?.FullName;
        }

        throw new Exception("Solution root not found.");
    }
}
