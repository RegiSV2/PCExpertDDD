namespace PCExpert.Core.Domain
{
	/// <summary>
	/// Characteristic that can have numeric argument values
	/// </summary>
	public class NumericCharacteristic : ComponentCharacteristic
	{
		#region Constructors

		protected NumericCharacteristic()
		{ }

		public NumericCharacteristic(string name, ComponentType type)
			: base(name, type)
		{ }

		#endregion

		#region Public Methods

		public NumericCharacteristicValue CreateValue(decimal value)
		{
			return new NumericCharacteristicValue(this, value);
		}

		#endregion
	}
}