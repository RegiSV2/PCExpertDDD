using System;
using System.Threading.Tasks;

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
			using (var transaction = _contextProvider.DbContext.Database.BeginTransaction())
			{
				try
				{
					action();
					await _contextProvider.DbContext.SaveChangesAsync();
					transaction.Commit();
				}
				catch (Exception)
				{
					transaction.Rollback();
				}
			}
		}
	}
}