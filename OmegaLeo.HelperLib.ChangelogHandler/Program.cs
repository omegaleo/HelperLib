// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using OmegaLeo.HelperLib.Changelog.Helpers;
using OmegaLeo.HelperLib.Helpers;
using OmegaLeo.HelperLib.Changelog.Models;
using OmegaLeo.HelperLib.Changelog.Tools;

var root = GetSolutionRoot();

var changelogs = new Dictionary<string, Assembly>()
{
    {Path.Join(root, "OmegaLeo.HelperLib.Changelog", "CHANGELOG.md"), typeof(Changelog).Assembly},
    {Path.Join(root, "OmegaLeo.HelperLib", "CHANGELOG.md"), typeof(BenchmarkUtility).Assembly},
    {Path.Join(root, "OmegaLeo.HelperLib.Game", "CHANGELOG.md"), typeof(OmegaLeo.HelperLib.Game.Models.ConsoleCommand).Assembly},
    {Path.Join(root, "OmegaLeo.HelperLib.Documentation", "CHANGELOG.md"), typeof(OmegaLeo.HelperLib.Documentation.Helpers.DocumentationHelperTool).Assembly},
    {Path.Join(root, "OmegaLeo.HelperLib.Shared", "CHANGELOG.md"), typeof(OmegaLeo.HelperLib.Shared.Attributes.ChangelogAttribute).Assembly},
};

foreach (var changelog in changelogs)
{
    GenerateChangelog(changelog.Key, changelog.Value);
}


void GenerateChangelog(string path, Assembly libraryAssembly)
{
    var changelogMarkdown = $@"# Changelog  
{ChangelogHelper.GetChangelogMarkdown(new []{libraryAssembly})}";
    
    File.WriteAllText(path,changelogMarkdown);
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