using System.Linq;
using PCExpert.Core.DomainFramework.Specifications;

namespace PCExpert.Core.Domain.Repositories
{
	public interface IComponentInterfaceRepository
	{
		IQueryable<ComponentInterface> Query(PersistenceAwareSpecification<ComponentInterface> specification);

		void Save(ComponentInterface componentInterface);
	}
}