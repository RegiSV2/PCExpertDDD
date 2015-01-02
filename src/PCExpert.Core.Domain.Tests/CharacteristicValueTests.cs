using System;
using System.Globalization;
using Moq;
using NUnit.Framework;

namespace PCExpert.Core.Domain.Tests
{
	[TestFixture]
	public class CharacteristicValueTests
	{
		private ComponentCharacteristic _characteristic;

		private class FakeCharacteristicValue : CharacteristicValue
		{
			public FakeCharacteristicValue(ComponentCharacteristic characteristic)
				:base(characteristic)
			{ }

			protected override string DoFormat(CultureInfo cultureInfo)
			{
				IsDoFormatCalled = true;
				return "";
			}

			public bool IsDoFormatCalled { get; private set; }
		}

		[SetUp]
		public void EstablishContext()
		{
			_characteristic = new Mock<ComponentCharacteristic>().Object;
		}

		[Test]
		public void Constructor_ValidArguments_ShouldCreateWithSpecifiedValue()
		{
			//Arrange
			var charValue = CreateCharacteristicWithSpecifiedDefaults();

			//Assert
			Assert.That(charValue.Characteristic, Is.EqualTo(_characteristic));
		}

		[Test]
		public void Constructor_NullCharacteristic_ShouldThrowArgumentNullException()
		{
			Assert.That(() => new FakeCharacteristicValue(null),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Format_NullCulture_ShouldThrowArgumentNullException()
		{
			var value = CreateCharacteristicWithSpecifiedDefaults();
			Assert.That(() => value.Format(null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Format_AnyCulture_ShouldCallDoFormatWithThatCulture()
		{
			//Arrange
			var value = CreateCharacteristicWithSpecifiedDefaults();
			var culture = CultureInfo.CurrentCulture;
			value.Format(culture);

			Assert.That(value.IsDoFormatCalled);
		}

		private FakeCharacteristicValue CreateCharacteristicWithSpecifiedDefaults()
		{
			return new FakeCharacteristicValue(_characteristic);
		}
	}
}