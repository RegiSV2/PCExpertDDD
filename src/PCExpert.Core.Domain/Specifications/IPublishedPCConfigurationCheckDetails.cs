using System.Collections.Generic;

namespace PCExpert.Core.Domain.Specifications
{
	public interface IPublishedPCConfigurationCheckDetails
	{
		bool NameNotEmptyFailure { get; }
		bool NameMaxLengthFailure { get; }
		List<ComponentType> RequiredButNotAddedTypes { get; }
		List<ComponentType> TypesViolatedUniqueConstraint { get; }
		bool ComponentsCycleFailure { get; }
		List<InterfaceDeficitInfo> NotFoundInterfaces { get; }
	}
}