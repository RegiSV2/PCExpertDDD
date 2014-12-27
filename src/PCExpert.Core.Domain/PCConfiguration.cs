using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PCExpert.Core.DomainFramework;
using PCExpert.Core.DomainFramework.Exceptions;
using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.Domain
{
	public class PCConfiguration : Entity
	{
		#region Constructors

		public PCConfiguration()
		{
			ContainedComponents = new List<PCComponent>();
		}

		#endregion

		#region Properties

		public string Name { get; private set; }

		private IList<PCComponent> ContainedComponents { get; set; }

		/// <summary>
		///     Components, included in this configuration
		/// </summary>
		public IReadOnlyCollection<PCComponent> Components
		{
			get { return new ReadOnlyCollection<PCComponent>(ContainedComponents); }
		}

		#endregion

		#region Methods

		/// <summary>
		///     Calculates sum price of all included components
		/// </summary>
		public decimal CalculatePrice()
		{
			return ContainedComponents.Sum(x => x.AveragePrice);
		}

		/// <summary>
		///     Sets new name for configuration
		/// </summary>
		public PCConfiguration WithName(string name)
		{
			Name = name;

			return this;
		}

		/// <summary>
		///     Adds component to configuration
		/// </summary>
		public PCConfiguration WithComponent(PCComponent component)
		{
			Argument.NotNull(component);
			if (ContainedComponents.Any(x => x.SameIdentityAs(component)))
				throw new DuplicateElementException("Component already added");

			ContainedComponents.Add(component);

			return this;
		}

		#endregion
	}
}