using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PCExpert.Core.Domain.Exceptions;
using PCExpert.Core.Domain.Utils;

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

		public PCComponent(string name)
		{
			Argument.NotNullAndNotEmpty(name);

			Name = name;
			PlugSlot = ComponentInterface.NullObject;

			Components = new List<PCComponent>();
			Slots = new List<ComponentInterface>();
		}

		#endregion

		#region Properties

		private IList<PCComponent> Components { get; set; }

		private IList<ComponentInterface> Slots { get; set; }

		public string Name { get; private set; }

		/// <summary>
		///     Average market price
		/// </summary>
		public decimal AveragePrice { get; private set; }

		/// <summary>
		///     A slot, that component plugs into
		/// </summary>
		public ComponentInterface PlugSlot { get; private set; }

		public IReadOnlyCollection<PCComponent> ContainedComponents
		{
			get { return new ReadOnlyCollection<PCComponent>(Components); }
		}

		public IReadOnlyCollection<ComponentInterface> ContainedSlots
		{
			get { return new ReadOnlyCollection<ComponentInterface>(Slots); }
		}

		#endregion

		#region Public Methods

		public PCComponent WithContainedComponent(PCComponent childComponent)
		{
			Argument.NotNull(childComponent);
			CheckIdentity(childComponent);

			if (Components.Contains(childComponent))
				throw new DuplicateElementException("Child component has been already added");

			Components.Add(childComponent);
			return this;
		}

		private void CheckIdentity(PCComponent childComponent)
		{
			if (this == childComponent ||
				(IsPersisted && SameIdentityAs(childComponent)))
				throw new ArgumentException("Cannot add component to itself");
		}

		public PCComponent WithContainedSlot(ComponentInterface containedSlot)
		{
			Argument.NotNull(containedSlot);

			if (Slots.Contains(containedSlot))
				throw new DuplicateElementException("Slot has been already added");

			Slots.Add(containedSlot);
			return this;
		}

		public PCComponent WithPlugSlot(ComponentInterface plugInterface)
		{
			Argument.NotNull(plugInterface);

			PlugSlot = plugInterface;
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

		#endregion
	}
}