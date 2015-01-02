using System.Globalization;
using Moq;
using NUnit.Framework;

namespace PCExpert.Core.Domain.Tests
{
	[TestFixture]
	public class BoolCharacteristicValueValueTests
		: ConcreteCharacteristicValueTests<BoolCharacteristic, BoolCharacteristicValue, bool>
	{
		[Test]
		public void Format_NotNullCulture_ShouldReturnStringifiedBoolValue()
		{
			//Arrange
			var culture = new CultureInfo("ru-RU");
			var value = CreateCharacteristicValueWithDefaults(Characteristic);

			//Assert
			Assert.That(value.Format(culture), Is.EqualTo("True"));
		}

		protected override BoolCharacteristicValue CreateCharacteristicValueWithDefaults(BoolCharacteristic characteristic)
		{
			return new BoolCharacteristicValue(characteristic, GetDefaultValue());
		}

		protected override bool GetValue(BoolCharacteristicValue charValue)
		{
			return charValue.Value;
		}

		protected override void SetValue(BoolCharacteristicValue charValue, bool value)
		{
			charValue.EditValue(value);
		}

		protected override bool GetDefaultValue()
		{
			return true;
		}

		protected override bool GetModifiedValue()
		{
			return false;
		}
	}
}