using System;
using System.Linq.Expressions;

namespace CompanyX.Base.Extensions
{
    /// <summary>
    /// Expression Extensions
    /// </summary>
    public static class ExpressionExtensions
    {
        public static string GetParameterName<T>(this Expression<Func<T>> parameterExpr)
        {
            var body = ((MemberExpression)parameterExpr.Body);
            return body.Member.Name;
        }
    }
}
