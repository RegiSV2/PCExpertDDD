using NUnit.Framework;
using PCExpert.Core.DomainFramework.Specifications;
using PCExpert.Core.DomainFramework.Specifications.Logic;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.DomainFramework.Tests.Specifications.Logic
{
	public class AndSpecificationTests : LogicSpecificationsTests
	{
		[Test]
		public void IsSatisfied_AllChildSpecsSatisfied_ShouldReturnTrue()
		{
			var andSpecification = CreateSpecification(1, 2);

			Assert.That(andSpecification.IsSatisfiedBy(Entity));
		}

		[Test]
		[TestCase(2, 2)]
		[TestCase(1, 1)]
		[TestCase(3, 3)]
		public void IsSatisfied_AnyOfChildSpecsNotSatisfied_ShouldReturnFalse(int specA, int specB)
		{
			var andSpecification = CreateSpecification(specA, specB);

			Assert.That(!andSpecification.IsSatisfiedBy(Entity));
		}

		private Specification<TestEntity> CreateSpecification(int specA, int specB)
		{
			return new AndSpecification<TestEntity>(
				new TestSpec<TestEntity>(x => x.A == specA),
				new TestSpec<TestEntity>(x => x.B == specB));
		}
	}
}