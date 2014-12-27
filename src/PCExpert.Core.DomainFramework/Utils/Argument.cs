﻿using System;
using System.Diagnostics.Contracts;

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
		public static void NotNegative(decimal value)
		{
			Contract.Requires<ArgumentOutOfRangeException>(value > 0);
		}

		[ContractAbbreviator]
		public static void ValidEnumItem<TEnum>(TEnum value)
		{
			Contract.Requires<ArgumentException>(Enum.IsDefined(typeof (TEnum), value));
		}
	}
}