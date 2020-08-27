using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace SokairykFramework.Extensions
{

#if NETCOREAPP3_1
    internal class InspectiveAssemblyLoadContext : AssemblyLoadContext
    {
        public InspectiveAssemblyLoadContext() : base(isCollectible: true) { }
        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }
    }
#endif

    public class ReflectionExtensions
    {
        private static readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string[] _assemblyExtensions = new[] { "dll", "exe" };
#if NETCOREAPP3_1
        private static readonly InspectiveAssemblyLoadContext _inspectiveAssemblyLoadContext = new InspectiveAssemblyLoadContext();
#endif

        private static IEnumerable<string> FindAllAssembliesNames(IEnumerable<string> paths = null, string filter = null, bool recursive = false)
        {
            var searchPaths = new List<string>(paths ?? new string[] { });
            searchPaths.Add(_baseDirectory);
            return searchPaths.Distinct()
                              .Where(p => Directory.Exists(p))
                              .SelectMany(d => _assemblyExtensions.SelectMany(e => Directory.GetFiles(_baseDirectory, $"*{filter}*.{e}", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)))
                              .Distinct();
        }

        public static IEnumerable<Type> FindTypeInAssemblies(Predicate<Type> predicate, IEnumerable<string> searchPaths = null, string filter = null, bool recursive = false)
        {
            predicate = predicate ?? (a => { return false; });

            foreach (var assemblyPath in FindAllAssembliesNames(searchPaths, filter, recursive))
            {
                if (!File.Exists(assemblyPath)) continue; //This shouldn't happen....

                //Load from stream to avoid assembly file locking
                using (var filestream = new FileStream(assemblyPath, FileMode.Open, FileAccess.Read))
                {
                    try
                    {

#if NETCOREAPP3_1
                        _inspectiveAssemblyLoadContext.LoadFromStream(filestream);
#else
                        AssemblyLoadContext.Default.LoadFromStream(filestream);
#endif
                    }
                    catch (BadImageFormatException ex)
                    {
                        // Supress exceptions from incompatible files
                    }
                }
            }

            var matchedTypes = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic)
                                            .SelectMany(a => a.GetTypes().Where(t =>
                                            {
                                                try { return predicate(t); }
                                                catch { return false; }
                                            }));

#if NETCOREAPP3_1
            //TODO: Test this line by instantiating a resolved type after a context unload
            _inspectiveAssemblyLoadContext.Unload();
#endif

            return matchedTypes;
        }

        public static IEnumerable<string> FindAssembliesFromTypes(IEnumerable<Type> types)
        {
            return types.Select(t => t.GetTypeInfo().Assembly.Location).Distinct();
        }
    }
}
