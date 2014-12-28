using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using PCExpert.Core.Domain.Repositories;
using PCExpert.Core.DomainFramework.DataAccess;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.Domain.Tests.Repositories
{
	[TestFixture]
	public class ComponentInterfaceRepositoryTests
	{
		private IComponentInterfaceRepository _repository;

		[Test]
		public void Save_AnyValue_ShouldDelegateToPersistenceWorkplace()
		{
			//Arrange
			var workplace = new TestPersistenceWorkplace();
			_repository = new ComponentInterfaceRepository(workplace);
			var componentToInsert = new Mock<ComponentInterface>();

			//Act
			_repository.Save(componentToInsert.Object);

			//Assert
			Assert.That(workplace.IsUpdateCalled);
			;
		}

		[Test]
		public void Query_WithSpecification_ShouldReturnFilteredQueryFromWorkplace()
		{
			//Arrange
			var workplace = new Mock<PersistenceWorkplace>();
			var list = new List<ComponentInterface>
			{
				new Mock<ComponentInterface>().WithId(Guid.NewGuid()).Object,
				new Mock<ComponentInterface>().Object,
				new Mock<ComponentInterface>().Object
			};
			workplace.Setup(x => x.Query<ComponentInterface>())
				.Returns(list.AsQueryable());
			_repository = new ComponentInterfaceRepository(workplace.Object);
			Expression<Func<ComponentInterface, bool>> specExpression = x => x.Id != Guid.Empty;

			//Act
			var result = _repository.Query(
				new TestPersistenceAwareSpec<ComponentInterface>(specExpression)).ToList();

			//Assert
			Assert.That(result.Count == list.Where(specExpression.Compile()).Count());
			foreach (var componentInterface in result)
				Assert.That(list.Where(specExpression.Compile()).Contains(componentInterface));
		}
	}
}