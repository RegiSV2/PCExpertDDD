using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using PCExpert.Core.Domain.Mechanisms;
using PCExpert.Core.DomainFramework;
using PCExpert.Core.DomainFramework.Specifications;
using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.Domain.Specifications
{
	/// <summary>
	///     All root components must be pluggable to other components (possibly contained in other root components)
	///     without cyclic dependencies (I suppose that there can be no cyclic graphs in "X plugs to Y" relations, because in
	///     practice it is true almost always)
	/// </summary>
	internal sealed class AllRootComponentsCanBePluggedSpecification : Specification<PCConfiguration>,
		IDetailedSpecification<PCConfiguration, ComponentPlugContraversions>
	{
		private readonly IEqualityComparer<ComponentInterface> _interfaceComparer = new EntityEqualityComparer<ComponentInterface>();
 
		public override bool IsSatisfiedBy(PCConfiguration entity)
		{
			return FindPlugContraversions(entity).CanPlugWithoutCycles;
		}

		SpecificationDetailedCheckResult<ComponentPlugContraversions>
			IDetailedSpecification<PCConfiguration, ComponentPlugContraversions>.IsSatisfiedBy(
			PCConfiguration entity)
		{
			var contraversionsInfo = FindPlugContraversions(entity);
			return CreateSpecificationResult(contraversionsInfo);
		}

		private ComponentPlugContraversions FindPlugContraversions(PCConfiguration configuration)
		{
			var requiredInterfaces = AggregateRequiredInterfaces(configuration);
			var availableInterfaces = AggregateAvailableInterfaces(configuration);
			var notFoundInterfaces = ComputeInterfacesDeficit(requiredInterfaces, availableInterfaces);

			var canConnectNodes = notFoundInterfaces.IsEmpty() && CanConnectNodesToDag(configuration);

			return new ComponentPlugContraversions(canConnectNodes, notFoundInterfaces);
		}

		private static SpecificationDetailedCheckResult<ComponentPlugContraversions> CreateSpecificationResult(
			ComponentPlugContraversions contraversionsInfo)
		{
			return new SpecificationDetailedCheckResult<ComponentPlugContraversions>(
				contraversionsInfo.CanPlugWithoutCycles, contraversionsInfo);
		}

		private ILookup<ComponentInterface, PCComponent> AggregateRequiredInterfaces(PCConfiguration configuration)
		{
			return configuration.Components
				.SelectMany(x => x.PlugSlots.Select(y => new { Component = x, Interface = y }))
					.ToLookup(x => x.Interface, x => x.Component, _interfaceComparer);
		}

		private Dictionary<ComponentInterface, int> AggregateAvailableInterfaces(PCConfiguration configuration)
		{
			return configuration.Components
				.SelectMany(x => x.EnumerateContainedSlots())
				.GroupBy(x => x, _interfaceComparer)
				.ToDictionary(x => x.Key, x => x.Count());
		}

		private List<InterfaceDeficitInfo> ComputeInterfacesDeficit(ILookup<ComponentInterface, PCComponent> requiredInterfaces, Dictionary<ComponentInterface, int> availableInterfaces)
		{
			return
				requiredInterfaces
					.Select(pair => new InterfaceDeficitInfo(pair.Key,
						pair.Count() - GetAvailableInterfacesCount(availableInterfaces, pair.Key), pair))
					.Where(info => info.Deficit > 0)
					.ToList();
		}

		private int GetAvailableInterfacesCount(IReadOnlyDictionary<ComponentInterface, int> interfaces, ComponentInterface arg)
		{
			return interfaces.ContainsKey(arg) ? interfaces[arg] : 0;
		}

		private static bool CanConnectNodesToDag(PCConfiguration configuration)
		{
			var dagComponentNodeManager = new PCComponentDagBuilderNodeManager(configuration.Components);
			var dagBuilder = new DagBuilderMechanism<PCComponent>(dagComponentNodeManager,
				GetNotRootComponents(configuration));
			return dagBuilder.CanConnectNodesToDag();
		}

		private static LinkedList<PCComponent> GetNotRootComponents(PCConfiguration entity)
		{
			return new LinkedList<PCComponent>(entity.Components.Where(x => x.PlugSlots.Any()));
		}
	}
}