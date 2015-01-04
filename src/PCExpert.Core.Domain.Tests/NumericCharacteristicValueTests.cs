using System.Globalization;
using NUnit.Framework;

namespace PCExpert.Core.Domain.Tests
{
	[TestFixture]
	public class NumericCharacteristicValueTests :
		ConcreteCharacteristicValueTests<NumericCharacteristic, NumericCharacteristicValue, decimal>
	{
		[Test]
		public void Format_NotNullCulture_ShouldReturnStringifiedIntValue()
		{
			//Arrange
			var culture = new CultureInfo("ru-RU");
			var value = CreateCharacteristicValueWithDefaults(Characteristic);

			//Assert
			Assert.That(value.Format(culture), Is.EqualTo("150"));
		}

		protected override NumericCharacteristicValue CreateCharacteristicValueWithDefaults(
			NumericCharacteristic characteristic)
		{
			return new NumericCharacteristicValue(characteristic, GetDefaultValue());
		}

		protected override decimal GetValue(NumericCharacteristicValue charValue)
		{
			return charValue.Value;
		}

		protected override void SetValue(NumericCharacteristicValue charValue, decimal value)
		{
			charValue.EditValue(value);
		}

		protected override decimal GetDefaultValue()
		{
			return 150;
		}

		protected override decimal GetModifiedValue()
		{
			return GetDefaultValue() + 10;
		}
	}
}