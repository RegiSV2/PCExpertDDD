using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PCExpert.Core.DomainFramework.Utils
{
	public static class EnumerableExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
		{
			Contract.Assume(enumerable != null);

			return !enumerable.Any();
		}
	}
}
