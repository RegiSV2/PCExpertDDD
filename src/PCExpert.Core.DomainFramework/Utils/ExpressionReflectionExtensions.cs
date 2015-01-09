using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PCExpert.Core.DomainFramework.Utils
{
	public static class ExpressionReflection
	{
		public static PropertyInfo Property<T>(Expression<Func<T, object>> propertyExpression)
		{
			return (PropertyInfo) ((MemberExpression) propertyExpression.Body).Member;
		}
	}
}