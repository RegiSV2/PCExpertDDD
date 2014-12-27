using System.Collections.Generic;
using System.Linq;
using PCExpert.Core.DomainFramework;
using PCExpert.Core.DomainFramework.Specifications;

namespace PCExpert.Core.Domain.Specifications
{
	/// <summary>
	///     Specification that checks if configuration satisfies all requirements to be published:
	///     1. Should have valid name
	///     2. Should contain all required components
	///     3. All component types that must be unique across the configuration should occur no more than once
	///     4. All root components must be pluggable to other components (possibly contained in other root components)
	///     without cyclic dependencies (I suppose that there can be no DAGs in "X can plug to Y" relations, because in
	///     practice it is true almost always)
	/// </summary>
	public class PublishedPCConfigurationSpecification : Specification<PCConfiguration>
	{
		public const int NameMaxLength = 256;
		private readonly Specification<PCConfiguration> _combinedSpecification;

		public PublishedPCConfigurationSpecification()
		{
			_combinedSpecification = new ConfigurationNameNotEmptySpecification()
			                         & new ConfigurationNameMaxLengthSpecification(NameMaxLength)
			                         & new PCConfigurationHasCorrectSetOfRequiredComponents()
			                         & new AllRootComponentsOfConfigurationCanBePluggedToOtherComponents();
		}

		public override bool IsSatisfiedBy(PCConfiguration configuration)
		{
			return _combinedSpecification.IsSatisfiedBy(configuration);
		}

		#region Partial specifications

		private class PCConfigurationHasCorrectSetOfRequiredComponents : Specification<PCConfiguration>
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

		private class AllRootComponentsOfConfigurationCanBePluggedToOtherComponents : Specification<PCConfiguration>
		{
			public override bool IsSatisfiedBy(PCConfiguration entity)
			{
				var availableInterfaces = GetRootComponentsInterfaces(entity);
				var notVisitedComponents = GetNotRootComponents(entity);

				return CanPlugComponentsToInterfaces(notVisitedComponents, availableInterfaces);
			}

			private static Dictionary<ComponentInterface, int> GetRootComponentsInterfaces(PCConfiguration entity)
			{
				var availableInterfaces = new Dictionary<ComponentInterface, int>(
					new EntityEqualityComparer<ComponentInterface>());

				var availableInterfacesEnum = entity.Components
					.Where(x => x.PlugSlot == ComponentInterface.NullObject)
					.SelectMany(GetComponentInterfaces);

				PutInterfacesInDictionary(availableInterfaces, availableInterfacesEnum);

				return availableInterfaces;
			}

			private static LinkedList<PCComponent> GetNotRootComponents(PCConfiguration entity)
			{
				var notVisitedComponents = new LinkedList<PCComponent>(entity.Components
					.Where(x => x.PlugSlot != ComponentInterface.NullObject));
				return notVisitedComponents;
			}

			private bool CanPlugComponentsToInterfaces(LinkedList<PCComponent> notVisitedComponents,
				Dictionary<ComponentInterface, int> availableInterfaces)
			{
				while (notVisitedComponents.Any())
				{
					var pluggableComponent = PopPluggableComponent(notVisitedComponents, availableInterfaces);
					if (pluggableComponent == null)
						return false;

					PopInterfaceFromDictionary(availableInterfaces, pluggableComponent.PlugSlot);
					PutInterfacesInDictionary(availableInterfaces, GetComponentInterfaces(pluggableComponent));
				}
				return true;
			}

			private PCComponent PopPluggableComponent(LinkedList<PCComponent> notVisitedComponents,
				Dictionary<ComponentInterface, int> availableInterfaces)
			{
				var candidateNode = notVisitedComponents.First;

				while (candidateNode != null)
				{
					if (availableInterfaces.ContainsKey(candidateNode.Value.PlugSlot))
					{
						notVisitedComponents.Remove(candidateNode);
						return candidateNode.Value;
					}
					candidateNode = candidateNode.Next;
				}
				return null;
			}

			private static IEnumerable<ComponentInterface> GetComponentInterfaces(PCComponent component)
			{
				foreach (var componentInterface in component.ContainedSlots)
					yield return componentInterface;
				foreach (var componentInterface in component.ContainedComponents.SelectMany(GetComponentInterfaces))
					yield return componentInterface;
			}

			private static void PutInterfacesInDictionary(Dictionary<ComponentInterface, int> availableInterfaces,
				IEnumerable<ComponentInterface> availableInterfacesEnum)
			{
				foreach (var availableInterface in availableInterfacesEnum)
				{
					if (!availableInterfaces.ContainsKey(availableInterface))
						availableInterfaces.Add(availableInterface, 0);
					availableInterfaces[availableInterface] += 1;
				}
			}

			private static void PopInterfaceFromDictionary(Dictionary<ComponentInterface, int> availableInterfaces,
				ComponentInterface compInt)
			{
				availableInterfaces[compInt] -= 1;
				if (availableInterfaces[compInt] == 0)
					availableInterfaces.Remove(compInt);
			}
		}

		#endregion
	}
}