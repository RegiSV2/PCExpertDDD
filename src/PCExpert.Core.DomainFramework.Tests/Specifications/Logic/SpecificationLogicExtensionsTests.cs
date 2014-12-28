using NUnit.Framework;
using PCExpert.Core.DomainFramework.Specifications;
using PCExpert.Core.DomainFramework.Specifications.Logic;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.DomainFramework.Tests.Specifications.Logic
{
	public class SpecificationLogicExtensionsTests : LogicSpecificationsTests
	{
		[Test]
		public void And_AllSpecificationsArePersistenceAware_ShouldReturnPersistenceAwareSpecification()
		{
			//Assert
			var specification1 = new TestPersistenceAwareSpec<TestEntity>(x => x.A == 1);
			var specification2 = new TestPersistenceAwareSpec<TestEntity>(x => x.B == 2);

			//Act
			var combinedSpec = SpecificationLogic.And(specification1, specification2);

			//Assert
			Assert.That(combinedSpec, Is.AssignableTo<PersistenceAndSpecification<TestEntity>>());
		}

		[Test]
		public void And_AnySpecificationIsNotPersistenceAware_ShouldReturnNotPersistenceAwareSpecification()
		{
			//Assert
			var specification1 = new TestPersistenceAwareSpec<TestEntity>(x => x.A == 1);
			var specification2 = new TestPersistenceAwareSpec<TestEntity>(x => x.B == 2);
			var specification3 = new TestSpec<TestEntity>(x => x.B == 2);

			//Act
			var combinedSpec = SpecificationLogic.And(specification1, specification2, specification3);

			//Assert
			Assert.That(combinedSpec,
				Is.Not.AssignableTo<PersistenceAndSpecification<TestEntity>>()
					.And.AssignableTo<Specification<TestEntity>>());
		}
	}
}