using System;
using Moq;
using NUnit.Framework;

namespace PCExpert.Core.Domain.Tests
{
	[TestFixture]
	public abstract class ConcreteCharacteristicValueTests<TCharacteristic, TCharacteristicValue, TVal>
		where TCharacteristicValue : CharacteristicValue
		where TCharacteristic : ComponentCharacteristic
	{
		protected TCharacteristic Characteristic;

		[SetUp]
		public void EstablishContext()
		{
			Characteristic = new Mock<TCharacteristic>().Object;
		}

		[Test]
		public void Constructor_ValidArguments_ShouldCreateWithSpecifiedValue()
		{
			//Arrange
			var charValue = CreateCharacteristicValueWithDefaults(Characteristic);

			//Assert
			Assert.That(GetValue(charValue), Is.EqualTo(GetDefaultValue()));
			Assert.That(charValue.Characteristic, Is.EqualTo(Characteristic));
		}

		[Test]
		public void Constructor_NullCharacteristic_ShouldThrowArgumentNullException()
		{
			Assert.That(() => new NumericCharacteristicValue(null, 1),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void EditValue_AnyValue_ShouldSetNewValue()
		{
			//Arrange
			var charValue = CreateCharacteristicValueWithDefaults(Characteristic);
			var newValue = GetModifiedValue();

			//Act
			SetValue(charValue, newValue);

			//Assert
			Assert.That(GetValue(charValue), Is.EqualTo(newValue));
		}

		protected abstract TCharacteristicValue CreateCharacteristicValueWithDefaults(TCharacteristic characteristic);

		protected abstract TVal GetValue(TCharacteristicValue charValue);

		protected abstract void SetValue(TCharacteristicValue charValue, TVal value);

		protected abstract TVal GetDefaultValue();

		protected abstract TVal GetModifiedValue();
	}
}