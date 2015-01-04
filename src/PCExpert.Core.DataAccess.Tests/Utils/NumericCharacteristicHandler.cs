using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess.Tests.Utils
{
	internal class NumericCharacteristicHandler : CharacteristicHandler
	{
		public NumericCharacteristicHandler()
			: base(typeof (NumericCharacteristic), typeof (NumericCharacteristicValue))
		{
		}

		public override CharacteristicValue CreateRandomValue(ComponentCharacteristic characteristic)
		{
			return ((NumericCharacteristic) characteristic).CreateValue(Random.Next());
		}

		protected override bool DoCompareValues(CharacteristicValue valueA, CharacteristicValue valueB)
		{
			return ((NumericCharacteristicValue) valueA).Value == ((NumericCharacteristicValue) valueB).Value;
		}

		protected override ComponentCharacteristic CreateCharacteristic(string name, ComponentType compType)
		{
			return new NumericCharacteristic(name, compType);
		}
	}
}