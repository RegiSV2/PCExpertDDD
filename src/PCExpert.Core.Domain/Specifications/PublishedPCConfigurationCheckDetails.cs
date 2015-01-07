using System.Collections.Generic;

namespace PCExpert.Core.Domain.Specifications
{
	/// <summary>
	///     Result of PublishedPCConfigurationDetailedSpecification satisfaction check
	/// </summary>
	public class PublishedPCConfigurationCheckDetails
	{
		public PublishedPCConfigurationCheckDetails(bool nameNotEmptyFailure, bool nameMaxLengthExceedsMax,
			bool nameUniqueFailure,
			List<ComponentType> requiredNotAddedTypes, List<ComponentType> typesViolatedUniqueConstraint,
			List<InterfaceDeficitInfo> notFoundInterfaces,
			List<PCComponent> componentPlugCycle)
		{
			NameNotEmptyFailure = nameNotEmptyFailure;
			NameMaxLengthFailure = nameMaxLengthExceedsMax;
			NameUniqueFailure = nameUniqueFailure;
			RequiredButNotAddedTypes = requiredNotAddedTypes;
			TypesViolatedUniqueConstraint = typesViolatedUniqueConstraint;
			ComponentPlugCycle = componentPlugCycle;
			NotFoundInterfaces = notFoundInterfaces;
		}

		public bool NameNotEmptyFailure { get; private set; }
		public bool NameMaxLengthFailure { get; private set; }
		public bool NameUniqueFailure { get; private set; }
		public List<ComponentType> RequiredButNotAddedTypes { get; private set; }
		public List<ComponentType> TypesViolatedUniqueConstraint { get; private set; }
		public List<PCComponent> ComponentPlugCycle { get; private set; }
		public List<InterfaceDeficitInfo> NotFoundInterfaces { get; private set; }
	}

	public class InterfaceDeficitInfo
	{
		public InterfaceDeficitInfo(ComponentInterface problemInterface, int deficit,
			IEnumerable<PCComponent> requiredByComponents)
		{
			ProblemInterface = problemInterface;
			Deficit = deficit;
			RequiredByComponents = new List<PCComponent>(requiredByComponents);
		}

		public ComponentInterface ProblemInterface { get; private set; }
		public int Deficit { get; private set; }
		public List<PCComponent> RequiredByComponents { get; private set; }
	}
}