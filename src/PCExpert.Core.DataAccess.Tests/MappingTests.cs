using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using PCExpert.Core.DataAccess.Tests.Utils;
using PCExpert.Core.Domain;
using PCExpert.Core.DomainFramework;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.DataAccess.Tests
{
	[TestFixture, Category("IntegrationTests")]
	public class MappingTests
	{
		private Random _random = new Random();

		[Test]
		public void DbOperationsTest()
		{
			using (var dbContext = TestContextCreator.Create())
			{
				var workplace = new EfWorkplace(dbContext);
				var savedModel = InsertModel(workplace);
				dbContext.SaveChanges();

				var loadedModel = LoadModel(workplace);

				AssertModelsAreEqual(savedModel, loadedModel);
			}
		}

		#region Loading

		private PCExpertModel LoadModel(EfWorkplace workplace)
		{
			return new PCExpertModel
			{
				Interfaces = workplace.Query<ComponentInterface>().ToList(),
				Components = workplace.Query<PCComponent>().ToList(),
				Configurations = workplace.Query<PCConfiguration>().ToList(),
				Characteristics = workplace.Query<ComponentCharacteristic>().ToList()
			};
		}

		#endregion

		private class PCExpertModel
		{
			public List<PCComponent> Components { get; set; }
			public List<ComponentInterface> Interfaces { get; set; }
			public List<PCConfiguration> Configurations { get; set; }
			public List<ComponentCharacteristic> Characteristics { get; set; }
		}

		#region Model creation

		private PCExpertModel InsertModel(EfWorkplace workplace)
		{
			var slotsToInsert = CreateSlots();
			var characteristics = CreateCharacteristics();
			var componentsToInsert = CreateComponents(slotsToInsert, characteristics);
			var configurations = CreateConfigurations(componentsToInsert);

			workplace.Insert<ComponentCharacteristic>(characteristics);
			workplace.Insert<ComponentInterface>(slotsToInsert);
			workplace.Insert<PCComponent>(componentsToInsert);
			workplace.Insert<PCConfiguration>(configurations);

			return new PCExpertModel
			{
				Components = componentsToInsert,
				Interfaces = slotsToInsert,
				Configurations = configurations,
				Characteristics = characteristics
			};
		}

		private List<ComponentCharacteristic> CreateCharacteristics()
		{
			var characteristics = new List<ComponentCharacteristic>();

			for (var i = 0; i < 9; i ++)
			{
				switch (i%2)
				{
					case 0:
						characteristics.Add(new BoolCharacteristic(
							NamesGenerator.CharacteristicName(i),
							ComponentType.PowerSupply)
							.WithPattern("bool pattern"));
						break;
					case 1:
						characteristics.Add(new IntCharacteristic(
							NamesGenerator.CharacteristicName(i),
							ComponentType.PowerSupply)
							.WithPattern("int pattern"));
						break;
				}
			}

			return characteristics;
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

		private List<PCComponent> CreateComponents(List<ComponentInterface> slotsToInsert, List<ComponentCharacteristic> characteristics)
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
				AddCharacteristicsToComponent(characteristics, newComponent);
			}
			CreateRandomLinks(componentsToInsert);
			return componentsToInsert;
		}

		private void AddCharacteristicsToComponent(List<ComponentCharacteristic> characteristics, PCComponent newComponent)
		{
			for (var j = 0; j < 3; j++)
			{
				var characteristic = characteristics.RandomElementExcept(newComponent.Characteristics.Keys.ToList());
				if (characteristic is BoolCharacteristic)
					newComponent.WithCharacteristicValue((characteristic as BoolCharacteristic).CreateValue(RandomBool()));
				else if (characteristic is IntCharacteristic)
					newComponent.WithCharacteristicValue((characteristic as IntCharacteristic).CreateValue(_random.Next()));
			}
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

		private bool RandomBool()
		{
			return _random.Next(2) != 0;
		}

		#endregion

		#region Assertions

		private void AssertModelsAreEqual(PCExpertModel savedModel, PCExpertModel loadedModel)
		{
			CompareCharacteristics(savedModel.Characteristics, loadedModel.Characteristics);
			CompareInterfaces(savedModel.Interfaces, loadedModel.Interfaces);
			CompareComponents(savedModel.Components, loadedModel.Components);
			CompareConfigurations(savedModel.Configurations, loadedModel.Configurations);
		}

		private void CompareCharacteristics(List<ComponentCharacteristic> savedChs, List<ComponentCharacteristic> loadedChs)
		{
			foreach (var savedCh in savedChs)
			{
				var loadedCh = loadedChs.FirstOrDefault(x => x.SameIdentityAs(savedCh));

				AssertCharacteristicsEqual(savedCh, loadedCh);
			}
		}

		private void AssertCharacteristicsEqual(ComponentCharacteristic savedCh, ComponentCharacteristic loadedCh)
		{
			Assert.That(loadedCh, Is.Not.Null);
			Assert.That(savedCh.ComponentType, Is.EqualTo(loadedCh.ComponentType));
			Assert.That(savedCh.FormattingPattern, Is.EqualTo(loadedCh.FormattingPattern));
			Assert.That(savedCh.Name, Is.EqualTo(loadedCh.Name));
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
			AssertCollectionsEqual(savedComp.ContainedSlots, loadedComp.ContainedSlots);
			AssertCollectionsEqual(savedComp.PlugSlots, loadedComp.PlugSlots);
			AssertCollectionsEqual(savedComp.ContainedComponents, loadedComp.ContainedComponents);
			AssertCollectionsEqual(savedComp.CharacteristicValues, loadedComp.CharacteristicValues,
				CompareCharacteristicValues);
		}

		private static bool CompareCharacteristicValues(CharacteristicValue valueA, CharacteristicValue valueB)
		{
			if (!valueA.Characteristic.SameIdentityAs(valueB.Characteristic))
				return false;
			if (valueA is BoolCharacteristicValue && valueB is BoolCharacteristicValue)
				return ((BoolCharacteristicValue)valueA).Value == ((BoolCharacteristicValue) valueB).Value;
			if (valueA is IntCharacteristicValue && valueB is IntCharacteristicValue)
				return ((IntCharacteristicValue) valueA).Value == ((IntCharacteristicValue) valueB).Value;
			return false;
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
			AssertCollectionsEqual(savedEntities, loadedEntities, (x, y) => x.SameIdentityAs(y));
		}

		private static void AssertCollectionsEqual<T>(IReadOnlyCollection<T> savedItems,
			IReadOnlyCollection<T> loadedItems, Func<T, T, bool> compare)
		{
			Assert.That(savedItems.Count == loadedItems.Count);
			foreach (var loadedEntity in loadedItems)
				Assert.That(savedItems.Count(x => compare(x, loadedEntity)) == 1);
		}

		#endregion
	}
}