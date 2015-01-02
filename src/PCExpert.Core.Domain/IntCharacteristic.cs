namespace PCExpert.Core.Domain
{
	/// <summary>
	/// Characteristic that can have integer argument values
	/// </summary>
	public class IntCharacteristic : ComponentCharacteristic
	{
		#region Constructors

		protected IntCharacteristic()
		{ }

		public IntCharacteristic(string name, ComponentType type)
			: base(name, type)
		{ }

		#endregion

		#region Public Methods

		public IntCharacteristicValue CreateValue(int value)
		{
			return new IntCharacteristicValue(this, value);
		}

		#endregion
	}
}