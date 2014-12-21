using System;
using System.Collections.Generic;

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
			if(list.Count <= 1)
				throw new InvalidOperationException();
			var index = list.IndexOf(except);

			while (true)
			{
				var resultIndex = Random.Next(list.Count);
				if (index != resultIndex)
					return list[resultIndex];
			}
		}
	}
}