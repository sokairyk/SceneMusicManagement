using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace SokairykFramework.CodeGeneration
{
    public interface ILanguageService
    {
        Compilation CreateLibraryCompilation(string code, string assemblyName, IEnumerable<string> assemblyReferences, bool enableOptimisations);
    }
}
