using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PCExpert.Core.Application.ViewObjects;
using PCExpert.Core.Domain;
using PCExpert.Core.Domain.Repositories;
using PCExpert.Core.Domain.Specifications;
using PCExpert.DomainFramework;
using PCExpert.DomainFramework.Exceptions;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.Core.Application.Impl
{
	public class ComponentInterfaceService : IComponentInterfaceService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IComponentInterfaceRepository _repository;

		public ComponentInterfaceService(IUnitOfWork unitOfWork, IComponentInterfaceRepository repository)
		{
			_unitOfWork = unitOfWork;
			_repository = repository;
		}

		public async Task CreateComponentInterface(ComponentInterfaceVO newInterface)
		{
			await _unitOfWork.Execute(() =>
				{
					var instance = new ComponentInterface(newInterface.Name);
					_repository.Save(instance);
				},
				ex =>
				{
					throw new BusinessLogicException(
						string.Format("Interface with the name \"{0}\" already exists", newInterface.Name), ex);
				});
		}

		public async Task<PagedResult<ComponentInterfaceVO>> GetComponentInterfaces(TableParameters parameters)
		{
			var query = _repository.Query(new ComponentInterfaceNameContainsSpecification(""));

			var orderExpression = ExpressionReflection.Expression<ComponentInterface>(parameters.OrderingParameters.OrderBy);
			if (parameters.OrderingParameters.Direction == SortDirection.Ascending)
				query = query.OrderBy(orderExpression);
			else if (parameters.OrderingParameters.Direction == SortDirection.Descending)
				query = query.OrderByDescending(orderExpression);

			var results = await query.Skip(parameters.PagingParameters.PageSize*parameters.PagingParameters.PageNumber)
				.Take(parameters.PagingParameters.PageSize)
				.Select(x => new ComponentInterfaceVO(x))
				.ToListAsync();
			var countTotal = await query.CountAsync();

			return new PagedResult<ComponentInterfaceVO>(
				parameters.PagingParameters, countTotal, results);
		}
	}
}