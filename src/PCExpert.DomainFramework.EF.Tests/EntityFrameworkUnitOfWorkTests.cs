using System.Linq;
using NUnit.Framework;
using PCExpert.Core.Domain;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.DomainFramework.EF.Tests
{
	[TestFixture, Category("IntegrationTests")]
	public class EntityFrameworkUnitOfWorkTests
	{
		[Test]
		public void Execute_DoesNotThrowException_ShouldCommitAllChanges()
		{
			//Arrange
			var provider = TestContextCreator.Create();
			var unitOfWork = new EntityFrameworkUnitOfWork(provider);

			//Act
			var addedComponent = DomainObjectsCreator.CreateComponent(1, ComponentType.HardDiskDrive);
			unitOfWork.Execute(() => { provider.PCExpertContext.PCComponents.Add(addedComponent); }).Wait();
			var loadedComponents = provider.PCExpertContext.PCComponents.ToList();

			//Assert
			Assert.That(loadedComponents.Count == 1);
			Assert.That(loadedComponents.First().SameIdentityAs(addedComponent));
		}
	}
}