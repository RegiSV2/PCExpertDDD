using System;
using System.Threading.Tasks;
using PCExpert.DomainFramework.Exceptions;

namespace PCExpert.DomainFramework
{
	/// <summary>
	///     Executes actions in a "unit of work" scope
	/// </summary>
	public interface IUnitOfWork
	{
		Task Execute(Action action);

		Task Execute(Action action, Action<PersistenceException> exceptionHandler);
	}
}