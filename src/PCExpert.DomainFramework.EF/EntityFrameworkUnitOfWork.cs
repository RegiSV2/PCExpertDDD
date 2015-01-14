using System;
using System.Threading.Tasks;
using PCExpert.DomainFramework.Exceptions;

namespace PCExpert.DomainFramework.EF
{
	public class EntityFrameworkUnitOfWork : IUnitOfWork
	{
		private readonly IDbContextProvider _contextProvider;

		public EntityFrameworkUnitOfWork(IDbContextProvider contextProvider)
		{
			_contextProvider = contextProvider;
		}

		public async Task Execute(Action action)
		{
			await Execute(action, x => { throw x; });
		}

		public async Task Execute(Action action, Action<PersistenceException> exceptionHandler)
		{
			using (var transaction = _contextProvider.DbContext.Database.BeginTransaction())
			{
				try
				{
					action();
					await _contextProvider.DbContext.SaveChangesAsync();
					transaction.Commit();
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					if (ex is PersistenceException)
						exceptionHandler(ex as PersistenceException);
					else
						throw;
				}
			}
		}
	}
}