using System;
using System.Globalization;
using NUnit.Framework;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.Domain.Tests
{
	[TestFixture]
	public class ComponentCharacteristicTests
	{
		private const ComponentType DefaultType = ComponentType.SolidStateDrice;

		private readonly string _defaultName = NamesGenerator.CharacteristicName();

		private class FakeCharacteristic : ComponentCharacteristic
		{
			public FakeCharacteristic(string name, ComponentType type)
				:base(name, type)
			{}
		}

		[Test]
		public void Constructor_ValidArguments_ShouldCreateNewCharacteristic()
		{
			//Act
			var characteristic = CreateCharacteristicWithSpecifiedDefaults();

			//Assert
			Assert.That(characteristic.ComponentType, Is.EqualTo(DefaultType));
			Assert.That(characteristic.Name, Is.EqualTo(_defaultName));
			Assert.That(characteristic.FormattingPattern, Is.Null);
		}

		[Test]
		public void Constructor_InvalidType_ShouldThrowArgumentException()
		{
			Assert.That(() => new FakeCharacteristic(
				NamesGenerator.CharacteristicName(),
				(ComponentType) 12345),
				Throws.ArgumentException);
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		public void Constructor_EmptyName_ShouldThrowArgumentNullException(string name)
		{
			Assert.That(() => new FakeCharacteristic(
				name, ComponentType.Motherboard),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		public void WithFormattingPattern_NullOrEmptyPattern_ShouldThrowArgumentNullException(string pattern)
		{
			//Arrange
			var characteristic = CreateCharacteristicWithSpecifiedDefaults();

			//Assert
			Assert.That(() => characteristic.WithPattern(pattern),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void WithFormattingPattern_NotEmptyPattern_ShouldSetPatternToNewValue()
		{
			//Arrange
			const string pattern = "{0} some pattern";
			var characteristic = CreateCharacteristicWithSpecifiedDefaults().WithPattern(pattern);

			//Assert
			Assert.That(characteristic.FormattingPattern, Is.EqualTo(pattern));
		}

		private FakeCharacteristic CreateCharacteristicWithSpecifiedDefaults()
		{
			return new FakeCharacteristic(_defaultName, DefaultType);
		}
	}
}