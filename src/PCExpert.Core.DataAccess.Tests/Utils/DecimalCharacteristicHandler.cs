using PCExpert.Core.Domain;
using PCExpert.Core.Tests.Utils;
namespace PCExpert.Core.DataAccess.Tests.Utils
{
	class DecimalCharacteristicHandler : CharacteristicHandler
	{
		public DecimalCharacteristicHandler() 
			: base(typeof(NumericCharacteristic), typeof(DecimalCharacteristicValue))
		{
		}

		public override CharacteristicValue CreateRandomValue(ComponentCharacteristic characteristic)
		{
			return ((NumericCharacteristic) characteristic).CreateValue(Random.Next());
		}

		protected override bool DoCompareValues(CharacteristicValue valueA, CharacteristicValue valueB)
		{
			return ((DecimalCharacteristicValue)valueA).Value == ((DecimalCharacteristicValue)valueB).Value;
		}

		protected override ComponentCharacteristic CreateCharacteristic(string name, ComponentType compType)
		{
			return new NumericCharacteristic(name, compType);
		}
	}
}