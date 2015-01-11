using System.Globalization;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.Core.Domain
{
	/// <summary>
	///     Value of
	///     <value>NumericCharacteristic</value>
	///     characteristic
	/// </summary>
	public class NumericCharacteristicValue : CharacteristicValue
	{
		public decimal Value { get; private set; }

		public void EditValue(decimal newValue)
		{
			Value = newValue;
		}

		protected override string DoFormat(CultureInfo cultureInfo)
		{
			return Value.ToString(cultureInfo);
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", GetType().Name, Value);
		}

		#region Constructors

		protected NumericCharacteristicValue()
		{
		}

		public NumericCharacteristicValue(NumericCharacteristic characteristic, decimal value)
			: base(characteristic)
		{
			Argument.NotNull(characteristic);

			Value = value;
		}

		#endregion
	}
}