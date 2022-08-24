using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Sokairyk.DynamicCode
{
    public abstract class CompilationManager : ILanguageCompilation
    {
        private readonly ILogger _logger;

        public CompilationManager(ILogger logger)
        {
            _logger = logger;
        }

        public abstract Compilation CreateAssemblyCompilation(string code, string assemblyName = null, IEnumerable<string> assemblyReferences = null, bool enableOptimisations = false);

        public Stream GetAssemblyStream(Compilation compilation, out Diagnostic[] diagnostics)
        {
            var stream = new MemoryStream();
            var result = compilation.Emit(stream);
            diagnostics = result.Diagnostics.ToArray();

            if (result.Success)
            {
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }

            return Stream.Null;
        }

        public bool CompileCodeAndLoadAssemblyIntoContext(string code, IEnumerable<string> assemblyReferences)
        {
            var stream = GetAssemblyStream(CreateAssemblyCompilation(code, assemblyReferences: assemblyReferences), out var diagnostics);

            if (stream == Stream.Null) return false;

            try
            {
                System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(stream);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }
    }
}
