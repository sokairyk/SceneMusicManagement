using Microsoft.CodeAnalysis;

namespace Sokairyk.DynamicCode
{
    public interface ILanguageCompilation
    {
        Compilation CreateAssemblyCompilation(string code, string assemblyName, IEnumerable<string> assemblyReferences, bool enableOptimisations);
    }
}
