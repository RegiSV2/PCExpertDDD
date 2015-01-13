using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace PCExpert.Core.Tests.Utils.FakeAsyncQuery
{
	internal class FakeAsyncEnumerator<T> : IDbAsyncEnumerator<T>
	{
		private readonly IEnumerator<T> _sourceEnumerator;

		public FakeAsyncEnumerator(IEnumerator<T> sourceEnumerable)
		{
			_sourceEnumerator = sourceEnumerable;
		}

		public void Dispose()
		{
			_sourceEnumerator.Dispose();
		}

		public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
		{
			return Task.FromResult(_sourceEnumerator.MoveNext());
		}

		public T Current
		{
			get { return _sourceEnumerator.Current; }
		}

		object IDbAsyncEnumerator.Current
		{
			get { return Current; }
		}
	}
}