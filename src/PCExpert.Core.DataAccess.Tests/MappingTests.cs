using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using PCExpert.Core.Domain;
using PCExpert.Core.DomainFramework;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.DataAccess.Tests
{
	[TestFixture, Category("IntegrationTests")]
	public class MappingTests
	{
		[Test]
		public void DbOperationsTest()
		{
			using (var dbContext = new PCExpertContext("testConString",
				new DropCreateDatabaseAlways<PCExpertContext>()))
			{
				var savedModel = InsertModel(dbContext);
				dbContext.SaveChanges();

				var loadedModel = LoadModel(dbContext);

				AssertModelsAreEqual(savedModel, loadedModel);
			}
		}

		#region Loading

		private PCExpertModel LoadModel(PCExpertContext dbContext)
		{
			return new PCExpertModel
			{
				Interfaces = dbContext.ComponentInterfaces.ToList(),
				Components = dbContext.PCComponents.ToList(),
				Configurations = dbContext.PCConfigurations.ToList()
			};
		}

		#endregion

		private class PCExpertModel
		{
			public List<PCComponent> Components { get; set; }
			public List<ComponentInterface> Interfaces { get; set; }
			public List<PCConfiguration> Configurations { get; set; }
		}

		#region Model creation

		private PCExpertModel InsertModel(PCExpertContext dbContext)
		{
			var slotsToInsert = CreateSlots();
			var componentsToInsert = CreateComponents(slotsToInsert);
			var configurations = CreateConfigurations(componentsToInsert);

			dbContext.ComponentInterfaces.AddRange(slotsToInsert);
			dbContext.PCComponents.AddRange(componentsToInsert);
			dbContext.PCConfigurations.AddRange(configurations);

			return new PCExpertModel
			{
				Components = componentsToInsert,
				Interfaces = slotsToInsert,
				Configurations = configurations
			};
		}

		private static void CreateRandomLinks(List<PCComponent> componentsToInsert)
		{
			for (var i = 0; i < 5; i++)
			{
				var parentElement = componentsToInsert.RandomElement();
				var childElement = componentsToInsert.RandomElementExcept(parentElement);
				if (!parentElement.ContainedComponents.Contains(childElement))
					parentElement.WithContainedComponent(childElement);
			}
		}

		private static List<PCComponent> CreateComponents(List<ComponentInterface> slotsToInsert)
		{
			var componentsToInsert = new List<PCComponent>();
			for (var i = 0; i < 5; i++)
			{
				var newComponent = new PCComponent(NamesGenerator.ComponentName(i), ComponentType.PowerSupply);
				newComponent
					.WithAveragePrice(100*(i + 1))
					.WithPlugSlot(slotsToInsert.RandomElement())
					.WithContainedSlot(slotsToInsert.RandomElement())
					.WithContainedSlot(slotsToInsert.RandomElementExcept(newComponent.ContainedSlots.ToList()));
				componentsToInsert.Add(newComponent);
			}
			CreateRandomLinks(componentsToInsert);
			return componentsToInsert;
		}

		private static List<ComponentInterface> CreateSlots()
		{
			var slotsToInsert = new List<ComponentInterface>();
			for (var i = 0; i < 5; i++)
				slotsToInsert.Add(new ComponentInterface(NamesGenerator.ComponentInterfaceName(i)));
			return slotsToInsert;
		}

		private List<PCConfiguration> CreateConfigurations(IList<PCComponent> componentsToInsert)
		{
			var configurations = new List<PCConfiguration>();

			for (var i = 0; i < 5; i++)
			{
				var configuration = new PCConfiguration();
				configuration.WithName(NamesGenerator.ConfigurationName(i))
					.WithComponent(componentsToInsert.RandomElementExcept(configuration.Components.ToList()))
					.WithComponent(componentsToInsert.RandomElementExcept(configuration.Components.ToList()))
					.WithComponent(componentsToInsert.RandomElementExcept(configuration.Components.ToList()));
				configurations.Add(configuration);
			}

			return configurations;
		}

		#endregion

		#region Assertions

		private void AssertModelsAreEqual(PCExpertModel savedModel, PCExpertModel loadedModel)
		{
			CompareInterfaces(savedModel.Interfaces, loadedModel.Interfaces);
			CompareComponents(savedModel.Components, loadedModel.Components);
			CompareConfigurations(savedModel.Configurations, loadedModel.Configurations);
		}

		private void CompareComponents(List<PCComponent> savedComps, List<PCComponent> loadedComps)
		{
			foreach (var savedComp in savedComps)
			{
				var loadedComp = loadedComps.FirstOrDefault(x => x.SameIdentityAs(savedComp));

				AssertComponentsEqual(savedComp, loadedComp);
			}
		}

		private static void AssertComponentsEqual(PCComponent savedComp, PCComponent loadedComp)
		{
			Assert.That(loadedComp, Is.Not.Null);
			Assert.That(loadedComp.AveragePrice, Is.EqualTo(savedComp.AveragePrice));
			Assert.That(loadedComp.Name, Is.EqualTo(savedComp.Name));
			Assert.That(loadedComp.PlugSlot.SameIdentityAs(savedComp.PlugSlot));
			AssertCollectionsEqual(savedComp.ContainedComponents, loadedComp.ContainedComponents);
		}

		private void CompareInterfaces(List<ComponentInterface> savedInts, List<ComponentInterface> loadedInts)
		{
			foreach (var savedInt in savedInts)
			{
				var loadedInt = loadedInts.FirstOrDefault(x => x.SameIdentityAs(savedInt));
				AssertInterfacesEqual(loadedInt, savedInt);
			}
		}

		private static void AssertInterfacesEqual(ComponentInterface loadedInt, ComponentInterface savedInt)
		{
			Assert.That(loadedInt, Is.Not.Null);
			Debug.Assert(loadedInt != null, "loadedInt != null");
			Assert.That(loadedInt.Name, Is.EqualTo(savedInt.Name));
		}

		private void CompareConfigurations(List<PCConfiguration> savedConfigs, List<PCConfiguration> loadedConfigs)
		{
			foreach (var savedConfig in savedConfigs)
			{
				var loadedConfig = loadedConfigs.FirstOrDefault(x => x.SameIdentityAs(savedConfig));
				AssertConfigsAreEqual(savedConfig, loadedConfig);
			}
		}

		private void AssertConfigsAreEqual(PCConfiguration savedConfig, PCConfiguration loadedConfig)
		{
			Assert.That(loadedConfig, Is.Not.Null);
			Assert.That(savedConfig.Name, Is.EqualTo(loadedConfig.Name));
		}

		private static void AssertCollectionsEqual<TEntity>(IReadOnlyCollection<TEntity> savedEntities,
			IReadOnlyCollection<TEntity> loadedEntities)
			where TEntity : Entity
		{
			Assert.That(savedEntities.Count == loadedEntities.Count);
			foreach (var loadedEntity in loadedEntities)
				Assert.That(savedEntities.Count(x => x.SameIdentityAs(loadedEntity)) == 1);
		}

		#endregion
	}
}