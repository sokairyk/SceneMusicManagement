﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace SokairykFramework.Extensions
{
#if NET5_0
    internal class InspectiveAssemblyLoadContext : AssemblyLoadContext
    {
        public InspectiveAssemblyLoadContext() : base(isCollectible: true) { }
        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }
    }
#endif

    public static class ReflectionExtensions
    {
        private static readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string[] _assemblyExtensions = new[] { "dll", "exe" };
#if NET5_0
        private static InspectiveAssemblyLoadContext _inspectiveAssemblyLoadContext;
#endif

        private static IEnumerable<string> FindAllAssembliesNames(IEnumerable<string> paths = null, string filter = null, bool recursive = false)
        {
            var searchPaths = new List<string>(paths ?? new string[] { }) { _baseDirectory };
            return searchPaths.Distinct()
                .Where(Directory.Exists)
                .SelectMany(d => _assemblyExtensions.SelectMany(e => Directory.GetFiles(_baseDirectory, $"*{filter}*.{e}", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)))
                .Distinct();
        }

        public static IEnumerable<Type> FindTypeInAssemblies(Predicate<Type> predicate, IEnumerable<string> searchPaths = null, string filter = null, bool recursive = false)
        {
            predicate = predicate ?? (a => { return false; });

#if NET5_0
            _inspectiveAssemblyLoadContext = new InspectiveAssemblyLoadContext();
#endif
            foreach (var assemblyPath in FindAllAssembliesNames(searchPaths, filter, recursive))
            {
                if (!File.Exists(assemblyPath)) continue; //This shouldn't happen....

                //Load from stream to avoid assembly file locking
                using (var filestream = new FileStream(assemblyPath, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
#if NET5_0
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
                    try
                    {
                        return predicate(t);
                    }
                    catch
                    {
                        return false;
                    }
                }));

#if NET5_0
            //TODO: Test this line by instantiating a resolved type after a context unload
            _inspectiveAssemblyLoadContext.Unload();
#endif

            return matchedTypes;
        }

        public static IEnumerable<string> FindAssembliesFromTypes(IEnumerable<Type> types)
        {
            return types.Select(t => t.GetTypeInfo().Assembly.Location).Distinct();
        }

        public static T GetPropertyValue<T>(this object instance, string propertyName)
        {
            var property = instance.GetType().GetProperty("Session");
            return (T)property?.GetValue(instance, null);
        }

        public static string GetFriendlyTypeName(this object instance)
        {
            var type = instance is Type ? (Type)instance : instance.GetType();
            var typeName = type.Name.IndexOf("`") > -1 ? type.Name.Substring(0, type.Name.IndexOf("`")) : type.Name;

            return type.IsGenericType
                ? $"{typeName}<{string.Join(", ", type.GenericTypeArguments.Select(t => t.GetFriendlyTypeName()))}>"
                : typeName;
        }
    }
}