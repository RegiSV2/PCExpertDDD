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
		#region Public Methods

		public PCComponent WithContainsComponent(PCComponent childComponent)
		{
			Argument.NotNull(childComponent);
			if (_containedComponents.Contains(childComponent))
				throw new DuplicateElementException("Child component has been already added");

			_containedComponents.Add(childComponent);
			return this;
		}

		#endregion

		#region Properties

		private readonly IList<PCComponent> _containedComponents;

		public string Name { get; private set; }

		/// <summary>
		///     Average market price
		/// </summary>
		public decimal AveragePrice { get; private set; }

		public IReadOnlyCollection<PCComponent> ContainedComponents
		{
			get { return new ReadOnlyCollection<PCComponent>(_containedComponents); }
		}

		#endregion

		#region Constructors

		protected PCComponent(Guid id)
			: base(id)
		{
		}

		public PCComponent(string name, decimal price)
		{
			Name = name;
			AveragePrice = price;
			_containedComponents = new List<PCComponent>();
		}

		#endregion
	}
}