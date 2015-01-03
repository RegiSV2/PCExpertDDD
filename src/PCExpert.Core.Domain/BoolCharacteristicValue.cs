using System.Globalization;
using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.Domain
{
	/// <summary>
	/// Value of <value>BoolCharacteristic</value> characteristic
	/// </summary>
	public class BoolCharacteristicValue : CharacteristicValue
	{
		#region Constructors

		protected BoolCharacteristicValue()
		{}

		public BoolCharacteristicValue(BoolCharacteristic characteristic, bool value) 
			: base(characteristic)
		{
			Argument.NotNull(characteristic);
			Value = value;
		}

		#endregion

		public bool Value { get; private set; }

		public void EditValue(bool newValue)
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
	}
}