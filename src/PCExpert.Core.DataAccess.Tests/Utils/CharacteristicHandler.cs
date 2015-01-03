using System;
using PCExpert.Core.Domain;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.DataAccess.Tests.Utils
{
	abstract class CharacteristicHandler
	{
		protected readonly Random Random = new Random();

		protected CharacteristicHandler(Type characteristicType, Type valueType)
		{
			CharacteristicType = characteristicType;
			CharacteristicValueType = valueType;
		}

		public Type CharacteristicType { get; private set; }

		private Type CharacteristicValueType { get; set; }

		public ComponentCharacteristic CreateRandomCharacteristic(int index)
		{
			return CreateCharacteristic(
				NamesGenerator.CharacteristicName(index),
				ComponentType.PowerSupply)
				.WithPattern("bool pattern");
		}

		public bool CompareValues(CharacteristicValue valueA, CharacteristicValue valueB)
		{
			if (CharacteristicValueType.IsInstanceOfType(valueA)
			    && CharacteristicValueType.IsInstanceOfType(valueB))
				return DoCompareValues(valueA, valueB);
			return false;
		}

		public abstract CharacteristicValue CreateRandomValue(ComponentCharacteristic characteristic);

		protected abstract bool DoCompareValues(CharacteristicValue valueA, CharacteristicValue valueB);

		protected abstract ComponentCharacteristic CreateCharacteristic(string name, ComponentType compType);
	}
}