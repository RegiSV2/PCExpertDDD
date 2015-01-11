using System;
using System.Linq;
using PCExpert.DomainFramework.DataAccess;

namespace PCExpert.Core.Domain.Repositories
{
	public sealed class PCConfigurationRepository : IPCConfigurationRepository
	{
		private readonly PersistenceWorkplace _workplace;

		public PCConfigurationRepository(PersistenceWorkplace workplace)
		{
			_workplace = workplace;
		}

		public IQueryable<PCConfiguration> FindPublishedConfigurations(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException();

			return _workplace.Query<PCConfiguration>()
				.Where(x => x.Status == PCConfigurationStatus.Published && x.Name == name);
		}

		public void Save(PCConfiguration configuration)
		{
			_workplace.Save(configuration);
		}
	}
}