using System.Globalization;
using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.Domain
{
	/// <summary>
	/// Represents a value of some characteristic for a <value>PCComponent</value>
	/// </summary>
	public abstract class CharacteristicValue
	{
		#region Constructors

		protected CharacteristicValue()
		{ }

		protected CharacteristicValue(ComponentCharacteristic characteristic)
		{
			Argument.NotNull(characteristic);

			Characteristic = characteristic;
		}

		#endregion

		#region Properties

		public ComponentCharacteristic Characteristic { get; private set; }

		#endregion

		public string Format(CultureInfo cultureInfo)
		{
			Argument.NotNull(cultureInfo);
			return DoFormat(cultureInfo);
		}

		protected abstract string DoFormat(CultureInfo cultureInfo);
	}
}