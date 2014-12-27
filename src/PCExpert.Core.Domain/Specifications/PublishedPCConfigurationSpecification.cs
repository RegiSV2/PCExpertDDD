using System.Collections.Generic;
using System.Linq;
using PCExpert.Core.DomainFramework.Specifications;

namespace PCExpert.Core.Domain.Specifications
{
	public class PublishedPCConfigurationSpecification : Specification<PCConfiguration>
	{
		public const int NameMaxLength = 256;
		private readonly Specification<PCConfiguration> _nameSpecification;

		private readonly ComponentType[] _requiredComponentTypes =
		{
			ComponentType.Motherboard,
			ComponentType.PowerSupply,
			ComponentType.CentralProcessingUnit,
			ComponentType.HardDiskDrive,
			ComponentType.RandomAccessMemory,
			ComponentType.VideoCard
		};

		private readonly ComponentType[] _uniqueComponentTypes =
		{
			ComponentType.Motherboard,
			ComponentType.PowerSupply
		};

		public PublishedPCConfigurationSpecification()
		{
			_nameSpecification = new ConfigurationNameNotEmptySpecification();
		}

		public override bool IsSatisfiedBy(PCConfiguration configuration)
		{
			if (!_nameSpecification.IsSatisfiedBy(configuration))
				return false;

			var componentTypesCountMap = CountComponentTypes(configuration);

			return _requiredComponentTypes.All(requiredType => componentTypesCountMap[requiredType] > 0)
			       && _uniqueComponentTypes.All(uniqueType => componentTypesCountMap[uniqueType] <= 1);
		}

		private Dictionary<ComponentType, int> CountComponentTypes(PCConfiguration configuration)
		{
			var componentTypesCountMap = _requiredComponentTypes
				.Concat(_uniqueComponentTypes)
				.Distinct()
				.ToDictionary(x => x, x => 0);
			foreach (var type in GetContainedComponentTypes(configuration)
				.Where(type => componentTypesCountMap.ContainsKey(type)))
				componentTypesCountMap[type] += 1;
			return componentTypesCountMap;
		}

		private IEnumerable<ComponentType> GetContainedComponentTypes(PCConfiguration configuration)
		{
			return configuration.Components.SelectMany(GetContainedComponentTypes);
		}

		private IEnumerable<ComponentType> GetContainedComponentTypes(PCComponent component)
		{
			yield return component.Type;
			foreach (var type in component.ContainedComponents.SelectMany(GetContainedComponentTypes))
				yield return type;
		}
	}
}