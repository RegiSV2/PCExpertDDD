using PCExpert.Core.Domain;
using PCExpert.Core.Tests.Utils;
namespace PCExpert.Core.DataAccess.Tests.Utils
{
	class IntCharacteristicHandler : CharacteristicHandler
	{
		public IntCharacteristicHandler() 
			: base(typeof(IntCharacteristic), typeof(IntCharacteristicValue))
		{
		}

		public override CharacteristicValue CreateRandomValue(ComponentCharacteristic characteristic)
		{
			return ((IntCharacteristic) characteristic).CreateValue(Random.Next());
		}

		protected override bool DoCompareValues(CharacteristicValue valueA, CharacteristicValue valueB)
		{
			return ((IntCharacteristicValue)valueA).Value == ((IntCharacteristicValue)valueB).Value;
		}

		protected override ComponentCharacteristic CreateCharacteristic(string name, ComponentType compType)
		{
			return new IntCharacteristic(name, compType);
		}
	}
}