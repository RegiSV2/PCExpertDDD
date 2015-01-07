using System.Linq;

namespace PCExpert.Core.Domain.Repositories
{
	public interface IPCComponentRepository
	{
		IQueryable<PCComponent> Query(ComponentType type);
		void Save(PCComponent component);
	}
}