using System.Collections.Generic;

namespace PCExpert.Core.Domain.Specifications
{
	/// <summary>
	///     Result of PublishedPCConfigurationDetailedSpecification satisfaction check
	/// </summary>
	internal class PublishedPCConfigurationCheckDetails : IPublishedPCConfigurationCheckDetails
	{
		public PublishedPCConfigurationCheckDetails()
		{
			RequiredButNotAddedTypes = new List<ComponentType>();
			TypesViolatedUniqueConstraint = new List<ComponentType>();
			ComponentPlugCycle = new List<PCComponent>();
			NotFoundInterfaces = new List<InterfaceDeficitInfo>();
		}

		public bool NameNotEmptyFailure { get; set; }

		public bool NameMaxLengthFailure { get; set; }

		public bool NameUniqueFailure { get; set; }

		public List<ComponentType> RequiredButNotAddedTypes { get; set; }

		public List<ComponentType> TypesViolatedUniqueConstraint { get; set; }

		public List<PCComponent> ComponentPlugCycle { get; set; }

		public List<InterfaceDeficitInfo> NotFoundInterfaces { get; set; }
	}
}