using System.Globalization;
using NUnit.Framework;

namespace PCExpert.Core.Domain.Tests
{
	[TestFixture]
	public class StringCharacteristicValueTests :
		ConcreteCharacteristicValueTests<StringCharacteristic, StringCharacteristicValue, string>
	{
		[Test]
		public void Format_NotNullCulture_ShouldReturnValue()
		{
			//Arrange
			var culture = new CultureInfo("ru-RU");
			var value = CreateCharacteristicValueWithDefaults(Characteristic);

			//Assert
			Assert.That(value.Format(culture), Is.EqualTo(value.Value));
		}

		protected override StringCharacteristicValue CreateCharacteristicValueWithDefaults(StringCharacteristic characteristic)
		{
			return new StringCharacteristicValue(characteristic, GetDefaultValue());
		}

		protected override string GetValue(StringCharacteristicValue charValue)
		{
			return charValue.Value;
		}

		protected override void SetValue(StringCharacteristicValue charValue, string value)
		{
			charValue.EditValue(value);
		}

		protected override string GetDefaultValue()
		{
			return "default value";
		}

		protected override string GetModifiedValue()
		{
			return "modified value";
		}
	}
}