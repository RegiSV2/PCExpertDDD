using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using PCExpert.Core.Domain.Repositories;
using PCExpert.Core.Tests.Utils;
using PCExpert.DomainFramework.DataAccess;

namespace PCExpert.Core.Domain.Tests.Repositories
{
	[TestFixture]
	public class ComponentInterfaceRepositoryTests
		: RepositoryTests<IComponentInterfaceRepository, ComponentInterface>
	{
		protected override IComponentInterfaceRepository CreateRepositoryWithWorkplace(PersistenceWorkplace workplace)
		{
			return new ComponentInterfaceRepository(workplace);
		}

		protected override void Save(IComponentInterfaceRepository repository, ComponentInterface entity)
		{
			repository.Save(entity);
		}

		[Test]
		public void Query_WithSpecification_ShouldReturnFilteredQueryFromWorkplace()
		{
			//Arrange
			var list = new List<ComponentInterface>
			{
				new Mock<ComponentInterface>().WithId(Guid.NewGuid()).Object,
				new Mock<ComponentInterface>().Object,
				new Mock<ComponentInterface>().Object
			};
			MockWorkplace.Setup(x => x.Query<ComponentInterface>())
				.Returns(list.AsQueryable());
			Repository = CreateRepositoryWithWorkplace(MockWorkplace.Object);
			Expression<Func<ComponentInterface, bool>> specExpression = x => x.Id != Guid.Empty;

			//Act
			var result = Repository.Query(
				new TestPersistenceAwareSpec<ComponentInterface>(specExpression)).ToList();

			//Assert
			Assert.That(result.Count == list.Where(specExpression.Compile()).Count());
			foreach (var componentInterface in result)
				Assert.That(list.Where(specExpression.Compile()).Contains(componentInterface));
		}
	}
}