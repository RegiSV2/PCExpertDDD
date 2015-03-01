using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PCExpert.DomainFramework.Utils
{
	public static class EnumerableExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
		{
			Contract.Assume(enumerable != null);

			return !enumerable.Any();
		}

		public static string ConcatToFriendlyEnumeration(this IEnumerable<string> strings, CultureInfo culture)
		{
			Argument.NotNull(strings);

			var stringsList = strings.ToArray();

			if (stringsList.Length == 0)
				return string.Empty;

			var result = stringsList.Last();
			if (stringsList.Length > 1)
				result = stringsList.Take(stringsList.Length - 1).Aggregate((a, b) => a + culture.TextInfo.ListSeparator + ' ' + b)
				         + Resources.LastListSeparator + ' ' + result;

			return result;
		}
	}
}