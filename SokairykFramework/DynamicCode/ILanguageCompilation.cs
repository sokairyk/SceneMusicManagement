using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace SokairykFramework.DynamicCode
{
    public interface ILanguageCompilation
    {
        Compilation CreateAssemblyCompilation(string code, string assemblyName, IEnumerable<string> assemblyReferences, bool enableOptimisations);
    }
}
