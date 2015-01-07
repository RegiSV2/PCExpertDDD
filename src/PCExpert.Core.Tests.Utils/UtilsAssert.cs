using System;
using System.Collections.Generic;
using System.Linq;
using PCExpert.Core.DomainFramework;

namespace PCExpert.Core.Tests.Utils
{
	public static class UtilsAssert
	{
		public static bool CollectionsEqual<TEntity>(IReadOnlyCollection<TEntity> expected,
			IReadOnlyCollection<TEntity> actual)
			where TEntity : Entity
		{
			return CollectionsEqual(expected, actual, (x, y) => x.SameIdentityAs(y));
		}

		public static bool CollectionsEqual<T>(IReadOnlyCollection<T> expected,
			IReadOnlyCollection<T> actual, Func<T, T, bool> compare)
		{
			return expected.Count == actual.Count
			       && actual.All(actualEntity => expected.Count(x => compare(x, actualEntity)) == 1);
		}
	}
}