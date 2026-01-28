using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace OmegaLeo.HelperLib.Git.Models
{
    public class ChangeFolder
    {
        public string FolderName { get; set; }
        public List<ChangeFolder> SubFolders { get; set; }
        public IEnumerable<TreeEntryChanges> ChangesInFolder { get; set; }

        public ChangeFolder()
        {
            FolderName = "";
            SubFolders = new List<ChangeFolder>();
            ChangesInFolder = new List<TreeEntryChanges>();
        }
        
        public ChangeFolder RecursiveGet(List<string> paths)
        {
            if (!paths.Any()) return null;
            
            var tempPaths = paths;
            var pathToCheck = tempPaths.FirstOrDefault();

            ChangeFolder folder = new ChangeFolder();

            if (FolderName.Equals(pathToCheck, StringComparison.OrdinalIgnoreCase))
            {
                folder = this;
            }
            else if (SubFolders.Any(x => x.FolderName.Equals(pathToCheck)))
            {
                folder = SubFolders.FirstOrDefault(x => x.FolderName.Equals(pathToCheck))!;
            }
            else
            {
                folder.FolderName = pathToCheck;
                SubFolders.Add(folder);
            }
            
            tempPaths.RemoveAt(0);

            if (tempPaths.Any())
            {
                return folder.RecursiveGet(tempPaths);
            }
            else
            {
                return folder;
            }
        }
    }
}