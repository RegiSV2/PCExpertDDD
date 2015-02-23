using System;
using System.Diagnostics;
using System.Reflection;
using NUnit.Framework;
using PCExpert.Core.Domain.Specifications;
using PCExpert.DomainFramework;

namespace PCExpert.Core.Domain.Tests.Specifications
{
	[TestFixture]
	public class EntityHasIdSpecificationTests
	{
		private class EntityStub : Entity
		{
			public EntityStub(Guid id)
				:base(id)
			{
			}
		}

		[Test]
		public void IsSatisfiedBy_EntityHasSpecifiedId_ShouldPass()
		{
			//Arrange
			var id = Guid.NewGuid();
			var specification = new EntityHasIdSpecification<EntityStub>(id);
			var entity = new EntityStub(id);

			//Assert
			Assert.That(specification.IsSatisfiedBy(entity));
		}

		[Test]
		public void IsSatisfiedBy_EntityHasNotSpecified_ShouldNotPass()
		{
			//Arrange
			var specification = new EntityHasIdSpecification<EntityStub>(Guid.NewGuid());
			var entity = new EntityStub(Guid.NewGuid());

			//Assert
			Assert.That(!specification.IsSatisfiedBy(entity));
		}
	}
}