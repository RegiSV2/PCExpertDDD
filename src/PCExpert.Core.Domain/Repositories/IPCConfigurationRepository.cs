using System.Linq;

namespace PCExpert.Core.Domain.Repositories
{
	public interface IPCConfigurationRepository
	{
		IQueryable<PCConfiguration> FindPublishedConfigurations(string name);

		void Save(PCConfiguration configuration);
	}
}