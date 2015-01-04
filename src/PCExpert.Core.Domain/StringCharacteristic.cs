namespace PCExpert.Core.Domain
{
	/// <summary>
	///     Represents a characteristic that can have string values
	/// </summary>
	public class StringCharacteristic : ComponentCharacteristic
	{
		#region Public Methods

		public StringCharacteristicValue CreateValue(string value)
		{
			return new StringCharacteristicValue(this, value);
		}

		#endregion

		#region Constructors

		protected StringCharacteristic()
		{
		}

		public StringCharacteristic(string name, ComponentType type)
			: base(name, type)
		{
		}

		#endregion
	}
}