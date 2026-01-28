// File: `OmegaLeo.HelperLib.Web/Models/LibraryDocumentationViewModel.cs`
using OmegaLeo.HelperLib.Documentation.Models;

namespace OmegaLeo.HelperLib.Web.Models
{
    public class LibraryDocumentationViewModel
    {
        public string LibraryName { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string NuGetPackageId { get; set; }
        public List<DocumentationStructure> Documentation { get; set; }
        public string ChangelogMarkdown { get; set; }
    }
}