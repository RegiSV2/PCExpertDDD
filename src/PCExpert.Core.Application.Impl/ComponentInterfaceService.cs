using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Monads;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
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
		private static readonly PagedResult<ComponentInterfaceVO> EmptyResult =
			new PagedResult<ComponentInterfaceVO>(new PagingParameters(0, 0), 0, new List<ComponentInterfaceVO>());

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
					string.Format(InterfaceWithNameAlreadyExistsMsg, newInterface.Name), ex);
			});
		}

		public async Task<PagedResult<ComponentInterfaceVO>> GetComponentInterfaces(TableParameters parameters)
		{
			var countTotal = await CountTotal();

			if (countTotal == 0)
				return EmptyResult;

			var pagingParameters = CorrectPagingParameters(parameters.PagingParameters, countTotal);
			var results = await SublistInterfaces(parameters.OrderingParameters, pagingParameters);
			return new PagedResult<ComponentInterfaceVO>(pagingParameters, countTotal, results);
		}

		public async Task<ComponentInterfaceVO> GetComponentInterface(Guid id)
		{
			var query = _repository.Query(new EntityHasIdSpecification<ComponentInterface>(id))
				.Project().To<ComponentInterfaceVO>();
			var result = await query.FirstOrDefaultAsync();
			if (result == null)
				throw new NotFoundException(string.Format(InterfaceNotFoundMsg, id));
			return result;
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
			var orderExpression = ParseOrderByParameter(parameters.OrderBy);

			if (parameters.Direction == SortDirection.Ascending)
				query = query.OrderBy(orderExpression);
			else if (parameters.Direction == SortDirection.Descending)
				query = query.OrderByDescending(orderExpression);
			return query;
		}

		private static Expression<Func<ComponentInterface, object>> ParseOrderByParameter(string orderBy)
		{
			Contract.Ensures(Contract.Result<Expression<Func<ComponentInterface, object>>>() != null);
			try
			{
				return ExpressionReflection.Expression<ComponentInterface>(orderBy);
			}
			catch (Exception ex)
			{
				throw new InvalidInputException(string.Format(Messages.InvalidOrderByParameterMsg, orderBy), ex);
			}
		}

		private IQueryable<ComponentInterface> SelectPage(IQueryable<ComponentInterface> query,
			PagingParameters parameters)
		{
			return query.Skip(parameters.PageSize*parameters.PageNumber).Take(parameters.PageSize);
		}

		private static Task<List<ComponentInterfaceVO>> ListQuery(IQueryable<ComponentInterface> query)
		{
			return query
				.Project().To<ComponentInterfaceVO>()
				.ToListAsync();
		}

		#region System messages

		private const string InterfaceNotFoundMsg = "Interface with id = {0} not found";

		private const string InterfaceWithNameAlreadyExistsMsg = "Interface with the name \"{0}\" already exists";

		#endregion

		#region Dependencies

		private readonly IComponentInterfaceRepository _repository;

		private readonly IUnitOfWork _unitOfWork;

		#endregion
	}
}