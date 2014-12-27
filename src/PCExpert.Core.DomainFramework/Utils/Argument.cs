﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using PCExpert.Core.DomainFramework.Exceptions;

namespace PCExpert.Core.DomainFramework.Utils
{
	/// <summary>
	///     Contains common preconditions for method arguments
	/// </summary>
	public static class Argument
	{
		[ContractAbbreviator]
		public static void NotNull(object value)
		{
			Contract.Requires<ArgumentNullException>(value != null);
		}

		[ContractAbbreviator]
		public static void NotNullAndNotEmpty(string value)
		{
			Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(value));
		}

		[ContractAbbreviator]
		public static void NotNullAndNotEmpty<T>(ICollection<T> collection)
		{
			Contract.Requires<ArgumentNullException>(collection != null);
			Contract.Requires<EmptyCollectionException>(collection.Count > 0);
		}

		[ContractAbbreviator]
		public static void NotNegative(decimal value)
		{
			Contract.Requires<ArgumentOutOfRangeException>(value > 0);
		}

		[ContractAbbreviator]
		public static void ValidEnumItem<TEnum>(TEnum value)
			where TEnum : struct
		{
			Contract.Requires<ArgumentException>(Enum.IsDefined(typeof (TEnum), value));
		}
	}
}