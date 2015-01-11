using System;
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
	}
}