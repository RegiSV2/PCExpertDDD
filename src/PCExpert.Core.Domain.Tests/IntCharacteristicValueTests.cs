using System.Globalization;
using NUnit.Framework;

namespace PCExpert.Core.Domain.Tests
{
	[TestFixture]
	public class IntCharacteristicValueTests :
		ConcreteCharacteristicValueTests<IntCharacteristic, IntCharacteristicValue, int>
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

		protected override IntCharacteristicValue CreateCharacteristicValueWithDefaults(IntCharacteristic characteristic)
		{
			return new IntCharacteristicValue(characteristic, GetDefaultValue());
		}

		protected override int GetValue(IntCharacteristicValue charValue)
		{
			return charValue.Value;
		}

		protected override void SetValue(IntCharacteristicValue charValue, int value)
		{
			charValue.EditValue(value);
		}

		protected override int GetDefaultValue()
		{
			return 150;
		}

		protected override int GetModifiedValue()
		{
			return GetDefaultValue() + 10;
		}
	}
}