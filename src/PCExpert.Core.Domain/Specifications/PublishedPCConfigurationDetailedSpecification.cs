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
		public const int NameMaxLength = 250;

		private readonly IDetailedSpecification<PCConfiguration, ComponentPlugContraversions>
			_componentsCanBePluggedSpecification;

		private readonly IDetailedSpecification<PCConfiguration, RequiredComponentTypesSetContraversions>
			_componentTypesSetSpecification;

		private readonly Specification<PCConfiguration>[] _internalSpecifications;
		private readonly ConfigurationNameMaxLengthSpecification _maxLengthSpecification;
		private readonly ConfigurationNameNotEmptySpecification _nameNotEmptySpecification;
		private readonly PCConfigurationNameIsUniqueSpecification _nameUniqueSpecification;

		public PublishedPCConfigurationDetailedSpecification(IPCConfigurationRepository configurationRepository)
		{
			_nameNotEmptySpecification = new ConfigurationNameNotEmptySpecification();
			_maxLengthSpecification = new ConfigurationNameMaxLengthSpecification(NameMaxLength);
			_nameUniqueSpecification = new PCConfigurationNameIsUniqueSpecification(configurationRepository);
			var componentTypesSetSpecification = new PCConfigurationHasCorrectSetOfRequiredComponents();
			_componentTypesSetSpecification = componentTypesSetSpecification;
			var componentsCanBePluggedSpecification = new AllRootComponentsCanBePluggedSpecification();
			_componentsCanBePluggedSpecification = componentsCanBePluggedSpecification;

			_internalSpecifications = new Specification<PCConfiguration>[]
			{
				_nameNotEmptySpecification,
				_maxLengthSpecification,
				_nameUniqueSpecification,
				componentTypesSetSpecification,
				componentsCanBePluggedSpecification
			};
		}

		SpecificationDetailedCheckResult<IPublishedPCConfigurationCheckDetails>
			IDetailedSpecification<PCConfiguration, IPublishedPCConfigurationCheckDetails>.IsSatisfiedBy(
			PCConfiguration entity)
		{
			var details = BuildCheckDetails(entity);
			return CreateResult(details);
		}

		public override bool IsSatisfiedBy(PCConfiguration configuration)
		{
			return _internalSpecifications.All(x => x.IsSatisfiedBy(configuration));
		}

		private PublishedPCConfigurationCheckDetails BuildCheckDetails(PCConfiguration entity)
		{
			var details = new PublishedPCConfigurationCheckDetails();

			if (!_nameNotEmptySpecification.IsSatisfiedBy(entity))
				details.NameNotEmptyFailure = true;
			if (!_maxLengthSpecification.IsSatisfiedBy(entity))
				details.NameMaxLengthFailure = true;
			if (!_nameUniqueSpecification.IsSatisfiedBy(entity))
				details.NameUniqueFailure = true;

			CheckComponentTypesSpecification(entity, details);
			CheckComponentsPlugSpecification(entity, details);

			return details;
		}

		private void CheckComponentsPlugSpecification(PCConfiguration entity, PublishedPCConfigurationCheckDetails details)
		{
			var componentsPlugDetails = _componentsCanBePluggedSpecification.IsSatisfiedBy(entity);
			if (componentsPlugDetails.IsSatisfied) return;
			details.ComponentsCycleFailure = !componentsPlugDetails.FailureDetails.CanPlugWithoutCycles;
			details.NotFoundInterfaces = componentsPlugDetails.FailureDetails.NotFoundInterfaces;
		}

		private void CheckComponentTypesSpecification(PCConfiguration entity, PublishedPCConfigurationCheckDetails details)
		{
			var typesSetDetails = _componentTypesSetSpecification.IsSatisfiedBy(entity);
			if (typesSetDetails.IsSatisfied) return;
			details.RequiredButNotAddedTypes = typesSetDetails.FailureDetails.RequiredButNotAddedTypes;
			details.TypesViolatedUniqueConstraint = typesSetDetails.FailureDetails.TypesViolatedUniqueConstraint;
		}

		private static SpecificationDetailedCheckResult<IPublishedPCConfigurationCheckDetails> CreateResult(
			PublishedPCConfigurationCheckDetails details)
		{
			var isSatisfied = details.AllRequirementsSatisfied();
			return new SpecificationDetailedCheckResult<IPublishedPCConfigurationCheckDetails>(isSatisfied, details);
		}
	}
}