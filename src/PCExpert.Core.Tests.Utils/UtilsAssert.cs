using System;
using System.Collections.Generic;
using System.Linq;
using PCExpert.DomainFramework;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.Core.Tests.Utils
{
	public static class UtilsAssert
	{
		public static bool CollectionsEqual<TEntity>(IReadOnlyCollection<TEntity> expected,
			IReadOnlyCollection<TEntity> actual)
			where TEntity : Entity
		{
			Argument.NotNull(expected);
			Argument.NotNull(actual);
			return CollectionsEqual(expected, actual, (x, y) => x.SameIdentityAs(y));
		}

		public static bool CollectionsEqual<TExpected, TActual>(IReadOnlyCollection<TExpected> expected,
			IReadOnlyCollection<TActual> actual, Func<TExpected, TActual, bool> compare)
		{
			Argument.NotNull(expected);
			Argument.NotNull(actual);
			return expected.Count == actual.Count
			       && actual.All(actualEntity => expected.Count(x => compare(x, actualEntity)) == 1);
		}
	}
}