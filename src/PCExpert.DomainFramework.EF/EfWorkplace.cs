using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PCExpert.DomainFramework.DataAccess;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.DomainFramework.EF
{
	public class EfWorkplace : PersistenceWorkplace
	{
		private readonly IDbContextProvider _contextProvider;

		public EfWorkplace(IDbContextProvider contextProvider)
		{
			Argument.NotNull(contextProvider);
			_contextProvider = contextProvider;
		}

		public override IQueryable<TEntity> Query<TEntity>()
		{
			return EntitySet<TEntity>();
		}

		public override void Insert<TEntity>(TEntity entity)
		{
			EntitySet<TEntity>().Add(entity);
		}

		public override void Insert<TEntity>(IEnumerable<TEntity> entities)
		{
			EntitySet<TEntity>().AddRange(entities);
		}

		public override void Update<TEntity>(TEntity entity)
		{
			_contextProvider.DbContext.Entry(entity).State = EntityState.Modified;
		}

		public override void Delete<TEntity>(TEntity entity)
		{
			EntitySet<TEntity>().Remove(entity);
		}

		public override void Delete<TEntity>(IEnumerable<TEntity> entities)
		{
			EntitySet<TEntity>().RemoveRange(entities);
		}

		private DbSet<TEntity> EntitySet<TEntity>()
			where TEntity : class
		{
			return _contextProvider.DbContext.Set<TEntity>();
		}
	}
}