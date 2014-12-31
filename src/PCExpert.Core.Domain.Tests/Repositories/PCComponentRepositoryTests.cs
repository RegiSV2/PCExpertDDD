using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PCExpert.Core.Domain.Repositories;
using PCExpert.Core.DomainFramework.DataAccess;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.Domain.Tests.Repositories
{
	[TestFixture]
	public class PCComponentRepositoryTests :
		RepositoryTests<PCComponentRepository, PCComponent>
	{
		protected override PCComponentRepository CreateRepositoryWithWorkplace(PersistenceWorkplace workplace)
		{
			return new PCComponentRepository(workplace);
		}

		protected override void Save(PCComponentRepository repository, PCComponent entity)
		{
			repository.Save(entity);
		}

		[Test]
		public override void Save_AnyValue_ShouldDelegateToPersistenceWorkplace()
		{
			base.Save_AnyValue_ShouldDelegateToPersistenceWorkplace();
		}

		[Test]
		public void Query_ValidComponentType_ShouldQueryComponentsOfSpecifiedTypes()
		{
			//Arrange
			var requestType = ComponentType.Motherboard;
			var components = new List<PCComponent>
			{
				DomainObjectsCreator.CreateComponent(0, requestType),
				DomainObjectsCreator.CreateComponent(0, ComponentType.HardDiskDrive),
				DomainObjectsCreator.CreateComponent(0, requestType),
				DomainObjectsCreator.CreateComponent(0, ComponentType.PowerSupply)
			};
			MockWorkplace.Setup(x => x.Query<PCComponent>())
				.Returns(components.AsQueryable());

			//Act
			var queriesComponents = _repository.Query(requestType).ToList();
			
			//Assert
			Assert.That(queriesComponents.Count == 2);
			Assert.That(queriesComponents.All(x => x.Type == requestType));
		}
	}
}