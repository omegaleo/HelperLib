using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using OmegaLeo.HelperLib.Shared.Attributes;

namespace OmegaLeo.HelperLib.Changelog.Helpers
{
    [Documentation(nameof(ChangelogHelper),
        "Helper class to generate changelog markdown from ChangelogAttributes in the provided Assemblies.")]
    [Changelog("1.2.0", "Created ChangelogHelper to generate markdown from changelog attributes.")]
    public static class ChangelogHelper
    {
        [Documentation(nameof(GetChangelogMarkdown),
            "Generates a markdown formatted changelog from the changelog attributes in the provided assemblies.")]
        public static string GetChangelogMarkdown(IEnumerable<Assembly> assemblies)
        {
            var changelog = new StringBuilder();
            var entriesByVersionDate =
                new Dictionary<(string Version, string Date),
                    Dictionary<string, List<(string ClassName, string Description)>>>();

            foreach (var assembly in assemblies)
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        // Get type-level changelog attributes
                        var typeAttrs = type.GetCustomAttributes<ChangelogAttribute>().ToList();

                        // Get member-level changelog attributes (methods, properties, fields, etc.)
                        var memberAttrs = type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic |
                                                          BindingFlags.Static | BindingFlags.Instance)
                            .SelectMany(m => m.GetCustomAttributes<ChangelogAttribute>()
                                .Select(attr => (Member: m, Attribute: attr)))
                            .ToList();

                        if (typeAttrs.Any() || memberAttrs.Any())
                        {
                            var namespaceName = type.Namespace ?? "Global";
                            var className = type.Name.Split('`')[0];

                            // Process type-level attributes
                            foreach (var attr in typeAttrs)
                            {
                                AddEntry(entriesByVersionDate, namespaceName, className, attr.Version, attr.Date,
                                    attr.Description);
                            }

                            // Process member-level attributes
                            foreach (var (member, attr) in memberAttrs)
                            {
                                var memberName = $"{className}.{member.Name}";
                                AddEntry(entriesByVersionDate, namespaceName, memberName, attr.Version, attr.Date,
                                    attr.Description);
                            }
                        }
                    }
                }
                catch (ReflectionTypeLoadException)
                {
                    // Skip assemblies that can't be fully loaded
                }
            }

            foreach (var versionGroup in entriesByVersionDate.OrderByDescending(x => x.Key.Version))
            {
                changelog.AppendLine($"## Version {versionGroup.Key.Version}");
                if (!string.IsNullOrEmpty(versionGroup.Key.Date))
                {
                    changelog.AppendLine($"### {versionGroup.Key.Date}");
                }

                changelog.AppendLine();

                foreach (var namespaceGroup in versionGroup.Value.OrderBy(x => x.Key))
                {
                    changelog.AppendLine($"**{namespaceGroup.Key}**");
                    foreach (var entry in namespaceGroup.Value.OrderBy(x => x.ClassName))
                    {
                        changelog.AppendLine($@"- **{entry.ClassName}**:
  - {entry.Description}");
                    }

                    changelog.AppendLine();
                }
            }

            return changelog.ToString();
        }

        private static void AddEntry(
            Dictionary<(string Version, string Date), Dictionary<string, List<(string ClassName, string Description)>>>
                entriesByVersionDate,
            string namespaceName,
            string className,
            string version,
            string date,
            string description)
        {
            var key = (version, date ?? string.Empty);

            if (!entriesByVersionDate.ContainsKey(key))
                entriesByVersionDate[key] = new Dictionary<string, List<(string, string)>>();

            if (!entriesByVersionDate[key].ContainsKey(namespaceName))
                entriesByVersionDate[key][namespaceName] = new List<(string, string)>();

            entriesByVersionDate[key][namespaceName].Add((className, description));
        }
    }
}