using System.Globalization;
using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.Domain
{
	/// <summary>
	/// Value of <value>NumericCharacteristic</value> characteristic
	/// </summary>
	public class DecimalCharacteristicValue : CharacteristicValue
	{
		#region Constructors

		protected DecimalCharacteristicValue()
		{}

		public DecimalCharacteristicValue(NumericCharacteristic characteristic, decimal value) 
			: base(characteristic)
		{
			Argument.NotNull(characteristic);

			Value = value;
		}

		#endregion

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
	}
}