namespace PCExpert.Core.Domain
{
	/// <summary>
	///     Represents a characteristic that can have only
	///     <value>True</value>
	///     /
	///     <value>False</value>
	///     values
	/// </summary>
	public class BoolCharacteristic : ComponentCharacteristic
	{
		#region Public Methods

		public BoolCharacteristicValue CreateValue(bool value)
		{
			return new BoolCharacteristicValue(this, value);
		}

		#endregion

		#region Constructors

		protected BoolCharacteristic()
		{
		}

		public BoolCharacteristic(string name, ComponentType type)
			: base(name, type)
		{
		}

		#endregion
	}
}