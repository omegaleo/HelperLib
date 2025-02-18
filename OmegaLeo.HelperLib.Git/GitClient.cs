using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameDevLibrary.Extensions;
using LibGit2Sharp;
using OmegaLeo.HelperLib.Git.Models;

namespace OmegaLeo.HelperLib.Git
{
    public class GitClient
    {
        private Repository _repo;
        
        public GitClient(string path)
        {
            if (path.IsNullOrEmpty())
            {
                path = AppDomain.CurrentDomain.BaseDirectory;
            }

            _repo = new Repository(path);
        }

        public void CheckoutBranch(string branchName)
        {
            var branch = _repo.Branches[branchName];

            if (branch == null)
            {
                branch = _repo.CreateBranch(branchName);
            }

            Commands.Checkout(_repo, branch);
        }

        public bool HasChanges() => _repo.Diff.Compare<TreeChanges>().Count > 0;
        
        public ChangeFolder GetChanges()
        {
            var folder = new ChangeFolder();
            
            if (HasChanges())
            {
                var changes = _repo.Diff.Compare<TreeChanges>();

                var allChanges = new List<TreeEntryChanges>();
                allChanges.AddRange(changes.Added);
                allChanges.AddRange(changes.Modified);
                allChanges.AddRange(changes.Deleted);

                var uniqueFolderPaths = allChanges.Select(x => Path.GetDirectoryName(x.Path)).Distinct();
                
                foreach (var path in uniqueFolderPaths)
                {
                    if (path.IsNullOrEmpty()) continue;
                    
                    if (path.Contains(Path.DirectorySeparatorChar))
                    {
                        var paths = path.Split(Path.DirectorySeparatorChar);

                        for (var i = 0; i < paths.Length; i++)
                        {
                            var p = paths[i];
                            
                            if (folder.FolderName.Equals(p))
                            {
                                if (folder.ChangesInFolder.Any()) continue;
                                
                                folder.ChangesInFolder =
                                    allChanges.Where(x => Path.GetDirectoryName(x.Path)!.Equals(p, StringComparison.OrdinalIgnoreCase));
                            }
                            else
                            {
                                var pathList = paths[0..(i+1)].ToList();
                                var subFolder = folder.RecursiveGet(pathList);
                                var fullPath = string.Join(Path.DirectorySeparatorChar, pathList);

                                if (!subFolder.ChangesInFolder.Any())
                                {
                                    subFolder.ChangesInFolder = allChanges.Where(x =>
                                        Path.GetDirectoryName(x.Path)!.Equals(fullPath, StringComparison.OrdinalIgnoreCase));
                                }
                            }
                        }
                    }
                    else
                    {
                        folder.FolderName = path;
                        folder.ChangesInFolder =
                            allChanges.Where(x => Path.GetDirectoryName(x.Path)!.Equals(path, StringComparison.OrdinalIgnoreCase));
                    }
                }
            }
            
            return folder;
        }

        public void Commit(string message, Signature author)
        {
            if (HasChanges())
            {
                var changes = _repo.Diff.Compare<TreeChanges>();
                var allChanges = new List<TreeEntryChanges>();
                allChanges.AddRange(changes.Added);
                allChanges.AddRange(changes.Modified);
                allChanges.AddRange(changes.Deleted);

                var filePaths = allChanges.Select(x => x.Path).Where(x => x.IsNotNullOrEmpty());
                
                Commands.Stage(_repo, filePaths);
            }
            
            _repo.Commit(message, author, author);
        }

        public void Push()
        {
            try {
                var remote = _repo.Network.Remotes["origin"];
                var options = new PushOptions();
                var pushRefSpec = @"refs/heads/master";
               _repo.Network.Push(remote, new string[] {pushRefSpec}, options);
            }
            catch (Exception e) {
                Console.WriteLine("Exception:RepoActions:PushChanges " + e.Message);
            }
        }
    }
}