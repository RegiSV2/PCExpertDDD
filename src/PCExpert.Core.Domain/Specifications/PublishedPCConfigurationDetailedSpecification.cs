﻿using System.Linq;
using PCExpert.Core.Domain.Repositories;
using PCExpert.Core.DomainFramework.Specifications;

namespace PCExpert.Core.Domain.Specifications
{
	/// <summary>
	///     Specification that checks if configuration satisfies all requirements to be published:
	///     1. Should have valid name
	///     2. Should contain all required components
	///     3. All required component types should be contained and
	///     component types that must be unique across the configuration should occur no more than once
	///     4. All root components must be pluggable to other components (possibly contained in other root components)
	///     without cyclic dependencies (I suppose that there can be no DAGs in "X can plug to Y" relations, because in
	///     practice it is true almost always)
	/// </summary>
	public class PublishedPCConfigurationDetailedSpecification : Specification<PCConfiguration>,
		IDetailedSpecification<PCConfiguration, IPublishedPCConfigurationCheckDetails>
	{
		public const int NameMaxLength = 256;
		private readonly Specification<PCConfiguration>[] _internalSpecifications;

		public PublishedPCConfigurationDetailedSpecification(IPCConfigurationRepository configurationRepository)
		{
			_internalSpecifications = new Specification<PCConfiguration>[]
			{
				new ConfigurationNameNotEmptySpecification(),
				new ConfigurationNameMaxLengthSpecification(NameMaxLength),
				new PCConfigurationNameIsUniqueSpecification(configurationRepository),
				new PCConfigurationHasCorrectSetOfRequiredComponents(),
				new AllRootComponentsOfConfigurationCanBePluggedToOtherComponents()
			};
		}

		SpecificationDetailedCheckResult<IPublishedPCConfigurationCheckDetails>
			IDetailedSpecification<PCConfiguration, IPublishedPCConfigurationCheckDetails>.IsSatisfiedBy(
			PCConfiguration entity)
		{
			if (IsSatisfiedBy(entity))
				return new SpecificationDetailedCheckResult<IPublishedPCConfigurationCheckDetails>(true, null);
			return new SpecificationDetailedCheckResult<IPublishedPCConfigurationCheckDetails>(false,
				new PublishedPCConfigurationCheckDetails());
		}

		public override bool IsSatisfiedBy(PCConfiguration configuration)
		{
			return _internalSpecifications.All(x => x.IsSatisfiedBy(configuration));
		}
	}
}