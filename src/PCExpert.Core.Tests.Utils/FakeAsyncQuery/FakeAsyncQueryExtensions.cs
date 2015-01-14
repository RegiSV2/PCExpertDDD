using System.Collections.Generic;
using System.Linq;

namespace PCExpert.Core.Tests.Utils.FakeAsyncQuery
{
	public static class FakeAsyncQueryExtensions
	{
		public static IQueryable<T> AsAsyncQueryable<T>(this IEnumerable<T> source)
		{
			return new FakeAsyncEnumerable<T>(source);
		}
	}
}