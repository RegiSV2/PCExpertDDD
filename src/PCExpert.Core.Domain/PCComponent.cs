using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PCExpert.Core.Domain.Exceptions;
using PCExpert.Core.Domain.Utils;

namespace PCExpert.Core.Domain
{
	/// <summary>
	///     Represents hardware component of PC Configuration
	/// </summary>
	public class PCComponent : Entity
	{
		#region Properties

		private readonly IList<PCComponent> _containedComponents = new List<PCComponent>();

		private readonly IList<ComponentInterface> _containedSlots = new List<ComponentInterface>();

		public string Name { get; private set; }

		/// <summary>
		///     Average market price
		/// </summary>
		public decimal AveragePrice { get; private set; }

		/// <summary>
		/// A slot, that component plugs into
		/// </summary>
		public ComponentInterface PlugSlot { get; private set; }

		public IReadOnlyCollection<PCComponent> ContainedComponents
		{
			get { return new ReadOnlyCollection<PCComponent>(_containedComponents); }
		}

		public IReadOnlyCollection<ComponentInterface> ContainedSlots
		{
			get { return new ReadOnlyCollection<ComponentInterface>(_containedSlots); }
		}

		#endregion

		#region Constructors

		public PCComponent(string name)
		{
			Argument.NotNullAndNotEmpty(name);

			Name = name;
			PlugSlot = ComponentInterface.NullObject;
		}

		#endregion

		#region Public Methods

		public PCComponent WithContainedComponent(PCComponent childComponent)
		{
			Argument.NotNull(childComponent);

			if (_containedComponents.Contains(childComponent))
				throw new DuplicateElementException("Child component has been already added");

			_containedComponents.Add(childComponent);
			return this;
		}

		public PCComponent WithContainedSlot(ComponentInterface containedSlot)
		{
			Argument.NotNull(containedSlot);

			if (_containedSlots.Contains(containedSlot))
				throw new DuplicateElementException("Slot has been already added");

			_containedSlots.Add(containedSlot);
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