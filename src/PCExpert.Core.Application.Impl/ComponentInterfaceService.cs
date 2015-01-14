using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Monads;
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
		private readonly IComponentInterfaceRepository _repository;
		private readonly IUnitOfWork _unitOfWork;

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
			}, ex =>
			{
				throw new BusinessLogicException(
					string.Format("Interface with the name \"{0}\" already exists", newInterface.Name), ex);
			});
		}

		public async Task<PagedResult<ComponentInterfaceVO>> GetComponentInterfaces(TableParameters parameters)
		{
			var countTotal = await CountTotal();
			var pagingParameters = CorrectPagingParameters(parameters.PagingParameters, countTotal);
			var results = await SublistInterfaces(parameters.OrderingParameters, pagingParameters);

			return new PagedResult<ComponentInterfaceVO>(pagingParameters, countTotal, results);
		}

		private Task<int> CountTotal()
		{
			return QueryInterfaces().CountAsync();
		}

		private static PagingParameters CorrectPagingParameters(PagingParameters pagingParameters, int countTotal)
		{
			var maxPage = countTotal/pagingParameters.PageSize;
			if (countTotal%pagingParameters.PageSize == 0)
				maxPage -= 1;

			var requestPage = pagingParameters.PageNumber;
			if (requestPage > maxPage)
				requestPage = maxPage;
			return new PagingParameters(requestPage, pagingParameters.PageSize);
		}

		private Task<List<ComponentInterfaceVO>> SublistInterfaces(OrderingParameters ordering, PagingParameters paging)
		{
			return QueryInterfaces()
				.With(x => OrderQuery(x, ordering))
				.With(x => SelectPage(x, paging))
				.With(ListQuery);
		}

		private IQueryable<ComponentInterface> QueryInterfaces()
		{
			return _repository.Query(new ComponentInterfaceNameContainsSpecification(""));
		}

		private static IQueryable<ComponentInterface> OrderQuery(IQueryable<ComponentInterface> query,
			OrderingParameters parameters)
		{
			var orderExpression = ExpressionReflection.Expression<ComponentInterface>(parameters.OrderBy);
			if (parameters.Direction == SortDirection.Ascending)
				query = query.OrderBy(orderExpression);
			else if (parameters.Direction == SortDirection.Descending)
				query = query.OrderByDescending(orderExpression);
			return query;
		}

		private IQueryable<ComponentInterface> SelectPage(IQueryable<ComponentInterface> query,
			PagingParameters parameters)
		{
			return query.Skip(parameters.PageSize*parameters.PageNumber).Take(parameters.PageSize);
		}

		private static Task<List<ComponentInterfaceVO>> ListQuery(IQueryable<ComponentInterface> query)
		{
			return query
				.Select(x => new ComponentInterfaceVO(x))
				.ToListAsync();
		}
	}
}