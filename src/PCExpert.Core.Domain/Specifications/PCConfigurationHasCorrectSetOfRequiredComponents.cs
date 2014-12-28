using System.Collections.Generic;
using System.Linq;
using PCExpert.Core.DomainFramework.Specifications;

namespace PCExpert.Core.Domain.Specifications
{
	/// <summary>
	/// All required component types should be contained and
	///     component types that must be unique across the configuration should occur no more than once
	/// </summary>
	internal sealed class PCConfigurationHasCorrectSetOfRequiredComponents : Specification<PCConfiguration>
	{
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

		public override bool IsSatisfiedBy(PCConfiguration entity)
		{
			var componentTypesCountMap = CountComponentTypes(entity);

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