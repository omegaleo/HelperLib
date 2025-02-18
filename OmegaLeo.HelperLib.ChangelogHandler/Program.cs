﻿// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using GameDevLibrary.Helpers;
using OmegaLeo.HelperLib.Changelog.Models;
using OmegaLeo.HelperLib.Changelog.Tools;

var root = GetSolutionRoot();

var changelogs = new Dictionary<string, Assembly>()
{
    {Path.Join(root, "OmegaLeo.HelperLib.Changelog", "CHANGELOG.md"), typeof(Changelog).Assembly},
    {Path.Join(root, "OmegaLeo.HelperLib", "CHANGELOG.md"), typeof(BenchmarkUtility).Assembly},
};

foreach (var changelog in changelogs)
{
    GenerateChangelog(changelog.Key, changelog.Value);
}


void GenerateChangelog(string path, Assembly libraryAssembly)
{
    var changelogMarkdown = ChangelogTool.GetMarkdown(libraryAssembly);
    
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