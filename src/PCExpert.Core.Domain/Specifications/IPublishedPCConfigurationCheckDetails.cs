using System.Collections.Generic;

namespace PCExpert.Core.Domain.Specifications
{
	public interface IPublishedPCConfigurationCheckDetails
	{
		bool NameNotEmptyFailure { get; }

		bool NameMaxLengthFailure { get; }

		bool NameUniqueFailure { get; }

		List<ComponentType> RequiredButNotAddedTypes { get; }

		List<ComponentType> TypesViolatedUniqueConstraint { get; }

		List<PCComponent> ComponentPlugCycle { get; }

		List<InterfaceDeficitInfo> NotFoundInterfaces { get; }
	}
}