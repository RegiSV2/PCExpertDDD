using System.Globalization;
using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.Domain
{
	/// <summary>
	/// Value of <value>IntCharacteristic</value> characteristic
	/// </summary>
	public class IntCharacteristicValue : CharacteristicValue
	{
		#region Constructors

		protected IntCharacteristicValue()
		{}

		public IntCharacteristicValue(IntCharacteristic characteristic, int value) 
			: base(characteristic)
		{
			Argument.NotNull(characteristic);

			Value = value;
		}

		#endregion

		public int Value { get; private set; }

		public void EditValue(int newValue)
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