using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;

namespace PCExpert.DomainFramework.Utils
{
	public static class ExpressionReflection
	{
		public static PropertyInfo Property<T>(Expression<Func<T, object>> propertyExpression)
		{
			Argument.NotNull(propertyExpression);
			var member = propertyExpression.Body as MemberExpression ??
			             (MemberExpression) ((UnaryExpression) propertyExpression.Body).Operand;
			return (PropertyInfo) member.Member;
		}

		public static Expression<Func<T, object>> Expression<T>(string propertyName)
		{
			Argument.NotNullAndNotEmpty(propertyName);
			Contract.Ensures(Contract.Result<Expression<Func<T, object>>>() != null);

			var parameter = System.Linq.Expressions.Expression.Parameter(typeof (T), "x");
			Expression property = System.Linq.Expressions.Expression.Property(parameter, typeof (T), propertyName);
			if (property.Type.IsValueType)
				property = System.Linq.Expressions.Expression.Convert(property, typeof (object));

			return System.Linq.Expressions.Expression.Lambda<Func<T, object>>(property, parameter);
		}
	}
}