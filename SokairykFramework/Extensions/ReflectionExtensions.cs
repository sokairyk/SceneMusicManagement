using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace SokairykFramework.Extensions
{
    public class ReflectionExtensions
    {
        private static readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string[] _assemblyExtensions = new[] { "dll", "exe" };

        private static IEnumerable<string> FindAllAssembliesNames(IEnumerable<string> paths = null)
        {
            var searchPaths = new List<string>(paths ?? new string[] { });
            searchPaths.Add(_baseDirectory);
            return searchPaths.Distinct()
                              .Where(p => Directory.Exists(p))
                              .SelectMany(d => _assemblyExtensions.SelectMany(e => Directory.GetFiles(_baseDirectory, $"*.{e}", SearchOption.AllDirectories)))
                              .Distinct();
        }

        public static IEnumerable<Type> FindTypeInAssemblies(Predicate<Type> predicate, IEnumerable<string> searchPaths = null)
        {
            predicate = predicate ?? (a => { return false; });

            foreach (var assemblyPath in FindAllAssembliesNames(searchPaths))
                AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);

            return AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic)
                                            .SelectMany(a => a.GetTypes().Where(t =>
                                            {
                                                try { return predicate(t); }
                                                catch { return false; }
                                            }));
        }

        public static IEnumerable<string> FindAssembliesFromTypes(IEnumerable<Type> types)
        {
            return types.Select(t => t.GetTypeInfo().Assembly.Location).Distinct();
        }
    }
}
