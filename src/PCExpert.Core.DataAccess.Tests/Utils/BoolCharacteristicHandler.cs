using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess.Tests.Utils
{
	class BoolCharacteristicHandler : CharacteristicHandler
	{
		public BoolCharacteristicHandler()
			: base(typeof(BoolCharacteristic), typeof(BoolCharacteristicValue))
		{
		}

		public override CharacteristicValue CreateRandomValue(ComponentCharacteristic characteristic)
		{
			return ((BoolCharacteristic) characteristic).CreateValue(RandomBool());
		}

		private bool RandomBool()
		{
			return Random.Next(2) != 0;
		}

		protected override bool DoCompareValues(CharacteristicValue valueA, CharacteristicValue valueB)
		{
			return ((BoolCharacteristicValue)valueA).Value == ((BoolCharacteristicValue)valueB).Value;
		}

		protected override ComponentCharacteristic CreateCharacteristic(string name, ComponentType compType)
		{
			return new BoolCharacteristic(name, compType);
		}
	}
}