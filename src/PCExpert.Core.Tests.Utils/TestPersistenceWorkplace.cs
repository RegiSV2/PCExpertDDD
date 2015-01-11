using System;
using System.Collections.Generic;
using System.Linq;
using PCExpert.DomainFramework.DataAccess;

namespace PCExpert.Core.Tests.Utils
{
	public class TestPersistenceWorkplace : PersistenceWorkplace
	{
		public bool IsInsertCalled { get; private set; }
		public bool IsUpdateCalled { get; private set; }

		public override IQueryable<TEntity> Query<TEntity>()
		{
			throw new NotImplementedException();
		}

		public override void Insert<TEntity>(TEntity entity)
		{
			IsInsertCalled = true;
		}

		public override void Insert<TEntity>(IEnumerable<TEntity> entities)
		{
			throw new NotImplementedException();
		}

		public override void Update<TEntity>(TEntity entity)
		{
			IsUpdateCalled = true;
		}

		public override void Delete<TEntity>(TEntity entity)
		{
			throw new NotImplementedException();
		}

		public override void Delete<TEntity>(IEnumerable<TEntity> entity)
		{
			throw new NotImplementedException();
		}
	}
}