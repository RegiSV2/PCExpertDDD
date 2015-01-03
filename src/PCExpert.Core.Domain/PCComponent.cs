using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PCExpert.Core.DomainFramework;
using PCExpert.Core.DomainFramework.Exceptions;
using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.Domain
{
	/// <summary>
	///     Represents hardware component of PC Configuration
	/// </summary>
	public class PCComponent : Entity
	{
		#region Constructors

		protected PCComponent()
		{
		}

		public PCComponent(string name, ComponentType type)
		{
			Argument.NotNullAndNotEmpty(name);
			Argument.ValidEnumItem(type);

			Name = name;

			Components = new List<PCComponent>();
			ContainedInterfaces = new List<ComponentInterface>();
			PlugInterfaces = new List<ComponentInterface>();
			CharacteristicVals = new List<CharacteristicValue>();
			_characteristics = new Lazy<IDictionary<ComponentCharacteristic, CharacteristicValue>>(
				InitCharacteristicsDictionary);
			_readOnlyCharacteristics = new Lazy<IReadOnlyDictionary<ComponentCharacteristic, CharacteristicValue>>(
				InitCharacteristicsReadOnlyDictionary);

			Type = type;
		}

		#endregion

		#region Properties

		private IList<PCComponent> Components { get; set; }

		private IList<ComponentInterface> ContainedInterfaces { get; set; }

		private IList<ComponentInterface> PlugInterfaces { get; set; }

		private IList<CharacteristicValue> CharacteristicVals { get; set; }

		private readonly Lazy<IDictionary<ComponentCharacteristic, CharacteristicValue>> _characteristics;

		private readonly Lazy<IReadOnlyDictionary<ComponentCharacteristic, CharacteristicValue>> _readOnlyCharacteristics;

		public string Name { get; private set; }

		/// <summary>
		///     Average market price
		/// </summary>
		public decimal AveragePrice { get; private set; }

		public ComponentType Type { get; private set; }

		public IReadOnlyCollection<PCComponent> ContainedComponents
		{
			get { return new ReadOnlyCollection<PCComponent>(Components); }
		}

		public IReadOnlyCollection<ComponentInterface> ContainedSlots
		{
			get { return new ReadOnlyCollection<ComponentInterface>(ContainedInterfaces); }
		}

		/// <summary>
		///     Collection of slots, that the component needs to plug to
		/// </summary>
		public IReadOnlyCollection<ComponentInterface> PlugSlots
		{
			get { return new ReadOnlyCollection<ComponentInterface>(PlugInterfaces); }
		}

		public IReadOnlyCollection<CharacteristicValue> CharacteristicValues
		{
			get { return new ReadOnlyCollection<CharacteristicValue>(CharacteristicVals); }
		}

		public IReadOnlyDictionary<ComponentCharacteristic, CharacteristicValue> Characteristics
		{
			get { return _readOnlyCharacteristics.Value; }
		}

		#endregion

		#region Public Methods

		public PCComponent WithContainedComponent(PCComponent childComponent)
		{
			Argument.NotNull(childComponent);

			if (SameIdentityAs(childComponent))
				throw new ArgumentException("Cannot add component to itself");
			if (Components.Any(x => x.SameIdentityAs(childComponent)))
				throw new DuplicateElementException("Child component has been already added");

			Components.Add(childComponent);
			return this;
		}

		public PCComponent WithContainedSlot(ComponentInterface containedSlot)
		{
			Argument.NotNull(containedSlot);

			ContainedInterfaces.Add(containedSlot);
			return this;
		}

		public PCComponent WithPlugSlot(ComponentInterface plugInterface)
		{
			Argument.NotNull(plugInterface);

			PlugInterfaces.Add(plugInterface);
			return this;
		}

		public PCComponent WithAveragePrice(decimal newAveragePrice)
		{
			Argument.NotNegative(newAveragePrice);

			AveragePrice = newAveragePrice;
			return this;
		}

		public PCComponent WithName(string newName)
		{
			Argument.NotNullAndNotEmpty(newName);

			Name = newName;
			return this;
		}

		public PCComponent WithType(ComponentType type)
		{
			Argument.ValidEnumItem(type);

			Type = type;

			return this;
		}

		public PCComponent WithCharacteristicValue(CharacteristicValue characteristicValue)
		{
			Argument.NotNull(characteristicValue);

			characteristicValue.AttachToComponent(this);
			CharacteristicVals.Add(characteristicValue);
			if(_characteristics.IsValueCreated)
				_characteristics.Value.Add(characteristicValue.Characteristic, characteristicValue);

			return this;
		}

		#endregion

		#region Private Methods

		private IReadOnlyDictionary<ComponentCharacteristic, CharacteristicValue> InitCharacteristicsReadOnlyDictionary()
		{
			return new ReadOnlyDictionary<ComponentCharacteristic, CharacteristicValue>(_characteristics.Value);
		}

		private IDictionary<ComponentCharacteristic, CharacteristicValue> InitCharacteristicsDictionary()
		{
			return CharacteristicVals.ToDictionary(
				x => x.Characteristic, x => x,
				new EntityEqualityComparer<ComponentCharacteristic>());
		}

		#endregion
	}
}