using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using OmegaLeo.HelperLib.Changelog.Attributes;

namespace OmegaLeo.HelperLib.Changelog.Tools
{
    [Changelog("v1.0.0", "Created the class ChangelogTool to extract changelogs via attributes")]
    public class ChangelogTool
    {
        public static Dictionary<string, List<Models.Changelog>> ExtractChangelog(Assembly assembly)
        {
            var changelogByVersion = new Dictionary<string, List<Models.Changelog>>();

            foreach (var type in assembly.GetTypes())
            {
                foreach (var attr in type.GetCustomAttributes<ChangelogAttribute>())
                {
                    AddChangelogEntry(changelogByVersion, attr.Version, $"Class: {type.Name}", attr.Description);
                }

                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                {
                    foreach (var attr in method.GetCustomAttributes<ChangelogAttribute>())
                        AddChangelogEntry(changelogByVersion, attr.Version, $"Method: {type.Name}.{method.Name}()", attr.Description);
                }
            }

            changelogByVersion = new Dictionary<string, List<Models.Changelog>>(changelogByVersion.OrderByDescending(x => x.Key));
        
            return changelogByVersion;
        }

        public static string GetMarkdown(Assembly assembly)
        {
            var changelog = ExtractChangelog(assembly);
        
            var sb = new StringBuilder();
        
            sb.AppendLine("# Changelog\n");
            foreach (var version in changelog)
            {
                sb.AppendLine($"## Version {version.Key}\n");
                foreach (var entry in version.Value)
                {
                    sb.AppendLine($"- **{entry.Target}**: {entry.Description}");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    
        private static void AddChangelogEntry(Dictionary<string, List<Models.Changelog>> changelog, string version, string target, string description)
        {
            if (!changelog.ContainsKey(version))
                changelog[version] = new List<Models.Changelog>();

            changelog[version].Add(new Models.Changelog(target, description));
        }
    }
}