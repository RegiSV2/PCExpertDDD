using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace PCExpert.Core.Tests.Utils.FakeAsyncQuery
{
	internal class FakeAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
	{
		public FakeAsyncEnumerable(IEnumerable<T> source)
			: base(source)
		{
		}

		public FakeAsyncEnumerable(Expression expression)
			: base(expression)
		{
		}

		public IDbAsyncEnumerator<T> GetAsyncEnumerator()
		{
			return new FakeAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
		}

		IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
		{
			return GetAsyncEnumerator();
		}

		IQueryProvider IQueryable.Provider
		{
			get { return new FakeAsyncQueryProvider<T>(this); }
		}
	}
}