using System.Linq;
using PCExpert.Core.DomainFramework.DataAccess;
using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.Domain.Repositories
{
	public class PCConfigurationRepository : IPCConfigurationRepository
	{
		private readonly PersistenceWorkplace _workplace;

		public PCConfigurationRepository(PersistenceWorkplace workplace)
		{
			_workplace = workplace;
		}

		public IQueryable<PCConfiguration> FindPublishedConfigurations(string name)
		{
			Argument.NotNullAndNotEmpty(name);
			return _workplace.Query<PCConfiguration>()
				.Where(x => x.Status == PCConfigurationStatus.Published && x.Name == name);
		}

		public void Save(PCConfiguration configuration)
		{
			_workplace.Save(configuration);
		}
	}
}