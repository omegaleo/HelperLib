using System;
using System.Collections.Generic;

namespace OmegaLeo.HelperLib.Documentation.Models
{
    public class DocumentationAssembly
    {
        public string AssemblyName;
        public IEnumerable<Type> Types;
    }
}