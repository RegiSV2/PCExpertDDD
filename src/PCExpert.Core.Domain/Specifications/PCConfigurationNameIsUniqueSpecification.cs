using System.Linq;
using PCExpert.Core.Domain.Repositories;
using PCExpert.DomainFramework.Specifications;

namespace PCExpert.Core.Domain.Specifications
{
	/// <summary>
	///     Configuration name should be unique among currently published specifications
	/// </summary>
	internal sealed class PCConfigurationNameIsUniqueSpecification : Specification<PCConfiguration>
	{
		private readonly IPCConfigurationRepository _configurationRepository;

		public PCConfigurationNameIsUniqueSpecification(IPCConfigurationRepository configurationRepository)
		{
			_configurationRepository = configurationRepository;
		}

		public override bool IsSatisfiedBy(PCConfiguration entity)
		{
			return !_configurationRepository.FindPublishedConfigurations(entity.Name).Any();
		}
	}
}