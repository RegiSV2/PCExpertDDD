using System.Collections.Generic;
using System.Linq;
using PCExpert.Core.Domain.Mechanisms;
using PCExpert.Core.DomainFramework.Specifications;

namespace PCExpert.Core.Domain.Specifications
{
	/// <summary>
	///     All root components must be pluggable to other components (possibly contained in other root components)
	///     without cyclic dependencies (I suppose that there can be no DAGs in "X can plug to Y" relations, because in
	///     practice it is true almost always)
	/// </summary>
	internal sealed class AllRootComponentsOfConfigurationCanBePluggedToOtherComponents : Specification<PCConfiguration>
	{
		public override bool IsSatisfiedBy(PCConfiguration entity)
		{
			var dagComponentNodeManager = new PCComponentDagBuilderNodeManager(entity.Components);
			var dagBuilder = new DagBuilderMechanism<PCComponent>(dagComponentNodeManager,
				GetNotRootComponents(entity));

			return dagBuilder.CanConnectNodesToDag();
		}

		private static LinkedList<PCComponent> GetNotRootComponents(PCConfiguration entity)
		{
			var notVisitedComponents = new LinkedList<PCComponent>(entity.Components
				.Where(x => x.PlugSlots.Any()));
			return notVisitedComponents;
		}
	}
}