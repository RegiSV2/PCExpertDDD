using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using PCExpert.DomainFramework;
using PCExpert.DomainFramework.Exceptions;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.Core.Tests.Utils
{
	public static class UtilsAssert
	{
		private const string ThrownExceptionOfTypeMSg = "Thrown exception of type ";

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

		public static void AssertThrowsAggregateException<TInnerException>(Action action)
		{
			try
			{
				action.DynamicInvoke();
				Assert.Fail();
			}
			catch (TargetInvocationException ex)
			{
				var innerEx = ex.InnerException as AggregateException;
				if (innerEx == null || !(innerEx.InnerExceptions.First() is TInnerException))
				{
					var thrownEx = innerEx == null ? ex : innerEx.InnerExceptions.First();
					Assert.Fail(ThrownExceptionOfTypeMSg + thrownEx.GetType().FullName);
				}
			}
			catch (Exception ex)
			{
				Assert.Fail(ThrownExceptionOfTypeMSg + ex.GetType().FullName);
			}
		}

		public static void AssertThrowsAggregateException<TInnerException, TResult>(Func<TResult> func)
		{
			AssertThrowsAggregateException<TInnerException>(() => { func(); });
		}
	}
}