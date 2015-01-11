using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using PCExpert.DomainFramework.Exceptions;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.DomainFramework.Tests.Utils
{
	[TestFixture]
	public class EnumerableExtensionsTests
	{
		[Test]
		public void IsEmpty_NoElements_ShouldReturnTrue()
		{
			Assert.That(Enumerable.Empty<string>().IsEmpty());
		}

		[Test]
		public void IsEmpty_AnyElements_ShouldReturnFalse()
		{
			Assert.That(!new[] {"a"}.IsEmpty());
		}

		[Test]
		public void ConcatToFriendlyEnumeration_NoElements_ShouldThrowEmptyCollectionException()
		{
			Assert.That(() => Concat(Enumerable.Empty<string>()),
				Throws.InstanceOf<EmptyCollectionException>());
		}

		[Test]
		public void ConcatToFriendlyEnumeration_OneElement_ShouldReturnThisElement()
		{
			const string element = "str";
			Assert.That(Concat(new[] {element}),
				Is.EqualTo(element));
		}

		[Test]
		public void ConcatToFriendlyEnumeration_TwoElements_ShouldReturnElementsConcattedWithSeparatorFromResources()
		{
			//Arrange
			var argument = new[] {"str1", "str2"};
			var expectedResult = "str1 and str2";

			Assert.That(Concat(argument), Is.EqualTo(expectedResult));
		}

		[Test]
		public void ConcatToFriendlyEnumeration_ManyElements_ShouldReturnElementsConcattedWithListSeparatorAndLastSeparator()
		{
			//Arrange
			var listSeparator = CultureInfo.InvariantCulture.TextInfo.ListSeparator;
			const string lastSeparator = "and";
			var argument = new[] {"str1", "str2", "str3"};
			var expectedResult = "str1" + listSeparator + " str2 " + lastSeparator + " str3";

			Assert.That(Concat(argument), Is.EqualTo(expectedResult));
		}

		private static string Concat(IEnumerable<string> argument)
		{
			return argument.ConcatToFriendlyEnumeration(CultureInfo.InvariantCulture);
		}
	}
}