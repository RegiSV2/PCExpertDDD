using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCExpert.Core.Domain
{
	/// <summary>
	/// Represents hardware component of PC Configuration
	/// </summary>
	public class PCComponent
	{
		public string Name { get; private set; }

		/// <summary>
		/// Average market price
		/// </summary>
		public decimal AveragePrice { get; private set; }

		public PCComponent(string name, decimal price)
		{
			
		}
	}
}
