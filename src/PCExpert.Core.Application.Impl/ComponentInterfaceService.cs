using System;
using System.Threading.Tasks;
using PCExpert.Core.Application.ViewObjects;
using PCExpert.Core.Domain.Repositories;

namespace PCExpert.Core.Application.Impl
{
	public class ComponentInterfaceService : IComponentInterfaceService
	{
		private readonly IComponentInterfaceRepository _repository;

		public ComponentInterfaceService(IComponentInterfaceRepository repository)
		{
			_repository = repository;
		}

		public Task CreateComponentInterface(ComponentInterfaceVO newInterface)
		{
			throw new NotImplementedException();
		}

		public Task<PagedResult<ComponentInterfaceVO>> GetComponentInterfaces(PagingParameters pagingParameters)
		{
			throw new NotImplementedException();
		}
	}
}