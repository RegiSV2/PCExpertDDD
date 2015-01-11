using System;
using System.Threading.Tasks;

namespace PCExpert.DomainFramework
{
	/// <summary>
	///     Executes actions in a "unit of work" scope
	/// </summary>
	public interface IUnitOfWork
	{
		Task Execute(Action action);
	}
}