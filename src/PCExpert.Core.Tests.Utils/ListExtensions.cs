using System;
using System.Collections.Generic;
using System.Linq;

namespace PCExpert.Core.Tests.Utils
{
	public static class ListExtensions
	{
		private static readonly Random Random = new Random();

		public static T RandomElement<T>(this IList<T> list)
		{
			return list[Random.Next(list.Count)];
		}

		public static T RandomElementExcept<T>(this IList<T> list, T except)
		{
			return list.RandomElementExcept(new List<T> {except});
		}

		public static T RandomElementExcept<T>(this IList<T> list, ICollection<T> except)
		{
			if (list.Count <= except.Count)
				throw new InvalidOperationException();

			while (true)
			{
				var resultIndex = Random.Next(list.Count);
				var result = list[resultIndex];
				if (!except.Contains(result))
					return result;
			}
		}
	}
}