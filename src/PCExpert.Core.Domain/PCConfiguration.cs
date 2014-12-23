using System.Collections.Generic;

namespace PCExpert.Core.Domain
{
	public class PCConfiguration : Entity
	{
		#region Constructors

		public PCConfiguration()
		{
			Components = new List<PCComponent>();
		}

		#endregion

		#region Properties

		public string Name { get; private set; }

		/// <summary>
		/// Components, included in this configuration
		/// </summary>
		public IList<PCComponent> Components { get; private set; }

		#endregion

		#region Methods

		/// <summary>
		/// Calculates sum price of all included components
		/// </summary>
		public decimal CalculatePrice()
		{
			return 0;
		}

		#endregion
	}
}