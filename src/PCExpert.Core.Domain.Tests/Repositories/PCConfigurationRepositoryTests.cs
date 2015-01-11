using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using PCExpert.Core.Domain.Repositories;
using PCExpert.Core.Tests.Utils;
using PCExpert.DomainFramework.DataAccess;

namespace PCExpert.Core.Domain.Tests.Repositories
{
	[TestFixture]
	public class PCConfigurationRepositoryTests
		: RepositoryTests<IPCConfigurationRepository, PCConfiguration>
	{
		protected override IPCConfigurationRepository CreateRepositoryWithWorkplace(PersistenceWorkplace workplace)
		{
			return new PCConfigurationRepository(workplace);
		}

		protected override void Save(IPCConfigurationRepository repository, PCConfiguration entity)
		{
			repository.Save(entity);
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		public void FindPublishedConfigurations_NullOrEmptyName_ShouldThrowArgumentNullException(string arg)
		{
			Assert.That(() => Repository.FindPublishedConfigurations(arg),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void FindPublishedConfigurations_NotEmptyName_ShouldReturnPublishedConfigurationsWithSpecifiedName()
		{
			//Arrange
			var requestedName = NamesGenerator.ConfigurationName();
			var configurationsList = new List<PCConfiguration>
			{
				new Mock<PCConfiguration>().Object.WithName(NamesGenerator.ConfigurationName(1)),
				new Mock<PCConfiguration>().Object.WithName(requestedName),
				new Mock<PCConfiguration>().Object.WithName(requestedName),
				new Mock<PCConfiguration>().Object.WithName(NamesGenerator.ConfigurationName(2))
			};
			configurationsList[1].MoveToStatus(PCConfigurationStatus.Published);
			configurationsList[3].MoveToStatus(PCConfigurationStatus.Published);
			MockWorkplace.Setup(x => x.Query<PCConfiguration>()).Returns(configurationsList.AsQueryable());

			//Act
			var retrievedConfigurations = Repository.FindPublishedConfigurations(requestedName).ToList();

			//Assert
			Assert.That(retrievedConfigurations.Count == 1);
			Assert.That(retrievedConfigurations.First().Status == PCConfigurationStatus.Published);
			Assert.That(retrievedConfigurations.First().Name == requestedName);
		}
	}
}