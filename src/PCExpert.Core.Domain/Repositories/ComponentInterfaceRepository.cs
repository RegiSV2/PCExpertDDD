using System.Linq;
using PCExpert.Core.DomainFramework.DataAccess;
using PCExpert.Core.DomainFramework.Specifications;

namespace PCExpert.Core.Domain.Repositories
{
	public class ComponentInterfaceRepository : IComponentInterfaceRepository
	{
		private readonly PersistenceWorkplace _workplace;

		public ComponentInterfaceRepository(PersistenceWorkplace workplace)
		{
			_workplace = workplace;
		}

		public IQueryable<ComponentInterface> Query(PersistenceAwareSpecification<ComponentInterface> specification)
		{
			return _workplace.Query<ComponentInterface>()
				.Where(specification.GetConditionExpression());
		}

		public void Save(ComponentInterface componentInterface)
		{
			_workplace.Save(componentInterface);
		}
	}
}