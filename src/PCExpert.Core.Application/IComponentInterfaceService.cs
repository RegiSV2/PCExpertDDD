using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using PCExpert.Core.Application.ViewObjects;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.Core.Application
{
	/// <summary>
	///     Provides operations for managing component interfaces
	/// </summary>
	[ContractClass(typeof (ComponentInterfaceServiceContracts))]
	public interface IComponentInterfaceService
	{
		Task CreateComponentInterface(ComponentInterfaceVO newInterface);
		Task<PagedResult<ComponentInterfaceVO>> GetComponentInterfaces(PagingParameters pagingParameters);
	}

	[ContractClassFor(typeof (IComponentInterfaceService))]
	internal abstract class ComponentInterfaceServiceContracts : IComponentInterfaceService
	{
		public Task CreateComponentInterface(ComponentInterfaceVO newInterface)
		{
			Argument.NotNull(newInterface);
			Contract.Ensures(Contract.Result<Task>() != null);
			return null;
		}

		public Task<PagedResult<ComponentInterfaceVO>> GetComponentInterfaces(PagingParameters pagingParameters)
		{
			throw new NotImplementedException();
		}
	}
}