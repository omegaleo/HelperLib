// See https://aka.ms/new-console-template for more information

// /media/omegaleo/Development/Library Dev/HelperLib

using GameDevLibrary.Extensions;
using OmegaLeo.HelperLib.Git;
using OmegaLeo.HelperLib.Git.Models;

Console.WriteLine("Insert repo path to check changes:");
var repo = Console.ReadLine();

if (repo.IsNullOrEmpty())
{
    repo = "/media/omegaleo/Development/Library Dev/HelperLib";
}

var git = new GitClient(repo);

var changes = git.GetChanges();

if (changes != null)
{
    RecursiveOutput(new List<ChangeFolder>() { changes });
}

Console.ReadLine();


void RecursiveOutput(List<ChangeFolder> folders, int depth = 0)
{
    foreach (var folder in folders)
    {
        Console.WriteLine($"{new string('\t', depth + 1)}>{folder.FolderName}");
        if (folder.SubFolders.Any())
        {
            RecursiveOutput(folder.SubFolders, depth + 1);
        }

        foreach (var change in folder.ChangesInFolder)
        {
            Console.WriteLine($"{new string('\t', depth + 2)}>{Path.GetFileName(change.Path)} ({change.Status})");
        }
    }
}