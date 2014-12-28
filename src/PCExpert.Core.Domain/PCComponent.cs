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

			Type = type;
		}

		#endregion

		#region Properties

		private IList<PCComponent> Components { get; set; }

		private IList<ComponentInterface> ContainedInterfaces { get; set; }

		private IList<ComponentInterface> PlugInterfaces { get; set; }

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

		#endregion
	}
}