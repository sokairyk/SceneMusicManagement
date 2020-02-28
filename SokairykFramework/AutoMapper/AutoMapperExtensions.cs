using AutoMapper;
using SokairykFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SokairykFramework.AutoMapper
{
    public static class AutoMapperExtensions
    {
        internal class ManualProfile : Profile
        {
            public ManualProfile()
            {
                //Clear default automatic mappings
                DefaultMemberConfig.MemberMappers.Clear();
                DefaultMemberConfig.NameMapper.NamedMappers.Clear();
                ShouldMapProperty = (p) => false;
                ShouldMapField = (f) => false;
            }
        }

        public static MapperConfiguration CreateConfig(IEnumerable<string> assemblyPaths = null)
        {
            var config = new MapperConfiguration(cfg =>
            {
                var manualProfile = new ManualProfile();
                cfg.AddProfile(manualProfile);

                foreach (var configuratorType in ReflectionExtensions.FindTypeInAssemblies(t => t.GetInterfaces().Any(i => i == typeof(IAutoMapperConfigurator)), assemblyPaths))
                {
                    var configuratorInstance = (IAutoMapperConfigurator)Activator.CreateInstance(configuratorType);
                    configuratorInstance.ManualConfiguration(manualProfile);
                }
            });

            config.CompileMappings();
            return config;
        }

        public static void CreateMappings<TSource, TDestination>(this IProfileExpression config, Expression<Func<TSource, TDestination>> expression)
        {
            var memberInitExpression = expression.Body as MemberInitExpression;

            if (expression.Body == null)
                throw new ArgumentException("Expression should only be an InitExpression! eg. (SourceClass x) => new DestinationClass{ Param1 = x.Param1, Param2 = x.Param3 ... }");

            var map = config.CreateMap<TSource, TDestination>();

            foreach (var binding in memberInitExpression.Bindings)
            {
                if (!(binding is MemberAssignment && binding.Member is PropertyInfo)) continue; //...or throw Excepetion?

                var bindingExpression = (binding as MemberAssignment).Expression;
                var bindingProperty = binding.Member as PropertyInfo;
                var memberDelegate = typeof(Func<,>).MakeGenericType(typeof(TSource), bindingProperty.PropertyType);
                var lambaMember = Expression.Lambda(memberDelegate, bindingExpression, expression.Parameters);

                var mapFromSpecificSignature = typeof(IMemberConfigurationExpression<TSource, TDestination, object>).GetMethods()
                                                                                                                    .SingleOrDefault(m => m.Name == "MapFrom"
                                                                                                                                          && m.GetParameters().Count() == 1
                                                                                                                                          && m.GetParameters()?.FirstOrDefault()?.Name.ToLower() == "mapexpression");
                if (mapFromSpecificSignature == null)
                    throw new Exception(@"AutoMapper's interface IMemberConfigurationExpression does not define a method with the following signature:
    void MapFrom<TSourceMember>(Expression<Func<TSource, TSourceMember>> mapExpression)");

                var methodInfo = mapFromSpecificSignature.MakeGenericMethod(bindingExpression.Type);
                map.ForMember(bindingProperty.Name, m => methodInfo.Invoke(m, new object[] { lambaMember }));
            }
        }

    }
}
