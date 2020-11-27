using System;
using System.Linq;
using System.Reflection;
using NHibernate;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Linq;

namespace SokairykFramework.Extensions
{
    public static class QueryableExtensions
    {
        public static string GetSQLStatement(this IQueryable queryable)
        {
            //NHibernate
            if (queryable.Provider is DefaultQueryProvider)
            {
                var session = queryable.Provider.GetPropertyValue<ISession>("Session");
                var sessionImplementation = session.GetSessionImplementation();

                var nhLinqExpression = new NhLinqExpression(queryable.Expression, sessionImplementation.Factory);
                var translatorFactory = new ASTQueryTranslatorFactory();
                var translator = translatorFactory.CreateQueryTranslators(nhLinqExpression, null, false, sessionImplementation.EnabledFilters, sessionImplementation.Factory).First();

                var parameters = nhLinqExpression.ParameterValuesByName.ToDictionary(x => x.Key, x => x.Value.Item1);

                return string.Join(Environment.NewLine, "Statement:",
                    translator.SQLString,
                    "",
                    string.Join(Environment.NewLine, parameters.Select(p => $"{p.Key}: {p.Value}")));
            }

            return $"{MethodBase.GetCurrentMethod().Name} method has not implementation for {queryable.Provider.GetFriendlyTypeName()}";
        }
    }
}