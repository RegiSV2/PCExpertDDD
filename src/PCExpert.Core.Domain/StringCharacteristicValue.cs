using System.Globalization;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.Core.Domain
{
	/// <summary>
	///     Value of
	///     <value>StringCharacteristic</value>
	///     characteristic
	/// </summary>
	public class StringCharacteristicValue : CharacteristicValue
	{
		public string Value { get; private set; }

		public void EditValue(string newValue)
		{
			Value = newValue;
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", GetType().Name, Value);
		}

		protected override string DoFormat(CultureInfo cultureInfo)
		{
			return Value.ToString(cultureInfo);
		}

		#region Constructors

		protected StringCharacteristicValue()
		{
		}

		public StringCharacteristicValue(StringCharacteristic characteristic, string value)
			: base(characteristic)
		{
			Argument.NotNull(characteristic);
			Value = value;
		}

		#endregion
	}
}