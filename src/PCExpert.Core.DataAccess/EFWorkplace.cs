using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PCExpert.Core.DomainFramework.DataAccess;
using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.DataAccess
{
	public class EfWorkplace : PersistenceWorkplace
	{
		private readonly PCExpertContext _context;

		public EfWorkplace(PCExpertContext context)
		{
			Argument.NotNull(context);
			_context = context;
		}

		public override IQueryable<TEntity> Query<TEntity>()
		{
			return _context.Set<TEntity>();
		}

		public override void Insert<TEntity>(TEntity entity)
		{
			_context.Set<TEntity>().Add(entity);
		}

		public override void Insert<TEntity>(IEnumerable<TEntity> entities)
		{
			_context.Set<TEntity>().AddRange(entities);
		}

		public override void Update<TEntity>(TEntity entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}

		public override void Delete<TEntity>(TEntity entity)
		{
			_context.Set<TEntity>().Remove(entity);
		}

		public override void Delete<TEntity>(IEnumerable<TEntity> entities)
		{
			_context.Set<TEntity>().RemoveRange(entities);
		}
	}
}