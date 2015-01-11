using System;
using System.Linq.Expressions;
using NUnit.Framework;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.DomainFramework.Tests.Utils
{
	[TestFixture]
	public class ExpressionReflectionTests
	{
		[Test]
		public void Property_NullArgument_ShouldThrowArgumentNullException()
		{
			Assert.That(() => ExpressionReflection.Property<DateTime>(null),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Property_NotNullArgument_ShouldReturnCalledProperty()
		{
			//Arrange
			Expression<Func<DateTime, object>> propertyExpression = x => x.Day;
			var property = typeof (DateTime).GetProperty("Day");

			Assert.That(ExpressionReflection.Property(propertyExpression), Is.EqualTo(property));
		}
	}
}