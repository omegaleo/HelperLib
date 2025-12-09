using System.Diagnostics;
using System.Reflection;
using GameDevLibrary.Helpers;
using Microsoft.AspNetCore.Mvc;
using OmegaLeo.HelperLib.Changelog.Tools;
using OmegaLeo.HelperLib.Web.Models;

namespace OmegaLeo.HelperLib.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var root = GetSolutionRoot();

        var changelogs = new Dictionary<string, Assembly>()
        {
            {Path.Join(root, "OmegaLeo.HelperLib", "CHANGELOG.md"), typeof(BenchmarkUtility).Assembly},
            {Path.Join(root, "OmegaLeo.HelperLib.Changelog", "CHANGELOG.md"), typeof(Changelog.Models.Changelog).Assembly},
        };
        
        return View(GenerateChangelog(changelogs));
    }

    private IEnumerable<string> GenerateChangelog(Dictionary<string, Assembly> changelogs)
    {
        foreach (var changelog in changelogs)
        {
            var changelogMarkdown = ChangelogTool.GetMarkdown(changelog.Value);

            yield return $"# {changelog.Value.GetName().Name}{Environment.NewLine}{changelogMarkdown}";
        }
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
    
    static string GetSolutionRoot()
    {
        string dir = AppContext.BaseDirectory;

        while (!string.IsNullOrEmpty(dir))
        {
            if (Directory.GetFiles(dir, "*.sln").Length > 0)
                return dir; // Found the solution root

            dir = Directory.GetParent(dir)?.FullName; // Move up a directory
        }

        throw new Exception("Solution root not found.");
    }
}