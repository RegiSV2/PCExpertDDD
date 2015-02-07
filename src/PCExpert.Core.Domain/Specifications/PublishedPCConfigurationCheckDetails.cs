using System.Collections.Generic;
using PCExpert.DomainFramework.Utils;

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
			NotFoundInterfaces = new List<InterfaceDeficitInfo>();
		}

		public bool NameNotEmptyFailure { get; set; }
		public bool NameMaxLengthFailure { get; set; }
		public List<ComponentType> RequiredButNotAddedTypes { get; set; }
		public List<ComponentType> TypesViolatedUniqueConstraint { get; set; }
		public bool ComponentsCycleFailure { get; set; }
		public List<InterfaceDeficitInfo> NotFoundInterfaces { get; set; }

		public bool AllRequirementsSatisfied()
		{
			return !NameMaxLengthFailure
			       && !NameNotEmptyFailure
			       && !ComponentsCycleFailure
			       && NotFoundInterfaces.IsEmpty()
			       && RequiredButNotAddedTypes.IsEmpty()
			       && TypesViolatedUniqueConstraint.IsEmpty();
		}
	}
}