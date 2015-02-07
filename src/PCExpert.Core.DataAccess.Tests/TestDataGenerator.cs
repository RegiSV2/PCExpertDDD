using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using PCExpert.Core.DataAccess.Tests.Utils;
using PCExpert.Core.Domain;
using PCExpert.Core.Tests.Utils;
using PCExpert.DomainFramework.DataAccess;

namespace PCExpert.Core.DataAccess.Tests
{
	/// <summary>
	/// Populates persistence workplace with test data
	/// </summary>
	public sealed class TestDataGenerator
	{
		private readonly CharacteristicHandler[] _characteristicHandlers =
		{
			new BoolCharacteristicHandler(),
			new NumericCharacteristicHandler(),
			new StringCharacteristicHandler()
		};

		private readonly Random _random = new Random();

		public PCExpertModel CreateRandomData(PersistenceWorkplace workplace)
		{
			Contract.Requires(workplace != null);

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
				characteristics.Add(
					_characteristicHandlers[i%3].CreateRandomCharacteristic(i));

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

		private List<PCComponent> CreateComponents(List<ComponentInterface> slotsToInsert,
			List<ComponentCharacteristic> characteristics)
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
				var value = _characteristicHandlers.First(x => x.CharacteristicType.IsInstanceOfType(characteristic))
					.CreateRandomValue(characteristic);
				newComponent.WithCharacteristicValue(value);
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
				if(_random.NextDouble() > 0.5)
					configuration.MoveToStatus(PCConfigurationStatus.Published);
			}

			return configurations;
		}
	}
}