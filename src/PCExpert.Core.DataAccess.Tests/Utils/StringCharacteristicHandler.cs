using System;
using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess.Tests.Utils
{
	class StringCharacteristicHandler : CharacteristicHandler
	{
		public StringCharacteristicHandler()
			: base(typeof(StringCharacteristic), typeof(StringCharacteristicValue))
		{
		}

		public override CharacteristicValue CreateRandomValue(ComponentCharacteristic characteristic)
		{
			return ((StringCharacteristic)characteristic).CreateValue(Guid.NewGuid().ToString());
		}

		protected override bool DoCompareValues(CharacteristicValue valueA, CharacteristicValue valueB)
		{
			return ((StringCharacteristicValue)valueA).Value == ((StringCharacteristicValue)valueB).Value;
		}

		protected override ComponentCharacteristic CreateCharacteristic(string name, ComponentType compType)
		{
			return new StringCharacteristic(name, compType);
		}
	}
}