using System.Globalization;
using NUnit.Framework;

namespace PCExpert.Core.Domain.Tests
{
	[TestFixture]
	public class DecimalCharacteristicValueTests :
		ConcreteCharacteristicValueTests<NumericCharacteristic, DecimalCharacteristicValue, decimal>
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

		protected override DecimalCharacteristicValue CreateCharacteristicValueWithDefaults(NumericCharacteristic characteristic)
		{
			return new DecimalCharacteristicValue(characteristic, GetDefaultValue());
		}

		protected override decimal GetValue(DecimalCharacteristicValue charValue)
		{
			return charValue.Value;
		}

		protected override void SetValue(DecimalCharacteristicValue charValue, decimal value)
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