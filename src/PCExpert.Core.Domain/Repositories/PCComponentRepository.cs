using System.Linq;
using PCExpert.Core.DomainFramework.DataAccess;

namespace PCExpert.Core.Domain.Repositories
{
	public class PCComponentRepository : IPCComponentRepository
	{
		private readonly PersistenceWorkplace _workplace;

		public PCComponentRepository(PersistenceWorkplace workplace)
		{
			_workplace = workplace;
		}

		public IQueryable<PCComponent> Query(ComponentType type)
		{
			return _workplace.Query<PCComponent>()
				.Where(x => x.Type == type);
		}

		public void Save(PCComponent component)
		{
			_workplace.Save(component);
		}
	}
}