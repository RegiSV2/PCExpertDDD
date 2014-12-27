using NUnit.Framework;
using PCExpert.Core.DomainFramework.Specifications;

namespace PCExpert.Core.DomainFramework.Tests.Specifications
{
	[TestFixture]
	public class LogicSpecificationsTests
	{
		protected TestEntity Entity;

		[SetUp]
		public void EstablishContext()
		{
			Entity = new TestEntity(1, 2);
		}

		protected class TestEntity : Entity
		{
			public TestEntity(int a, int b)
			{
				A = a;
				B = b;
			}

			public int A { get; set; }
			public int B { get; set; }
		}
	}

	public class PersistenceAndSpecificationTests : LogicSpecificationsTests
	{
		[Test]
		public void IsSatisfied_ConditionIsTrue_ShouldReturnTrue()
		{
			var andSpecification = CreateSpecification(1, 2);

			Assert.That(andSpecification.IsSatisfiedBy(Entity));
		}

		[Test]
		[TestCase(2, 2)]
		[TestCase(1, 1)]
		[TestCase(3, 3)]
		public void IsSatisfied_ConditionIsFalse_ShouldReturnFalse(int specA, int specB)
		{
			var andSpecification = CreateSpecification(specA, specB);

			Assert.That(!andSpecification.IsSatisfiedBy(Entity));
		}

		private ISpecification<TestEntity> CreateSpecification(int specA, int specB)
		{
			return new PersistenceAndSpecification<TestEntity>(
				new TestPersistenceAwareSpec<TestEntity>(x => x.A == specA),
				new TestPersistenceAwareSpec<TestEntity>(x => x.B == specB));
		}
	}
}