using System;
using System.Linq.Expressions;

namespace Core.Utilities
{
    public static class GenerateExpression
    {
        public static Expression GetMemberExpression(Expression parameter, string propertyName)
        {
            if (propertyName.Contains("."))
            {
                var index = propertyName.IndexOf(".", StringComparison.Ordinal);
                var subParam = Expression.Property(parameter, propertyName.Substring(0, index));
                return GetMemberExpression(subParam, propertyName.Substring(index + 1));
            }

            return Expression.Property(parameter, propertyName);
        }
    }
}