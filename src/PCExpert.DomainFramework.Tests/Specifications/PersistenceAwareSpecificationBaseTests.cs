using System;
using NUnit.Framework;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.DomainFramework.Tests.Specifications
{
	[TestFixture]
	public class PersistenceAwareSpecificationBaseTests
	{
		[Test]
		public void IsSatisfiedBy_ShouldReturnSameResultAs_GetConditionExpressionCall()
		{
			//Arrange
			var entity = new TestEntity();
			var specification = new TestPersistenceAwareSpec<TestEntity>(x => x.Id != Guid.Empty);

			AssertResultsAreEqual(specification, entity);

			entity.WithId(Guid.NewGuid());

			AssertResultsAreEqual(specification, entity);
		}

		private static void AssertResultsAreEqual(TestPersistenceAwareSpec<TestEntity> specification, TestEntity entity)
		{
			Assert.That(specification.IsSatisfiedBy(entity) ==
			            specification.GetConditionExpression().Compile().Invoke(entity));
		}

		private class TestEntity : Entity
		{
		}
	}
}