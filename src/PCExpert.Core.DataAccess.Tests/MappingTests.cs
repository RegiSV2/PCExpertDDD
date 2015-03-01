using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using PCExpert.Core.DataAccess.Tests.Utils;
using PCExpert.Core.Domain;
using PCExpert.Core.Tests.Utils;
using PCExpert.DomainFramework.EF;

namespace PCExpert.Core.DataAccess.Tests
{
	[TestFixture, Category("IntegrationTests")]
	public class MappingTests
	{
		private readonly CharacteristicHandler[] _characteristicHandlers =
		{
			new BoolCharacteristicHandler(),
			new NumericCharacteristicHandler(),
			new StringCharacteristicHandler()
		};

		private readonly TestDataGenerator _testDataGenerator = new TestDataGenerator();

		[Test]
		public void DbOperationsTest()
		{
			var dbContextProvider = TestContextCreator.Create();
			using (var dbContext = dbContextProvider.DbContext)
			{
				var workplace = new EfWorkplace(dbContextProvider);
				var savedModel = _testDataGenerator.CreateRandomData(workplace);
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

		private void AssertComponentsEqual(PCComponent savedComp, PCComponent loadedComp)
		{
			Assert.That(loadedComp, Is.Not.Null);
			Assert.That(loadedComp.AveragePrice, Is.EqualTo(savedComp.AveragePrice));
			Assert.That(loadedComp.Name, Is.EqualTo(savedComp.Name));
			Assert.That(UtilsAssert.CollectionsEqual(savedComp.ContainedSlots, loadedComp.ContainedSlots));
			Assert.That(UtilsAssert.CollectionsEqual(savedComp.PlugSlots, loadedComp.PlugSlots));
			Assert.That(UtilsAssert.CollectionsEqual(savedComp.ContainedComponents, loadedComp.ContainedComponents));
			Assert.That(UtilsAssert.CollectionsEqual(savedComp.CharacteristicValues, loadedComp.CharacteristicValues,
				CompareCharacteristicValues));
		}

		private bool CompareCharacteristicValues(CharacteristicValue valueA, CharacteristicValue valueB)
		{
			if (!valueA.Characteristic.SameIdentityAs(valueB.Characteristic))
				return false;
			return _characteristicHandlers.Any(x => x.CompareValues(valueA, valueB));
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
			Assert.That(savedConfig.Status, Is.EqualTo(loadedConfig.Status));
			Assert.That(savedConfig.Name, Is.EqualTo(loadedConfig.Name));
			Assert.That(savedConfig.PublicName, Is.EqualTo(loadedConfig.PublicName));
		}

		#endregion
	}
}