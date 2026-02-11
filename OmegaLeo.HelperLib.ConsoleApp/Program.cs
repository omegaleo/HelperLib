// See https://aka.ms/new-console-template for more information

// /media/omegaleo/Development/Library Dev/HelperLib

using System.Reflection;
using OmegaLeo.HelperLib.Extensions;
using OmegaLeo.HelperLib.Helpers;
using LibGit2Sharp;
using OmegaLeo.HelperLib.Changelog.Tools;
using OmegaLeo.HelperLib.Git;
using OmegaLeo.HelperLib.Git.Models;

Console.WriteLine("Insert repo path to check changes:");
var repo = Console.ReadLine();

if (repo.IsNullOrEmpty())
{
    repo = "/media/omegaleo/Development/Library Dev/HelperLib";
}

var git = new GitClient(repo);

var result = BenchmarkUtility.Record(() =>
{
    var changes = git.GetChanges();

    if (changes.FolderName.IsNotNullOrEmpty())
    {
        RecursiveOutput(new List<ChangeFolder>() { changes });

        var option = 99;

        while (option != 0)
        {
            ShowOptions();
            if (option != 99)
            {
                switch (option)
                {
                    case 1:
                        Console.WriteLine("Write the commit message");
                        var msg = Console.ReadLine();

                        while (msg.IsNullOrEmpty())
                        {
                            Console.WriteLine("Empty message detected, please write a commit message...");
                            msg = Console.ReadLine();
                        }

                        var signature = new Signature("Nuno 'Omega Leo' Diogo", "nunodiogo@omegaleo.pt",
                            DateTimeOffset.Now);
                        git.Commit(msg, signature);

                        break;
                    case 2:
                        git.Push();
                        break;
                    default:
                        Console.WriteLine("Invalid Option!");
                        break;
                }
            }

            int.TryParse(Console.ReadLine(), out option);
        }
    }
    else
    {
        Console.WriteLine("No changes found in specified repository");
    }
});

Console.WriteLine($"Time for checking for changes: {result} ms");

Console.ReadLine();


void RecursiveOutput(List<ChangeFolder> folders, int depth = 0)
{
    foreach (var folder in folders)
    {
        Console.WriteLine($"{new string('\t', depth)}>{folder.FolderName}");
        if (folder.SubFolders.Any())
        {
            RecursiveOutput(folder.SubFolders, depth + 1);
        }

        foreach (var change in folder.ChangesInFolder)
        {
            Console.WriteLine($"{new string('\t', depth + 1)}>{Path.GetFileName(change.Path)} ({change.Status})");
        }
    }
}

void ShowOptions()
{
    Console.Clear();
    Console.WriteLine(new string('=', 20));
    Console.WriteLine("What do you want to do in this repository?");
    Console.WriteLine("1. Commit all changes");
    Console.WriteLine("2. Push");
}